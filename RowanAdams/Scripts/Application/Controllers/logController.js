var controllers = angular.getOrCreateModule('ApplicationControllers');

controllers.controller('LogController', [
	'$scope', 'LogService', 'ChoresService', function ($scope, LogService, ChoresService) {
		function getLogEntries() {
			LogService.getLogEntriesForMonth($scope.currentMonthStarting).then(function(response) {
				$scope.logEntries = response.data || [];
			}, function(response) {
				$scope.error.message = response.statusText;
			});
		};
		function getChores() {
			ChoresService.getActiveChores().then(function (response) {
				$scope.chores = response.data;
			}, function (response) {
				$scope.error.message = response.statusText;
			});
		};
		function getNewLogEntry() {
			return {
				choreId: null,
				HasChoreIdError: false,
				completedDate: moment(new Date()).format('MM/DD/YYYY'),
				HasCompletedDateError: false
			};
		};
		function hasError() {
			$scope.error.message = undefined;
			$scope.newLogEntry.HasChoreIdError = false;
			$scope.newLogEntry.HasCompletedDateError = false;

			if (!helpers.hasValue($scope.newLogEntry.choreId))
				$scope.newLogEntry.HasChoreIdError = true;
			if (!helpers.hasValue($scope.newLogEntry.completedDate))
				$scope.newLogEntry.HasCompletedDateError = true;

			return $scope.newLogEntry.HasChoreIdError || $scope.newLogEntry.HasCompletedDateError;
		};
		function openDatePicker($elem, startDate, setDateFunc) {
			var drp = $elem.data('daterangepicker');
			if (!drp) {
				$elem.daterangepicker({
					startDate: startDate,
					singleDatePicker: true,
					showDropdowns: true
				}, function (start, end, label) {
					$scope.$apply(setDateFunc(start, end, label));
				});

				drp = $elem.data('daterangepicker');

				$elem.on('hide.daterangepicker', function (ev, picker) {
					drp.remove();
					$elem.off('hide.daterangepicker');
				});
			}
			drp.show();
		};

		$scope.chores = [];
		$scope.logEntries = [];
		$scope.newLogEntryVisible = false;
		$scope.newLogEntry = getNewLogEntry();
		$scope.currentMonthStarting = (new moment()).startOf('month');
		$scope.status = { opened: false };
		$scope.error = { message: undefined };
		$scope.showNewLogEntry = function() {
			$scope.newLogEntry = getNewLogEntry();
			$scope.newLogEntryVisible = true;
		};
		$scope.hideNewLogEntry = function() {
			$scope.newLogEntryVisible = false;
		};
		$scope.getTotal = function() {
			var sum = 0;
			$scope.logEntries.forEach(function(entry) { sum += entry.Value; });
			return sum;
		};
		$scope.nextMonth = function() {
			$scope.currentMonthStarting = $scope.currentMonthStarting.add(1, 'months');
			$scope.logEntries = [];
			getLogEntries();
		};
		$scope.previousMonth = function() {
			$scope.currentMonthStarting = $scope.currentMonthStarting.subtract(1, 'months');
			$scope.logEntries = [];
			getLogEntries();
		};
		$scope.createLogEntry = function () {
			if (hasError())
				return;

			LogService.postLogEntry($scope.newLogEntry).then(function (response) {
				getLogEntries();
				$scope.hideNewLogEntry();
			}, function (response) {
				$scope.error.message = response.statusText;
			});
		};
		$scope.onClickCurrentDate = function (ev) {
			var $elem = $(ev.target || ev.srcElement);
			openDatePicker($elem, $scope.currentMonthStarting.toDate(), function (start, end, label) {
				$scope.currentMonthStarting = moment(start).startOf('month');
				getLogEntries();
			});
		};
		$scope.onClickNewLogDate = function (ev) {
			var $elem = $(ev.target || ev.srcElement);
			openDatePicker($elem, $scope.newLogEntry.completedDate, function (start, end, label) {
				$scope.newLogEntry.date = start;
			});
		}

		//angular.element(document).ready(function () {
		//});

		getChores();
		getLogEntries();
	}
]);