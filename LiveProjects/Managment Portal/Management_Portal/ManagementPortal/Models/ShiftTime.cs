using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementPortal.Models
{
    public class ShiftTime
    {
        [Key]
        [Display(Name = "Shift Times ID")]
        public Guid ShiftTimeId { get; set; }
        [Display(Name = "Monday")]
        public string Monday { get; set; }
        [Display(Name = "Tuesday")]
        public string Tuesday { get; set; }
        [Display(Name = "Wednesday")]
        public string Wednesday { get; set; }
        [Display(Name = "Thursday")]
        public string Thursday { get; set; }
        [Display(Name = "Friday")]
        public string Friday { get; set; }
        [Display(Name = "Saturday")]
        public string Saturday { get; set; }
        [Display(Name = "Sunday")]
        public string Sunday { get; set; }
        [Display(Name = "Default")]
        public string Default { get; set; }
    }
}
