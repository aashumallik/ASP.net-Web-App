

var ngAppImpl = function () {
    var self = this;

    return {
        pushData: function (target, data) {
            angular.element('[ng-controller=' + target + ']').scope().newData(data);
        }      

    }
}
var ngApp = new ngAppImpl();
