using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ConstructionNew.Enums;

namespace ConstructionNew.Models
{
    public class JobDetailsViewModel
    {
        [Display(Name = "ID")]
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
        public string ShiftTimes { get; set; }
        [Display(Name = "Foreman")]
        public string ForemanName { get; set; }
        [Display(Name ="Phone")]
        public string Phone { get; set; }

        
    }
}