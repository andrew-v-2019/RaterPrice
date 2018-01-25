var goodsController = function ($scope, GoodsService) {

    GoodsService.GetGoods().then(function (d) { console.log(d)}, function ()
    { alert('error while fetching talks from server') })
    $scope.state = "I work";
}