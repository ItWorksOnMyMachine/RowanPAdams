var controllers = angular.getOrCreateModule('ApplicationControllers');

controllers.controller('SiteController', ['$scope', 'SessionService', '$state', function ($scope, SessionService, $state) {
	$scope.models = {
		isBound: true
	};
	$scope.navbarProperties = {
		isCollapsed: true
	};
	$scope.loggedIn = function() {
		return SessionService.getToken() !== undefined;
	};
	$state.transitionTo('home');
}]);