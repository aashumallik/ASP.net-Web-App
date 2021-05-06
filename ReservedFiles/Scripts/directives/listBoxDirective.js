function listBoxDirective() {
    return {
        restrict: 'E',
        replace: 'true',
        scope: {
            leftSide: '=leftSide',
            rightSide: '=rightSide',
            leftHeader: '@leftHeader',
            rightHeader: '@rightHeader',
            required: '=required',
            errorText: '=errorText',
            selectSize: '@selectSize',
            allowUserSortRight: '=allowUserSortRight',
            allowUserSortLeft: '=allowUserSortLeft',
            sortBy: '=sortBy'
        },
        templateUrl: 'ReservedFiles/templates/list-box.html',
        controller: ['$scope', function ($scope) {
            $scope.toMoveRight = [];
            $scope.toMoveLeft = [];

            $scope.moveRight = function () {
                for (var i = 0; i < $scope.toMoveRight.length; i++) {
                    if ($scope.rightSide.indexOf($scope.toMoveRight[i]) < 0)
                        $scope.rightSide.push($scope.toMoveRight[i]);
                    if ($scope.leftSide.indexOf($scope.toMoveRight[i]) >= 0)
                        $scope.leftSide.splice($scope.leftSide.indexOf($scope.toMoveRight[i]), 1);
                }
                $scope.toMoveRight = [];
            };

            $scope.moveLeft = function () {
                for (var i = 0; i < $scope.toMoveLeft.length; i++) {
                    if ($scope.leftSide.indexOf($scope.toMoveLeft[i]) < 0)
                        $scope.leftSide.push($scope.toMoveLeft[i]);
                    if ($scope.rightSide.indexOf($scope.toMoveLeft[i]) >= 0)
                        $scope.rightSide.splice($scope.rightSide.indexOf($scope.toMoveLeft[i]), 1);
                }
                $scope.toMoveLeft = [];
            };

            $scope.moveUp = function (selected, sourceList) {
                if (selected.length === 1 && sourceList.length > 1) {
                    moving = selected[0];
                    var idx = sourceList.indexOf(moving);
                    if (idx > 0) {
                        var prevItem = sourceList[idx - 1];
                        sourceList.splice(idx, 1);
                        sourceList.splice(idx - 1, 0, moving);
                    }
                }
            };

            $scope.moveDown = function (selected, sourceList) {
                if (selected.length === 1 && sourceList.length > 1) {
                    moving = selected[0];
                    var idx = sourceList.indexOf(moving);
                    if (idx <= sourceList.length) {
                        var nextItem = sourceList[idx + 1];
                        sourceList.splice(idx, 1);
                        sourceList.splice(idx + 1, 0, moving);
                    }
                }
            };
        }]
    };
}