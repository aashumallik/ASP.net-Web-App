using System;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using FirestoneWebSupportLibrary;
using System.Configuration;


namespace FirestoneWebTemplate
{
    public partial class EmailPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // JH 3/9/2020 Security Begin ///////////////////////////////////////////////////////////////////////////

            SecurityService s = new SecurityService();
            if (s.isConfiguredToUseService() == true)
            {
                //This will flow through code one time per session
                if (Session["MySessBool"] == null)
                {
                    Session["MySessBool"] = true;
                }

                if ((bool)Session["MySessBool"] == true)
                {
                    //Load the current user into the system along with their permissions:
                    string myValue = HttpContext.Current.Request.LogonUserIdentity.Name; //Windows Auth
                                                                                         //Check if web.config is setup for service.. If not, display message on screen, otherwise
                                                                                         //set the current Teammate and Permissions...
                    //set the current Teammate... If returned False, shoo away
                    if (!s.setCurrentTeammate(myValue, SecurityService.TeammateType.WINDOWS_USER)) //Default To Windows Auth
                    {
                        //redirect
                        Server.Transfer("NotAllowed.html");
                    }
                    //set the current permissionObject...If returned False, shoo away
                    if (!s.setCurrentPermissions(myValue, SecurityService.PermissionType.WINDOWS_USER, Int32.Parse(ConfigurationManager.AppSettings["appId"])))
                    {
                        //redirect
                        Server.Transfer("NotAllowed.html");
                    }
                    //if no permissions, shoo away
                    if (s.getCurrentPermissions().Count() == 0)
                    {
                        Server.Transfer("NotAllowed.html");
                    }
                }

                //bump session
                Session["MySessBool"] = false;

                // JH 3/9/2020 Security End ///////////////////////////////////////////////////////////////////////////

            }




        }



    }


}
