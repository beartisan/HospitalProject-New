using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hospital_project.Models
{
    public class Patient
    {
        [Key]
        public int patient_id { get; set; }

        public int healthcard_id { get; set; }
        public string patient_fname { get; set; }
        public string patient_surname { get; set; }

        public DateTime patient_birthday { get; set; }

        public string patient_phoneNum { get; set; }

        public string patient_condition { get; set; }


        // Foreign Key of Physician Id will point to specific Patient as primary Physician
        [ForeignKey("Physician")]

        public int physician_id { get; set; }

        public virtual Physician Physician { get; set; }
    }

}