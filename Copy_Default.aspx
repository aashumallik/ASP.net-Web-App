<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Copy_Default.aspx.cs" Inherits="FirestoneWebTemplate.Copy_Default" %>
<%@ Register Assembly="FirestoneWebSupportLibrary" Namespace="FirestoneWebSupportLibrary.Controls" TagPrefix="cc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=10" />
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
            <Header ID="Header" Title="WebTemplate" SystemName="System Name" ApplicationName="Application Name" Location="Location" runat="server">
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
                        <cc1:MenuItem Text="Home" OnClick="setActiveScreen('Home');" Selected="true"></cc1:MenuItem>           
                        <cc1:MenuItem Text="Page1" OnClick="setActiveScreen('Page1')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page2" OnClick="setActiveScreen('Page2')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page3" OnClick="setActiveScreen('Page3')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page4" OnClick="setActiveScreen('Page4')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page5" OnClick="setActiveScreen('Page5')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page6" OnClick="setActiveScreen('Page6')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page7" OnClick="setActiveScreen('Page7')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page8" OnClick="setActiveScreen('Page8')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page9" OnClick="setActiveScreen('Page9')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page10" OnClick="setActiveScreen('Page10')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page11" OnClick="setActiveScreen('Page11')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page12" OnClick="setActiveScreen('Page12')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page13" OnClick="setActiveScreen('Page13')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page14" OnClick="setActiveScreen('Page14')" Selected="false"></cc1:MenuItem>
                        <cc1:MenuItem Text="Page15" OnClick="setActiveScreen('Page15')" Selected="false"></cc1:MenuItem>
                    </Items>
                </TabMenu>
            </Header>
            <Navigation>
                <div>Plant</div>
                <div>Safety</div>
                <div>PDIC</div>
            </Navigation>                        
            <Contents>                
                <div>Hello world this is the area, or body of web page.
                    <div class="divFilter">  
                        <input id="txtFilterSerial" type="text" value="" /><a onclick="LoadSpecs();"><img src="Images/Find.jpg" /></a> 
                    </div>
                    <div class="divFilter">  
                        
                    </div>
                </div>
                <div class="fsTable divSpecTable" ng-controller="SampleSpecsCtrl">
                    <div class="fsTableHeader divSampleSpecsHeader">
                        <span>Spec
                            <a class="fsSmallFilterImage" title="Filter Record" ng-click="filterShow('true');">Filter</a>
                            <select id="ddlSpec" ng-model="search.Spec" ng-show="true" ng-change="filterClear();">
                                <option value="(All)">(All)</option> 
                                <option value="{{item.Spec}}" ng-repeat="item in data">{{setEmpty(item.Spec);}}</option>                                 
                            </select>
                        </span>                    
                        <span>Product Code
                            <a class="fsSmallFilterImage" title="Filter Record" ng-click="filterShow('ProductCode');">Filter</a>
                            <select id="ddlProductCode" ng-model="search.ProductCode" ng-show="$scope.filterShow('ProductCode');" ng-change="filterClear();">
                                <option value="(All)">(All)</option> 
                                <option value="{{item.ProductCode}}" ng-repeat="item in data">{{setEmpty(item.ProductCode);}}</option>                                 
                            </select>
                        </span>
                        <span>Tread Code</span>
                        <span>Spec Desc</span>
                        <span>Tire Weight</span>
                        <span>Carcass Weight</span>                
                        <span></span>                  
                     </div>                    
                    <div class="fsTableDetail divSampleSpecsDetail" ng-repeat="item in data | filter:search:true">                            
                        <div>
                            <span name='spSpec' ng-show="isReadOnlytMode(item.Spec);">{{item.Spec}}</span>
                            <input name='txtSpec' ng-show="isEditMode(item.Spec);" value="{{item.Spec}}" />
                        </div>
                        <div>
                            <span name='spProductCode' ng-show="isReadOnlytMode(item.Spec);">{{item.ProductCode}}</span>
                            <input name='spProductCode' ng-show="isEditMode(item.Spec);" value="{{item.ProductCode}}" />                            
                        </div>
                        <div>
                            <span name='spTreadCode' ng-show="isReadOnlytMode(item.Spec);">{{item.TreadCode}}</span>
                            <input name='spTreadCode' ng-show="isEditMode(item.Spec);" value="{{item.TreadCode}}" />
                        </div>                            
                        <div>                                
                            <span name='spSpecDesc' ng-show="isReadOnlytMode(item.Spec);">{{item.SpecDesc}}</span>
                            <input name='spSpecDesc' ng-show="isEditMode(item.Spec);" value="{{item.SpecDesc}}" />
                        </div>
                        <div>                                
                            <span name='spTireWeight' ng-show="isReadOnlytMode(item.Spec);">{{item.TireWeight}}</span>
                            <input name='spTireWeight' ng-show="isEditMode(item.Spec);" value="{{item.TireWeight}}" />
                        </div>
                        <div>                                
                            <span name='spCarcassWeight' ng-show="isReadOnlytMode(item.Spec);">{{item.CarcassWeight}}</span>
                            <input name='spCarcassWeight' ng-show="isEditMode(item.Spec);" value="{{item.CarcassWeight}}" />
                        </div>
                        <div>
                            <span>
                                <a class="fsEditImage" title="Edit">Edit</a>
                                <a class="fsDeleteImage" title="Delete">Delete</a>
                                <a class="fsSaveImage" title="Save" onclick="fsSave();">Save</a>
                            </span>
                        </div>                                                    
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
    <script  type="text/javascript">
        $(document).ready(function () {
            //alert("docReady");                       
            LoadSpecs();                       
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
