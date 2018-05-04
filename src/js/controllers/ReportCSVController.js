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

      
        $scope.download = function (row) {
            debugger
            //window.open(row.ReportGeneratedFullPath);

            // var id = '#downloadcsv-' + row.ReportGeneratedFileName;

            // setTimeout(function () {
            //     $(id).click();
            // }, 1000);

            dataService.GetFile(row).then(function (response) {
                debugger
                if (response && response.data) {
                
                    var data = [response.data];
                    var blob = new Blob(data, {
                        type: 'text/csv;charset=UTF-8'
                    });
                    var s=row.ReportGeneratedFileName.split('.')
                    var data1 = window.URL.createObjectURL(blob);
                    var link = document.createElement('a');
                    link.href = data1;
                    link.download=s[0]+".csv"
                    link.click();
               
               
                }
            }, function onError() {
                debugger
            });
  
        };

    }])