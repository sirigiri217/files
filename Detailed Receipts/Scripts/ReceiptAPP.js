var app = angular.module('ReceiptApp', ['ngRoute', 'ngAnimate', 'long2know', 'ngMaterial', 'cgBusy', 'ui.bootstrap', 'ngMessages', 'material.svgAssetsCache']);
app.controller('HomeController', function ($scope, $http, $filter, $mdMedia, $timeout, $q, $mdSidenav) {

   

   // $scope.envurl = ''; //local
    $scope.envurl = '/Receipts_dev'; //dev
    // $scope.envurl = '/Receipts'; // prod

    $scope.refreshdate = '';
    $scope.nonsupplier = true;
   
    $scope.dropdownloader = true;
  
    $scope.myDate = new Date();
   
   
    $scope.minDate = new Date(
        2015,
        01,
        01
    );
    $scope.fromdate = $scope.minDate;
    $scope.maxDate = new Date(
        $scope.myDate.getFullYear(),
        $scope.myDate.getMonth(),
        $scope.myDate.getDate()-1
    );

    window.onbeforeunload = function () { window.scrollTo(0, 0); };
    $scope.openLeftMenu = function () {
        $mdSidenav('left').toggle();
    };

    $scope.openRightMenu = function () {
        $mdSidenav('right').toggle();
    };

    $scope.Getdropdownvalues = function () {
        $scope.dropdownloader = true;
        $http({
            method: 'get',
            url: $scope.envurl + '/api/values',
            //url: '/OpenPO_dev/api/values',

            headers: { 'Access-Control-Allow-Origin': '*' }
        }).then(function (response) {
            console.log(angular.fromJson(response.data));

           
            $scope.dropdowndata = response.data.plantcd;
           
            $scope.plantcdoptions = angular.fromJson(response.data.plantcd);
            $scope.suppliersitenameoptions = angular.fromJson(response.data.suppliersitename);
            $scope.ParentSupplierNameoptions = angular.fromJson(response.data.ParentSupplierName);
            $scope.dropdownloader = false;
        }, function (error) {
            console.log(error, 'can not get dropdown values.');
            $scope.dropdownloader = false;
            alert('Error in loading dropdwon content please reload')
        });

    };
    $scope.Getdropdownvalues();

    $scope.GetReport = function (val) {

        console.log(val)
        var materialnumselect = $scope.materialnum;
        if (typeof (materialnumselect) === "undefined") { materialnumselect = '' }
        else { materialnumselect = materialnumselect.replace(/\n/g, ",").replace(" ", "");}

        var plantcd = [$scope.plantcdselection];
        var plantcdselect = '';
        for (i = 0; i < plantcd[0].length; i++) {

            plantcdselect += plantcd[0][i].plant_Code + '#';
        }
        plantcdselect = plantcdselect.replace(/#\s*$/, "");


        var suppliersitename = [$scope.suppliersitenameselection];
        var suppliersitenameselect = '';
        for (i = 0; i < suppliersitename[0].length; i++) {

            suppliersitenameselect += suppliersitename[0][i].Supplier_Site_Name.replace("'", "''''") + '#';

        }
        suppliersitenameselect = suppliersitenameselect.replace(/#\s*$/, "");

        var ParentSupplierName = [$scope.ParentSupplierNameselection];
        var ParentSupplierNameselect = '';
        for (i = 0; i < ParentSupplierName[0].length; i++) {

            ParentSupplierNameselect += ParentSupplierName[0][i].ParentSupplierName.replace("'", "''''") + '#';

        }
        ParentSupplierNameselect = ParentSupplierNameselect.replace(/#\s*$/, "");

        //var ttt = $filter('date')($scope.todate, "MM-dd-yyyy"); 

        //console.log(typeof(ttt),'todate');

        var fromd = new Date($scope.fromdate);
        console.log(fromd);
        var tod = new Date($scope.todate);
        console.log(tod);

        if (fromd > tod) {
            alert('From Date should less than To Date');
        }
        else if (fromd <= tod) {

             
            var dataflag=''
            

            if (val == 1) {
                console.log('Summary');
                dataflag = 'Summary'
                
            }
            else if (val == 2) {
                console.log('Detailed');
                dataflag = 'Detailed'
            }

            var data = { dataflag: dataflag, materialnum: materialnumselect, plantcd: plantcdselect, suppliersitename: suppliersitenameselect, parentsuppliername: ParentSupplierNameselect, fromdate: $filter('date')($scope.fromdate, "MM-dd-yyyy"), todate: $filter('date')($scope.todate, "MM-dd-yyyy") }
            console.log(data);

            $http({
                method: 'Post',
                url: $scope.envurl + '/api/values',
                data: data,
               

                headers: { 'Access-Control-Allow-Origin': '*' }
            }).then(function (response) {
                console.log(angular.fromJson(response.data));
                
                $scope.loadingstatus = false;


                if (dataflag == 'Summary') {
                    if (angular.fromJson(response.data.Summary).length === 0) { alert('No Data Available'); }
                    else {
                        window.open($scope.envurl + '/Handler.ashx?action=Summarydownload&id=' + $scope.loggedinuser, '_blank', '', '');
                    }

                }
                else if (dataflag == 'Detailed') {
                    if (angular.fromJson(response.data.Detailed).length === 0) { alert('No Data Available'); }
                    else {
                        window.open($scope.envurl + '/Handler.ashx?action=Detaileddownload&id=' + $scope.loggedinuser, '_blank', '', '');
                    }

                }
                //$scope.controlsshow = false;
                //if (angular.fromJson(response.data).length === 0
                //) {
                //    $scope.norecords = true;
                //    $scope.downloadTableDataBtn = true;
                //    $scope.showgrid = false;
                //    $scope.controlsshow = true;
                //}
                //else {
                //    $scope.norecords = false;

                //    $scope.showgrid = true;
                //    $scope.downloadTableDataBtn = false;

                //}

            }, function (error) {
                console.log(error, 'can not get report data.');
            });




        }

       






    };

    $scope.resetDetails = function () {
        $scope.dropdownloader = true;
        $scope.loadingstatus = false;
        $scope.showgrid = false;
        $scope.norecords = false;
        $scope.downloadTableDataBtn = true;

        $scope.ponum = '';
        $scope.materialnum = '';
        $scope.plantcdselection = [];
        $scope.pripbgselection = [];
        $scope.potypeselection = [];
        $scope.suppliersitenameselection = [];
       
        $scope.ParentSupplierNameselection = [];
        $scope.dropdownloader = false;
        $scope.todate = new Date($scope.refreshdate);
        $scope.fromdate = new Date($scope.minDate);

    };

    function getUsername() {

        $http({
            method: 'get',
            url: $scope.envurl + '/api/values/1',


            headers: { 'Access-Control-Allow-Origin': '*' }
        }).then(function (response) {
            console.log(angular.fromJson(response.data));
            var result = angular.fromJson(response.data);
            console.log(result[0].nt_name);
            $scope.loggedinuser = result[0].nt_name;
            if (result.length > 0) {
                console.log(result[0].nt_name);

                $scope.loggedinuser = result[0].nt_name;
                $scope.nonsupplier = true;
            }
            else { $scope.nonsupplier = false; }

        }, function (error) {
            console.log(error, 'can not get username.');
        });


    };
    getUsername();

    function getrefreshdate() {

        $http({
            method: 'get',
            url: $scope.envurl + '/api/values/4',


            headers: { 'Access-Control-Allow-Origin': '*' }
        }).then(function (response) {
            console.log(angular.fromJson(response));
            var result = angular.fromJson(response.data);
            console.log(result[0].refreshdate);
            if (result[0].refreshdate.length > 0) {
                $scope.refreshdate = result[0].refreshdate.substring(0, 10);
                $scope.todate = new Date($scope.refreshdate);
               // $scope.fromdate = new Date($scope.refreshdate);
                $scope.maxDate = new Date($scope.refreshdate);
            }

        }, function (error) {
            console.log(error, 'can not get refreshdate.');
        });


    };
    getrefreshdate();
    function logaccess() {
        $http({
            method: 'get',
            url: $scope.envurl + '/api/values/2',


            headers: { 'Access-Control-Allow-Origin': '*' }
        }).then(function (response) {
            console.log(angular.fromJson(response.data));



        }, function (error) {
            console.log(error, 'can not log access .');
        });
    };
    logaccess();

});

