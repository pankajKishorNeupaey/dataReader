using System;
using System.Collections.Generic;
using System.Text;

namespace SifmsXmlDataReader.Services
{
    public class SendEmails
    {
 
        public int mailer( EmailSettings emailSettings)
        {
            List<string> emailIds = new List<string>();
            emailIds.Add("alina.bhutia@sibingroup.com");
            emailIds.Add("pankaj.neupaney@sibingroup.com");
            emailIds.Add("nishant.thapa@sibingroup.com");
            var subject = "subject";
            var emailBody = "hello";


            foreach (var email in emailIds)
            {
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmailAsync(email, subject, emailBody, emailSettings);
            }
       
           int a = 1;
           return a;
        }
    }
}
