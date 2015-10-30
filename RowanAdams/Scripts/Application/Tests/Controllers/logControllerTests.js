/// <reference path="../_references.js" />
describe('Application controllers', function () {

	describe('LogController', function () {
		var $httpBackend, $rootScope, $controller, $logService, $choresService;
		function createController() {
			return $controller('LogController', { '$scope': $rootScope, LogService: $logService, ChoresService: $choresService });
		};

		beforeEach(module('Application'));

		beforeEach(inject(function ($injector) {
			$httpBackend = $injector.get('$httpBackend');
			$httpBackend.expectGET(/\/api\/chores\/active/).respond([{
				Id: '1',
				Name: 'Test 1',
				Value: 1,
				Active: true
			}, {
				Id: '2',
				Name: 'Test 2',
				Value: 2,
				Active: true
			}]);
			$httpBackend.expectGET(/\/api\/logentries\/month/).respond([{
				Id: '1',
				ChoreId: '1',
				Name: 'Test 1',
				Value: 1,
				CompletedDate: new Date('10/01/2015')
			}, {
				Id: '2',
				ChoreId: '2',
				Name: 'Test 2',
				Value: 2,
				CompletedDate: new Date('10/01/2015')
			}]);
			$rootScope = $injector.get('$rootScope');
			$controller = $injector.get('$controller');
			$logService = $injector.get('LogService');
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
			expect($rootScope.logEntries).toBeDefined();
			expect($rootScope.logEntries.length).toBe(0);
			expect($rootScope.newLogEntryVisible).toBe(false);
			expect($rootScope.newLogEntry).toBeDefined();
			expect($rootScope.newLogEntry.choreId).toBe(null);
			expect($rootScope.newLogEntry.HasChoreIdError).toBe(false);
			expect($rootScope.newLogEntry.completedDate).toEqual(moment(new Date()).format('MM/DD/YYYY'));
			expect($rootScope.newLogEntry.HasCompletedDateError).toBe(false);
			expect($rootScope.currentMonthStarting).toEqual((new moment()).startOf('month'));
			expect($rootScope.status).toBeDefined();
			expect($rootScope.status.opened).toBe(false);
			expect($rootScope.error).toBeDefined();
			expect($rootScope.error.message).toBe(undefined);
			expect($rootScope.showNewLogEntry).toBeDefined();
			expect($rootScope.hideNewLogEntry).toBeDefined();
			expect($rootScope.getTotal).toBeDefined();
			expect($rootScope.nextMonth).toBeDefined();
			expect($rootScope.previousMonth).toBeDefined();
			expect($rootScope.createLogEntry).toBeDefined();
			expect($rootScope.onClickCurrentDate).toBeDefined();
			expect($rootScope.onClickNewLogDate).toBeDefined();
			$httpBackend.flush();
		});

		it('should make server call to get chores and logEntries.', function () {
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
			expect($rootScope.chores[1].Active).toBe(true);

			expect($rootScope.logEntries.length).toBe(2);
			expect($rootScope.logEntries[0].Id).toBe('1');
			expect($rootScope.logEntries[0].ChoreId).toBe('1');
			expect($rootScope.logEntries[0].Name).toBe('Test 1');
			expect($rootScope.logEntries[0].Value).toBe(1);
			expect($rootScope.logEntries[0].CompletedDate).toEqual(new Date('10/01/2015'));
			expect($rootScope.logEntries[1].Id).toBe('2');
			expect($rootScope.logEntries[1].ChoreId).toBe('2');
			expect($rootScope.logEntries[1].Name).toBe('Test 2');
			expect($rootScope.logEntries[1].Value).toBe(2);
			expect($rootScope.logEntries[1].CompletedDate).toEqual(new Date('10/01/2015'));
		});

		it('should set error message on chores initialization failure.', function () {
			$httpBackend.resetExpectations();
			$httpBackend.expectGET(/\/api\/chores\/active/).respond(400, 'Bad Request', null, 'Bad Request Status Text');
			$httpBackend.expectGET(/\/api\/logentries\/month/).respond([{
				Id: '1',
				ChoreId: '1',
				Name: 'Test 1',
				Value: 1,
				CompletedDate: new Date('10/01/2015')
			}, {
				Id: '2',
				ChoreId: '2',
				Name: 'Test 2',
				Value: 2,
				CompletedDate: new Date('10/01/2015')
			}]);

			var controller = createController();
			$httpBackend.flush();
			expect($rootScope.error.message).toBe('Bad Request Status Text');
		});

		it('should set error message on logentries initialization failure.', function () {
			$httpBackend.resetExpectations();
			$httpBackend.expectGET(/\/api\/chores\/active/).respond([{
				Id: '1',
				Name: 'Test 1',
				Value: 1,
				Active: true
			}, {
				Id: '2',
				Name: 'Test 2',
				Value: 2,
				Active: true
			}]);
			$httpBackend.expectGET(/\/api\/logentries\/month/).respond(400, 'Bad Request', null, 'Bad Request Status Text');

			var controller = createController();
			$httpBackend.flush();
			expect($rootScope.error.message).toBe('Bad Request Status Text');
		});

		it('should reset newLogEntry object and show new log entry.', function () {
			var controller = createController();
			$httpBackend.flush();

			$rootScope.newLogEntry.choreId = 'Test';
			$rootScope.newLogEntry.HasChoreIdError = true;
			$rootScope.newLogEntry.completedDate = 'Test';
			$rootScope.newLogEntry.HasCompletedDateError = true;

			$rootScope.showNewLogEntry();

			expect($rootScope.newLogEntryVisible).toBe(true);
			expect($rootScope.newLogEntry).toBeDefined();
			expect($rootScope.newLogEntry.choreId).toBe(null);
			expect($rootScope.newLogEntry.HasChoreIdError).toBe(false);
			expect($rootScope.newLogEntry.completedDate).toEqual(moment(new Date()).format('MM/DD/YYYY'));
			expect($rootScope.newLogEntry.HasCompletedDateError).toBe(false);
		});

		it('should hide new chore.', function () {
			var controller = createController();
			$httpBackend.flush();
			$rootScope.hideNewLogEntry();
			expect($rootScope.newLogEntryVisible).toBe(false);
		});

		it('should get total value of all log entries.', function () {
			var controller = createController();
			$httpBackend.flush();
			expect($rootScope.getTotal()).toBe(3);
		});

		it('should move to next month and reset log entries.', function () {
			var controller = createController();
			$httpBackend.flush();
			$httpBackend.resetExpectations();
			$httpBackend.expectGET(/\/api\/logentries\/month/).respond([{
				Id: '3',
				ChoreId: '3',
				Name: 'Test 3',
				Value: 3,
				CompletedDate: new Date('11/01/2015')
			}]);

			$rootScope.nextMonth();
			$httpBackend.flush();
			expect($rootScope.currentMonthStarting).toEqual((new moment()).startOf('month').add(1, 'months'));
			expect($rootScope.logEntries.length).toBe(1);
			expect($rootScope.logEntries[0].Id).toBe('3');
			expect($rootScope.logEntries[0].ChoreId).toBe('3');
			expect($rootScope.logEntries[0].Name).toBe('Test 3');
			expect($rootScope.logEntries[0].Value).toBe(3);
			expect($rootScope.logEntries[0].CompletedDate).toEqual(new Date('11/01/2015'));
		});

		it('should move to previous month and reset log entries.', function () {
			var controller = createController();
			$httpBackend.flush();
			$httpBackend.resetExpectations();
			$httpBackend.expectGET(/\/api\/logentries\/month/).respond([{
				Id: '0',
				ChoreId: '0',
				Name: 'Test 0',
				Value: 0,
				CompletedDate: new Date('09/01/2015')
			}]);

			$rootScope.previousMonth();
			$httpBackend.flush();
			expect($rootScope.currentMonthStarting).toEqual((new moment()).startOf('month').subtract(1, 'months'));
			expect($rootScope.logEntries.length).toBe(1);
			expect($rootScope.logEntries[0].Id).toBe('0');
			expect($rootScope.logEntries[0].ChoreId).toBe('0');
			expect($rootScope.logEntries[0].Name).toBe('Test 0');
			expect($rootScope.logEntries[0].Value).toBe(0);
			expect($rootScope.logEntries[0].CompletedDate).toEqual(new Date('09/01/2015'));
		});

		it('should create a new logentry and update.', function () {
			var controller = createController();
			$httpBackend.flush();

			$httpBackend.resetExpectations();
			$httpBackend.expectPOST(/\/api\/logentries/).respond(204);
			$httpBackend.expectGET(/\/api\/logentries\/month/).respond([{
				Id: '1',
				ChoreId: '1',
				Name: 'Test 1',
				Value: 1,
				CompletedDate: new Date('10/01/2015')
			}, {
				Id: '2',
				ChoreId: '2',
				Name: 'Test 2',
				Value: 2,
				CompletedDate: new Date('10/01/2015')
			}, {
				Id: '3',
				ChoreId: '3',
				Name: 'Test 3',
				Value: 3,
				CompletedDate: new Date('10/01/2015')
			}]);


			$rootScope.newLogEntry.choreId = '3';
			$rootScope.newLogEntry.completedDate = '10/01/2015';

			$rootScope.createLogEntry();
			$httpBackend.flush();

			expect($rootScope.newLogEntryVisible).toBe(false);
			expect($rootScope.logEntries.length).toBe(3);
			expect($rootScope.logEntries[0].Id).toBe('1');
			expect($rootScope.logEntries[0].ChoreId).toBe('1');
			expect($rootScope.logEntries[0].Name).toBe('Test 1');
			expect($rootScope.logEntries[0].Value).toBe(1);
			expect($rootScope.logEntries[0].CompletedDate).toEqual(new Date('10/01/2015'));
			expect($rootScope.logEntries[1].Id).toBe('2');
			expect($rootScope.logEntries[1].ChoreId).toBe('2');
			expect($rootScope.logEntries[1].Name).toBe('Test 2');
			expect($rootScope.logEntries[1].Value).toBe(2);
			expect($rootScope.logEntries[1].CompletedDate).toEqual(new Date('10/01/2015'));
			expect($rootScope.logEntries[2].Id).toBe('3');
			expect($rootScope.logEntries[2].ChoreId).toBe('3');
			expect($rootScope.logEntries[2].Name).toBe('Test 3');
			expect($rootScope.logEntries[2].Value).toBe(3);
			expect($rootScope.logEntries[2].CompletedDate).toEqual(new Date('10/01/2015'));
		});

		it('should set error states on a new log entry and not POST.', function () {
			var controller = createController();
			$httpBackend.flush();

			$rootScope.newLogEntry.choreId = '';
			$rootScope.newLogEntry.HasChoreIdError = false;
			$rootScope.newLogEntry.completedDate = '';
			$rootScope.newLogEntry.HasCompletedDateError = false;

			$rootScope.createLogEntry();

			expect($rootScope.newLogEntry.HasChoreIdError).toBe(true);
			expect($rootScope.newLogEntry.HasCompletedDateError).toBe(true);
		});

		it('should set error message createLogEntry error.', function () {
			var controller = createController();
			$httpBackend.flush();

			$httpBackend.expectPOST(/\/api\/logentries/).respond(400, 'Bad Request', null, 'Bad Request Status Text');
			$rootScope.newLogEntry.choreId = '3';
			$rootScope.newLogEntry.completedDate = '10/01/2015';
			$rootScope.createLogEntry();
			$httpBackend.flush();
			expect($rootScope.error.message).toBe('Bad Request Status Text');
		});
	});
});
