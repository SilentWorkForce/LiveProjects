using ConstructionNew.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConstructionNew.Models
{
    public class Job
    {
        [Key]
        [Display(Name = "Job ID")]
        public Guid JobId { get; set; }
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        [Display(Name = "Job#")]
        public string JobNumber { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "State")]
        public State State { get; set; }
        [Display(Name = "Zip")]
        public int? Zipcode { get; set; }
        [Display(Name = "Notes")]
        public string Note { get; set; }
        [Display(Name = "Shift Times")]
        public virtual ShiftTime ShiftTimes { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }


    }
}