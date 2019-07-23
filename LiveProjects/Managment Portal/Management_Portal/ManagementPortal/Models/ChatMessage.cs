using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagementPortal.Models
{
    public class ChatMessage
    {
        [Display(Name = "Id")]
        public Guid ChatMessageId { get; set; }
        [Display(Name = "Sender")]
        public string Sender { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}