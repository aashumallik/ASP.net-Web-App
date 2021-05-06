


function SampleSpecsCtrl($scope) {
    $scope.data = [];    
    //$scope.showFilter = new Object();
    $scope.showFilter = true;
    

    $scope.newData = function (data) {                
        $scope.data = data;
        $scope.showFilter = new Object();
        Object.keys(data[0]).forEach(function (key) {
            //alert("key:" + key )            
        });

        $scope.$apply();
    }

    
    $scope.filterShow = function (property) {

        if (property == "true")
            $scope.showFilter = true;

        if ($scope.search == undefined)
            $scope.showFilter = true;

        Object.keys($scope.search).forEach(function (key) {
            //alert("key:" + key + " value:" + $scope.search[key])
            if (property == key && $scope.search[key] == "(All)") {
                $scope.showFilter = false;
            }
        });        
        $scope.showFilter = true;
    }


    $scope.filterClear = function () {
        Object.keys($scope.search).forEach(function (key) { 
            if ($scope.search[key] == "(All)") {
                $scope.search[key] = undefined;                
            }
        });        
    }

    $scope.setEmpty = function (value) {
        if (value == "")
            return "(Empty)";
        else
            return value;        
    }
    

    
    $scope.isEditMode = function (spec) {
        if (spec == '036256-53')
            return true;
        else
            return false;
        }

    $scope.isReadOnlytMode = function (spec) {
        if (spec == '036256-53')
            return false;
        else
            return true;
    }


};



function fsList() {


    function add (property, value) {

        if (property == "true")
            $scope.showFilter = true;

        if ($scope.search == undefined)
            $scope.showFilter = true;

        Object.keys($scope.search).forEach(function (key) {
            //alert("key:" + key + " value:" + $scope.search[key])
            if (property == key && $scope.search[key] == "(All)") {
                $scope.showFilter = false;
            }
        });        
        $scope.showFilter = true;
    }

}
