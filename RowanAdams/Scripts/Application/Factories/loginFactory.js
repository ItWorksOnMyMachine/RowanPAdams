var factories = angular.getOrCreateModule('ApplicationFactories');

factories.factory('LoginFactory', [
	'$http', '$q', function($http, $q) {
		return function(username, password) {
			var result = $q.defer();

			var params = { grant_type: "password", userName: username, password: password };

			$http({
				method: 'POST',
				url: window.apiUrl + '/token',
				transformRequest: function(obj) {
					var str = [];
					for (var p in obj)
						str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
					return str.join("&");
				},
				data: params,
				headers: { 'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8;' }
			}).success(function(response) {
				result.resolve(response);
			}).error(function(response) {
				result.reject(response);
			});

			return result.promise;
		}
	}
]);