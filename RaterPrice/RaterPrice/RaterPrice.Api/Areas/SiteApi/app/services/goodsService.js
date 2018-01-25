angular.module('rpApp').factory("goodsService", function ($http, $q) {
    return {
        GetGoods: function (filter) {
            var deferred = $q.defer();
            console.log(filter);
            $http.get("/siteapi/goodsapi/getgoods", { params: filter }).success(deferred.resolve).error(deferred.reject);
            return deferred.promise;
        }
    };
});