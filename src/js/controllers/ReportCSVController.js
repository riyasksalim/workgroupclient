// ReportCSVController

angular.module('app')

.controller('ReportCSVController', ['$scope', 'dataService', function ($scope, dataService) {
  

//     var
//     nameList = ['Pierre', 'Pol', 'Jacques', 'Robert', 'Elisa'],
//     familyName = ['Dupont', 'Germain', 'Delcourt', 'bjip', 'Menez'];

// function createRandomItem() {
//     var
//         firstName = nameList[Math.floor(Math.random() * 4)],
//         lastName = familyName[Math.floor(Math.random() * 4)],
//         age = Math.floor(Math.random() * 100),
//         email = firstName + lastName + '@whatever.com',
//         balance = Math.random() * 3000;

//     return{
//         firstName: firstName,
//         lastName: lastName,
//         age: age,
//         email: email,
//         balance: balance
//     };
// }

//     scope.itemsByPage=1;

// scope.rowCollection = [];
// for (var j = 0; j < 500; j++) {
//     scope.rowCollection.push(createRandomItem());
// }

















  
  
  
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
$scope.itemsByPage='15';
    $scope.search = function () {
        var params = {
            StartDate: $scope.StartDate,
            EndDate: $scope.EndDate,
            WorkGroupID: $scope.workgroupselected.WorkGroupId
        };
        dataService.GetWorkGroupReport(params).then(function (response) {
            if (response && response.data) {
                $scope.WorkGroupsReport = response.data;
                $scope.WorkGroupsReportSafe=angular.copy(response.data);
                debugger
            }
        }, function onError() {

        });
    };


   
   
    $scope.rowCollection1 = [
         { firstName: 'Laurent', lastName: 'Renard', birthDate: new Date('1987-05-21'), balance: 102, email: 'whatever@gmail.com' },
         { firstName: 'Blatwt443tndine', lastName: 'Faivre', birthDate: new Date('1987-04-25'), balance: -2323.22, email: 'oufblandou@gmail.com' },
         { firstName: 'Francoise', lastName: 'Frere', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' },
         { firstName: 'Fra4wetncoise', lastName: 'Frere', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' },
         { firstName: 'Frarewtgncoise', lastName: 'vcds', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' },
         { firstName: 'Franw4twrtw4coise', lastName: 'Fregewgewre', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raeewtymondef@gmail.com' },
         { firstName: 'Francoise', lastName: 'Frere', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' },
         { firstName: 'Francoregregise', lastName: 'Frvdsvdsvdvdsvere', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' },
         { firstName: 'Francoise', lastName: 'Freewfewfre', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raedfewfefymondef@gmail.com' },
         { firstName: 'Fragrgrewgnvegcoise', lastName: 'Fregewre', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' },
         { firstName: 'Francoise', lastName: 'Fewgewrere', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymonewvedef@gmail.com' },
         { firstName: 'Francrgoise', lastName: 'Fregewre', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' },
         { firstName: 'Francoise', lastName: 'Frgewgere', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' },
         { firstName: 'Francoise', lastName: 'Frere', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@swvvvgmail.com' },
         { firstName: 'Francervgresvgoise', lastName: 'Freewgewre', birthDate: new Date('1955-08-27'), balance: 42343, email: 'raymondef@gmail.com' }
    ];

    
}])
