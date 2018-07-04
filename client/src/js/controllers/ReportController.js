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

            required: true,
            message: "This is a required field",
            dateFormat: 'mm-dd-yy',
            onClose: function() { $(this).valid(); },
            onSelect: function(date) {
                $scope.StartDate = date;
                $scope.$apply();
            }
        });
        $("#EndDate").datepicker({
            dateFormat: 'mm-dd-yy',
            required: true,
            message: "This is a required field",

            onClose: function() { $(this).valid(); },
            onSelect: function(date) {
                debugger
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

    function GetAllTemplates() {

        dataService.GetAllTemplates().then(function(response) {
            if (response && response.data) {
                $scope.Templates = response.data;

                if (response.data.length == 0) {
                    // $("#select2-drop").css('display', 'none');
                }
                $.unblockUI();
            }
        }, function onError() {
            $.unblockUI();
        });
    };
    var httpRequest = null;
    GetAllWorkGroups();
    GetAllTemplates();
    $scope.checkdate = function() {

        var a = $scope.beginDate;
        var date = new Date(a);


    };

    function isDate(x) {
        return (null != x) && !isNaN(x) && ("undefined" !== typeof x.getDate);
    }

    function checkparams() {

        if ($scope.StartDate == undefined || $scope.StartDate == "" || $scope.EndDate == undefined || $scope.StartDate == "" || $scope.TemplateSelected == undefined) {

            $.toaster({
                settings: settings,
                message: 'Input Parameter required'
            });
            return false;
        } else if (!checkdateformat($scope.StartDate, $scope.EndDate)) {
            $.toaster({
                settings: settings,
                message: 'Not a Date Format'
            });
            $scope.StartDate = undefined;
            $scope.EndDate = undefined;
            return false;
        } else if (new Date($scope.StartDate) > new Date($scope.EndDate)) {
            $.toaster({
                settings: settings,
                message: 'End Date Should be larger than start Date'
            });
            return false;
        } else {
            return true;
        }
    }

    function checkdateformat(start, end) {

        var startdate = validate(start);
        var enddate = validate(end);
        if (startdate && enddate) {
            return true;
        } else {
            return false;
        }

    }

    function validate(text) {

        var comp = text.split('-');
        var m = parseInt(comp[0], 10);
        var d = parseInt(comp[1], 10);
        var y = parseInt(comp[2], 10);
        var date = new Date(y, m - 1, d);
        if (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d) {
            return true;
        } else {
            return false;
        }

    }
    $scope.$watch('StartDate', function(newValue, oldValue) {
        console.log(newValue);
        $scope.StartDate = newValue

    });
    $scope.$watch('EndDate', function(newValue, oldValue) {
        console.log(newValue);
        $scope.EndDate = newValue

    });
    $scope.search = function() {
        $scope.WorkGroupsReport = [];


        if (!checkparams()) {

            return false;
        }

        var params = {
            StartDate: $scope.StartDate,
            EndDate: $scope.EndDate,
            WorkGroupID: ($scope.workgroupselected) ? $scope.workgroupselected.join().toString() : "",
            TemplateID: ($scope.TemplateSelected) ? $scope.TemplateSelected.join().toString() : ""
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
                if (response.data.length > 0) {
                    $.toaster({
                        settings: settings,
                        message: 'CSV Generated Succesfully..',
                    });
                } else {
                    $.toaster({
                        settings: settings,
                        message: 'No Results Found!'
                    });

                }

            }

        }, function onError(err) {

            $.unblockUI();
            var msg = JSON.stringify(err);
            $.toaster({
                settings: settings,
                message: msg,
            });
        }));
    };
    $scope.abortExecutingApi = function() {
        $.unblockUI();
        return (httpRequest && httpRequest.abortCall());
    };



    $scope.select2Options = {

    };




}])