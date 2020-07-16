using System;
using System.Net;
using System.Net.Mail;

namespace SifmsXmlDataReader.Services
{

    public class EmailSender 
    {
        public void SendEmailAsync(string email, string subject, string message,EmailSettings emailSettings)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential(emailSettings.Sender, emailSettings.Password);

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress(emailSettings.Sender, emailSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(email));

                // Smtp client
                var client = new SmtpClient()
                {

                    Port = emailSettings.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = emailSettings.MailServer,
                    EnableSsl = true,
                    Credentials = credentials
                };

                // Send it...         
                client.Send(mail);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }

           // return Task.CompletedTask;
        }

    }
}
