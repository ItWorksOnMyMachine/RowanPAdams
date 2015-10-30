var controllers = angular.getOrCreateModule('ApplicationControllers');

controllers.controller('ValuesController', [
	'$scope', 'ValuesFactory', function($scope, ValuesFactory) {
		$scope.values = [];
		$scope.error = {
			message: undefined
		};

		$scope.getValues = function() {
			ValuesFactory()
				.then(function(response) {
					$scope.values = response;
				}, function(response) {
					$scope.error.message = response.Message;
				});
		}
	}
]);