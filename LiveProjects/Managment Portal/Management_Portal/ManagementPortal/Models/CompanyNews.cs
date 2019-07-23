using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ManagementPortal.Models
{
    public class CompanyNews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsId { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy hh:mm tt}")]
        public string DateStamp { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Body")]
        public string NewsItem { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Expiration Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ExpirationDate { get; set; }
    }
}