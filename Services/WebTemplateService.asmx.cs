using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Web.Services;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Script.Services;
using FirestoneWebTemplate.Classes;
using System.Web.Services.Protocols;
using FirestoneWebTemplate.EmailService;

namespace FirestoneWebTemplate
{
  /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://FirestoneWebTemplate.bfusa.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]    
    public class WebTemplateService : System.Web.Services.WebService
    {
        private string sConnection = ConfigurationManager.ConnectionStrings["Template_DB"].ConnectionString;


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
    


      
        public bool SendMessage(EmailMessage email)
        {           
            var client = new EmailServiceClient();
            var to = email.To.Split(';');

            var result = client.SendEmail(to, email.From, email.Subject, email.Body, true, "Normal", new string[] { "" }, new string[] { "" });

            return result;
        }

    }


}
