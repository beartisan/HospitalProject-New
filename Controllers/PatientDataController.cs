using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using hospital_project.Models;

namespace hospital_project.Controllers
{
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List of all patients' information
        /// </summary>
        /// <example>
        /// GET: /api/PatientData/ListPatients
        /// </example>
        /// <returns>
        /// CONTENT: All list of patients in the database
        /// </returns>

        // GET: api/PatientData/ListPatients

        [HttpGet]
        public IEnumerable<PatientDto> ListPatients()
        {
            List<Patient> Patients = db.Patients.ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();

            Patients.ForEach(pt => PatientDtos.Add(new PatientDto()
            {
                patient_id = pt.patient_id,
                healthcard_id = pt.healthcard_id,
                patient_fname = pt.patient_fname,
                patient_surname = pt.patient_surname,
                patient_birthday = pt.patient_birthday,
                patient_phoneNum = pt.patient_phoneNum,
                patient_condition = pt.patient_condition,
                physician_id = pt.Physician.physician_id,
                first_name = pt.Physician.first_name,
                last_name = pt.Physician.last_name
            }));

            return PatientDtos;
        }

        /// <summary>
        /// gives information about patients related to a particular physician
        /// </summary>
        /// <param name="id">Physician's primary key</param>
        /// <returns>
        /// GET: api/patientdata/ListPatientsForPhysician/1
        /// </returns>
        [HttpGet]
        [ResponseType(typeof(PatientDto))]
        public IHttpActionResult ListPatientsForPhysician (int id)
        {
            // all patients that have a physician which matches with ID
            List<Patient> Patients = db.Patients.Where(
                pt => pt.Physician.Any(
                    Physician=>physician_id==id)).ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();

            Patients.ForEach(pt => PatientDtos.Add(new PatientDto()
            {
                patient_id = pt.patient_id,
                healthcard_id = pt.healthcard_id,
                patient_fname = pt.patient_fname,
                patient_surname = pt.patient_surname,
                patient_birthday = pt.patient_birthday,
                patient_phoneNum = pt.patient_phoneNum,
                patient_condition = pt.patient_condition,
                physician_id = pt.Physician.physician_id,
                first_name = pt.Physician.first_name,
                last_name = pt.Physician.last_name
            }));

            return Ok(PatientDtos);
        }

