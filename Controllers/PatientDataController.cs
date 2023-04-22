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

            Patients.ForEach(p => PatientDtos.Add(new PatientDto()
            {
                patient_id = p.patient_id,
                healthcard_id = p.healthcard_id,
                patient_fname = p.patient_fname,
                patient_surname = p.patient_surname,
                patient_birthday = p.patient_birthday,
                patient_phoneNum = p.patient_phoneNum,
                patient_condition = p.patient_condition,
                physician_id = p.Physician.physician_id,
                first_name = p.Physician.first_name,
                last_name = p.Physician.last_name
            }));

            return PatientDtos;
        }

        /// <summary>
        /// Find a patient based on patient_id and will display information
        /// </summary>
        /// <param name="id">represents primary key of Patient ID</param>
        /// <example>
        /// localhost/api/PatientData/FindPatient/2
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
        /// In the terminal --> Curl Request: curl -d @patient.json -H "Content-type: application/json" localhost/api/PatientData/UpdatePatient/2
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
        /// curl -d @Patient.json -H "Content-type: application/json" localhost/api/PatientData/AddPatient
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
        /// curl -d "" localhost/api/PatientData/DeletePatient/3
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