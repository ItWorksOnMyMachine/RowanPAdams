/// <reference path="../_references.js" />
describe('Application controllers', function () {

	describe('SiteController', function () {
		var $httpBackend, $rootScope, $controller, $sessionService, $state;
		function createController() {
			return $controller('SiteController', { '$scope': $rootScope, SessionService: $sessionService, '$state': $state });
		};

		beforeEach(module('Application'));

		beforeEach(inject(function ($injector) {
			$httpBackend = $injector.get('$httpBackend');
			$rootScope = $injector.get('$rootScope');
			$state = $injector.get('$state');
			$controller = $injector.get('$controller');
			$sessionService = $injector.get('SessionService');
		}));


		it('should create "models" model with isBound set to true', function () {
			var controller = createController();
			expect($rootScope.models.isBound).toBe(true);
		});

		it('should create "navbarProperties" model with isCollapsed set to true', function () {
			var controller = createController();
			expect($rootScope.navbarProperties.isCollapsed).toBe(true);
		});

		it('should not be logged in', function () {
			var controller = createController();
			expect($rootScope.loggedIn()).toBe(false);
		});

		it('should be logged in after setting token', inject(function (SessionService) {
			var controller = createController();
			SessionService.setToken('dummy token');
			expect($rootScope.loggedIn()).toBe(true);
		}));

		//it('should go to login scope on 401.', function () {
		//	$httpBackend.resetExpectations();
		//	$httpBackend.expectGET(/\/api\/logentries\/all/).respond(401, { Message: 'Authorization has been denied for this request.' }, null, 'Unauthorized');

		//	var controller = createController();
		//	$httpBackend.flush();
		//	expect($rootScope.error.message).toBe('Bad Request Status Text');
		//});

	});

});
