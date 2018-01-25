'use strict';
angular.module('rpApp')
  .controller('registerController', ['$scope', '$state', 'User', function ($scope, $state, User) {
      $scope.hasRegistrationError = false;
      $scope.registrationSuccess = false;
      $scope.regDisabled = false;
      $scope.registerEmail = '';
      $scope.registerPassword = '';
      $scope.registerPassword2 = '';
      $scope.registrationErrorDescription = [];

      function onSuccessfulRegistration() {
          $scope.registrationSuccess = true;
          $scope.hasRegistrationError = false;
      }

      function onFailedRegistration(data) {
          $scope.registrationErrorDescription = [];
          $scope.hasRegistrationError = true;
          var errorMessage = data.Message;
          debugger;
          if (data.ModelState) {
              if (data.ModelState['model.RegisterEmail'])
                  $scope.registrationErrorDescription.push(data.ModelState['model.RegisterEmail'][0].trim());
              if (data.ModelState['model.RegisterPassword'])
                  $scope.registrationErrorDescription.push(data.ModelState['model.RegisterPassword'][0]);
              if (data.ModelState['model.RegisterPassword2'])
                  $scope.registrationErrorDescription.push(data.ModelState['model.RegisterPassword2'][0]);
          }
          if ($scope.registrationErrorDescription.length == 0) {
              $scope.registrationErrorDescription.push(errorMessage);
          }
          $scope.enableRegButton();
      }

      $scope.disableRegButton = function () {
          $scope.regDisabled = true;
      }

      $scope.enableRegButton = function () {
          $scope.regDisabled = false;
      }

      $scope.register = function () {
          if ($scope.regDisabled == false) {
              var data = {
                  RegisterEmail: $scope.registerEmail,
                  RegisterPassword: $scope.registerPassword,
                  RegisterPassword2: $scope.registerPassword2
              };
              $scope.registrationErrorDescription = [];
              $scope.disableRegButton();
              User.register(data, onSuccessfulRegistration, onFailedRegistration);
          }
      };

  }]);