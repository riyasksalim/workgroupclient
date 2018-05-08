angular.module('app')
.factory('dataService', ['$http', 'ApiUrl', '$q', '$rootScope', function ($http, ApiUrl, $q, $rootScope) {
    var baseUrl = ApiUrl;

    
    function PendingRequest() {


        //debugger
        if (!window.gettotal) {

            $rootScope.total = $http.pendingRequests.length;
            window.gettotal = true;
            // console.log('%c Request Remaining "' + $http.pendingRequests.length + '" ', 'background: blue; color: #bada55');

        }
        else {

            var a = $http.pendingRequests.length;

            var c = a / $rootScope.total;
            c = c * 100;


            var statusloading = $rootScope.total - $http.pendingRequests.length;

            if (statusloading > 0) {
                Metronic.blockUI({ boxed: true, message: 'LOADING...' + ($rootScope.total - $http.pendingRequests.length) + " of " + $rootScope.total });
            }
            else {
                Metronic.blockUI({ boxed: true, message: 'LOADING...' });
            }


        }

        if ($http.pendingRequests.length == 0) {

            window.gettotal = false;
            //console.log('%c Loading Completed unblocking the UI Loader" ', 'background: gray; color: #bada55');
            Metronic.unblockUI();
        }



    };

    
    //function GetWorkGroupReport(Params) {
        
    //    return $http.post(baseUrl + 'Reports/GetCustomerServiceDetails/', Params);
    //};
    function GetAllWorkGroups() {
        return $http.get(baseUrl + 'Reports/GetAllWorkGroups');
    };
    function GetCSVList() {
        return $http.get(baseUrl + 'Reports/GetCSVList');
    };

    function GetFile(row) {
       // return $http.get('http://localhost:51310/Reports/GetFile?Filename=');
     return $http.get(baseUrl + 'Reports/GetFile?Filename='+row.ReportGeneratedFullPath);
    };

    function GetWorkGroupReport(Params) {
        var promise = $q.defer();
        var request = $http.post(baseUrl + 'Reports/GetCustomerServiceDetails/', Params,
            {
                timeout: promise.promise
            })
        .then(getReportDataComplete)
        .catch(getReportDataFailed);
        request.abortCall = function () {
            promise.resolve();
        }
        function getReportDataComplete(response) {
            return response;
        }
        function getReportDataFailed(error) {$q.reject();}
        return request;
    };
    function downloadcsv(row) {
        return $http.get(baseUrl + 'Reports/Getdownloadcsv?file=' + row.ReportGenerated);
    };



    return {
        PendingRequest: PendingRequest,
        GetWorkGroupReport: GetWorkGroupReport,
        GetAllWorkGroups: GetAllWorkGroups,
        GetCSVList: GetCSVList,
        downloadcsv: downloadcsv,
        GetFile:GetFile
       

    }
}]);