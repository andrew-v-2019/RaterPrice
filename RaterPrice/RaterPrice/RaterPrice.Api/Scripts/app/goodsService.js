goodsApp.factory("GoodsService", function ($http, $q) {
    return {
        GetGoods: function () {
            // Get the deferred object
            var deferred = $q.defer();
            // Initiates the AJAX call
            $http({ method: 'GET', url: '/goodsapi/Getbybarcode/67' }).success(deferred.resolve).error(deferred.reject);
            // Returns the promise - Contains result once request completes
            return deferred.promise;
        }
    }
});