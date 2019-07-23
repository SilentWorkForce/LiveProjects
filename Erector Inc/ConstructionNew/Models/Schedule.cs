using ConstructionNew.Enums;
using ConstructionNew.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConstructionNew.Models
{
    public class Schedule
    {
        [Key]
        [Display(Name = "ID")]
        public Guid ScheduleId { get; set; }
        [Display(Name = "Job ")]
        public virtual Job Job { get; set; }
        [Display(Name = "Employee")]
        public virtual ApplicationUser Person { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? EndDate { get; set; } //EndDate Not Required, Nullable

        public Schedule()
        {

        }

        public Schedule(ApplicationUser person, Job job, DateTime startdate, DateTime? enddate)
        {
            ScheduleId = Guid.NewGuid();
            Job = job;
            Person = person;
            StartDate = startdate;
            EndDate = enddate;
        }
    }
}