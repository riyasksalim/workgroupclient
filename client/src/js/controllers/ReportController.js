angular.module('app')

.controller('ReportController', ['$scope', 'dataService', 'ApiUrl', '$http', '$timeout', function($scope, dataService, ApiUrl, $http, $timeout) {
    $scope.itemsByPage = 15
    var baseUrl = ApiUrl;
    $scope.loading = false;
    $.blockUI({
        message: '<img src="../img/loading.gif"/>',
        css: {
            border: 'none',
            backgroundColor: 'transparent'
        }
    });
    var settings = {
        'toaster': {
            'id': 'toaster',
            'container': 'body',
            'template': '<div></div>',
            'class': 'toaster',
            'css': {
                'position': 'fixed',
                'top': '10px',
                'right': '10px',
                'width': '300px',
                'zIndex': 50000
            }
        },

        'toast': {
            'template': '<div class="alert alert-%priority% alert-dismissible" role="alert">' +
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

            'display': function($toast) {
                return $toast.fadeIn(settings.toast.fade);
            },

            'remove': function($toast, callback) {
                return $toast.animate({
                    opacity: '0',
                    padding: '0px',
                    margin: '0px',
                    height: '0px'
                }, {
                    duration: settings.toast.fade,
                    complete: callback
                });
            }
        },

        'debug': false,
        'timeout': 5500,
        'stylesheet': null,
        'donotdismiss': []
    };
    $(function() {
        $("#StartDate").datepicker({
            onSelect: function(date) {
                $scope.StartDate = date;
                $scope.$apply();
            }
        });
        $("#EndDate").datepicker({
            onSelect: function(date) {
                $scope.EndDate = date;
                $scope.$apply();
            }
        });
    });

    function GetAllWorkGroups() {

        dataService.GetAllWorkGroups().then(function(response) {
            if (response && response.data) {
                $scope.WorkGroups = response.data;

                if (response.data.length == 0) {
                    $("#select2-drop").css('display', 'none');
                }
                $.unblockUI();
            }
        }, function onError() {
            $.unblockUI();
        });
    };
    var httpRequest = null;
    GetAllWorkGroups();

    $scope.search = function() {
        $scope.WorkGroupsReport = [];
        if ($scope.StartDate == undefined || $scope.EndDate == undefined || $scope.workgroupselected == undefined) {

            $.toaster({
                settings: settings,
                message: 'Input Parameter required'
            });
            return false;


        }
        if ($scope.StartDate > $scope.EndDate) {
            $.toaster({
                settings: settings,
                message: 'End Date Should be larger than start Date'
            });
            return false;
        }

        var params = {
            StartDate: $scope.StartDate,
            EndDate: $scope.EndDate,
            WorkGroupID: $scope.workgroupselected
        };
        $.blockUI({
            message: '<img src="../img/loading.gif"/>',
            css: {
                border: 'none',
                backgroundColor: 'transparent'
            }
        });
        (httpRequest = dataService.GetWorkGroupReport(params).then(function(response) {
            if (response && response.data) {
                $scope.WorkGroupsReport = response.data;

                $.unblockUI();
                $.toaster({
                    settings: settings,
                    message: 'CSV Generated Succesfully..',


                });
            }

        }, function onError() {
            $.unblockUI();
        }));
    };
    $scope.abortExecutingApi = function() {

        return (httpRequest && httpRequest.abortCall());
    };



    $scope.select2Options = {
        // formatNoMatches: function(term) {

        //     var message = '<a ng-click="addTag()">Add tag:"' + term + '"</a>';
        //     if(!$scope.$$phase) {
        //         $scope.$apply(function() {
        //             $scope.noResultsTag = term;
        //         });
        //     }
        //     return message;
        // }
    };

    // $scope.addTag = function() {
    //     $scope.tags.push({
    //         id: $scope.tags.length,
    //         name: $scope.noResultsTag
    //     });
    // };

    // $scope.$watch('noResultsTag', function(newVal, oldVal) {
    //     if(newVal && newVal !== oldVal) {
    //         $timeout(function() {
    //             var noResultsLink = $('.select2-no-results');
    //             console.log(noResultsLink.contents());
    //             $compile(noResultsLink.contents())($scope);
    //         });
    //     }
    // }, true);





}])