var application = angular.module('Application', ['ui.router', 'ui.bootstrap', 'ngCookies', 'ApplicationControllers', 'ApplicationServices', 'ApplicationFactories']);

application.config(['$stateProvider', '$httpProvider', '$locationProvider', function ($stateProvider, $httpProvider, $locationProvider) {

		$locationProvider.hashPrefix('!').html5Mode(true);

		$stateProvider.state('home', {
			url: '/home',
			views: {
				"mainContainer": {
					templateUrl: 'Home/Home'
				}
			}
		}).state('log', {
			url: '/log',
			views: {
				"mainContainer": {
					templateUrl: 'Home/Log'
				}
			}
		}).state('chores', {
			url: '/chores',
			views: {
				"mainContainer": {
					templateUrl: 'Home/Chores'
				}
			}
		}).state('history', {
			url: '/history',
			views: {
				"mainContainer": {
					templateUrl: 'Home/History'
				}
			}
		}).state('login', {
			url: '/login?returnUrl',
			views: {
				"mainContainer": {
					templateUrl: 'Account/Login',
					//controller: 'LoginController'
				}
			}
		}).state('register', {
			url: '/register?returnUrl',
			views: {
				"mainContainer": {
					templateUrl: 'Account/Register',
					//controller: 'RegisterController'
				}
			}
		}).state('logout', {
			url: '/logout',
			views: {
				"mainContainer": {
					templateUrl: 'Home/Home',
					controller: 'LogoutController'
				}
			}
		});

		$httpProvider.interceptors.push('AuthRequestInterceptor');
		$httpProvider.interceptors.push('AuthResponseInterceptor');
	}
]);

/*
.state('stateOne', {
	url: '/stateOne',
	views: {
		"mainContainer": {
			templateUrl: 'Home/One'
		}
	}
}).state('stateTwo', {
	url: '/stateTwo?num',
	views: {
		"mainContainer": {
			templateUrl: function(params) { return 'Home/Two?num=' + params.num; }
		}
	}
}).state('stateThree', {
	url: '/stateThree',
	views: {
		"mainContainer": {
			templateUrl: 'Home/Three'
		}
	}
}).state('register', {
	url: '/register',
	views: {
		"mainContainer": {
			templateUrl: 'Account/Register',
			controller: 'RegisterController'
		}
	}
})
*/