var services = angular.getOrCreateModule('ApplicationServices');

services.service('SessionService', ['$cookies', function ($cookies) {
		this.token = undefined;

		this.getToken = function () {
			if (!$cookies.get('autoToken')) {
				if (!this.token) {
					return undefined;
				}
				this.setToken(this.token);
			}
			return $cookies.get('autoToken');
		}

		this.setToken = function(token) {
			this.token = token;
			var expireDate = new Date();
			expireDate.setDate(expireDate.getDate() + 365);
			$cookies.put('autoToken', token, { 'expires': expireDate });
		}

		this.logout = function () {
			this.token = undefined;
			$cookies.remove('autoToken');
		}
	}
]);