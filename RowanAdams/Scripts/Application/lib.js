angular.getOrCreateModule = function (moduleName) {
	var module = null;
	try {
		module = angular.module(moduleName);
	} catch (e) {
		module = angular.module(moduleName, []);
	}
	return module;
};

window.helpers = {
	rtrim: /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,
	trim: function( text ) {
		return text == null ?
			"" :
			( text + "" ).replace( helpers.rtrim, "" );
	},
	hasValue: function (obj) {
		return (obj !== null && obj !== undefined && helpers.trim(obj) !== '');
	}
};