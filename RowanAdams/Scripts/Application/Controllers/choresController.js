var controllers = angular.getOrCreateModule('ApplicationControllers');

controllers.controller('ChoresController', [
	'$scope', 'ChoresService', function ($scope, ChoresService) {
		function getChores() {
			ChoresService.getAllChores().then(function (response) {
				$scope.chores = response.data;
			}, function (response) {
				$scope.error.message = response.statusText;
			});
		};
		function getNewChore() {
			return {
				Name: '',
				HasNameError: false,
				Value: null,
				HasValueError: false
			};
		};
		function hasError() {
			$scope.error.message = undefined;
			$scope.newChore.HasNameError = false;
			$scope.newChore.HasValueError = false;

			if (!helpers.hasValue($scope.newChore.Name))
				$scope.newChore.HasNameError = true;
			if (!helpers.hasValue($scope.newChore.Value))
				$scope.newChore.HasValueError = true;

			return $scope.newChore.HasNameError || $scope.newChore.HasValueError;
		};

		$scope.chores = [];
		$scope.newChore = getNewChore();
		$scope.newChoreVisible = false;
		$scope.error = {
			message: undefined
		};
		$scope.activateDeactivate = function (chore) {
			$scope.error.message = undefined;
			chore.Active = !chore.Active;
			ChoresService.putChore(chore).then(function (response) {
			}, function (response) {
				chore.Active = !chore.Active;
				$scope.error.message = response.statusText;
			});
		};
		$scope.showNewChore = function() {
			$scope.newChore = getNewChore();
			$scope.newChoreVisible = true;
		};
		$scope.hideNewChore = function () {
			$scope.newChoreVisible = false;
		};
		$scope.createChore = function () {
			if (hasError())
				return;

			ChoresService.postChore($scope.newChore).then(function (response) {
				getChores();
				$scope.hideNewChore();
			}, function (response) {
				$scope.error.message = response.statusText;
			});
		};

		getChores();
	}
]).directive('validNumber', function () {
	return {
		require: '?ngModel',
		link: function (scope, element, attrs, ngModelCtrl) {
			if (!ngModelCtrl) {
				return;
			}

			ngModelCtrl.$parsers.push(function (val) {
				if (angular.isUndefined(val)) {
					var val = '';
				}
				var clean = val.replace(/[^0-9.]+/g, '');
				if (val !== clean) {
					ngModelCtrl.$setViewValue(clean);
					ngModelCtrl.$render();
				}
				return clean;
			});

			element.bind('keypress', function (event) {
				if (event.keyCode === 32) {
					event.preventDefault();
				}
			});
		}
	};
});