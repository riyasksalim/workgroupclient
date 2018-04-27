angular.module('app')

.controller('ReportController', ['$scope', 'dataService', function ($scope, dataService, $q, DTOptionsBuilder) {
    debugger

    $scope.data = [
       [
           "Tiger Nixon",
           "System Architect",
           "Edinburgh",
           "5421",
           "2011\/04\/25",
           "$320,800"
       ],
       [
           "Garrett Winters",
           "Accountant",
           "Tokyo",
           "8422",
           "2011\/07\/25",
           "$170,750"
       ],
       [
           "Ashton Cox",
           "Junior Technical Author",
           "San Francisco",
           "1562",
           "2009\/01\/12",
           "$86,000"
       ],
       [
           "Cedric Kelly",
           "Senior Javascript Developer",
           "Edinburgh",
           "6224",
           "2012\/03\/29",
           "$433,060"
       ],
       [
           "Airi Satou",
           "Accountant",
           "Tokyo",
           "5407",
           "2008\/11\/28",
           "$162,700"
       ],
       [
           "Brielle Williamson",
           "Integration Specialist",
           "New York",
           "4804",
           "2012\/12\/02",
           "$372,000"
       ],
       [
           "Herrod Chandler",
           "Sales Assistant",
           "San Francisco",
           "9608",
           "2012\/08\/06",
           "$137,500"
       ],
       [
           "Rhona Davidson",
           "Integration Specialist",
           "Tokyo",
           "6200",
           "2010\/10\/14",
           "$327,900"
       ],
       [
           "Colleen Hurst",
           "Javascript Developer",
           "San Francisco",
           "2360",
           "2009\/09\/15",
           "$205,500"
       ],
       [
           "Sonya Frost",
           "Software Engineer",
           "Edinburgh",
           "1667",
           "2008\/12\/13",
           "$103,600"
       ],
       [
           "Jena Gaines",
           "Office Manager",
           "London",
           "3814",
           "2008\/12\/19",
           "$90,560"
       ],
       [
           "Quinn Flynn",
           "Support Lead",
           "Edinburgh",
           "9497",
           "2013\/03\/03",
           "$342,000"
       ],
       [
           "Charde Marshall",
           "Regional Director",
           "San Francisco",
           "6741",
           "2008\/10\/16",
           "$470,600"
       ],
       [
           "Haley Kennedy",
           "Senior Marketing Designer",
           "London",
           "3597",
           "2012\/12\/18",
           "$313,500"
       ],
       [
           "Tatyana Fitzpatrick",
           "Regional Director",
           "London",
           "1965",
           "2010\/03\/17",
           "$385,750"
       ],
       [
           "Michael Silva",
           "Marketing Designer",
           "London",
           "1581",
           "2012\/11\/27",
           "$198,500"
       ],
       [
           "Paul Byrd",
           "Chief Financial Officer (CFO)",
           "New York",
           "3059",
           "2010\/06\/09",
           "$725,000"
       ],
       [
           "Gloria Little",
           "Systems Administrator",
           "New York",
           "1721",
           "2009\/04\/10",
           "$237,500"
       ],
       [
           "Bradley Greer",
           "Software Engineer",
           "London",
           "2558",
           "2012\/10\/13",
           "$132,000"
       ],
       [
           "Dai Rios",
           "Personnel Lead",
           "Edinburgh",
           "2290",
           "2012\/09\/26",
           "$217,500"
       ],
       [
           "Jenette Caldwell",
           "Development Lead",
           "New York",
           "1937",
           "2011\/09\/03",
           "$345,000"
       ],
       [
           "Yuri Berry",
           "Chief Marketing Officer (CMO)",
           "New York",
           "6154",
           "2009\/06\/25",
           "$675,000"
       ],
       [
           "Caesar Vance",
           "Pre-Sales Support",
           "New York",
           "8330",
           "2011\/12\/12",
           "$106,450"
       ],
       [
           "Doris Wilder",
           "Sales Assistant",
           "Sidney",
           "3023",
           "2010\/09\/20",
           "$85,600"
       ],
       [
           "Angelica Ramos",
           "Chief Executive Officer (CEO)",
           "London",
           "5797",
           "2009\/10\/09",
           "$1,200,000"
       ],
       [
           "Gavin Joyce",
           "Developer",
           "Edinburgh",
           "8822",
           "2010\/12\/22",
           "$92,575"
       ],
       [
           "Jennifer Chang",
           "Regional Director",
           "Singapore",
           "9239",
           "2010\/11\/14",
           "$357,650"
       ],
       [
           "Brenden Wagner",
           "Software Engineer",
           "San Francisco",
           "1314",
           "2011\/06\/07",
           "$206,850"
       ],
       [
           "Fiona Green",
           "Chief Operating Officer (COO)",
           "San Francisco",
           "2947",
           "2010\/03\/11",
           "$850,000"
       ],
       [
           "Shou Itou",
           "Regional Marketing",
           "Tokyo",
           "8899",
           "2011\/08\/14",
           "$163,000"
       ],
       [
           "Michelle House",
           "Integration Specialist",
           "Sidney",
           "2769",
           "2011\/06\/02",
           "$95,400"
       ],
       [
           "Suki Burks",
           "Developer",
           "London",
           "6832",
           "2009\/10\/22",
           "$114,500"
       ],
       [
           "Prescott Bartlett",
           "Technical Author",
           "London",
           "3606",
           "2011\/05\/07",
           "$145,000"
       ],
       [
           "Gavin Cortez",
           "Team Leader",
           "San Francisco",
           "2860",
           "2008\/10\/26",
           "$235,500"
       ],
       [
           "Martena Mccray",
           "Post-Sales support",
           "Edinburgh",
           "8240",
           "2011\/03\/09",
           "$324,050"
       ],
       [
           "Unity Butler",
           "Marketing Designer",
           "San Francisco",
           "5384",
           "2009\/12\/09",
           "$85,675"
       ],
       [
           "Howard Hatfield",
           "Office Manager",
           "San Francisco",
           "7031",
           "2008\/12\/16",
           "$164,500"
       ],
       [
           "Hope Fuentes",
           "Secretary",
           "San Francisco",
           "6318",
           "2010\/02\/12",
           "$109,850"
       ],
       [
           "Vivian Harrell",
           "Financial Controller",
           "San Francisco",
           "9422",
           "2009\/02\/14",
           "$452,500"
       ],
       [
           "Timothy Mooney",
           "Office Manager",
           "London",
           "7580",
           "2008\/12\/11",
           "$136,200"
       ],
       [
           "Jackson Bradshaw",
           "Director",
           "New York",
           "1042",
           "2008\/09\/26",
           "$645,750"
       ],
       [
           "Olivia Liang",
           "Support Engineer",
           "Singapore",
           "2120",
           "2011\/02\/03",
           "$234,500"
       ],
       [
           "Bruno Nash",
           "Software Engineer",
           "London",
           "6222",
           "2011\/05\/03",
           "$163,500"
       ],
       [
           "Sakura Yamamoto",
           "Support Engineer",
           "Tokyo",
           "9383",
           "2009\/08\/19",
           "$139,575"
       ],
       [
           "Thor Walton",
           "Developer",
           "New York",
           "8327",
           "2013\/08\/11",
           "$98,540"
       ],
       [
           "Finn Camacho",
           "Support Engineer",
           "San Francisco",
           "2927",
           "2009\/07\/07",
           "$87,500"
       ],
       [
           "Serge Baldwin",
           "Data Coordinator",
           "Singapore",
           "8352",
           "2012\/04\/09",
           "$138,575"
       ],
       [
           "Zenaida Frank",
           "Software Engineer",
           "New York",
           "7439",
           "2010\/01\/04",
           "$125,250"
       ],
       [
           "Zorita Serrano",
           "Software Engineer",
           "San Francisco",
           "4389",
           "2012\/06\/01",
           "$115,000"
       ],
       [
           "Jennifer Acosta",
           "Junior Javascript Developer",
           "Edinburgh",
           "3431",
           "2013\/02\/01",
           "$75,650"
       ],
       [
           "Cara Stevens",
           "Sales Assistant",
           "New York",
           "3990",
           "2011\/12\/06",
           "$145,600"
       ],
       [
           "Hermione Butler",
           "Regional Director",
           "London",
           "1016",
           "2011\/03\/21",
           "$356,250"
       ],
       [
           "Lael Greer",
           "Systems Administrator",
           "London",
           "6733",
           "2009\/02\/27",
           "$103,500"
       ],
       [
           "Jonas Alexander",
           "Developer",
           "San Francisco",
           "8196",
           "2010\/07\/14",
           "$86,500"
       ],
       [
           "Shad Decker",
           "Regional Director",
           "Edinburgh",
           "6373",
           "2008\/11\/13",
           "$183,000"
       ],
       [
           "Michael Bruce",
           "Javascript Developer",
           "Singapore",
           "5384",
           "2011\/06\/27",
           "$183,000"
       ],
       [
           "Donna Snider",
           "Customer Support",
           "New York",
           "4226",
           "2011\/01\/25",
           "$112,000"
       ]
    ]


    $scope.dataTableOpt = {
        //custom datatable options 
        // or load data through ajax call also
        "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
    };


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

    $scope.search = function () {
        var params = {
            StartDate: $scope.StartDate,
            EndDate: $scope.EndDate,
            WorkGroupID: $scope.workgroupselected.WorkGroupId
        };
        dataService.GetWorkGroupReport(params).then(function (response) {
            if (response && response.data) {
                $scope.WorkGroupsReport = response.data;
                debugger
            }
        }, function onError() {

        });
    };


   
    $scope.rowCollection = [];
    $scope.rowCollection = [{
        "id": 860,
        "firstName": "Superman",
        "lastName": "Yoda"
    }, {
        "id": 870,
        "firstName": "Foo",
        "lastName": "Whateveryournameis"
    }, {
        "id": 590,
        "firstName": "Toto",
        "lastName": "Titi"
    }, {
        "id": 803,
        "firstName": "Luke",
        "lastName": "Kyle"
    }, {
        "id": 474,
        "firstName": "Toto",
        "lastName": "Bar"
    }, {
        "id": 476,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 464,
        "firstName": "Cartman",
        "lastName": "Kyle"
    }, {
        "id": 505,
        "firstName": "Superman",
        "lastName": "Yoda"
    }, {
        "id": 308,
        "firstName": "Louis",
        "lastName": "Kyle"
    }, {
        "id": 184,
        "firstName": "Toto",
        "lastName": "Bar"
    }, {
        "id": 411,
        "firstName": "Luke",
        "lastName": "Yoda"
    }, {
        "id": 154,
        "firstName": "Luke",
        "lastName": "Moliku"
    }, {
        "id": 623,
        "firstName": "Someone First Name",
        "lastName": "Moliku"
    }, {
        "id": 499,
        "firstName": "Luke",
        "lastName": "Bar"
    }, {
        "id": 482,
        "firstName": "Batman",
        "lastName": "Lara"
    }, {
        "id": 255,
        "firstName": "Louis",
        "lastName": "Kyle"
    }, {
        "id": 772,
        "firstName": "Zed",
        "lastName": "Whateveryournameis"
    }, {
        "id": 398,
        "firstName": "Zed",
        "lastName": "Moliku"
    }, {
        "id": 840,
        "firstName": "Superman",
        "lastName": "Lara"
    }, {
        "id": 894,
        "firstName": "Luke",
        "lastName": "Bar"
    }, {
        "id": 591,
        "firstName": "Luke",
        "lastName": "Titi"
    }, {
        "id": 767,
        "firstName": "Luke",
        "lastName": "Moliku"
    }, {
        "id": 133,
        "firstName": "Cartman",
        "lastName": "Moliku"
    }, {
        "id": 274,
        "firstName": "Toto",
        "lastName": "Lara"
    }, {
        "id": 996,
        "firstName": "Superman",
        "lastName": "Someone Last Name"
    }, {
        "id": 780,
        "firstName": "Batman",
        "lastName": "Kyle"
    }, {
        "id": 931,
        "firstName": "Batman",
        "lastName": "Moliku"
    }, {
        "id": 326,
        "firstName": "Louis",
        "lastName": "Bar"
    }, {
        "id": 318,
        "firstName": "Superman",
        "lastName": "Yoda"
    }, {
        "id": 434,
        "firstName": "Zed",
        "lastName": "Bar"
    }, {
        "id": 480,
        "firstName": "Toto",
        "lastName": "Kyle"
    }, {
        "id": 187,
        "firstName": "Someone First Name",
        "lastName": "Bar"
    }, {
        "id": 829,
        "firstName": "Cartman",
        "lastName": "Bar"
    }, {
        "id": 937,
        "firstName": "Cartman",
        "lastName": "Lara"
    }, {
        "id": 355,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 258,
        "firstName": "Someone First Name",
        "lastName": "Moliku"
    }, {
        "id": 826,
        "firstName": "Cartman",
        "lastName": "Yoda"
    }, {
        "id": 586,
        "firstName": "Cartman",
        "lastName": "Lara"
    }, {
        "id": 32,
        "firstName": "Batman",
        "lastName": "Lara"
    }, {
        "id": 676,
        "firstName": "Batman",
        "lastName": "Kyle"
    }, {
        "id": 403,
        "firstName": "Toto",
        "lastName": "Titi"
    }, {
        "id": 222,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 507,
        "firstName": "Zed",
        "lastName": "Someone Last Name"
    }, {
        "id": 135,
        "firstName": "Superman",
        "lastName": "Whateveryournameis"
    }, {
        "id": 818,
        "firstName": "Zed",
        "lastName": "Yoda"
    }, {
        "id": 321,
        "firstName": "Luke",
        "lastName": "Kyle"
    }, {
        "id": 187,
        "firstName": "Cartman",
        "lastName": "Someone Last Name"
    }, {
        "id": 327,
        "firstName": "Toto",
        "lastName": "Bar"
    }, {
        "id": 187,
        "firstName": "Louis",
        "lastName": "Lara"
    }, {
        "id": 417,
        "firstName": "Louis",
        "lastName": "Titi"
    }, {
        "id": 97,
        "firstName": "Zed",
        "lastName": "Bar"
    }, {
        "id": 710,
        "firstName": "Batman",
        "lastName": "Lara"
    }, {
        "id": 975,
        "firstName": "Toto",
        "lastName": "Yoda"
    }, {
        "id": 926,
        "firstName": "Foo",
        "lastName": "Bar"
    }, {
        "id": 976,
        "firstName": "Toto",
        "lastName": "Lara"
    }, {
        "id": 680,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 275,
        "firstName": "Louis",
        "lastName": "Kyle"
    }, {
        "id": 742,
        "firstName": "Foo",
        "lastName": "Someone Last Name"
    }, {
        "id": 598,
        "firstName": "Zed",
        "lastName": "Lara"
    }, {
        "id": 113,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 228,
        "firstName": "Superman",
        "lastName": "Someone Last Name"
    }, {
        "id": 820,
        "firstName": "Cartman",
        "lastName": "Whateveryournameis"
    }, {
        "id": 700,
        "firstName": "Cartman",
        "lastName": "Someone Last Name"
    }, {
        "id": 556,
        "firstName": "Toto",
        "lastName": "Lara"
    }, {
        "id": 687,
        "firstName": "Foo",
        "lastName": "Kyle"
    }, {
        "id": 794,
        "firstName": "Toto",
        "lastName": "Lara"
    }, {
        "id": 349,
        "firstName": "Someone First Name",
        "lastName": "Whateveryournameis"
    }, {
        "id": 283,
        "firstName": "Batman",
        "lastName": "Someone Last Name"
    }, {
        "id": 862,
        "firstName": "Cartman",
        "lastName": "Lara"
    }, {
        "id": 674,
        "firstName": "Cartman",
        "lastName": "Bar"
    }, {
        "id": 954,
        "firstName": "Louis",
        "lastName": "Lara"
    }, {
        "id": 243,
        "firstName": "Superman",
        "lastName": "Someone Last Name"
    }, {
        "id": 578,
        "firstName": "Superman",
        "lastName": "Lara"
    }, {
        "id": 660,
        "firstName": "Batman",
        "lastName": "Bar"
    }, {
        "id": 653,
        "firstName": "Luke",
        "lastName": "Whateveryournameis"
    }, {
        "id": 583,
        "firstName": "Toto",
        "lastName": "Moliku"
    }, {
        "id": 321,
        "firstName": "Zed",
        "lastName": "Yoda"
    }, {
        "id": 171,
        "firstName": "Superman",
        "lastName": "Kyle"
    }, {
        "id": 41,
        "firstName": "Superman",
        "lastName": "Yoda"
    }, {
        "id": 704,
        "firstName": "Louis",
        "lastName": "Titi"
    }, {
        "id": 344,
        "firstName": "Louis",
        "lastName": "Lara"
    }, {
        "id": 840,
        "firstName": "Toto",
        "lastName": "Whateveryournameis"
    }, {
        "id": 476,
        "firstName": "Foo",
        "lastName": "Kyle"
    }, {
        "id": 644,
        "firstName": "Superman",
        "lastName": "Moliku"
    }, {
        "id": 359,
        "firstName": "Superman",
        "lastName": "Moliku"
    }, {
        "id": 856,
        "firstName": "Luke",
        "lastName": "Lara"
    }, {
        "id": 760,
        "firstName": "Foo",
        "lastName": "Someone Last Name"
    }, {
        "id": 432,
        "firstName": "Zed",
        "lastName": "Yoda"
    }, {
        "id": 299,
        "firstName": "Superman",
        "lastName": "Kyle"
    }, {
        "id": 693,
        "firstName": "Foo",
        "lastName": "Whateveryournameis"
    }, {
        "id": 11,
        "firstName": "Toto",
        "lastName": "Lara"
    }, {
        "id": 305,
        "firstName": "Luke",
        "lastName": "Yoda"
    }, {
        "id": 961,
        "firstName": "Luke",
        "lastName": "Yoda"
    }, {
        "id": 54,
        "firstName": "Luke",
        "lastName": "Bar"
    }, {
        "id": 734,
        "firstName": "Superman",
        "lastName": "Yoda"
    }, {
        "id": 466,
        "firstName": "Cartman",
        "lastName": "Titi"
    }, {
        "id": 439,
        "firstName": "Louis",
        "lastName": "Lara"
    }, {
        "id": 995,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 878,
        "firstName": "Luke",
        "lastName": "Bar"
    }, {
        "id": 479,
        "firstName": "Luke",
        "lastName": "Yoda"
    }, {
        "id": 252,
        "firstName": "Cartman",
        "lastName": "Moliku"
    }, {
        "id": 355,
        "firstName": "Zed",
        "lastName": "Moliku"
    }, {
        "id": 355,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 694,
        "firstName": "Louis",
        "lastName": "Bar"
    }, {
        "id": 882,
        "firstName": "Cartman",
        "lastName": "Yoda"
    }, {
        "id": 620,
        "firstName": "Luke",
        "lastName": "Lara"
    }, {
        "id": 390,
        "firstName": "Superman",
        "lastName": "Lara"
    }, {
        "id": 247,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 510,
        "firstName": "Batman",
        "lastName": "Moliku"
    }, {
        "id": 510,
        "firstName": "Batman",
        "lastName": "Lara"
    }, {
        "id": 472,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 533,
        "firstName": "Someone First Name",
        "lastName": "Kyle"
    }, {
        "id": 725,
        "firstName": "Superman",
        "lastName": "Kyle"
    }, {
        "id": 221,
        "firstName": "Zed",
        "lastName": "Lara"
    }, {
        "id": 302,
        "firstName": "Louis",
        "lastName": "Whateveryournameis"
    }, {
        "id": 755,
        "firstName": "Louis",
        "lastName": "Someone Last Name"
    }, {
        "id": 671,
        "firstName": "Batman",
        "lastName": "Lara"
    }, {
        "id": 649,
        "firstName": "Louis",
        "lastName": "Whateveryournameis"
    }, {
        "id": 22,
        "firstName": "Luke",
        "lastName": "Yoda"
    }, {
        "id": 544,
        "firstName": "Louis",
        "lastName": "Lara"
    }, {
        "id": 114,
        "firstName": "Someone First Name",
        "lastName": "Titi"
    }, {
        "id": 674,
        "firstName": "Someone First Name",
        "lastName": "Lara"
    }, {
        "id": 571,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 554,
        "firstName": "Louis",
        "lastName": "Titi"
    }, {
        "id": 203,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 89,
        "firstName": "Luke",
        "lastName": "Whateveryournameis"
    }, {
        "id": 299,
        "firstName": "Luke",
        "lastName": "Bar"
    }, {
        "id": 48,
        "firstName": "Toto",
        "lastName": "Bar"
    }, {
        "id": 726,
        "firstName": "Batman",
        "lastName": "Whateveryournameis"
    }, {
        "id": 121,
        "firstName": "Toto",
        "lastName": "Bar"
    }, {
        "id": 992,
        "firstName": "Superman",
        "lastName": "Whateveryournameis"
    }, {
        "id": 551,
        "firstName": "Toto",
        "lastName": "Kyle"
    }, {
        "id": 831,
        "firstName": "Louis",
        "lastName": "Lara"
    }, {
        "id": 940,
        "firstName": "Luke",
        "lastName": "Moliku"
    }, {
        "id": 974,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 579,
        "firstName": "Luke",
        "lastName": "Moliku"
    }, {
        "id": 752,
        "firstName": "Cartman",
        "lastName": "Yoda"
    }, {
        "id": 873,
        "firstName": "Batman",
        "lastName": "Someone Last Name"
    }, {
        "id": 939,
        "firstName": "Louis",
        "lastName": "Whateveryournameis"
    }, {
        "id": 240,
        "firstName": "Luke",
        "lastName": "Yoda"
    }, {
        "id": 969,
        "firstName": "Cartman",
        "lastName": "Lara"
    }, {
        "id": 247,
        "firstName": "Luke",
        "lastName": "Someone Last Name"
    }, {
        "id": 3,
        "firstName": "Cartman",
        "lastName": "Whateveryournameis"
    }, {
        "id": 154,
        "firstName": "Batman",
        "lastName": "Bar"
    }, {
        "id": 274,
        "firstName": "Toto",
        "lastName": "Someone Last Name"
    }, {
        "id": 31,
        "firstName": "Luke",
        "lastName": "Someone Last Name"
    }, {
        "id": 789,
        "firstName": "Louis",
        "lastName": "Titi"
    }, {
        "id": 634,
        "firstName": "Zed",
        "lastName": "Yoda"
    }, {
        "id": 972,
        "firstName": "Toto",
        "lastName": "Kyle"
    }, {
        "id": 199,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 562,
        "firstName": "Louis",
        "lastName": "Titi"
    }, {
        "id": 460,
        "firstName": "Superman",
        "lastName": "Yoda"
    }, {
        "id": 817,
        "firstName": "Cartman",
        "lastName": "Someone Last Name"
    }, {
        "id": 307,
        "firstName": "Cartman",
        "lastName": "Bar"
    }, {
        "id": 10,
        "firstName": "Cartman",
        "lastName": "Titi"
    }, {
        "id": 167,
        "firstName": "Toto",
        "lastName": "Someone Last Name"
    }, {
        "id": 107,
        "firstName": "Cartman",
        "lastName": "Whateveryournameis"
    }, {
        "id": 432,
        "firstName": "Batman",
        "lastName": "Kyle"
    }, {
        "id": 381,
        "firstName": "Luke",
        "lastName": "Yoda"
    }, {
        "id": 517,
        "firstName": "Louis",
        "lastName": "Lara"
    }, {
        "id": 575,
        "firstName": "Superman",
        "lastName": "Kyle"
    }, {
        "id": 716,
        "firstName": "Cartman",
        "lastName": "Titi"
    }, {
        "id": 646,
        "firstName": "Foo",
        "lastName": "Whateveryournameis"
    }, {
        "id": 144,
        "firstName": "Someone First Name",
        "lastName": "Yoda"
    }, {
        "id": 306,
        "firstName": "Luke",
        "lastName": "Whateveryournameis"
    }, {
        "id": 395,
        "firstName": "Luke",
        "lastName": "Bar"
    }, {
        "id": 777,
        "firstName": "Toto",
        "lastName": "Moliku"
    }, {
        "id": 624,
        "firstName": "Louis",
        "lastName": "Someone Last Name"
    }, {
        "id": 994,
        "firstName": "Superman",
        "lastName": "Moliku"
    }, {
        "id": 653,
        "firstName": "Batman",
        "lastName": "Moliku"
    }, {
        "id": 198,
        "firstName": "Foo",
        "lastName": "Bar"
    }, {
        "id": 157,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 955,
        "firstName": "Luke",
        "lastName": "Someone Last Name"
    }, {
        "id": 339,
        "firstName": "Foo",
        "lastName": "Bar"
    }, {
        "id": 552,
        "firstName": "Batman",
        "lastName": "Titi"
    }, {
        "id": 735,
        "firstName": "Louis",
        "lastName": "Bar"
    }, {
        "id": 294,
        "firstName": "Batman",
        "lastName": "Bar"
    }, {
        "id": 287,
        "firstName": "Someone First Name",
        "lastName": "Bar"
    }, {
        "id": 399,
        "firstName": "Cartman",
        "lastName": "Yoda"
    }, {
        "id": 741,
        "firstName": "Foo",
        "lastName": "Kyle"
    }, {
        "id": 670,
        "firstName": "Foo",
        "lastName": "Bar"
    }, {
        "id": 260,
        "firstName": "Toto",
        "lastName": "Lara"
    }, {
        "id": 294,
        "firstName": "Toto",
        "lastName": "Titi"
    }, {
        "id": 294,
        "firstName": "Zed",
        "lastName": "Lara"
    }, {
        "id": 840,
        "firstName": "Zed",
        "lastName": "Titi"
    }, {
        "id": 448,
        "firstName": "Foo",
        "lastName": "Kyle"
    }, {
        "id": 260,
        "firstName": "Luke",
        "lastName": "Whateveryournameis"
    }, {
        "id": 119,
        "firstName": "Zed",
        "lastName": "Someone Last Name"
    }, {
        "id": 702,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 87,
        "firstName": "Zed",
        "lastName": "Someone Last Name"
    }, {
        "id": 161,
        "firstName": "Foo",
        "lastName": "Lara"
    }, {
        "id": 404,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 871,
        "firstName": "Toto",
        "lastName": "Lara"
    }, {
        "id": 908,
        "firstName": "Someone First Name",
        "lastName": "Moliku"
    }, {
        "id": 484,
        "firstName": "Louis",
        "lastName": "Bar"
    }, {
        "id": 966,
        "firstName": "Cartman",
        "lastName": "Titi"
    }, {
        "id": 392,
        "firstName": "Someone First Name",
        "lastName": "Lara"
    }, {
        "id": 738,
        "firstName": "Batman",
        "lastName": "Lara"
    }, {
        "id": 560,
        "firstName": "Louis",
        "lastName": "Kyle"
    }, {
        "id": 507,
        "firstName": "Zed",
        "lastName": "Whateveryournameis"
    }, {
        "id": 660,
        "firstName": "Louis",
        "lastName": "Whateveryournameis"
    }, {
        "id": 929,
        "firstName": "Superman",
        "lastName": "Moliku"
    }, {
        "id": 42,
        "firstName": "Batman",
        "lastName": "Moliku"
    }, {
        "id": 853,
        "firstName": "Luke",
        "lastName": "Titi"
    }, {
        "id": 977,
        "firstName": "Louis",
        "lastName": "Moliku"
    }, {
        "id": 104,
        "firstName": "Toto",
        "lastName": "Kyle"
    }, {
        "id": 820,
        "firstName": "Luke",
        "lastName": "Someone Last Name"
    }, {
        "id": 187,
        "firstName": "Batman",
        "lastName": "Titi"
    }, {
        "id": 524,
        "firstName": "Louis",
        "lastName": "Yoda"
    }, {
        "id": 830,
        "firstName": "Cartman",
        "lastName": "Whateveryournameis"
    }, {
        "id": 156,
        "firstName": "Someone First Name",
        "lastName": "Lara"
    }, {
        "id": 918,
        "firstName": "Foo",
        "lastName": "Whateveryournameis"
    }, {
        "id": 286,
        "firstName": "Batman",
        "lastName": "Moliku"
    }, {
        "id": 715,
        "firstName": "Louis",
        "lastName": "Kyle"
    }, {
        "id": 501,
        "firstName": "Superman",
        "lastName": "Whateveryournameis"
    }, {
        "id": 463,
        "firstName": "Foo",
        "lastName": "Kyle"
    }, {
        "id": 419,
        "firstName": "Toto",
        "lastName": "Yoda"
    }, {
        "id": 752,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 754,
        "firstName": "Louis",
        "lastName": "Titi"
    }, {
        "id": 497,
        "firstName": "Someone First Name",
        "lastName": "Kyle"
    }, {
        "id": 722,
        "firstName": "Louis",
        "lastName": "Moliku"
    }, {
        "id": 986,
        "firstName": "Batman",
        "lastName": "Someone Last Name"
    }, {
        "id": 908,
        "firstName": "Someone First Name",
        "lastName": "Titi"
    }, {
        "id": 559,
        "firstName": "Superman",
        "lastName": "Bar"
    }, {
        "id": 816,
        "firstName": "Foo",
        "lastName": "Bar"
    }, {
        "id": 517,
        "firstName": "Louis",
        "lastName": "Bar"
    }, {
        "id": 188,
        "firstName": "Superman",
        "lastName": "Bar"
    }, {
        "id": 762,
        "firstName": "Batman",
        "lastName": "Someone Last Name"
    }, {
        "id": 872,
        "firstName": "Batman",
        "lastName": "Titi"
    }, {
        "id": 107,
        "firstName": "Louis",
        "lastName": "Lara"
    }, {
        "id": 968,
        "firstName": "Louis",
        "lastName": "Moliku"
    }, {
        "id": 643,
        "firstName": "Toto",
        "lastName": "Someone Last Name"
    }, {
        "id": 88,
        "firstName": "Toto",
        "lastName": "Titi"
    }, {
        "id": 844,
        "firstName": "Foo",
        "lastName": "Kyle"
    }, {
        "id": 334,
        "firstName": "Batman",
        "lastName": "Someone Last Name"
    }, {
        "id": 43,
        "firstName": "Zed",
        "lastName": "Lara"
    }, {
        "id": 600,
        "firstName": "Someone First Name",
        "lastName": "Kyle"
    }, {
        "id": 719,
        "firstName": "Luke",
        "lastName": "Lara"
    }, {
        "id": 698,
        "firstName": "Zed",
        "lastName": "Yoda"
    }, {
        "id": 994,
        "firstName": "Zed",
        "lastName": "Whateveryournameis"
    }, {
        "id": 595,
        "firstName": "Someone First Name",
        "lastName": "Someone Last Name"
    }, {
        "id": 223,
        "firstName": "Toto",
        "lastName": "Yoda"
    }, {
        "id": 392,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 972,
        "firstName": "Toto",
        "lastName": "Whateveryournameis"
    }, {
        "id": 155,
        "firstName": "Louis",
        "lastName": "Whateveryournameis"
    }, {
        "id": 956,
        "firstName": "Louis",
        "lastName": "Yoda"
    }, {
        "id": 62,
        "firstName": "Foo",
        "lastName": "Kyle"
    }, {
        "id": 689,
        "firstName": "Superman",
        "lastName": "Titi"
    }, {
        "id": 46,
        "firstName": "Foo",
        "lastName": "Someone Last Name"
    }, {
        "id": 401,
        "firstName": "Toto",
        "lastName": "Someone Last Name"
    }, {
        "id": 658,
        "firstName": "Louis",
        "lastName": "Bar"
    }, {
        "id": 375,
        "firstName": "Someone First Name",
        "lastName": "Bar"
    }, {
        "id": 877,
        "firstName": "Toto",
        "lastName": "Someone Last Name"
    }, {
        "id": 923,
        "firstName": "Cartman",
        "lastName": "Lara"
    }, {
        "id": 37,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 416,
        "firstName": "Cartman",
        "lastName": "Yoda"
    }, {
        "id": 546,
        "firstName": "Zed",
        "lastName": "Yoda"
    }, {
        "id": 282,
        "firstName": "Luke",
        "lastName": "Lara"
    }, {
        "id": 943,
        "firstName": "Superman",
        "lastName": "Yoda"
    }, {
        "id": 319,
        "firstName": "Foo",
        "lastName": "Whateveryournameis"
    }, {
        "id": 390,
        "firstName": "Louis",
        "lastName": "Lara"
    }, {
        "id": 556,
        "firstName": "Luke",
        "lastName": "Kyle"
    }, {
        "id": 255,
        "firstName": "Cartman",
        "lastName": "Whateveryournameis"
    }, {
        "id": 80,
        "firstName": "Zed",
        "lastName": "Kyle"
    }, {
        "id": 760,
        "firstName": "Louis",
        "lastName": "Moliku"
    }, {
        "id": 291,
        "firstName": "Louis",
        "lastName": "Titi"
    }, {
        "id": 916,
        "firstName": "Louis",
        "lastName": "Bar"
    }, {
        "id": 212,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 445,
        "firstName": "Luke",
        "lastName": "Whateveryournameis"
    }, {
        "id": 101,
        "firstName": "Someone First Name",
        "lastName": "Someone Last Name"
    }, {
        "id": 565,
        "firstName": "Superman",
        "lastName": "Kyle"
    }, {
        "id": 304,
        "firstName": "Luke",
        "lastName": "Someone Last Name"
    }, {
        "id": 557,
        "firstName": "Foo",
        "lastName": "Titi"
    }, {
        "id": 544,
        "firstName": "Toto",
        "lastName": "Kyle"
    }, {
        "id": 244,
        "firstName": "Zed",
        "lastName": "Titi"
    }, {
        "id": 464,
        "firstName": "Someone First Name",
        "lastName": "Bar"
    }, {
        "id": 225,
        "firstName": "Toto",
        "lastName": "Titi"
    }, {
        "id": 727,
        "firstName": "Superman",
        "lastName": "Someone Last Name"
    }, {
        "id": 735,
        "firstName": "Louis",
        "lastName": "Bar"
    }, {
        "id": 334,
        "firstName": "Foo",
        "lastName": "Lara"
    }, {
        "id": 982,
        "firstName": "Batman",
        "lastName": "Kyle"
    }, {
        "id": 48,
        "firstName": "Batman",
        "lastName": "Lara"
    }, {
        "id": 175,
        "firstName": "Luke",
        "lastName": "Moliku"
    }, {
        "id": 885,
        "firstName": "Louis",
        "lastName": "Moliku"
    }, {
        "id": 675,
        "firstName": "Toto",
        "lastName": "Moliku"
    }, {
        "id": 47,
        "firstName": "Superman",
        "lastName": "Someone Last Name"
    }, {
        "id": 105,
        "firstName": "Toto",
        "lastName": "Titi"
    }, {
        "id": 616,
        "firstName": "Cartman",
        "lastName": "Lara"
    }, {
        "id": 134,
        "firstName": "Someone First Name",
        "lastName": "Someone Last Name"
    }, {
        "id": 26,
        "firstName": "Foo",
        "lastName": "Moliku"
    }, {
        "id": 134,
        "firstName": "Toto",
        "lastName": "Whateveryournameis"
    }, {
        "id": 680,
        "firstName": "Zed",
        "lastName": "Lara"
    }, {
        "id": 208,
        "firstName": "Luke",
        "lastName": "Someone Last Name"
    }, {
        "id": 233,
        "firstName": "Someone First Name",
        "lastName": "Moliku"
    }, {
        "id": 131,
        "firstName": "Louis",
        "lastName": "Moliku"
    }, {
        "id": 87,
        "firstName": "Toto",
        "lastName": "Yoda"
    }, {
        "id": 356,
        "firstName": "Batman",
        "lastName": "Kyle"
    }, {
        "id": 39,
        "firstName": "Louis",
        "lastName": "Whateveryournameis"
    }, {
        "id": 867,
        "firstName": "Batman",
        "lastName": "Lara"
    }, {
        "id": 382,
        "firstName": "Someone First Name",
        "lastName": "Bar"
    }];

    $scope.schedule = $scope.rowCollection;
    $scope.singledayschedule = [];

    angular.forEach($scope.schedule, function (value, key) {
        angular.forEach(value, function (value, key) {
            debugger
            $scope.singledayschedule.push(value);
        });
    });
}])
