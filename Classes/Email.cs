using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace FirestoneWebTemplate.Classes
{
    [Serializable()]
    public class EmailMessage
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Template { get; set; }
        public bool IsBodyHtml { get; set; }
        //public List<Attachment> Attachments { get; set; }
        public string Priority { get; set; }


    }
}