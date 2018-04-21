angular.module('app')

.controller('ReportController', ['$scope', 'dataService', function ($scope, dataService) {
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
           
          //  Metronic.unblockUI();
            //NotificationService.Error("Error upon the API request");
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
            debugger
            //  Metronic.unblockUI();
            //NotificationService.Error("Error upon the API request");
        });

    };

    GetAllWorkGroups();


   // GetReports(Params);


   
   
    $scope.rowCollection = [
         { firstName: 'Laurent', lastName: 'Renard', birthDate: new Date('1987-05-21'), balance: 102, email: 'whatever@gmail.com' },
         { firstName: 'Blandine', lastName: 'Faivre', birthDate: new Date('1987-04-25'), balance: -2323.22, email: 'oufblandou@gmail.com' },
         { firstName: 'Francoise', lastName: 'Frere', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' }
    ];
}])
