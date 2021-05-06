<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Formappoval.aspx.cs" Inherits="FirestoneWebTemplate.Formappoval" %>
<%@ Register Assembly="FirestoneWebSupportLibrary" Namespace="FirestoneWebSupportLibrary.Controls" TagPrefix="cc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10" />
    <link href="ReservedFiles/Styles/ui-lightness/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <link href="ReservedFiles/Styles/standardStyle.css" rel="stylesheet" />
    <link href="Styles/FirestoneWebTemplate.css" rel="stylesheet" />    
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager" runat="server">
                <Services>
                    <asp:ServiceReference Path="Services/WebTemplateService.asmx" />
                </Services>
            </asp:ScriptManager>
            <cc1:page id="DefaultPage" runat="server" navigationstyle="Left">            
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
                        <cc1:MenuItem Text="Email" OnClick="setActiveScreen('Email')" Selected="false" NavigationURL="Email.aspx"></cc1:MenuItem>
                        <cc1:MenuItem Text="Request" OnClick="setActiveScreen('Request')" Selected="false" NavigationURL="Form_request.aspx"></cc1:MenuItem>
                        <cc1:MenuItem Text="Approval" OnClick="setActiveScreen('Approval')" Selected="true" NavigationURL="Formappoval.aspx"></cc1:MenuItem>
                 
                    </Items>
                </TabMenu>
            </Header>
            <Navigation>
                <%--JH 3/9/2020 - For Managing Security--%>
                <div><a id="SSMC_Assignment" href="" class="SSMCClass" style="display:none;">Manage Security</a></div>

                
            </Navigation>                        
            <Contents>
                <div class="container">
               <div class="row">
                  <div class="col-md-6">
                     <div class="form-group">
                        <label>Originated:</label>
                        <asp:TextBox ID="txt_ori_date" runat="server" TextMode="Date" ></asp:TextBox>
                     </div>
                     <div class="form-group">
                        <label>Requester:</label>
                        <asp:DropDownList ID="drp_reuq" runat="server" >
                        </asp:DropDownList>
                     </div>
                     <div class="form-group">
                        <label>Bin Location:</label>
                        <asp:TextBox ID="txt_bin_location" runat="server"></asp:TextBox>
                     </div>
                     <div class="form-group">
                        <label>Priority:</label>
                        <asp:TextBox ID="txt_priority" runat="server"></asp:TextBox>
                     </div>
                  </div>
                  <div class="col-md-6">
                     <div class="form-group">
                        <label for="Student">ID:</label>
                        <asp:TextBox ID="txt_id" runat="server"></asp:TextBox>
                     </div>
                     <div class="form-group">
                        <label>Min/Max:</label>
                        <asp:TextBox ID="txt_min" runat="server"></asp:TextBox>
                        <asp:TextBox ID="txt_max" runat="server"></asp:TextBox>
                     </div>
                     <div class="form-group">
                        <label>Common#</label>
                        <asp:TextBox ID="txt_common" runat="server"></asp:TextBox>
                     </div>
                     <div class="form-group">
                        <label>Lead Time(Weeks)</label>
                        <asp:TextBox ID="txt_lead_time" runat="server"></asp:TextBox>
                        <label>Price $</label>
                        <asp:TextBox ID="txt_price" runat="server"></asp:TextBox>
                     </div>
                  </div>
               </div>
               <div class="row">
                  <div class="col-md-12">
                     <div>
                        <label>Description:</label>
                     </div>
                     <asp:TextBox ID="txt_part_desc" runat="server" Width="100%"></asp:TextBox>
                     <asp:TextBox ID="txt_part_desc1" runat="server" Width="100%"></asp:TextBox>
                     <asp:TextBox ID="txt_part_desc2" runat="server" Width="100%"></asp:TextBox>
                     <asp:TextBox ID="txt_part_desc3" runat="server" Width="100%"></asp:TextBox>
                     <asp:TextBox ID="txt_part_desc4" runat="server" Width="100%"></asp:TextBox>
                  </div>
               </div>
               <div class="row">
                  <div class="col-md-12">
                     <div>
                        <label>Why Stock</label>
                     </div>
                     <asp:TextBox ID="txt_why_stock" runat="server" TextMode="MultiLine" Rows="2" Width="100%"></asp:TextBox>
                  </div>
               </div>
               <div class="row">
                  <div class="col-md-12">
                     <div class="col-md-6" style="padding-top: 10px">
                        <div class="row">
                           <div class="col-md-12">
                              <div class="col-md-6">
                                 <label style="vertical-align: top;">Approval Tracking</label>
                                 <asp:TextBox ID="txt_approval_level" runat="server"></asp:TextBox>
                                 <asp:TextBox ID="txt_sent_to_date" runat="server" TextMode="Date" ></asp:TextBox>
                                 <asp:Button ID="btn_previous_record" runat="server" Text="Previous Record" OnClick="btn_previous_record_Click"></asp:Button>
                              </div>
                              <div class="col-md-6">
                                 <asp:TextBox ID="txt_approval_tracking" runat="server" TextMode="MultiLine" Rows="10">
                                 </asp:TextBox>
                                 <asp:Button ID="btn_next_record" runat="server" Text="Next Record" OnClick="btn_next_record_Click"></asp:Button>
                                 <asp:Button ID="btn_get" runat="server" Text="Get"></asp:Button>
                              </div>
                           </div>
                        </div>
                     </div>
                     <div class="col-md-6">
                        <div class="form-group">
                           <label>Currently in Approval Cue</label>
                           <asp:TextBox ID="txt_cue" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                           <label>Environmental Approval Required</label>
                           <asp:CheckBox ID="ch_env_app" runat="server">
                           </asp:CheckBox>
                        </div>
                        <div class="form-group">
                           <label>Send to Next - Choices</label>
                           <asp:DropDownList ID="drp_app_choices" runat="server" >
                           </asp:DropDownList>
                        </div>
                        <div class="form-group">
                           <asp:Button ID="btn_send_next" runat="server" Text="Send to Next Approval"></asp:Button>
                        </div>
                     </div>
                  </div>
               </div>
               <div class="row">
                  <div class="col-md-12">
                     <div class="col-md-8">
                        <div>
                           <label>Comments</label>
                        </div>
                        <asp:TextBox ID="txt_comments" runat="server" Width="100%" Rows="4" TextMode="MultiLine"></asp:TextBox>
                     </div>
                     <div class="col-md-4">
                        <asp:Button ID="btn_print" runat="server" Text="View/Edit/Print Details"></asp:Button>
                        <asp:Button ID="btn_quit" runat="server" Text="Quit"></asp:Button>
                        <asp:Button ID="btn_administer" runat="server" Text="Administer"></asp:Button>
                        <asp:Button ID="btn_login" runat="server" Text="Login"></asp:Button>
                     </div>
                  </div>
               </div>
            </div>
            </Contents>
        </cc1:page>
        </div>

    </form>
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
    <script  type="text/javascript">
        $(document).ready(function () {
            //alert("docReady");                       
            LoadSpecs();  

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


   function OnSuccess_GetSampleSpecs(data) {
        ngApp.pushData("SampleSpecsCtrl", data);
    }



    function LoadSpecs() {
        var filter = document.getElementById("txtFilterSerial").value

        //alert("filter:" + filter);
        //myWeb.MyServiceClass.HelloWorld("Binky", OnSuccess, OnTimeOut, OnError) 
        FirestoneWebTemplate.WebTemplateService.GetSampleSpecs(filter, OnSuccess_GetSampleSpecs);
    }

    function fsSave() {
        var fsTableDetail = document.getElementsByClassName("fsTableDetail");
        //var fsTableDetail = $(".fsTableDetail");
                        
        //hdDelayLockMinutes.$('td[name=tcol1]')
        alert(fsTableDetail.length)        
        alert($(fsTableDetail[1]).find("span[name='spSpec']")[0].innerText);
       
        
    }


    </script>
</body>
</html>
