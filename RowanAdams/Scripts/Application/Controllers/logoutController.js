var controllers = angular.getOrCreateModule('ApplicationControllers');

controllers.controller('LogoutController', ['$state', 'SessionService', function ($state, SessionService) {
	SessionService.logout();
	$state.transitionTo('home');
}]);