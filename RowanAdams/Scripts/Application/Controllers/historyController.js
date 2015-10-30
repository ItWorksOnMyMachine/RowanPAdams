var controllers = angular.getOrCreateModule('ApplicationControllers');

controllers.controller('HistoryController', [
	'$scope', 'LogService', function ($scope, LogService) {
		function getLogEntries() {
			LogService.getAllLogEntries().then(function (response) {
				$scope.logEntryGroupings = response.data;
			}, function (response) {
				$scope.error.message = response.statusText;
			});
		};
		
		$scope.logEntryGroupings = [];
		$scope.error = {
			message: undefined
		};
		
		getLogEntries();
	}
]);