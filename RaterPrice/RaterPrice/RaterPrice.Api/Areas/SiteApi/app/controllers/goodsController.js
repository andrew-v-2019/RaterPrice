
angular.module('rpApp').controller('goodsController', ['$scope', 'goodsService', 'filterModel',
 function ($scope, goodsService, filterModel) {

     $scope.filter = new filterModel();
     $scope.filter.Popular = true;
     $scope.goods = null;


     goodsService.GetGoods($scope.filter).then(function (d) { $scope.goods = d, console.log($scope.goods) }, function () {
         console.log("error while fetching talks from server");
     });
     $scope.state = "I work";
 }]);
