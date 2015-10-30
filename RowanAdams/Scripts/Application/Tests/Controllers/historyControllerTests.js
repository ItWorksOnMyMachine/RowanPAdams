/// <reference path="../_references.js" />
describe('Application controllers', function () {

	describe('HistoryController', function () {
		var $httpBackend, $rootScope, $controller, $logService
		function createController() {
			return $controller('HistoryController', { '$scope': $rootScope, LogService: $logService });
		};

		beforeEach(module('Application'));

		beforeEach(inject(function ($injector) {
			$httpBackend = $injector.get('$httpBackend');
			$httpBackend.expectGET(/\/api\/logentries\/all/).respond([{
				GroupName: 'October 2015',
				GroupTotal: 3,
				LogEntries: [{
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
				}]
			}, {
				GroupName: 'September 2015',
				GroupTotal: 5,
				LogEntries: [{
					Id: '3',
					ChoreId: '3',
					Name: 'Test 3',
					Value: 3,
					CompletedDate: new Date('09/01/2015')
				}, {
					Id: '4',
					ChoreId: '4',
					Name: 'Test 4',
					Value: 2,
					CompletedDate: new Date('09/01/2015')
				}]
			}]);
			$rootScope = $injector.get('$rootScope');
			$controller = $injector.get('$controller');
			$logService = $injector.get('LogService');
		}));

		afterEach(function () {
			$httpBackend.verifyNoOutstandingExpectation();
			$httpBackend.verifyNoOutstandingRequest();
			$httpBackend.resetExpectations();
		});

		it('should initialize scope.', function() {
			var controller = createController();
			expect($rootScope.logEntryGroupings).toBeDefined();
			expect($rootScope.logEntryGroupings.length).toBe(0);
			expect($rootScope.error).toBeDefined();
			expect($rootScope.error.message).toBe(undefined);
			$httpBackend.flush();
		});

		it('should make server call to get logEntryGroupings.', function () {
			var controller = createController();
			$httpBackend.flush();

			expect($rootScope.logEntryGroupings.length).toBe(2);
			expect($rootScope.logEntryGroupings[0].GroupName).toBe('October 2015');
			expect($rootScope.logEntryGroupings[0].GroupTotal).toBe(3);
			expect($rootScope.logEntryGroupings[0].LogEntries.length).toBe(2);
			expect($rootScope.logEntryGroupings[0].LogEntries[0].Id).toBe('1');
			expect($rootScope.logEntryGroupings[0].LogEntries[0].ChoreId).toBe('1');
			expect($rootScope.logEntryGroupings[0].LogEntries[0].Name).toBe('Test 1');
			expect($rootScope.logEntryGroupings[0].LogEntries[0].Value).toBe(1);
			expect($rootScope.logEntryGroupings[0].LogEntries[0].CompletedDate).toEqual(new Date('10/01/2015'));
			expect($rootScope.logEntryGroupings[0].LogEntries[1].Id).toBe('2');
			expect($rootScope.logEntryGroupings[0].LogEntries[1].ChoreId).toBe('2');
			expect($rootScope.logEntryGroupings[0].LogEntries[1].Name).toBe('Test 2');
			expect($rootScope.logEntryGroupings[0].LogEntries[1].Value).toBe(2);
			expect($rootScope.logEntryGroupings[0].LogEntries[1].CompletedDate).toEqual(new Date('10/01/2015'));

			expect($rootScope.logEntryGroupings[1].GroupName).toBe('September 2015');
			expect($rootScope.logEntryGroupings[1].GroupTotal).toBe(5);
			expect($rootScope.logEntryGroupings[1].LogEntries.length).toBe(2);
			expect($rootScope.logEntryGroupings[1].LogEntries[0].Id).toBe('3');
			expect($rootScope.logEntryGroupings[1].LogEntries[0].ChoreId).toBe('3');
			expect($rootScope.logEntryGroupings[1].LogEntries[0].Name).toBe('Test 3');
			expect($rootScope.logEntryGroupings[1].LogEntries[0].Value).toBe(3);
			expect($rootScope.logEntryGroupings[1].LogEntries[0].CompletedDate).toEqual(new Date('09/01/2015'));
			expect($rootScope.logEntryGroupings[1].LogEntries[1].Id).toBe('4');
			expect($rootScope.logEntryGroupings[1].LogEntries[1].ChoreId).toBe('4');
			expect($rootScope.logEntryGroupings[1].LogEntries[1].Name).toBe('Test 4');
			expect($rootScope.logEntryGroupings[1].LogEntries[1].Value).toBe(2);
			expect($rootScope.logEntryGroupings[1].LogEntries[1].CompletedDate).toEqual(new Date('09/01/2015'));
		});

		it('should set error message on chores initialization failure.', function () {
			$httpBackend.resetExpectations();
			$httpBackend.expectGET(/\/api\/logentries\/all/).respond(400, 'Bad Request', null, 'Bad Request Status Text');

			var controller = createController();
			$httpBackend.flush();
			expect($rootScope.error.message).toBe('Bad Request Status Text');
		});

	});

});
