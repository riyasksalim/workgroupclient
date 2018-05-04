angular.module('app')


.value("isDebugging", true)

//if (window.location.href.indexOf('localhost') > 0) {

//    MetronicApp.constant('ApiUrl', 'http://localhost:55335/');

//    // MetronicApp.constant('ApiUrl', 'http://dev-dashboard.nogalesproduce.com/api/');


.constant('ApiUrl', 'http://localhost:51310/');

//}
//else {
  //  MetronicApp.constant('ApiUrl', window.location.origin + '/api/');

//}