        /// <summary>
        /// associates a physician to a particular patient
        /// </summary>
        /// <param name="patient_id">Primary key of Patient ID</param>
        /// <param name="physician_id">Primary key of Physician ID</param>
        /// <returns></returns>
        /// POST api/PatientData/PatientWithPrimaryPhysician/{patient_id}/{physician_id}
        [Route("api/PatientData/PatientWithPrimaryPhysician/{patient_id}/{physician_id}")]
        [HttpPost]
        public IHttpActionResult PatientWithPrimaryPhysician (int patient_id, int physician_id)
        {

            Patient SelectedPatient = db.Patients.Include(pt => pt.Physicians).Where(pt => pt.patient_id == patient_id).FirstOrDefault();
            Physician SelectedPhysician = db.Physicians.Find(physician_id);

            if (SelectedPatient == null || SelectedPhysician == null)
            {
                return NotFound();
            }

            SelectedPatient.Physicians.Add(SelectedPhysician);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// removes association of a  physician to a particular patient
        /// </summary>
        /// <param name="patient_id">Primary key of Patient ID</param>
        /// <param name="physician_id">Primary key of Physician ID</param>
        /// <returns></returns>
        /// POST api/PatientData/PatientRemovePrimaryPhysician/{patient_id}/{physician_id}
        [Route("api/PatientData/PatientRemovePrimaryPhysician/{patient_id}/{physician_id}")]
        [HttpPost]
        public IHttpActionResult PatientRemovePrimaryPhysician(int patient_id, int physician_id)
        {

            Patient SelectedPatient = db.Patients.Include(pt => pt.Physicians).Where(pt => pt.patient_id == patient_id).FirstOrDefault();
            Physician SelectedPhysician = db.Physicians.Find(physician_id);

            if (SelectedPatient == null || SelectedPhysician == null)
            {
                return NotFound();
            }

            SelectedPatient.Physicians.Remove(SelectedPhysician);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Find a patient based on patient_id and will display information
        /// </summary>
        /// <param name="id">represents primary key of Patient ID</param>
        /// <example>
        /// https://localhost:44324/api/PatientData/FindPatient/2
        /// patient_id: 2
        /// healthcard_id: 203045
        /// patient_fname: Monica
        /// patient_surname: Geller
        /// patient_birthday: February 26, 1970
        /// patient_phoneNum: 678-999-8212
        /// patient_condition: OCD
        /// physician_id: 3
        /// </example>
        /// <returns>
        /// All information of a particular patient
        /// </returns>
        // GET: api/PatientData/FindPatient/2
        [ResponseType(typeof(PatientDto))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            PatientDto PatientDto = new PatientDto()
            {
                patient_id = patient.patient_id,
                healthcard_id = patient.healthcard_id,
                patient_fname = patient.patient_fname,
                patient_surname = patient.patient_surname,
                patient_birthday = patient.patient_birthday,
                patient_phoneNum = patient.patient_phoneNum,
                patient_condition = patient.patient_condition,
                physician_id = patient.Physician.physician_id,
                first_name = patient.Physician.first_name,
                last_name = patient.Physician.last_name
            };

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(PatientDto);
        }

        /// <summary>
        /// Updates a particular patient through POST data input based on patient_id
        /// </summary>
        /// <param name="id">represents the primary key of patient's id</param>
        /// <param name="patient">Form Data in JSON of a patient </param>
        /// <example>
        /// Before update:
        /// patient_id: 2
        /// healthcard_id: 203045
        /// patient_fname: Monica
        /// patient_surname: Geller
        /// patient_birthday: February 26, 1970
        /// patient_phoneNum: 678-999-8212
        /// patient_condition: OCD
        /// physician_id: 3
        /// 
        /// In the terminal --> Curl Request: curl -d @patient.json -H "Content-type: application/json" https://localhost:44324/api/PatientData/UpdatePatient/2
        ///
        /// After the update:
        /// patient_id: 2
        /// healthcard_id: 203045
        /// patient_fname: Monica
        /// patient_surname: Bing
        /// patient_birthday: February 26, 1970
        /// patient_phoneNum: 636-555-3226
        /// patient_condition: Pregnancy - Third Trimester
        /// physician_id: 4
        /// </example>
        /// <returns> updates the patient's particular database
        /// </returns>
        /// POST: api/PatientData/UpdatePatient/5
        /// Form Data: Patient JSON Object

        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.patient_id)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a new patient to the Patients Table
        /// </summary>
        /// <param name="patient">JSON FORM Data of a patient</param>
        /// <example>
        /// In the terminal:
        /// curl -d @Patient.json -H "Content-type: application/json" https://localhost:44324/api/PatientData/AddPatient
        /// </example>
        /// <returns>
        /// All contents including Patient ID, and Patient Data
        /// </returns>
        // POST: api/PatientData/AddPatient
        /// Form Data: Patient JSON Object

        [ResponseType(typeof(Patient))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.patient_id }, patient);
        }

        /// <summary>
        /// Deletes a patient through patient_id from the system
        /// </summary>
        /// <example>
        /// curl -d ""https://localhost:44324/api/PatientData/DeletePatient/3
        /// </example>
        /// <param name="id">the Patient_id is primary key of a patient</param>
        /// <returns>
        /// Deletes a patient through patient_id from the system in post request
        /// </returns>
        // POST: api/PatientData/DeletePatient/3

        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.patient_id == id) > 0;
        }
    }
}