angular.module('bsk-app')
.controller('LoginCtrl', function ($scope, $http, $location, UserService, REST_API_ADDRESS) {
    if (UserService.getLoggedUser()) {
        $location.url('/');
    }

    $scope.loading = false;

    $scope.form = {
        login: '',
        password: ''
    };
});