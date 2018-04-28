angular.module('app')

.controller('ReportController', ['$scope', 'dataService', 'ApiUrl', '$http', function ($scope, dataService, ApiUrl, $http) {
    $scope.itemsByPage=15
    var baseUrl = ApiUrl;
    $scope.loading = false;
    $(function () {
        $("#StartDate").datepicker({
            onSelect: function (date) {
                $scope.StartDate = date;
                $scope.$apply();
            }
         });
        $("#EndDate").datepicker({
            onSelect: function (date) {
                $scope.EndDate = date;
                $scope.$apply();
            }
        });
    });

  function GetAllWorkGroups() {
        dataService.GetAllWorkGroups().then(function (response) {
            if (response && response.data) {
                $scope.WorkGroups = response.data;
            }
        }, function onError() {
        });
    };

    GetAllWorkGroups();
    $scope.WorkGroupsReport = [];
    $scope.search = function () {
        var params = {
            StartDate: $scope.StartDate,
            EndDate: $scope.EndDate,
            WorkGroupID: $scope.workgroupselected.WorkGroupId
        };

        dataService.GetWorkGroupReport(params).then(function (response) {
            if (response && response.data) {
                $scope.WorkGroupsReport = response.data;
            }

        }, function onError() {
        });
    };

}])

