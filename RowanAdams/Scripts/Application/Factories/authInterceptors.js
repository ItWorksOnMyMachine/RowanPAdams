var factories = angular.getOrCreateModule('ApplicationFactories', ['ui.router']);

factories.factory('AuthRequestInterceptor', ['$q', '$injector', 'SessionService', function ($q, $injector, SessionService) {
	var anonymousUrls = [
		/Home\/Home/ig,
		/Account\/Login/ig,
		/Account\/Register/ig,
		/\/token$/ig
	];

	function getReturnUrlFromStates(config, $state) {
		var states = $state.get() || [];
		for (var i = 0; i < states.length; i++) {
			var s = states[i];
			if (s.views && s.views.mainContainer && s.views.mainContainer.templateUrl === config.url)
				return s.name;
		}
		return $state.current.name;
	};

	function redirectToLoginIfUrlDoesNotAllowAnonymousAccess(config) {
		var allowAnonymous = false;
		for (var i = 0; i < anonymousUrls.length && !allowAnonymous; i++)
			allowAnonymous = config.url.match(anonymousUrls[i]) !== null;
		if (!allowAnonymous) {
			var def = $q.defer();
			def.reject();
			var $state = $injector.get('$state');
			var returnUrl = getReturnUrlFromStates(config, $state);
			$state.go('login', { returnUrl: returnUrl });
			return def.promise;
		}
		return config;
	};

	var requestInterceptor = {
		request: function (config) {
			if (config.headers['Authorization'] === undefined) {
				if (SessionService.getToken() !== undefined)
					config.headers['Authorization'] = 'Bearer ' + SessionService.getToken();
				else
					return redirectToLoginIfUrlDoesNotAllowAnonymousAccess(config);
			}
			return config;
		}
	};

	return requestInterceptor;
}]);

factories.factory('AuthResponseInterceptor', [
	'$q', '$injector', function ($q, $injector) {
		return {
			response: function(response) {
				if (response.status === 401) {
					console.log("Response 401");
				}
				return response || $q.when(response);
			},
			responseError: function(rejection) {
				if (rejection && rejection.status === 401) {
					console.log("Response Error 401", rejection);
					// Injecting $state gives a circular dependency error.
					var $state = $injector.get('$state');
					$state.go('login', { returnUrl: $state.current.name });
				}
				return $q.reject(rejection);
			}
		}
	}
]);

