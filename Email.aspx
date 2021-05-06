<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Email.aspx.cs" Inherits="FirestoneWebTemplate.EmailPage" %>
<%@ Register Assembly="FirestoneWebSupportLibrary" Namespace="FirestoneWebSupportLibrary.Controls" TagPrefix="cc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10" />
    <link href="ReservedFiles/Styles/ui-lightness/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <link href="ReservedFiles/Styles/standardStyle.css" rel="stylesheet" />
    <link href="Styles/FirestoneWebTemplate.css" rel="stylesheet" />    
</head>
<body>    
    <form id="form1" runat="server">
    <div ng-app="">
        <asp:ScriptManager ID="ScriptManager" runat="server">       
            <Services>                
                <asp:ServiceReference Path="Services/WebTemplateService.asmx" />
            </Services>                 
        </asp:ScriptManager>          
        <cc1:Page ID="DefaultPage" runat="server" NavigationStyle="Left">            
            <Header ID="Header" Title="WebTemplate" SystemName="Access Enterprise" ApplicationName="Store Room Stock Request" Location="Location" runat="server">
                <FileMenu runat="server">
                    <Items>
                        <cc1:MenuItem Text="Security" OnClick="" Selected="true"></cc1:MenuItem>           
                        <cc1:MenuItem Text="Configuration" OnClick="" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="About" OnClick="" Selected="false">
                            <Items>
                                <cc1:MenuItem Text="About 1" OnClick="" Selected="false"></cc1:MenuItem>
                                <cc1:MenuItem Text="About 2" OnClick="" Selected="false"></cc1:MenuItem>
                                <cc1:MenuItem Text="About 3" OnClick="" Selected="false"></cc1:MenuItem>
                            </Items>
                        </cc1:MenuItem>
                    </Items>                
                </FileMenu>
                <TabMenu runat="server">
                    <Items>
                        <cc1:MenuItem Text="Home" OnClick="setActiveScreen('Home');" Selected="false" NavigationURL="Default.aspx"></cc1:MenuItem>           
                        <cc1:MenuItem Text="Email" OnClick="setActiveScreen('Email')" Selected="true"></cc1:MenuItem>
                     <cc1:MenuItem Text="Request" OnClick="setActiveScreen('Request')" Selected="false" NavigationURL="Form_request.aspx"></cc1:MenuItem>
                        <cc1:MenuItem Text="Approval" OnClick="setActiveScreen('Approval')" Selected="false" NavigationURL="Formappoval.aspx"></cc1:MenuItem>
                      
                    </Items>
                </TabMenu>
            </Header>
            <Navigation>
                <%--JH 3/9/2020 - For Managing Security--%>
                <div><a id="SSMC_Assignment" href="" class="SSMCClass" style="display:none;">Manage Security</a></div>

          
            </Navigation>                        
            <Contents>   
                <%--JH 3/9/2020 - This will be displayed if Security has not been setup in web.config for the appID--%>
                <div id="isSecurityServiceConfigured_DIV" style="display:none;"><i>Reminder: This application's web.config needs to have the Security Service information added in order to utilize the Security Service properly.</i></div> 
                <br />
                <br />
              <div class="divEmailTable" ng-controller="EmailController">
                <div class="divEmailDetail">
                    <span><label class="emailLabel" translate>To:</label></span>
                    <span style="margin-left:15px"><input ng-model="email.To" class="emailInput" style="width:500px;"/></span>
                </div>
                <div class="divEmailDetail">
                    <span><label class="emailLabel" translate>From:</label></span>
                    <span style="margin-left:15px"><input ng-model="email.From" class="emailInput" style="width:500px;"/></span>
                </div>
                <div class="divEmailDetail">
                    <span><label class=" emailLabel" translate>Subject:</label></span>
                    <span style="margin-left:15px"><input ng-model="email.Subject" class="emailInput" style="width:500px;"/></span>
                </div>
                <div class="divEmailDetail">
                    <span><label class="emailLabel" translate>Body:</label></span>
                    <span><textarea ng-model="email.Body" class="emailInput" cols="500" rows="20" style="width:500px;"></textarea></span>
                </div>
                <div class="divEmailDetail">
                    <span><label class="emailLabel" translate>Is HTML:</label></span>
                    <span><input type="checkbox" ng-model="email.IsHtml" class="emailInput" /></span>
                </div>
                <div class="divEmailDetail">
                    <span></span>
                    <span class="saveBtnSpan"><a class="fsButtons saveBtn" ng-click="sendEmail();" translate>Send</a></span>
                </div>
            </div>

            </Contents>
        </cc1:Page>
    </div>    
    </form>
    </body>
    <script src="ReservedFiles/Scripts/jquery-1.10.2.js"></script>
    <script src="ReservedFiles/Scripts/jquery-ui-1.10.4.custom.js"></script>
    <script src="ReservedFiles/Scripts/jquery.json-2.4.js"></script>
    <script src="ReservedFiles/Scripts/json2.js"></script>
    <script src="ReservedFiles/Scripts/json2.min.js"></script>
    <script src="ReservedFiles/Scripts/angular.js" type="text/javascript"></script>
    <script src="ReservedFiles/Scripts/directives/helperMessages.js" type="text/javascript"></script>  
    <script src="ReservedFiles/Scripts/fsAngularApp.js" type="text/javascript"></script>  
    <script src="Scripts/SampleSpecsCtrl.js"></script>
    <script src="Scripts/SecurityService_JS.js"></script>
    <script src="Scripts/EmailController.js"></script>
    <script  type="text/javascript">
        $(document).ready(function () {

            //3/9/2020 - JH Security Begin///////////////////////////////////////////////////////////////////////////////
            //check and see if application is configured, if so do stuff
            if (isConfiguredToUseService() == true) {
                //set the initial items / properties
                LoadTemplateSecurityDDL();
                //set the src for the PA Screen iFrame don't need this
                setIFrameData(  <%=ConfigurationManager.AppSettings["SecurityServiceManagementConsole_URL"] %> + "",
                                <%=ConfigurationManager.AppSettings["appId"] %> + "");
                //disable all selections except hightest permission for default page
                //$("#DefaultPage_Header_ddlSecurity").click(function () {
                //    $("#DefaultPage_Header_ddlSecurity option").not(':first-child').each(function (index) {
                //        $(this).prop('disabled', true);
                //    });
                //});
            }
            else
            {
                //not configured,  //Display reminder setup message
                document.getElementById("isSecurityServiceConfigured_DIV").style.display = "inline";
             };

           //This will determine wheter or not to show the Manage Security Menu Item on page load
           var getCurrentlySetPermissions = getCurrentPermissions(); //get the impersonated permission if one
           var CanManageLowerPermissionsFlag = false;
           if (getCurrentlySetPermissions != null) {
               for (var i = 0; i < getCurrentlySetPermissions.length; i++) {
                   if (getCurrentlySetPermissions[i].CanManageLowerPermissions == "True") {
                       CanManageLowerPermissionsFlag = true;
                       break;
                   }
               }
           }
          //Show if flag is true.. try catch because it wont work in PA and default page, as control isnt there
           if (CanManageLowerPermissionsFlag == true) {
               try {
                   document.getElementById("SSMC_Assignment").style.display = "inline";
               } catch (e) {
                   alert("$(document).ready() SSMC_Assignment element.. " + e);
               }
           }
           else {
               try {
                   document.getElementById("SSMC_Assignment").style.display = "none";
               } catch (e) {
                   alert("$(document).ready() SSMC_Assignment element.. " + e);
               }
            }
            //3/9/2020 - JH Security End//////////////////////////////////////////////////////////////////////////////////////////

        }
    );
    


    //***********************************************************************************
    //* Screen menu display
    //***********************************************************************************
    function setActiveScreen(screenName) {
        var MenuItems = document.getElementById("fsTabMenu").getElementsByTagName("li");
        var MenuItems = $("input[name='txtSpec']")
                
        for (r = 0; r < MenuItems.length; r++) {            
            if (MenuItems[r].innerText == screenName) {
                MenuItems[r].className = MenuItems[r].className.split(" ")[0] + ' Selected';                
            }
            else {
                MenuItems[r].className = MenuItems[r].className.split(" ")[0];
            }            
        }
    }
</script>
</html>
