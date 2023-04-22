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

        // GET: api/PatientData/ListPatients
        public IQueryable<Patient> ListPatients()
        {
            return db.Patients;
        }

        // GET: api/PatientData/FindPatient/5
        [ResponseType(typeof(Patient))]
        public IHttpActionResult FindPatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // POST: api/PatientData/UpdatePatient/5
        [ResponseType(typeof(void))]
        [HttpPost]
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

        // POST: api/PatientData/AddPatient
        [ResponseType(typeof(Patient))]
        [HttpPost]
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

        // POST: api/PatientData/5
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

            return Ok(patient);
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