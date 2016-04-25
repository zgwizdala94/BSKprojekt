angular.module('bsk-app', ['ngResource', 'ngRoute', 'LocalStorageModule', 'datePicker'])
.config(['$routeProvider', '$locationProvider',
    function ($routeProvider, $locationProvider) {
        $locationProvider.html5Mode(false);
        $routeProvider
            .when('/', {
                templateUrl: 'Views/LogIn/Login.cshtml',
                controller: 'LoginCtrl'
            })

            .when('/logout', {
                templateUrl: 'modules/main/logout.html',
                controller: 'LogoutCtrl'
            })


            .otherwise({
                redirectTo: '/'
            });
    }])