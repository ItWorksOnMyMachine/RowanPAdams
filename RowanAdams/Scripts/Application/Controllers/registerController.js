var controllers = angular.getOrCreateModule('ApplicationControllers');

controllers.controller('RegisterController', [
	'$scope', '$state', 'LoginFactory', 'RegistrationFactory', 'SessionService', function($scope, $state, LoginFactory, RegistrationFactory, SessionService) {
		$scope.registerForm = {
			username: undefined,
			password: undefined,
			confirmPassword: undefined,
			errorMessage: undefined,
			buttonDisabled: false
		};

		$scope.register = function () {
			$scope.buttonDisabled = true;
			RegistrationFactory($scope.registerForm.username, $scope.registerForm.password, $scope.registerForm.confirmPassword)
				.then(function() {
					LoginFactory($scope.registerForm.username, $scope.registerForm.password)
						.then(function(response) {
							SessionService.setToken(response.access_token);
							$state.transitionTo('home');
						}, function(response) {
							$scope.registerForm.errorMessage = response;
						});
				}, function(response) {
					$scope.registerForm.errorMessage = response;
					$scope.buttonDisabled = false;
				});
		}
	}
]);