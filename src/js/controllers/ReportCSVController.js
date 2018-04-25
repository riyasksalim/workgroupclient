// ReportCSVController

angular.module('app')

.controller('ReportCSVController', ['$scope', 'dataService', function ($scope, dataService) {
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


    function GetReports(Params) {
      
       dataService.GetReports(Params).then(function (response) {
            if (response && response.data) {

              
            }
        }, function onError() {
        
        });


    };
    var Params = {
        StartDate: new Date(),
        EndDate: new Date(),
        param: "dewfewf"
    };


    function GetAllWorkGroups() {
        dataService.GetAllWorkGroups().then(function (response) {
            if (response && response.data) {
                $scope.WorkGroups = response.data;
                debugger
            }
        }, function onError() {
       
        });

    };

    GetAllWorkGroups();

    $scope.search = function () {
        var params = {
            StartDate: $scope.StartDate,
            EndDate: $scope.EndDate,
            WorkGroupID: $scope.workgroupselected.WorkGroupId
        };
        dataService.GetWorkGroupReport(params).then(function (response) {
            if (response && response.data) {
                $scope.WorkGroupsReport = response.data;
                debugger
            }
        }, function onError() {

        });
    };


   
   
    $scope.rowCollection = [
         { firstName: 'Laurent', lastName: 'Renard', birthDate: new Date('1987-05-21'), balance: 102, email: 'whatever@gmail.com' },
         { firstName: 'Blandine', lastName: 'Faivre', birthDate: new Date('1987-04-25'), balance: -2323.22, email: 'oufblandou@gmail.com' },
         { firstName: 'Francoise', lastName: 'Frere', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' }
    ];
}])
