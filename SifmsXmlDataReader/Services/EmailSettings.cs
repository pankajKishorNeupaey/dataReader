using System.Collections.Generic;

namespace SifmsXmlDataReader.Services
{


    public class EmailSettings
    {
            public string MailServer { get; set; }
            public int MailPort { get; set; }
            public string SenderName { get; set; }
            public string Sender { get; set; }
            public string Password { get; set; }
    }
    
    public class EmailAddressList
    {
        public List<EmailAddress> EmailList{ get; set;}
    }
    public class EmailAddress
    {
        public string emailAddress { get; set; }
    }
    
}
