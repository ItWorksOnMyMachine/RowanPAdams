var services = angular.getOrCreateModule('ApplicationServices');

services.service('LogService', [
	'$http', '$q', 'SessionService', function ($http, $q, SessionService) {
		this.getLogEntriesForMonth = function (date) {
			var result = $q.defer();

			$http({
				method: 'GET',
				url: window.apiUrl + '/api/logentries/month?monthStartingOn=' + moment(date).format('MM/01/YYYY'),
				headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + SessionService.getToken() }
			}).then(function (response) {
				result.resolve(response);
			}, function (response) {
				result.reject(response);
			});

			return result.promise;
		};

		this.getAllLogEntries = function () {
			var result = $q.defer();

			$http({
				method: 'GET',
				url: window.apiUrl + '/api/logentries/all',
				headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + SessionService.getToken() }
			}).then(function (response) {
				result.resolve(response);
			}, function (response) {
				result.reject(response);
			});

			return result.promise;
		};

		this.postLogEntry = function (logEntry) {
			var result = $q.defer();

			$http({
				method: 'POST',
				url: window.apiUrl + '/api/logentries',// + chore.Id,
				headers: { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + SessionService.getToken() },
				data: logEntry
			}).then(function (response) {
				result.resolve(response);
			}, function (response) {
				result.reject(response);
			});

			return result.promise;
		};
	}
]);