function EmailController($scope) {

    $scope.email = {};
    
    $scope.sendEmail = function () {
        FirestoneWebTemplate.WebTemplateService.SendMessage($scope.email);
    };
}



