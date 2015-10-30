/// <reference path="../_references.js" />
describe('Application controllers', function () {

	describe('ChoresController', function () {
		var $httpBackend, $rootScope, $controller, $choresService;
		function createController() {
			return $controller('ChoresController', { '$scope': $rootScope, ChoresService: $choresService });
		};

		beforeEach(module('Application'));

		beforeEach(inject(function ($injector) {
			$httpBackend = $injector.get('$httpBackend');
			$httpBackend.expectGET(/\/api\/chores\/all/).respond([{
				Id: '1',
				Name: 'Test 1',
				Value: 1,
				Active: true
			}, {
				Id: '2',
				Name: 'Test 2',
				Value: 2,
				Active: false
			}]);
			$rootScope = $injector.get('$rootScope');
			$controller = $injector.get('$controller');
			$choresService = $injector.get('ChoresService');

			
		}));

		afterEach(function () {
			$httpBackend.verifyNoOutstandingExpectation();
			$httpBackend.verifyNoOutstandingRequest();
			$httpBackend.resetExpectations();
		});

		it('should initialize scope.', function () {
			var controller = createController();
			expect($rootScope.chores).toBeDefined();
			expect($rootScope.chores.length).toBe(0);
			expect($rootScope.newChore).toBeDefined();
			expect($rootScope.newChore.Name).toBe('');
			expect($rootScope.newChore.HasNameError).toBe(false);
			expect($rootScope.newChore.Value).toBe(null);
			expect($rootScope.newChore.HasValueError).toBe(false);
			expect($rootScope.newChoreVisible).toBe(false);
			expect($rootScope.error).toBeDefined();
			expect($rootScope.error.message).toBe(undefined);
			expect($rootScope.activateDeactivate).toBeDefined();
			expect($rootScope.showNewChore).toBeDefined();
			expect($rootScope.hideNewChore).toBeDefined();
			expect($rootScope.createChore).toBeDefined();
			$httpBackend.flush();
		});

		it('should make server call to get chores.', function () {
			var controller = createController();
			$httpBackend.flush();
			expect($rootScope.chores.length).toBe(2);
			expect($rootScope.chores[0].Id).toBe('1');
			expect($rootScope.chores[0].Name).toBe('Test 1');
			expect($rootScope.chores[0].Value).toBe(1);
			expect($rootScope.chores[0].Active).toBe(true);
			expect($rootScope.chores[1].Id).toBe('2');
			expect($rootScope.chores[1].Name).toBe('Test 2');
			expect($rootScope.chores[1].Value).toBe(2);
			expect($rootScope.chores[1].Active).toBe(false);
		});

		it('should set error message on initialization failure.', function () {
			$httpBackend.resetExpectations();
			$httpBackend.expectGET(/\/api\/chores\/all/).respond(400, 'Bad Request', null, 'Bad Request Status Text');
			var controller = createController();
			$httpBackend.flush();
			expect($rootScope.error.message).toBe('Bad Request Status Text');
		});

		it('should deactivate a chore.', function () {
			var controller = createController();
			$httpBackend.flush();

			$httpBackend.expectPUT(/\/api\/chores/).respond(204);
			$rootScope.activateDeactivate($rootScope.chores[0]);
			$httpBackend.flush();

			expect($rootScope.chores.length).toBe(2);
			expect($rootScope.chores[0].Id).toBe('1');
			expect($rootScope.chores[0].Name).toBe('Test 1');
			expect($rootScope.chores[0].Value).toBe(1);
			expect($rootScope.chores[0].Active).toBe(false);
			expect($rootScope.chores[1].Id).toBe('2');
			expect($rootScope.chores[1].Name).toBe('Test 2');
			expect($rootScope.chores[1].Value).toBe(2);
			expect($rootScope.chores[1].Active).toBe(false);
		});

		it('should activate a chore.', function () {
			var controller = createController();
			$httpBackend.flush();

			$httpBackend.expectPUT(/\/api\/chores/).respond(204);
			$rootScope.activateDeactivate($rootScope.chores[1]);
			$httpBackend.flush();

			expect($rootScope.chores.length).toBe(2);
			expect($rootScope.chores[0].Id).toBe('1');
			expect($rootScope.chores[0].Name).toBe('Test 1');
			expect($rootScope.chores[0].Value).toBe(1);
			expect($rootScope.chores[0].Active).toBe(true);
			expect($rootScope.chores[1].Id).toBe('2');
			expect($rootScope.chores[1].Name).toBe('Test 2');
			expect($rootScope.chores[1].Value).toBe(2);
			expect($rootScope.chores[1].Active).toBe(true);
		});

		it('should set error message activateDeactivate error.', function () {
			var controller = createController();
			$httpBackend.flush();

			$httpBackend.expectPUT(/\/api\/chores/).respond(400, 'Bad Request', null, 'Bad Request Status Text');
			$rootScope.activateDeactivate($rootScope.chores[1]);
			$httpBackend.flush();
			expect($rootScope.error.message).toBe('Bad Request Status Text');
		});

		it('should reset newChore object and show new chore.', function () {
			var controller = createController();
			$httpBackend.flush();

			$rootScope.newChore.Name = 'Test';
			$rootScope.newChore.HasNameError = true;
			$rootScope.newChore.Value = 1000;
			$rootScope.newChore.HasValueError = true;

			$rootScope.showNewChore();

			expect($rootScope.newChore.Name).toBe('');
			expect($rootScope.newChore.HasNameError).toBe(false);
			expect($rootScope.newChore.Value).toBe(null);
			expect($rootScope.newChore.HasValueError).toBe(false);
			expect($rootScope.newChoreVisible).toBe(true);
		});

		it('should hide new chore.', function () {
			var controller = createController();
			$httpBackend.flush();
			$rootScope.hideNewChore();
			expect($rootScope.newChoreVisible).toBe(false);
		});

		it('should set error states on a new chore and not POST.', function () {
			var controller = createController();
			$httpBackend.flush();

			$rootScope.newChore.Name = '';
			$rootScope.newChore.HasNameError = false;
			$rootScope.newChore.Value = '';
			$rootScope.newChore.HasValueError = false;

			$rootScope.createChore();

			expect($rootScope.newChore.HasNameError).toBe(true);
			expect($rootScope.newChore.HasValueError).toBe(true);
		});

		it('should create a new chore and update.', function () {
			var controller = createController();
			$httpBackend.flush();

			$httpBackend.resetExpectations();
			$httpBackend.expectPOST(/\/api\/chores/).respond(204);
			$httpBackend.expectGET(/\/api\/chores\/all/).respond([{
				Id: '1',
				Name: 'Test 1',
				Value: 1,
				Active: true
			}, {
				Id: '2',
				Name: 'Test 2',
				Value: 2,
				Active: false
			}, {
				Id: '3',
				Name: 'Test 3',
				Value: 3,
				Active: true
			}]);

			$rootScope.newChore.Name = 'Test 3';
			$rootScope.newChore.Value = '3';

			$rootScope.createChore();
			$httpBackend.flush();

			expect($rootScope.newChoreVisible).toBe(false);
			expect($rootScope.chores.length).toBe(3);
			expect($rootScope.chores[0].Id).toBe('1');
			expect($rootScope.chores[0].Name).toBe('Test 1');
			expect($rootScope.chores[0].Value).toBe(1);
			expect($rootScope.chores[0].Active).toBe(true);
			expect($rootScope.chores[1].Id).toBe('2');
			expect($rootScope.chores[1].Name).toBe('Test 2');
			expect($rootScope.chores[1].Value).toBe(2);
			expect($rootScope.chores[1].Active).toBe(false);
			expect($rootScope.chores[2].Id).toBe('3');
			expect($rootScope.chores[2].Name).toBe('Test 3');
			expect($rootScope.chores[2].Value).toBe(3);
			expect($rootScope.chores[2].Active).toBe(true);
		});

		it('should set error message createChore error.', function () {
			var controller = createController();
			$httpBackend.flush();

			$httpBackend.expectPOST(/\/api\/chores/).respond(400, 'Bad Request', null, 'Bad Request Status Text');
			$rootScope.newChore.Name = 'Test 3';
			$rootScope.newChore.Value = '3';
			$rootScope.createChore();
			$httpBackend.flush();
			expect($rootScope.error.message).toBe('Bad Request Status Text');
		});

	});

});
