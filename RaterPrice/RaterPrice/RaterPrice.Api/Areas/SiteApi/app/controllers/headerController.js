'use strict';

angular.module('rpApp')
  .controller('headerController', ['$scope', 'User', function ($scope, User) {
      $scope.user = User.getUserData();
  }]);