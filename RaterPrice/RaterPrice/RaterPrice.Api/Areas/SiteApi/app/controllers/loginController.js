'use strict';
angular.module('rpApp')
  .controller('loginController', ['$scope', '$state', 'User', function ($scope, $state, User) {
      $scope.username = '';
      $scope.password = '';
      $scope.errors = [];
      $scope.loginDisabled = false;

      function disableLoginButton() {
          $scope.loginDisabled = true;
      }

      function enableLoginButton(message) {
          $scope.loginDisabled = false;
      }

      function onSuccessfulLogin() {
          $state.go('main');
      }

      function onFailedLogin(error) {
          if (error['Message']) {
              $scope.errors.push(error['Message']);
          }
          if (error.ModelState) {
              if (error.ModelState['model.UserName'][0]) {
                  $scope.errors.push(error.ModelState['model.UserName'][0]);
              }
              if (error.ModelState['model.Password'][0]) {
                  $scope.errors.push(error.ModelState['model.Password'][0]);
              }            
          }
          else {
              if (typeof error === 'string' && $scope.errors.indexOf(error) === -1) {
                  $scope.errors.push(error);
              }
          }
          enableLoginButton();
      }

      $scope.login = function () {
          $scope.errors = [];
          disableLoginButton();
          User.authenticate($scope.username, $scope.password, onSuccessfulLogin, onFailedLogin);
      };
  }]);