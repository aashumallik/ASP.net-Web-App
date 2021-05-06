*******************************************************************************************
PAGINATION
*******************************************************************************************
This file will give you an example of how to use pagination. This form of pagination works 
with angular where you would use an ng-repeat (data-grid, unordered list, etc)
===========================================================================================
ExampleApp.js
-------------------------------------------------------------------------------------------
(function () {
	angular.module("AppName", ["angularUtils.directives.dirPagination"])
	.controller("ExampleCtrl", ["$scope", function ($scope) {
		$scope.data = [];
		$scope.pageSize = 10;
	}]);
})();
===========================================================================================
ExamplePage.aspx
-------------------------------------------------------------------------------------------
<!-- All the stuff Microsoft fills in for you -->
<head>
    <link href="ReservedFiles/Styles/standardStyle.css" rel="stylesheet" />
    <!-- Additional App-specific Stylesheet(s) -->
    <!-- Script references can go here or below body -->
    <script src="ReservedFiles/Scripts/jquery-1.10.2.js"></script>
    <script src="ReservedFiles/Scripts/jquery-ui-1.10.4.custom.js"></script>
    <script src="ReservedFiles/Scripts/jquery.json-2.4.js"></script>
    <script src="ReservedFiles/Scripts/json2.js"></script>
    <script src="ReservedFiles/Scripts/json2.min.js"></script>
    <script src="ReservedFiles/Scripts/angular.js" type="text/javascript"></script>
    <script src="ReservedFiles/Scripts/directives/dirPagination.js"></script>
    <script src="ExampleCtrl.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div ng-app="AppName">

        <!--Page content laid out according to rest of Template -->
		<!-- This is a pretty simple way to make tables with css -->
        <div class="table" ng-controller="ExampleCtrl">
            <div> <!-- Header Row -->
                <span>Column 1 Header</span>
                <span>Column 2 Header</span>
            </div>
            <div dir-paginate="item in data | itemsPerPage: pageSize"> <!-- Table Body rows -->
            <!-- Any filter that ng-repeat uses, dir-paginate can use. Just put them before
                 itemsPerPage in the |-separated list -->
            <!-- itemsPerPage can also be an int. Above, it uses the controller's
                 $scope.pageSize to store how many items are displayed per page. -->
            <!-- Attributes (Add as attributes of the html tag. All are optional.)
                 current-page: Specify a property within the controller's $scope to store 
                               the current page number.
                 pagination-id: groups pagination controls with dir-paginates in case more 
                                than one exists on a page.
                 total-items: Specify the total number of items when working with
				              asynchronous data. -->
                <span>{{item.Column1}}</span>
                <span>{{item.Column2}}</span>
            </div>
        </div>

        <!-- Anywhere else on page -->
        <dir-pagination-controls></dir-pagination-controls> 
        <!-- Attributes (Add as attributes of tag. All are optional.)
             max-size: default 9, the number of pagination links to display.
             direction-links: default true, display Previous/Next links.
             boundary-links: default false, display First/Last links.
             on-page-change: default null, sets the callback for page change. Callback takes
                             newPageNumber and oldPageNumber as parameters, exact names must
                           be used and callback must be specified in your controller.
             pagination-id: groups pagination controls with dir-paginates in case more than
                            one exists on a page.
             auto-hide: default true, hide pagination-controls when there is only one page. -->
    
    </div>
    </form>
</body>
</html>
===========================================================================================
*******************************************************************************************
TABLE CSS
*******************************************************************************************
The .table class in standardStyle.css formats divs and spans as a table. See the example
below for how to use.

<div class="table">
	<div class="theader">
		<span>Row 1, Column 1</span>
		<span>Row 1, Column 2</span>
	</div>
	<div class="tbody">
		<span>Row 2, Column 1</span>
		<span>Row 2, Column 2</span>
	</div>
</div>

css selector to format header row:
.table .theader { }
css selectors to produce banding: 
.table .tbody:nth-child(2n+1) /* Odd rows, e.g. first, third, fifth, etc. */ { }
.table .tbody:nth-child(2n) /* Even rows, e.g. second, fourth, sixth, etc. */ { }
*******************************************************************************************
LIST BOXES
*******************************************************************************************
