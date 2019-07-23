using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

//this Folder is to test only SMTP for email confirmation and password reset link
namespace ConstructionNew.GmailSMTP
{
    public class GmailEmailService : SmtpClient
    {
        // Gmail user-name
        public string UserName { get; set; }

        public GmailEmailService() :
            base(ConfigurationManager.AppSettings["GmailHost"], Int32.Parse(ConfigurationManager.AppSettings["GmailPort"]))
        {
            //Get values from web.config file:
            UserName = ConfigurationManager.AppSettings["GmailUserName"];
            EnableSsl = bool.Parse(ConfigurationManager.AppSettings["GmailSsl"]);
            UseDefaultCredentials = false;
            Credentials = new System.Net.NetworkCredential(UserName, ConfigurationManager.AppSettings["GmailPassword"]);
        }


    }
}