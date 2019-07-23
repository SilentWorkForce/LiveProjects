using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ManagementPortal.Models
{
    public class EventModel
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Job")]
        public Job Job { get; set; }                                                        //Foreign Key to Job table

        [Display(Name = "Type")]
        [StringLength(100)]                                                                 //StringLength attribute limits nvarchar length
        public string Type { get; set; }

        [Display(Name = "Name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Date Ordered")]
        public DateTime DateOrdered { get; set; }

        [Display(Name = "Date ETA")]
        public DateTime DateETA { get; set; }

        [Display(Name = "URL")]
        [StringLength(1000)]                                                                //set at 1000 as per assignment request
        public string URL { get; set; }

    }
}