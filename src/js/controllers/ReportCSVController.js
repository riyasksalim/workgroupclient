angular.module('app')
.controller('ReportCSVController', ['$scope', 'dataService', function ($scope, dataService) {
        $scope.itemsByPage = 15
        function GetCSVList() {
            dataService.GetCSVList().then(function (response) {
                if (response && response.data) {
                    $scope.csvlist = response.data;
                }
            }, function onError() {
            });
        };
        GetCSVList();
    }])