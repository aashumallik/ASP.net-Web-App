using FirestoneWebTemplate.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Services;
using System.Web.Services;

namespace FirestoneWebTemplate
{
    /// <summary>
    /// Summary description for SecurityService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SecurityService : System.Web.UI.Page//System.Web.Services.WebService
    {
        private string sConnection = ConfigurationManager.ConnectionStrings["SecurityDBConn"].ConnectionString;

        public enum PermissionType
        {
            CLOCK_CARD = 1,
            WINDOWS_USER = 2,
            AD_GROUP = 3,
            JOB_CLASS = 4,
            DEPARTMENT = 5,
            RFID_BADGE = 6,
            BUSINESS_AREA = 7,
            UID = 8,
            JOB_CATEGORY = 9
        }

        public enum TeammateType
        {
            CLOCK_CARD = 1,
            WINDOWS_USER = 2,
            AD_GROUP = 3,
            JOB_CLASS = 4,
            DEPARTMENT = 5,
            RFID_BADGE = 6,
            BUSINESS_AREA = 7,
            UID = 8,
            JOB_CATEGORY = 9
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public bool setCurrentTeammate(string value, TeammateType Teammatetype)
        {
            bool rVal = false;
            // this will be the entry point where the app will set the current user.. 
            // kill any existing session vars, as we're starting over with a new login..
            clearSessionData();
            clearSessionDataIFRAME();

            int type = 0;
            switch (Teammatetype)
            {
                case TeammateType.CLOCK_CARD:
                    type = 1;
                    break;
                case TeammateType.WINDOWS_USER:
                    type = 2;
                    break;
                case TeammateType.AD_GROUP:
                    type = 3;
                    break;
                case TeammateType.JOB_CLASS:
                    type = 4;
                    break;
                case TeammateType.DEPARTMENT:
                    type = 5;
                    break;
                case TeammateType.RFID_BADGE:
                    type = 6;
                    break;
                case TeammateType.BUSINESS_AREA:
                    type = 7;
                    break;
                case TeammateType.UID:
                    type = 8;
                    break;
                case TeammateType.JOB_CATEGORY:
                    type = 9;
                    break;
            }

            //What i am doing here is grabbing a date time from SQL and encrypting it. This is for the security of the URL. It will be passed to the webservice which will
            //decrypt it and compare date/time. If greater than a few seconds difference then it will return null
            List<Teammates_Security_tbl> Teammates = new List<Teammates_Security_tbl>();
            string dateTime = EncryptQueryString(GetDateTime().ToString(), ConfigurationManager.AppSettings["SecurityKey"], ConfigurationManager.AppSettings["SecuritySalt"]);

            string myURL = "";

            for (int i = 0; i < 10; i++) //try 10 times to ccount for network issues and or decryption issues
            {
                System.Threading.Thread.Sleep(1000); //this is for testing - can comment out before adding to prod

                myURL = ConfigurationManager.AppSettings["SecurityServiceURL_ASP"] + "Teammate?type=" + type + "&value=" + HttpContext.Current.Server.UrlEncode(value) + "&d=" + dateTime;

                WebRequest request = WebRequest.Create(myURL);
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                String responseString;
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                }

                var dynamicObject = Json.Decode(responseString);
                Teammates = JsonConvert.DeserializeObject<List<Teammates_Security_tbl>>(dynamicObject);

                if (Teammates != null)
                {
                    break;
                }

                dateTime = EncryptQueryString(GetDateTime().AddSeconds(1).ToString(), ConfigurationManager.AppSettings["SecurityKey"], ConfigurationManager.AppSettings["SecuritySalt"]);

            }


            try
            {
                if (Teammates.Count() < 1)
                {
                    //not allowed... problem getting the teammate..
                    clearSessionData();
                    SendErrorEmail("SSMCAlert - Error setting Teammate", "SSMCAlert - did not get a return Teammate for : " + value + " at " + DateTime.Now.ToString());
                    //Response.Redirect("NotAllowed.html");
                    //Server.Transfer("NotAllowed.html");
                    return rVal;
                }
            }
            catch (System.Exception ex)
            {
                //not allowed... problem getting the teammate..
                clearSessionData();
                SendErrorEmail("SSMCAlert - Error setting Teammate", "SSMCAlert - did not get a return Teammate for : " + value + " at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                //Response.Redirect("NotAllowed.html");
                //Server.Transfer("NotAllowed.html");
                return rVal;

            }

            //try to stuff it in SQL server session vars
            Guid g = Guid.NewGuid();
            Session["Session_GUID"] = g.ToString();
            Session["Session_CurrentTeammateObject"] = Teammates.FirstOrDefault();
            Session["Session_CurrentTeammateValue"] = value;
            Session["Session_CurrentTeammateType"] = Teammatetype;
            Session["Session_CurrentTeammateFullName"] = Teammates.FirstOrDefault().FullName;
            Session["UserFirstLoad"] = true;

            rVal = true; //we have a Teammate

            //grab the user's windows AD account, then grab their groups and stuff in table so can use for the permission object
            if (Teammates.Count() > 0)
            {
                int uid = 0;

                if (Teammates[0].uid != null)
                {
                    uid = Teammates[0].uid;
                }

                //get a list of groups
                if (Teammates[0].windowsAccount != "")
                {
                    List<string> Groups = new List<string>();
                    Groups = GetGroups(Teammates[0].windowsAccount);
                    //write to ol DB table
                    if (Groups.Count() > 0)
                    {
                        FireStoneDBO dbo = new FireStoneDBO();
                        dbo.Connection = sConnection;
                        dbo.OpenConnection();



                        try
                        {
                            foreach (string groupie in Groups)
                            {
                                //stuff in SQL table for app use
                                dbo.QueryText = "update_UID_Group_Assignment";
                                dbo.QueryType = System.Data.CommandType.StoredProcedure;
                                dbo.Parameters.Add("@UID", SqlDbType.Int).Value = uid;
                                dbo.Parameters.Add("@Group", SqlDbType.VarChar).Value = groupie.ToString().Replace("BFUSA\\", ""); //get rid of BFUSA\
                                dbo.ExecNonQuery();
                                dbo.Parameters.Clear();
                            }
                        }
                        catch (System.Exception ex)
                        {
                            SendErrorEmail("SSMCAlert - Error writing groups for Teammate: " + Teammates[0].windowsAccount.ToString(), "SSMCAlert - did not get a return Teammate for : " + value + " at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                            throw ex;
                        }
                        finally
                        {
                            dbo.CloseConnection();
                        }

                    }

                    //all groups should be in there now for SQL to play with in returning the correct permission for any groups in the permission object

                }

            }

            return rVal;

        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public void setCurrentTeammateIFRAME(string value, TeammateType Teammatetype)
        {

            // kill any existing session vars, as we're starting over with a new login..
            clearSessionDataIFRAME();


            int type = 0;
            switch (Teammatetype)
            {
                case TeammateType.CLOCK_CARD:
                    type = 1;
                    break;
                case TeammateType.WINDOWS_USER:
                    type = 2;
                    break;
                case TeammateType.AD_GROUP:
                    type = 3;
                    break;
                case TeammateType.JOB_CLASS:
                    type = 4;
                    break;
                case TeammateType.DEPARTMENT:
                    type = 5;
                    break;
                case TeammateType.RFID_BADGE:
                    type = 6;
                    break;
                case TeammateType.BUSINESS_AREA:
                    type = 7;
                    break;
                case TeammateType.UID:
                    type = 8;
                    break;
                case TeammateType.JOB_CATEGORY:
                    type = 9;
                    break;
            }

            //What i am doing here is grabbing a date time from SQL and encrypting it. This is for the security of the URL. It will be passed to the webservice which will
            //decrypt it and compare date/time. If greater than a few seconds difference then it will return null
            List<Teammates_Security_tbl> Teammates = new List<Teammates_Security_tbl>();
            string dateTime = EncryptQueryString(GetDateTime().ToString(), ConfigurationManager.AppSettings["SecurityKey"], ConfigurationManager.AppSettings["SecuritySalt"]);

            string myURL = "";

            for (int i = 0; i < 10; i++) //try 10 times to ccount for network issues and or decryption issues
            {
                System.Threading.Thread.Sleep(1000); //this is for testing - can comment out before adding to prod

                myURL = ConfigurationManager.AppSettings["SecurityServiceURL_ASP"] + "Teammate?type=" + type + "&value=" + HttpContext.Current.Server.UrlEncode(value) + "&d=" + dateTime;

                WebRequest request = WebRequest.Create(myURL);
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                String responseString;
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                }

                var dynamicObject = Json.Decode(responseString);
                Teammates = JsonConvert.DeserializeObject<List<Teammates_Security_tbl>>(dynamicObject);

                if (Teammates != null)
                {
                    break;
                }

                dateTime = EncryptQueryString(GetDateTime().AddSeconds(1).ToString(), ConfigurationManager.AppSettings["SecurityKey"], ConfigurationManager.AppSettings["SecuritySalt"]);

            }


            try
            {
                if (Teammates.Count() < 1)
                {
                    //not allowed... problem getting the teammate..
                    clearSessionDataIFRAME();
                    SendErrorEmail("SSMCAlert - Error setting Teammate", "SSMCAlert - did not get a return Teammate for : " + value + " at " + DateTime.Now.ToString());
                    //Response.Redirect("NotAllowed.html");
                    Server.Transfer("NotAllowed.html");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                //not allowed... problem getting the teammate..
                clearSessionDataIFRAME();
                SendErrorEmail("SSMCAlert - Error setting Teammate", "SSMCAlert - did not get a return Teammate for : " + value + " at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                Server.Transfer("NotAllowed.html");
                return;
            }

            //try to stuff it in SQL server session vars
            Guid g = Guid.NewGuid();
            Session["Session_GUID_IFRAME"] = g.ToString();
            Session["Session_CurrentTeammateObject_IFRAME"] = Teammates.FirstOrDefault();
            Session["Session_CurrentTeammateValue_IFRAME"] = value;
            Session["Session_CurrentTeammateType_IFRAME"] = Teammatetype;
            Session["Session_CurrentTeammateFullName_IFRAME"] = Teammates.FirstOrDefault().FullName;

            //grab the user's windows AD account, then grab their groups and stuff in table so can use for the permission object
            if (Teammates.Count() > 0)
            {
                int uid = 0;

                if (Teammates[0].uid != null)
                {
                    uid = Teammates[0].uid;
                }

                //get a list of groups
                if (Teammates[0].windowsAccount != "")
                {
                    List<string> Groups = new List<string>();
                    Groups = GetGroups(Teammates[0].windowsAccount);
                    //write to ol DB table
                    if (Groups.Count() > 0)
                    {

                        FireStoneDBO dbo = new FireStoneDBO();
                        dbo.Connection = sConnection;
                        dbo.OpenConnection();

                        try
                        {
                            foreach (string groupie in Groups)
                            {
                                //stuff in SQL table for app use
                                dbo.QueryText = "update_UID_Group_Assignment";
                                dbo.QueryType = System.Data.CommandType.StoredProcedure;
                                dbo.Parameters.Add("@UID", SqlDbType.Int).Value = uid;
                                dbo.Parameters.Add("@Group", SqlDbType.VarChar).Value = groupie.ToString().Replace("BFUSA\\", ""); //get rid of BFUSA\
                                dbo.ExecNonQuery();
                                dbo.Parameters.Clear();
                            }
                        }
                        catch (System.Exception ex)
                        {
                            SendErrorEmail("SSMCAlert - Error writing groups for Teammate: " + Teammates[0].windowsAccount.ToString(), "SSMCAlert - did not get a return Teammate for : " + value + " at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                            throw ex;
                        }
                        finally
                        {
                            dbo.CloseConnection();
                        }

                    }

                    //all groups should be in there now for SQL to play with in returning the correct permission for any groups in the permission object

                }

            }

        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public bool setCurrentPermissions(string value, PermissionType PermissionType, int appId)
        {

            bool rVal = false;

            //need to have a check in place here. If developer does not have the web.config values set, do not continue method. Have JS display message on screen
            //advising them to set this up
            if (isConfiguredToUseService() == false)
            {
                return rVal;
            }

            //begin routine
            int type = 0;
            switch (PermissionType)
            {
                case PermissionType.CLOCK_CARD:
                    type = 1;
                    break;
                case PermissionType.WINDOWS_USER:
                    type = 2;
                    break;
                case PermissionType.AD_GROUP:
                    type = 3;
                    break;
                case PermissionType.JOB_CLASS:
                    type = 4;
                    break;
                case PermissionType.DEPARTMENT:
                    type = 5;
                    break;
                case PermissionType.RFID_BADGE:
                    type = 6;
                    break;
                case PermissionType.BUSINESS_AREA:
                    type = 7;
                    break;
                case PermissionType.UID:
                    type = 8;
                    break;
                case PermissionType.JOB_CATEGORY:
                    type = 9;
                    break;

            }


            string myURL = ConfigurationManager.AppSettings["SecurityServiceURL_ASP"] + "PermissionObject?type=" + type + "&value=" + HttpContext.Current.Server.UrlEncode(value) + "&appId=" + appId;

            List<AllowedPermission> AllowedPermissions = new List<AllowedPermission>();

            for (int i = 0; i < 10; i++)
            {
                WebRequest request = WebRequest.Create(myURL);
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                String responseString;
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                }

                var dynamicObject = Json.Decode(responseString);

                AllowedPermissions = JsonConvert.DeserializeObject<List<AllowedPermission>>(dynamicObject);

                if (AllowedPermissions != null)
                {
                    break;
                }

            }

            //write the user's highest permission level to be used for default and to control impersonation
            string myHighestPermission = "";
            if (AllowedPermissions.Count() > 0)
            {
                rVal = true;

                try
                {
                    myHighestPermission = AllowedPermissions[0].Permission.ToString();
                    Session["Session_HighestPermissionObject"] = myHighestPermission;
                }
                catch (Exception ex)
                {
                    //send email
                    SendErrorEmail("SSMCAlert - error looking for teammate's highest permission: " + value, "error looking for teammate's highest permission " + value + " at " + DateTime.Now.ToString());
                }

            }
            else
            {

                //send email
                SendErrorEmail("SSMCAlert - no permisions found for Teammate: " + value, "SSMCAlert - no permisions found for Teammate: " + value + " at " + DateTime.Now.ToString());
                return rVal;
            }

            //write permission object to session
            Session["Session_CurrentPermissionObject"] = AllowedPermissions;
            //write permission object for current user for the DDL
            Session["Session_CurrentPermissionObject_CU"] = AllowedPermissions;

            return rVal;

        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public void setCurrentPermissionsIFRAME(string value, PermissionType PermissionType, int appId)
        {

            //need to have a check in place here. If developer does not have the web.config values set, do not continue method. Have JS display message on screen
            //advising them to set this up
            if (isConfiguredToUseService() == false)
            {
                return;
            }

            //begin routine
            int type = 0;
            switch (PermissionType)
            {
                case PermissionType.CLOCK_CARD:
                    type = 1;
                    break;
                case PermissionType.WINDOWS_USER:
                    type = 2;
                    break;
                case PermissionType.AD_GROUP:
                    type = 3;
                    break;
                case PermissionType.JOB_CLASS:
                    type = 4;
                    break;
                case PermissionType.DEPARTMENT:
                    type = 5;
                    break;
                case PermissionType.RFID_BADGE:
                    type = 6;
                    break;
                case PermissionType.BUSINESS_AREA:
                    type = 7;
                    break;
                case PermissionType.UID:
                    type = 8;
                    break;
                case PermissionType.JOB_CATEGORY:
                    type = 9;
                    break;

            }


            string myURL = ConfigurationManager.AppSettings["SecurityServiceURL_ASP"] + "PermissionObject?type=" + type + "&value=" + HttpContext.Current.Server.UrlEncode(value) + "&appId=" + appId;

            List<AllowedPermission> AllowedPermissions = new List<AllowedPermission>();

            for (int i = 0; i < 10; i++)
            {
                WebRequest request = WebRequest.Create(myURL);
                request.Method = "GET";
                WebResponse response = request.GetResponse();

                String responseString;
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseString = reader.ReadToEnd();
                }

                var dynamicObject = Json.Decode(responseString);

                AllowedPermissions = JsonConvert.DeserializeObject<List<AllowedPermission>>(dynamicObject);

                if (AllowedPermissions != null)
                {
                    break;
                }

            }

            //write the user's highest permission level to be used for default and to control impersonation
            string myHighestPermission = "";
            if (AllowedPermissions.Count() > 0)
            {

                try
                {
                    myHighestPermission = AllowedPermissions[0].Permission.ToString();
                    Session["Session_HighestPermissionObject_IFRAME"] = myHighestPermission;
                }
                catch (Exception ex)
                {
                    //send email
                    SendErrorEmail("SSMCAlert - IFRAME error looking for teammate's highest permission: " + value, "error looking for teammate's highest permission " + value + " at " + DateTime.Now.ToString());
                }

            }
            else
            {
                //send email
                SendErrorEmail("SSMCAlert - IFRAME no permisions found for Teammate: " + value, "SSMCAlert - no permisions found for Teammate: " + value + " at " + DateTime.Now.ToString());
            }

            //write permission object to session
            Session["Session_CurrentPermissionObject_IFRAME"] = AllowedPermissions;
            //write permission object for current user for the DDL
            Session["Session_CurrentPermissionObject_CU_IFRAME"] = AllowedPermissions;

        }




        //this is executed when a user changes the selection of the ddl
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public void setCurrentDropDownSelection(string value)
        {
            //grab the current impersonated permission for use with ***endImpersonation
            try
            {
                Session["Session_CurrentDropDownSelection"] = value;
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error setCurrentDropDownSelection: " + value, "SSMCAlert - setCurrentDropDownSelection: " + value + " at " + DateTime.Now.ToString() + "for teammate " + getCurrentTeammateFullName() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            //so if selected value <> the highest permission then begin impersonation, otherwise, end it..
            if (value != Session["Session_HighestPermissionObject"].ToString())
            {
                //need user, appId and impersonated permission
                beginImpersonation(getCurrentTeammate().FullName, Int32.Parse(ConfigurationManager.AppSettings["appId"]), value);
            }
            else
            {
                //user, appId, impersonated permission
                endImpersonation(getCurrentTeammate().FullName, Int32.Parse(ConfigurationManager.AppSettings["appId"]), value);
            }

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public Teammates_Security_tbl getCurrentTeammate() //returns current teammate
        {
            Teammates_Security_tbl Teammate = new Teammates_Security_tbl();
            //look at Session
            try
            {
                Teammate = (Teammates_Security_tbl)Session["Session_CurrentTeammateObject"];
            }
            catch (System.Exception ex)
            {   //send email
                SendErrorEmail("SSMCAlert - error getCurrentTeammate: ", "SSMCAlert - getCurrentTeammate: at " + DateTime.Now.ToString() + "for teammate" + getCurrentTeammateFullName() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return Teammate;

        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public Teammates_Security_tbl getCurrentTeammateIFRAME() //returns current teammate
        {
            Teammates_Security_tbl Teammate = new Teammates_Security_tbl();
            //look at Session
            try
            {
                Teammate = (Teammates_Security_tbl)Session["Session_CurrentTeammateObject_IFRAME"];
            }
            catch (System.Exception ex)
            {   //send email
                SendErrorEmail("SSMCAlert - IFRAME error getCurrentTeammate: ", "SSMCAlert - getCurrentTeammate: at " + DateTime.Now.ToString() + "for teammate" + getCurrentTeammateFullName() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return Teammate;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public List<AllowedPermission> getCurrentPermissions() //returns current allowed permission
        {

            List<AllowedPermission> Permissions = new List<AllowedPermission>();

            try
            {
                Permissions = (List<AllowedPermission>)Session["Session_CurrentPermissionObject"];
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getCurrentPermissions: ", "SSMCAlert - getCurrentPermissions: at " + DateTime.Now.ToString() + "for teammate" + getCurrentTeammateFullName() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return Permissions;

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public List<AllowedPermission> getCurrentPermissionsIFRAME() //returns current allowed permission
        {

            List<AllowedPermission> Permissions = new List<AllowedPermission>();

            try
            {
                Permissions = (List<AllowedPermission>)Session["Session_CurrentPermissionObjectIFRAME"];
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - IFRAME error getCurrentPermissions: ", "SSMCAlert - getCurrentPermissions: at " + DateTime.Now.ToString() + "for teammate" + getCurrentTeammateFullName() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return Permissions;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public List<AllowedPermission> getCurrentPermissionsForDDL()
        {

            List<AllowedPermission> Permissions = new List<AllowedPermission>();

            //This is what loads the security drop down.

            try
            {
                Permissions = (List<AllowedPermission>)Session["Session_CurrentPermissionObject_CU"];
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getCurrentPermissionsForDDL: ", "SSMCAlert - getCurrentPermissionsForDDL: at " + DateTime.Now.ToString() + "for teammate " + getCurrentTeammateFullName() + " ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return Permissions;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public string getHighestPermission() //returns user's highest permission - default
        {

            string HP = "";

            try
            {
                HP = Session["Session_HighestPermissionObject"].ToString();

            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getHighestPermission: ", "SSMCAlert - getHighestPermission: at " + DateTime.Now.ToString() + "for teammate " + getCurrentTeammateFullName() + " ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return HP;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public string getCurrentDropDownSelection()
        {

            string currentSelection = "";

            if (Session["Session_HighestPermissionObject"] == null)
            {
                Session["Session_HighestPermissionObject"] = "";
            }

            //look at Session
            try
            {
                currentSelection = Session["Session_CurrentDropDownSelection"].ToString();
                if (currentSelection == "")
                {
                    currentSelection = Session["Session_HighestPermissionObject"].ToString();
                }
            }
            catch (System.Exception ex)
            {
                currentSelection = Session["Session_HighestPermissionObject"].ToString();
                //send email
                SendErrorEmail("SSMCAlert - error getCurrentDropDownSelection: ", "SSMCAlert - getCurrentDropDownSelection: at " + DateTime.Now.ToString() + "for teammate " + getCurrentTeammateFullName() + " ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return currentSelection;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public List<Teammates_Security_tbl> getOtherTeammate(string value, TeammateType Teammatetype) //get a list of Teammate info
        {

            int type = 0;
            switch (Teammatetype)
            {
                case TeammateType.CLOCK_CARD:
                    type = 1;
                    break;
                case TeammateType.WINDOWS_USER:
                    type = 2;
                    break;
                case TeammateType.AD_GROUP:
                    type = 3;
                    break;
                case TeammateType.JOB_CLASS:
                    type = 4;
                    break;
                case TeammateType.DEPARTMENT:
                    type = 5;
                    break;
                case TeammateType.RFID_BADGE:
                    type = 6;
                    break;
                case TeammateType.BUSINESS_AREA:
                    type = 7;
                    break;
                case TeammateType.UID:
                    type = 8;
                    break;
                case TeammateType.JOB_CATEGORY:
                    type = 9;
                    break;
            }

            List<Teammates_Security_tbl> Teammates = new List<Teammates_Security_tbl>();

            string dateTime = EncryptQueryString(GetDateTime().ToString(), ConfigurationManager.AppSettings["SecurityKey"], ConfigurationManager.AppSettings["SecuritySalt"]); //get currentDateTime from SQL Server using extra security here so people can't grab uid etc..
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    System.Threading.Thread.Sleep(1000);

                    string myURL = ConfigurationManager.AppSettings["SecurityServiceURL_ASP"] + "Teammate?type=" + type + "&value=" + HttpContext.Current.Server.UrlEncode(value) + "&d=" + dateTime;

                    WebRequest request = WebRequest.Create(myURL);
                    request.Method = "GET";
                    WebResponse response = request.GetResponse();

                    String responseString;
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                    }

                    var dynamicObject = Json.Decode(responseString);
                    Teammates = JsonConvert.DeserializeObject<List<Teammates_Security_tbl>>(dynamicObject);

                    if (Teammates != null)
                    {
                        if (Teammates.Count() > 0)
                        {
                            break;
                        }
                    }

                    dateTime = EncryptQueryString(GetDateTime().ToString(), ConfigurationManager.AppSettings["SecurityKey"], ConfigurationManager.AppSettings["SecuritySalt"]); //adding seonds here to help with possible issue with dycrpting on the webservice side

                }
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getOtherTeammate: ", "SSMCAlert - getOtherTeammate: at " + DateTime.Now.ToString() + "for teammate" + value + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return Teammates;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        //public void beginImpersonation(string user, PermissionType pt, int appId, string ImpersonatedPermission)
        public void beginImpersonation(string user, int appId, string ImpersonatedPermission)
        {

            //write to session 
            Session["Session_IsImpersonating"] = true;

            //grab current permission session for CurrentUser and store them for use later after impersonation ends:
            //Session["Session_CurrentPermissionObject_CU"] = Session["Session_CurrentPermissionObject"];

            //set the CurrentPermissionObject to the new impersonated levels
            Session["Session_CurrentPermissionObject"] = getImpersonatedPermissions(ImpersonatedPermission);

            //Log the event
            string myURL = ConfigurationManager.AppSettings["SecurityServiceURL_ASP"] + "BeginImpersonation?user=" + user + "&permission=" + HttpContext.Current.Server.UrlEncode(ImpersonatedPermission) + "&appId=" + appId;
            WebRequest request = WebRequest.Create(myURL);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public void endImpersonation(string user, int appId, string ImpersonatedPermission)
        {
            //write to session 
            Session["Session_IsImpersonating"] = false;

            //put CurrentPermissionObject back to original (IE highest permission level and descendants
            Session["Session_CurrentPermissionObject"] = Session["Session_CurrentPermissionObject_CU"];

            //log event
            string myURL = ConfigurationManager.AppSettings["SecurityServiceURL_ASP"] + "EndImpersonation?user=" + user + "&permission=" + HttpContext.Current.Server.UrlEncode(ImpersonatedPermission) + "&appId=" + appId;
            WebRequest request = WebRequest.Create(myURL);
            request.Method = "GET";
            WebResponse response = request.GetResponse();

        }


        //this is to get the impersonated permissions
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public List<AllowedPermission> getImpersonatedPermissions(string impersonatedPermission)
        {

            List<AllowedPermission> Permissions = (List<AllowedPermission>)Session["Session_CurrentPermissionObject_CU"]; //This is correct 11/10/2019
            List<AllowedPermission> ImpersonatedPermissions = new List<AllowedPermission>();
            List<AllowedPermission> ImpersonatedPermissionsTemp = new List<AllowedPermission>();

            try
            {
                foreach (var item in Permissions)
                {
                    if (item.Permission == impersonatedPermission || item.ParentPermissionName == impersonatedPermission || item.AssignedByParentPermissionName == impersonatedPermission)
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }


                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();
                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();
                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();
                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();
                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();
                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();
                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();
                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();
                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();
                foreach (var i in Permissions)
                {
                    foreach (var item in ImpersonatedPermissions)
                    {
                        if (!ImpersonatedPermissions.Contains(i))
                        {
                            if (i.ParentPermissionName == item.Permission && i.Permission != impersonatedPermission)
                            {
                                ImpersonatedPermissionsTemp.Add(i);
                            }
                        }
                    }
                }
                foreach (var item in ImpersonatedPermissionsTemp)
                {
                    if (!ImpersonatedPermissions.Contains(item))
                    {
                        ImpersonatedPermissions.Add(item);
                    }
                }
                ImpersonatedPermissionsTemp.Clear();



            }
            catch (Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getOtherTeammate: ", "SSMCAlert - getOtherTeammate: at " + DateTime.Now.ToString() + "for teammate " + impersonatedPermission + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return ImpersonatedPermissions.OrderBy(p => p.Order).ToList();

        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public void clearSessionData() //Remove all session data
        {
            //remove all session and cookie data...
            Session["Session_CurrentPermissionObject_CU"] = null;
            Session["Session_CurrentPermissionObject"] = null;
            Session["Session_CurrentTeammateObject"] = null;
            Session["Session_HighestPermissionObject"] = null;
            Session["Session_CurrentDropDownSelection"] = null;
            Session["Session_IsImpersonating"] = null;
            Session["Session_CurrentTeammateValue"] = null;
            Session["Session_CurrentTeammateType"] = null;
            Session["Session_GUID"] = null;
            Session["Session_CurrentTeammateFullName"] = null;
            Session["UserFirstLoad"] = null;

            Session.Remove("Session_CurrentPermissionObject_CU");
            Session.Remove("Session_CurrentPermissionObject");
            Session.Remove("Session_CurrentTeammateObject");
            Session.Remove("Session_HighestPermissionObject");
            Session.Remove("Session_CurrentDropDownSelection");
            Session.Remove("Session_IsImpersonating");
            Session.Remove("Session_CurrentTeammateValue");
            Session.Remove("Session_CurrentTeammateType");
            Session.Remove("Session_GUID");
            Session.Remove("Session_CurrentTeammateFullName");
            Session.Remove("Session_CurrentPermissionObject_CU");
            Session.Remove("UserFirstLoad");


        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public void clearSessionDataIFRAME() //Remove all session data
        {
            //remove all session and cookie data...
            Session.Remove("Session_CurrentTeammateObject_IFRAME");
            Session.Remove("Session_GUID_IFRAME");
            Session.Remove("Session_CurrentTeammateValue_IFRAME");
            Session.Remove("Session_CurrentTeammateType_IFRAME");
            Session.Remove("Session_CurrentTeammateFullName_IFRAME");
            Session.Remove("Session_HighestPermissionObject_IFRAME");
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public string getCurrentTeammateValue()
        {
            try
            {
                return HttpContext.Current.Server.UrlEncode(Session["Session_CurrentTeammateValue"].ToString());
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getCurrentTeammateValue: ", "SSMCAlert - getCurrentTeammateValue: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                return "";
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public string getCurrentTeammateFullName() //fullname
        {
            try
            {
                return Session["Session_CurrentTeammateFullName"].ToString();
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getCurrentTeammateValue: ", "SSMCAlert - getCurrentTeammateValue: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                return "";
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public string getCurrentTeammateFullNameIFRAME() //fullname
        {
            try
            {
                return Session["Session_CurrentTeammateFullName_IFRAME"].ToString();
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getCurrentTeammateValueIFRAME: ", "SSMCAlert - getCurrentTeammateValue: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                return "";
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public string getCurrentTeammateType() //String of Teammate type.. Windows,RFID,ClockCard etc..
        {
            try
            {
                return HttpContext.Current.Server.UrlEncode(Session["Session_CurrentTeammateType"].ToString());
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getCurrentTeammateType: ", "SSMCAlert - getCurrentTeammateType: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                return "";
            }
        }


        //Is the web.config configured to use the service?
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public bool isConfiguredToUseService()
        {

            bool retVal = false;

            try
            {
                string appId = ConfigurationManager.AppSettings["appId"];
                string SecurityServiceManagementConsole_URL = ConfigurationManager.AppSettings["SecurityServiceManagementConsole_URL"];
                string SecurityServiceURL_JS = ConfigurationManager.AppSettings["SecurityServiceURL_JS"];
                string SecurityServiceURL_ASP = ConfigurationManager.AppSettings["SecurityServiceURL_ASP"];

                //ensure the app has appropriate settings configured in the web.config or else let developer know it is not wired up
                if (appId != "" && SecurityServiceManagementConsole_URL != "" && SecurityServiceURL_JS != "" && SecurityServiceURL_ASP != "")
                {
                    retVal = true;
                }
            }
            catch (System.Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error isConfiguredToUseService: ", "SSMCAlert - isConfiguredToUseService: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
            }

            return retVal;

        }


        //get a windows user group list  for current windows auth account    
        public static List<string> GetCurrentUserGroups()
        {
            var groups = new List<string>();
            foreach (System.Security.Principal.IdentityReference group in HttpContext.Current.Request.LogonUserIdentity.Groups)
            {
                try
                {
                    groups.Add(group.Translate(typeof(System.Security.Principal.NTAccount)).ToString());
                }
                catch (Exception ex)
                {
                    //send email
                    SecurityService s = new SecurityService();
                    s.SendErrorEmail("SSMCAlert - error GetCurrentUserGroups: ", "SSMCAlert - GetCurrentUserGroups: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                    continue;

                }
            }

            return groups;

        }


        //get groups for who ever you want to pass in....
        public static List<string> GetGroups(string userName) //Get Windows Groups for user passed in
        {
            List<string> result = new List<string>();
            userName = userName.ToUpper();
            userName = userName.Replace("BFUSA\\", string.Empty).ToUpper(); //remove domain if there...

            try
            {
                WindowsIdentity wi = new WindowsIdentity(userName);



                foreach (IdentityReference group in wi.Groups)
                {
                    try
                    {
                        result.Add(group.Translate(typeof(NTAccount)).ToString());
                    }
                    catch (Exception ex)
                    {
                        //send email
                        SecurityService s = new SecurityService();
                        s.SendErrorEmail("SSMCAlert - error GetGroups: ", "SSMCAlert - GetGroups: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                    }
                }


            }
            catch (Exception)
            {
            }


            result.Sort();
            return result;

        }


        //For Shawn
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public List<string> getPermissionLevelsForApplication(int appId) //Return active permission levels for a given application (per Shawn)
        {
            List<string> retVals = new List<string>();
            FireStoneDBO dbo = new FireStoneDBO();
            dbo.Connection = sConnection;
            dbo.QueryText = "getPermissionLevelsForApplication";
            dbo.QueryType = System.Data.CommandType.StoredProcedure;
            dbo.Parameters.Add("@appId", SqlDbType.Int).Value = appId;
            dbo.OpenConnection();
            SqlDataReader dr = dbo.ExecReader();

            try
            {
                while (dr.Read())
                {
                    retVals.Add(dr["PermissionName"].ToString());
                }
            }
            catch (Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error getPermissionLevelsForApplication: ", "SSMCAlert - getPermissionLevelsForApplication() for applicationID: " + appId.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                throw ex;
            }
            finally
            {
                dbo.CloseConnection();
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }

            return retVals;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public DateTime GetDateTime()
        {
            FireStoneDBO dbo = new FireStoneDBO();
            dbo.Connection = sConnection;
            dbo.QueryText = "getDateTime";
            dbo.QueryType = System.Data.CommandType.StoredProcedure;
            dbo.OpenConnection();
            SqlDataReader dr = dbo.ExecReader();

            DateTime returnDateTime = DateTime.Now;
            try
            {
                while (dr.Read())
                {

                    returnDateTime = DateTime.Parse(dr["currentDateTime"].ToString());

                }
            }
            catch (Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error GetDateTime: ", "SSMCAlert - GetDateTime: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                throw ex;
            }
            finally
            {
                dbo.CloseConnection();
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }
            }

            return returnDateTime;

        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public string GetDateTimeStringURLforIFRAME()
        {
            string encrpyDateTime = "";
            try
            {
                string dateTime = GetDateTime().ToString();
                encrpyDateTime = EncryptQueryString(dateTime, ConfigurationManager.AppSettings["SecurityKey"], ConfigurationManager.AppSettings["SecuritySalt"]); //get currentDateTime from SQL Server using extra security here so people can't grab uid etc..
            }
            catch (Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error GetDateTimeStringURLforIFRAME: ", "SSMCAlert - GetDateTimeStringURLforIFRAME: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                throw;
            }

            return encrpyDateTime;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, XmlSerializeString = true)]
        public void SendErrorEmail(string subject, string body)
        {
            try
            {
                //send email
                Email e = new Email();
                e.From = ConfigurationManager.AppSettings["EmailFrom"].ToString();
                e.To = ConfigurationManager.AppSettings["EmailTo"].ToString();
                e.Subject = subject;
                e.Body = body + " *** Application ID = " + ConfigurationManager.AppSettings["appId"];
                e.Send();
            }
            catch (Exception)
            {
                //dont do squat
            }

        }


        public string EncryptQueryString(string inputText, string key, string salt)
        {
            byte[] plainText = Encoding.UTF8.GetBytes(inputText);

            try
            {
                using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
                {
                    PasswordDeriveBytes secretKey = new PasswordDeriveBytes(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(salt));
                    using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainText, 0, plainText.Length);
                                cryptoStream.FlushFinalBlock();
                                string base64 = Convert.ToBase64String(memoryStream.ToArray());

                                // Generate a string that won't get screwed up when passed as a query string.
                                string urlEncoded = HttpUtility.UrlEncode(base64);
                                return urlEncoded;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error EncryptQueryString: ", "SSMCAlert - EncryptQueryString: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString());
                throw ex;
            }

        }


        public string DecryptQueryString(string inputText, string key, string salt)
        {
            try
            {
                byte[] encryptedData = Convert.FromBase64String(inputText);
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(salt));

                using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
                {
                    using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainText = new byte[encryptedData.Length];
                                cryptoStream.Read(plainText, 0, plainText.Length);
                                string utf8 = Encoding.UTF8.GetString(plainText);
                                return utf8;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //send email
                SendErrorEmail("SSMCAlert - error DecryptQueryString: ", "SSMCAlert - DecryptQueryString: at " + DateTime.Now.ToString() + "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + ex.ToString() + "            " + inputText);
                throw;
            }

        }

    }


    [Serializable()]
    public class AllowedPermission
    {
        public string Permission;
        public string permissionlevel;
        public string AssignedByParentPermissionName;
        public string ParentPermissionName;
        public string CanManageLowerPermissions;
        public string Order;
    }


    [Serializable()]
    public class Teammates_Security_tbl
    {
        public int Id { get; set; }
        public int uid { get; set; }
        public string windowsAccount { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string clockNumber { get; set; }
        public string badgerfid { get; set; }
        public string email { get; set; }
        public string shift { get; set; }
        public int? SAPJobID { get; set; }
        public int? JobID { get; set; }
        public int? DepartmentID { get; set; }
        public int? ManagerID { get; set; }
        public DateTime? SeniorityDate { get; set; }
        public string SeniorityDateStr { get; set; }
        public int? TeammateStatusID { get; set; }
        public bool? Active { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? EmpTypeID { get; set; }
        public string Birthdate { get; set; }
        public string FullName { get; set; }
        public string Manager { get; set; }
        public string ManagerEmail { get; set; }
        public bool? IsManager { get; set; }
        public string DeptCode { get; set; }
        public string TIPSJobCode { get; set; }
        public int? JobID_Duplicate { get; set; }
        public int? sap_JobID { get; set; }
        public string SAPJobCode { get; set; }
        public string SAPJobDescript { get; set; }
        public string BrowseValue { get; set; }
        public string tipsJobCatCode { get; set; }
    }



    public class Email : IDisposable
    {
        private string to = string.Empty;
        private string from = string.Empty;
        private string subject = string.Empty;
        private string body = string.Empty;

        /// <summary>
        /// End each email with a semi-colon
        /// </summary>
        public string To
        {
            set
            {
                to = value;
            }
        }

        /// <summary>
        /// End With Semi-Colon
        /// </summary>
        public string From
        {
            set
            {
                from = value;
            }
        }

        /// <summary>
        /// Subject of the email.
        /// </summary>
        public string Subject
        {
            set { subject = value; }
        }

        /// <summary>
        /// Set the body of the Email, plan text or HTML
        /// </summary>
        public string Body
        {
            set { body = value; }
        }

        /// <summary>
        /// Just for the tag characters, if they exist then say it contains HTML
        /// </summary>
        /// <returns>bool/returns>
        private bool IsHtml()
        {
            return body.Contains("<") && body.Contains(">");
        }

        public bool Send()
        {
            if (body.StartsWith("iisexpress.exe ")) return true;

            try
            {
                if (string.IsNullOrEmpty(subject)) throw new NullReferenceException("Email cannot have a blank message.");
                if (string.IsNullOrEmpty(to)) throw new NullReferenceException("Email cannot have a blank to.");
                if (string.IsNullOrEmpty(from)) throw new NullReferenceException("Email cannot have a blank from address.");
                if (string.IsNullOrEmpty(body)) throw new NullReferenceException("Email cannot have a blank body.");

                MailMessage message = new MailMessage();
                SmtpClient smtpServer = new SmtpClient(ConfigurationManager.AppSettings["EmailClient"].ToString());
                message.Subject = subject;
                message.Body = body;
                message.From = new MailAddress(from.Replace(";", ""));
                string[] ToDelimitd = to.Split(';');
                for (int i = 0; i <= ToDelimitd.Length - 1; i++)
                    if (ToDelimitd[i].Length > 0)
                        message.To.Add(ToDelimitd[i]);
                //send message
                smtpServer.Send(message);
                //release memeory                
                smtpServer.Dispose();
                message.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                //want to bubble the exception up to the email trace lister

            }

        }

        public void Dispose()
        {
            //release string variables from memeory
            to = null;
            from = null;
            body = null;
            subject = null;

        }
    }





    public static class StringUtil
    {
        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        public static string Crypt(this string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        public static string Decrypt(this string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }
    }
}
