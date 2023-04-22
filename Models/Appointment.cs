using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hospital_project.Models
{
    public class Appointment
    { 
        [Key]
        public int appointment_id { get; set; }
        public string appointment_name { get; set; }
        public DateTime appointment_date { get; set; }
        //appointment belongs to one patient
        [ForeignKey("Patient")]
        public int patient_id { get; set; }
        public string patient_condition { get; set; }
        public virtual Patient Patient { get; set; }
        //appointment belongs to physician
        [ForeignKey("Physician")]
        public int physician_id { get; set; }
        public virtual Physician Physician { get; set; }
    }
    public class AppointmentDto
    {
        public int appointment_id { get; set; }
        public string appointment_name { get; set; }
        public DateTime appointment_date { get; set; }
        public int patient_id { get; set; }
        public string patient_condition { get; set; }
        public int physician_id { get; set; }

    }

}