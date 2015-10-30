var controllers = angular.getOrCreateModule('ApplicationControllers');

controllers.controller('LoginController', ['$scope', '$state', 'LoginFactory', 'SessionService', function($scope, $state, LoginFactory, SessionService) {
		$scope.loginForm = {
			username: undefined,
			password: undefined,
			errorMessage: undefined,
			buttonDisabled: false
		};

		$scope.login = function () {
			$scope.buttonDisabled = true;
			LoginFactory($scope.loginForm.username, $scope.loginForm.password)
				.then(function(response) {
					SessionService.setToken(response.access_token);
					if ($state.params['returnUrl'])
						$state.transitionTo($state.params['returnUrl']);
					else
						$state.transitionTo('home');
				}, function(response) {
					$scope.loginForm.errorMessage = response.error_description;
					$scope.buttonDisabled = false;
				});
		}
	}
]);