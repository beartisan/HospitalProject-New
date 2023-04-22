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
    public class AppointmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all appointments in the system
        /// </summary>
        /// <returns>
        /// content: List of all bookings in the database
        /// </returns>
        /// <example>
        /// GET: api/AppointmentData
        /// </example>
        /// 
        [HttpGet]
        [ResponseType(typeof(AppointmentDto))]
        //[Authorize(Roles = "Admin, Guest")]
        public IHttpActionResult ListAppointments()
        {
            List<Appointment> Appointments = db.Appointments.ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto>();

            Appointments.ForEach(a => AppointmentDtos.Add(new AppointmentDto()
            {
                appointment_id = a.appointment_id,
                appointment_name = a.appointment_name,
                appointment_date = a.appointment_date,
                patient_id = a.patient_id,
                patient_condition = a.patient_condition,
                physician_id = a.physician_id
            }));

            return Ok(AppointmentDtos);
        }


        /// <summary>
        /// Returns all appointments in the system
        /// </summary>
        /// <param name="id">Primary key of appointment ID</param>
        /// <returns>
        /// CONTENT: An appointment matching to the Appointment ID 
        /// </returns>

        // GET: api/AppointmentData/5

        [ResponseType(typeof(Appointment))]
        public IHttpActionResult FindAppointment(int id)
        {
            Appointment Appointment = db.Appointments.Find(id);
            AppointmentDto AppointmentDto = new AppointmentDto() 
            { 
                appointment_id = Appointment.appointment_id,
                appointment_name = Appointment.appointment_name,
                appointment_date = Appointment.appointment_date,
                patient_id = Appointment.patient_id,
                patient_condition = Appointment.patient_condition,
                physician_id = Appointment.physician_id
            };

            if (Appointment == null)
            {
                return NotFound();
            }

            return Ok(AppointmentDto);
        }

        /// <summary>
        /// Updates an appointment through POST Data Input
        /// </summary>
        /// <param name="id">Primary key of Appoinment_id</param>
        /// <param name="appointment">JSON Form Data of Appointment</param>
        /// <returns>
        /// Content: All Appointment Details
        /// </returns>
        /// <example>
        /// 
        /// Before Update:
        /// appointment_id = 1
        /// appointment_name = Mental Health Therapy
        /// appointment_date = August 31, 2023
        /// patient_id = 2
        /// patient_condition = Obsessive Compulsive Disorder
        /// physician_id = 3
        /// 
        /// curl -d @Appointment.json -H "Content-type: application/json" https://localhost:44324/api/AppointmentData/UpdateAppointment/5
        /// 
        /// After Update:
        /// appointment_id = 1
        /// appointment_name = Mental Health Therapy
        /// appointment_date = September 5, 2023
        /// patient_id = 2
        /// patient_condition = Obsessive Compulsive Disorder
        /// physician_id = 3
        /// </example>
        
        // POST: api/AppointmentData/UpdateAppointment/5
        [ResponseType(typeof(void))]
        [HttpPost]
        //[Authorize(Roles = "Admin, Guest")]
        public IHttpActionResult UpdateAppointment(int id, Appointment Appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Appointment.appointment_id)
            {
                return BadRequest();
            }

            db.Entry(Appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        // POST: api/AppointmentData
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult PostAppointment(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(appointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointment.appointment_id }, appointment);
        }

        // DELETE: api/AppointmentData/5
        [ResponseType(typeof(Appointment))]
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(appointment);
            db.SaveChanges();

            return Ok(appointment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.appointment_id == id) > 0;
        }
    }
}