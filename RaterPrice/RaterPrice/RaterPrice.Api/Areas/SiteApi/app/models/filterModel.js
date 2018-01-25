
angular.module('rpApp').service("filterModel", [
    function() {

        var filterModel = function() {
            this.Popular = true;
            this.Take = 20;
            this.Page = 0;
            this.Barcode = "";
            this.Name = "";
        };

        filterModel.prototype.clear = function() {
            this.Popular = true;
            this.Take = 20;
            this.Page = 1;
            this.Barcode = "";
            this.Name = "";
        };

        filterModel.prototype.fill = function(obj) {
            if (obj) {
                var keys = Object.keys(obj), i, key;
                for (i = 0; i < keys.length; i++) {
                    key = keys[i];

                    //TODO: ref
                    if (angular.isArray(this[key])) {
                        this[key] = obj[key].split(",");
                    } else {
                        this[key] = obj[key];
                    }
                }
            }
        };

        filterModel.prototype.getQueryString = function() {
            var keys = Object.keys(this), query = "?", i, key;

            for (i = 0; i < keys.length; i++) {
                key = keys[i];
                if (typeof this[key] !== "function" && this[key]) {
                    query = query + key + "=" + this[key] + "&";
                }
            }

            return query.substring(0, query.length - 1);
        };

        return filterModel;
    }
]);