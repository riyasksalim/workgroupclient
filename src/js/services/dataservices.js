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


    //function GetReports(filter) {
    //    return $http.get(baseUrl + "Home/GetCustomerServiceDetails?filterId=" + filter.Id);
    //};
    
    function GetReports(Params) {
        
        return $http.post(baseUrl + 'Reports/GetCustomerServiceDetails/', Params);
    };


    return {
        PendingRequest: PendingRequest,
        GetReports: GetReports,
       

    }
}]);