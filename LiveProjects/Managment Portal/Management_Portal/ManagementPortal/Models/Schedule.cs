using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ManagementPortal.Models;
using System.ComponentModel;
using ManagementPortal.Enums;

namespace ManagementPortal.Models
{
    public class Schedule
    {
        [DisplayName("Schedule ID")]
        public Guid ScheduleId { get; set; }
        [DisplayName("Job")]
        public Job Job { get; set; }
        [DisplayName("Person")]
        public ApplicationUser Person { get; set; }
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }
    }
}