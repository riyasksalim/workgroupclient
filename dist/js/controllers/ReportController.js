angular.module('app')

.controller('ReportController', ['$scope', 'dataService', 'ApiUrl', '$http', function ($scope, dataService, ApiUrl, $http) {
    $scope.itemsByPage=15
    var baseUrl = ApiUrl;
    $scope.loading = false;
    var settings = {
        'toaster':
        {
            'id': 'toaster',
            'container': 'body',
            'template': '<div></div>',
            'class': 'toaster',
            'css':
            {
                'position': 'fixed',
                'top': '10px',
                'right': '10px',
                'width': '300px',
                'zIndex': 50000
            }
        },

        'toast':
        {
            'template':
            '<div class="alert alert-%priority% alert-dismissible" role="alert">' +
                '<button type="button" class="close" data-dismiss="alert">' +
                    '<span aria-hidden="true">&times;</span>' +
                    '<span class="sr-only">Close</span>' +
                '</button>' +
                '<span class="title"></span>: <span class="message"></span>' +
            '</div>',

            'css': {},
            'cssm': {},
            'csst': { 'fontWeight': 'bold' },

            'fade': 'slow',

            'display': function ($toast) {
                return $toast.fadeIn(settings.toast.fade);
            },

            'remove': function ($toast, callback) {
                return $toast.animate(
                    {
                        opacity: '0',
                        padding: '0px',
                        margin: '0px',
                        height: '0px'
                    },
                    {
                        duration: settings.toast.fade,
                        complete: callback
                    }
                );
            }
        },

        'debug': false,
        'timeout': 5500,
        'stylesheet': null,
        'donotdismiss': []
    };

    $.toaster({
        settings:settings,
        message: 'Your message hereqqqqqqqqqqq',
        title: 'Yor title'

    });

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
  var httpRequest = null;
    GetAllWorkGroups();
    $scope.WorkGroupsReport = [];
    $scope.search = function () {
        var params = {
            StartDate: $scope.StartDate,
            EndDate: $scope.EndDate,
            WorkGroupID: $scope.workgroupselected.WorkGroupId
        };

        (httpRequest = dataService.GetWorkGroupReport(params).then(function (response) {
            if (response && response.data) {
                $scope.WorkGroupsReport = response.data;
            }

        }, function onError() {
        }));
    };
    $scope.abortExecutingApi = function () {
        debugger
        return (httpRequest && httpRequest.abortCall());
    };
}])

