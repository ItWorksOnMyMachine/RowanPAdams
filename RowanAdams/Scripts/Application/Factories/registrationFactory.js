var factories = angular.getOrCreateModule('ApplicationFactories');

factories.factory('RegistrationFactory', [
	'$http', '$q', 'SessionService', function($http, $q, SessionService) {
		return function(email, password, confirmPassword) {
			var result = $q.defer();

			$http({
					method: 'POST',
					url: window.apiUrl + '/api/Account/Register',
					data: { Email: email, Password: password, ConfirmPassword: confirmPassword },
					headers: { 'Content-Type': 'application/json' }
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