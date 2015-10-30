var services = angular.getOrCreateModule('ApplicationServices');

services.service('ChoresService', [
	'$http', '$q', 'SessionService', function ($http, $q, SessionService) {
		function getChores(url) {
			var result = $q.defer();

			$http({
				method: 'GET',
				url: url,
				headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + SessionService.getToken() }
			}).then(function (response) {
				result.resolve(response);
			}, function (response) {
				result.reject(response);
			});

			return result.promise;
		};

		this.getAllChores = function () {
			return getChores(window.apiUrl + '/api/chores/all');
		};

		this.getActiveChores = function () {
			return getChores(window.apiUrl + '/api/chores/active');
		};

		this.putChore = function (chore) {
			var result = $q.defer();

			$http({
				method: 'PUT',
				url: window.apiUrl + '/api/chores',// + chore.Id,
				headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + SessionService.getToken() },
				data: chore
			}).then(function (response) {
				result.resolve(response);
			},function (response) {
				result.reject(response);
			});

			return result.promise;
		};

		this.postChore = function (chore) {
			var result = $q.defer();

			$http({
				method: 'POST',
				url: window.apiUrl + '/api/chores',// + chore.Id,
				headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + SessionService.getToken() },
				data: chore
			}).then(function (response) {
				result.resolve(response);
			}, function (response) {
				result.reject(response);
			});

			return result.promise;
		};
	}
]);