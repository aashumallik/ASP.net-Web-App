<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form_request.aspx.cs" Inherits="FirestoneWebTemplate.Form_request" %>
<%@ Register Assembly="FirestoneWebSupportLibrary" Namespace="FirestoneWebSupportLibrary.Controls" TagPrefix="cc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10" />

    <meta http-equiv="X-UA-Compatible" content="IE=10" />
    <link href="ReservedFiles/Styles/ui-lightness/jquery-ui-1.10.4.custom.css" rel="stylesheet" />
    <link href="ReservedFiles/Styles/standardStyle.css" rel="stylesheet" />
    <link href="Styles/FirestoneWebTemplate.css" rel="stylesheet" />   
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
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
                        <cc1:MenuItem Text="Request" OnClick="setActiveScreen('Request')" Selected="true" NavigationURL="Form_request.aspx"></cc1:MenuItem>
                        <cc1:MenuItem Text="Approval" OnClick="setActiveScreen('Approval')" Selected="false" NavigationURL="Formappoval.aspx"></cc1:MenuItem>
                      
                    </Items>
                </TabMenu>
            </Header>
            <Navigation>
                <%--JH 3/9/2020 - For Managing Security--%>
                <div><a id="SSMC_Assignment" href="" class="SSMCClass" style="display:none;">Manage Security</a></div>

                
            </Navigation>                        
            <contents>
            <div class="container">
               <div class="row">
                  <div class="col-md-12">
                     <fieldset>
                        <legend style="text-align: center">Request for New Stock Item
                        </legend>
                        <div class="row">
                           <div class="col-md-12">
                              <label for="Student">Reuquested by:</label>
                              <asp:DropDownList ID="drp_reuq" runat="server" >
                              </asp:DropDownList>
                              <asp:TextBox ID="txt_req" runat="server"></asp:TextBox>
                              <label>Phone</label>
                              <asp:TextBox ID="txt_phone_number" runat="server"></asp:TextBox>
                              <label>Date</label>
                              <asp:TextBox ID="txt_date" TextMode="Date" runat="server"></asp:TextBox>
                              <label>ID</label>
                              <asp:TextBox ID="txt_id" runat="server"></asp:TextBox>
                              <asp:RegularExpressionValidator style="color:Red;" ID="RegularExpressionValidator1" ControlToValidate="txt_id" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                           </div>
                           <div class="col-md-12">
                              <label>Dept</label>
                              <asp:DropDownList ID="drp_dept" runat="server">
                              </asp:DropDownList>
                              <label>Action To Be Taken</label>
                              <asp:DropDownList ID="drp_action" runat="server">
                              </asp:DropDownList>
                              <label>Updated</label>
                              <asp:TextBox ID="txt_update" TextMode="Date" runat="server"></asp:TextBox>
                           </div>
                           <div class="col-md-12">
                              <label>Prev Punch</label>
                              <asp:DropDownList ID="drp_prev" runat="server">
                              </asp:DropDownList>
                              <label>If Yes, From Whom</label>
                              <asp:TextBox ID="txt_from_where" runat="server"></asp:TextBox>
                           </div>
                           <div class="col-md-12">
                              <label>PO#</label>
                              <asp:TextBox ID="txt_po" runat="server"></asp:TextBox>
                              <label>RFQ #</label>
                              <asp:TextBox ID="txt_rfq" runat="server"></asp:TextBox>
                              <label>Are there Parts to Turn In</label>
                              <asp:DropDownList ID="drp_turn_in" runat="server">
                              </asp:DropDownList>
                              <label>How Many</label>
                              <asp:TextBox ID="txt_how_many" runat="server"></asp:TextBox>
                              <asp:RegularExpressionValidator style="color:Red;" ID="RegularExpressionValidator2" ControlToValidate="txt_how_many" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                           </div>
                           <div class="col-md-12">
                              <label>Drawing</label>
                              <asp:TextBox ID="txt_drawing" runat="server"></asp:TextBox>
                              <label>Amount in use</label>
                              <asp:TextBox ID="txt_amount" runat="server"></asp:TextBox>
                              <label>Using SAP, Was any match found</label>
                              <asp:DropDownList ID="drp_sap" runat="server">
                              </asp:DropDownList>
                           </div>
                           <div class="col-md-12">
                              <label>Common Number</label>
                              <asp:TextBox ID="txt_common_number" runat="server"></asp:TextBox>
                              <asp:TextBox ID="txt_approval_cue" runat="server"></asp:TextBox>
                              <label>Stock at Other Plant</label>
                              <asp:DropDownList ID="drp_stock_at_other_plan" runat="server">
                              </asp:DropDownList>
                           </div>
                           <div class="col-md-12">
                              <label>Add to Local DTR Catalog Section</label>
                              <asp:TextBox ID="txt_catalog_sect" runat="server"></asp:TextBox>
                              <label>Next App</label>
                              <asp:DropDownList ID="drp_next_app" runat="server">
                              </asp:DropDownList>
                              <label>Unit</label>
                              <asp:DropDownList ID="drp_unit" runat="server">
                              </asp:DropDownList>
                           </div>
                           <div class="col-md-12">
                              <label>Bin Location</label>
                              <asp:TextBox ID="txt_bin_location" runat="server"></asp:TextBox>
                              <label>Grouping Number</label>
                              <asp:TextBox ID="txt_grup_number" runat="server"></asp:TextBox>
                              <label>Min</label>
                              <asp:TextBox ID="txt_min" runat="server"></asp:TextBox>
                              <label>Max</label>
                              <asp:TextBox ID="txt_max" runat="server"></asp:TextBox>
                           </div>
                           <div class="col-md-12">
                              <label>Category</label>
                              <asp:DropDownList ID="drp_category" runat="server">
                              </asp:DropDownList>
                              <label>Priority</label>
                              <asp:DropDownList ID="drp_priority" runat="server">
                              </asp:DropDownList>
                           </div>
                           <div class="col-md-12" style="display:flex">
                              <div class="col-md-8">
                                 <div class="col-md-12" style="display:flex; margin-right:20px">
                                    <div class="col-md-6" style="margin-right:20px">
                                       <label>Field Description</label>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field2" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field3" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field4" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field5" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field6" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field7" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field8" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field9" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field10" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field11" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field12" runat="server"></asp:TextBox>
                                       </div>
                                    </div>
                                    <div class="col-md-6">
                                       <label>Field Information</label>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field2_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field3_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field4_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field5_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field6_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field7_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field8_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field9_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field10_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field11_info" runat="server"></asp:TextBox>
                                       </div>
                                       <div style="margin-bottom:3px">
                                          <asp:TextBox ID="txt_field12_info" runat="server"></asp:TextBox>
                                       </div>
                                    </div>
                                 </div>
                              </div>
                              <div class="col-md-4">
                                 <div class="col-md-12" style="display:flex; margin-right:20px">
                                    <div class="row" style="margin-bottom:3px;">
                                       <label>Est. Usage</label>
                                       <asp:TextBox ID="txt_est" runat="server"></asp:TextBox>
                                       <asp:DropDownList ID="drp_year" runat="server">
                                       </asp:DropDownList>
                                    </div>
                                 </div>
                                 <div class="col-md-12" style="display:flex; margin-right:20px">
                                    <div  class="row" style="margin-bottom:3px">
                                       <asp:TextBox ID="txt_approval_tracking" runat="server" TextMode="MultiLine" Rows="16" Width="100%">
                                       </asp:TextBox>
                                    </div>
                                 </div>
                              </div>
                           </div>
                        </div>
                     </fieldset>
                     <fieldset>
                        <legend style="text-align: center">Request for New Stock Item</legend>
                        <div class="row">
                           <div class="col-md-12">
                              <label>Why Stock</label>
                              <br />
                              <asp:TextBox ID="txt_why_stock" runat="server" TextMode="MultiLine" Rows="2" Width="100%"></asp:TextBox>
                           </div>
                           <div class="col-md-12">
                              <label>Part Description or Change</label>
                              <br />
                              <div style="margin-bottom:3px">
                                 <asp:TextBox ID="txt_part_desc" runat="server" Width="100%">
                                 </asp:TextBox>
                              </div>
                              <div style="margin-bottom:3px">
                                 <asp:TextBox ID="txt_part_desc1" runat="server" Width="100%">
                                 </asp:TextBox>
                              </div>
                              <div style="margin-bottom:3px">
                                 <asp:TextBox ID="txt_part_desc2" runat="server" Width="100%">
                                 </asp:TextBox>
                              </div>
                              <div style="margin-bottom:3px">
                                 <asp:TextBox ID="txt_part_desc3" runat="server" Width="100%">
                                 </asp:TextBox>
                              </div>
                              <div style="margin-bottom:3px">
                                 <asp:TextBox ID="txt_part_desc4" runat="server" Width="100%">
                                 </asp:TextBox>
                              </div>
                           </div>
                        </div>
                        <div class="row">
                           <div style="display:flex"; class="col-md-12">
                              <div class="col-md-6" style="margin-right:10px">
                                 <div>
                                    <label>Where Used</label>
                                 </div>
                                 <div>
                                    <div style="margin-bottom:3px">
                                       <asp:DropDownList ID="drp_used" runat="server">
                                       </asp:DropDownList>
                                    </div>
                                    <div style="margin-bottom:3px">
                                       <asp:DropDownList ID="drp_used2" runat="server">
                                       </asp:DropDownList>
                                    </div>
                                    <div style="margin-bottom:3px">
                                       <asp:DropDownList ID="drp_used3" runat="server">
                                       </asp:DropDownList>
                                    </div>
                                    <div style="margin-bottom:3px">
                                       <asp:DropDownList ID="drp_used4" runat="server">
                                       </asp:DropDownList>
                                    </div>
                                 </div>
                              </div>
                              <div class="col-md-6">
                                 <div>
                                    <label>Machine Area</label>
                                 </div>
                                 <div>
                                    <div style="margin-bottom:3px">
                                       <asp:TextBox ID="txt_used_area" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                    <div style="margin-bottom:3px">
                                       <asp:TextBox ID="txt_used_area2" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                    <div style="margin-bottom:3px">
                                       <asp:TextBox ID="txt_used_area3" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                    <div style="margin-bottom:3px">
                                       <asp:TextBox ID="txt_used_area4" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                 </div>
                                 <div>
                                 </div>
                              </div>
                           </div>
                        </div>
                        <div class="row">
                           <div class="col-md-12"  style="display:flex">
                              <div class="col-md-8" style="margin-right:30px">
                                 <div>
                                    <label>Manufacturer</label>
                                    <asp:TextBox ID="txt_man1" runat="server" Width="100%"></asp:TextBox>
                                 </div>
                                 <div>
                                    <asp:TextBox ID="txt_man2" runat="server" Width="100%"></asp:TextBox>
                                 </div>
                              </div>
                              <div class="col-md-4">
                                 <div>
                                    <label>Part Number</label>
                                    <asp:TextBox ID="txt_man_p_n_1" runat="server" Width="100%"></asp:TextBox>
                                 </div>
                                 <div>
                                    <asp:TextBox ID="txt_man_p_n_2" runat="server" Width="100%"></asp:TextBox>
                                 </div>
                              </div>
                           </div>
                        </div>
                        <div class="col-md-12">
                           <label>Buyer</label>
                           <asp:TextBox ID="txt_buyer" runat="server">
                           </asp:TextBox>
                           <label>Vendor</label>
                           <asp:TextBox ID="txt_vender" runat="server">
                           </asp:TextBox>
                           <label>Lead Time(Weeks)</label>
                           <asp:TextBox ID="txt_lead_time" runat="server">
                           </asp:TextBox>
                        </div>
                        <div class="col-md-12">
                           <label>Tax ID</label>
                           <asp:TextBox ID="txt_tax_id" runat="server">
                           </asp:TextBox>
                           <label>Material Group</label>
                           <asp:TextBox ID="txt_material" runat="server">
                           </asp:TextBox>
                           <label>Repairable</label>
                           <asp:DropDownList ID="drp_repairable" runat="server">
                           </asp:DropDownList>
                        </div>
                        <div class="col-md-8">
                           <div class="row">
                              <div class="col-md-12">
                                 <label>Critical</label>
                                 <asp:DropDownList ID="drp_critical" runat="server">
                                 </asp:DropDownList>
                                 <label>Valuation Class</label>
                                 <asp:TextBox ID="txt_valuation" runat="server">
                                 </asp:TextBox>
                                 <label>If Repairable or Order on Request only , Initial ordering instructions</label>
                              </div>
                              <div class="col-md-12">
                                 <label>Env App</label>
                                 <asp:CheckBox ID="ch_env_app" runat="server">
                                 </asp:CheckBox>
                                 <label>Price $</label>
                                 <asp:TextBox ID="txt_price" runat="server">
                                 </asp:TextBox>
                                 <asp:RegularExpressionValidator style="color:Red;" runat="server" ID="rev1" ControlToValidate="txt_price"
                                    ValidationExpression="^\d*[0-9](?:\.[0-9]{1,2})?$" ErrorMessage="Only decimal values are allowed"
                                    Display="Dynamic"></asp:RegularExpressionValidator>
                                 <label>
                                 Order Qty:
                                 </label>
                                 <asp:TextBox ID="txt_order_qty" runat="server"></asp:TextBox>
                                 <asp:RegularExpressionValidator style="color:Red;" ID="RegularExpressionValidator3" ControlToValidate="txt_order_qty" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                 <label>Stock</label>
                                 <asp:CheckBox ID="ch_stock_checked" runat="server">
                                 </asp:CheckBox>
                                 <label>
                                 Use
                                 </label>
                                 <asp:CheckBox ID="ch_user_checked" runat="server">
                                 </asp:CheckBox>
                              </div>
                              <div class="col-md-12">
                                 <label>PO Date</label>
                                 <asp:TextBox ID="txt_po_date" runat="server" TextMode="Date"></asp:TextBox>
                                 <label>Replace Other Parts</label>
                                 <asp:DropDownList ID="drp_replace_other_part" runat="server">
                                 </asp:DropDownList>
                                 <label>
                                 Qty On Hand To Tunrn In For Stock
                                 </label>
                                 <asp:TextBox ID="txt_stock" runat="server"></asp:TextBox>
                                 <asp:RegularExpressionValidator style="color:Red;" ID="RegularExpressionValidator4" ControlToValidate="txt_stock" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                              </div>
                           </div>
                        </div>
                        <div class="col-md-4" style="border: 1px solid black; padding: 5px">
                           <label>Common # Replaced</label>
                           <asp:TextBox ID="txt_replaced" runat="server"> </asp:TextBox>
                        </div>
                        <asp:Label ID="savedSucess" runat="server"></asp:Label>
                     </fieldset>
                      <asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="btn_submit_Click"></asp:Button>
                  </div>
               </div>
            </div>
         </contents>                
        </cc1:page>
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
            else {
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
</html>
