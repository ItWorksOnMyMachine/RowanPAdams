var factories = angular.getOrCreateModule('ApplicationFactories');

factories.factory('ValuesFactory', [
	'$http', '$q', 'SessionService', function($http, $q, SessionService) {
		return function() {
			var result = $q.defer();

			$http({
					method: 'GET',
					url: window.apiUrl + '/api/Values',
					headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + SessionService.getToken() }
				})
				.success(function(response) {
					result.resolve(response);
				})
				.error(function(response) {
					result.reject(response);
				});

			return result.promise;
		}
	}
]);