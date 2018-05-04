angular.module('app')


.value("isDebugging", true)

//if (window.location.href.indexOf('localhost') > 0) {

//    MetronicApp.constant('ApiUrl', 'http://localhost:55335/');




.constant('ApiUrl', 'http://localhost:90/api/');

//}
//else {
  //  MetronicApp.constant('ApiUrl', window.location.origin + '/api/');

//}