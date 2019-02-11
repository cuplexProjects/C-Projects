function isEnter(ev)
{
	var key = -1;
	if (window.event)
		key = window.event.keyCode;
	else if (ev)
		key = ev.which;
	if (key == 13)
		return true;
	else
		return false;
}

String.prototype.trim = function () {
	if (this != null) {
		return this.replace(/^\s+/, '').replace(/\s+$/, '');
	}
	return this;
};

var InputSymbols = {
	_REPLACE_ACTION: 0,
	_REPORT_ACTION: 1,
	
	/*
	 * Checks input through regExp. In case of failure performs action (replace or report).
	 */
	_check: function (input, regExp, action, msg)
	{
		try {
			if (action == this._REPORT_ACTION) Tip.Hide('');
			var value = input.value.trim();
			if (value != '') {
				var temp_value = value.replace(regExp, '');
				if (temp_value != value) {
					switch (action) {
						case this._REPLACE_ACTION:
							input.value = temp_value;
							break;
						case this._REPORT_ACTION:
							Tip.Show(msg, input, '');
							break;
					}
					return false;
				}
			}
			return true;
		}
		catch (errorMsg) {
			ShowError(errorMsg, '003');
			return false;
		}
	},
	
	/*
	 * Replaces all the characters except 0-9.
	 */
	ReplaceNonDigits: function (input)
	{
		this._check(input, /[^0-9]/gi, this._REPLACE_ACTION);
	},
	
	/*
	 * Replaces all the characters except 0-9 and '.'.
	 */
	ReplaceNonFloatNumber: function (input)
	{
		this._check(input, /[^0-9.]/gi, this._REPLACE_ACTION);
	},
	
	/*
	 * Replaces all the characters except 0-9 and ','.
	 */
	ReplaceNonDigitsComma: function (input)
	{
		this._check(input, /[^0-9,]/gi, this._REPLACE_ACTION);
	},
	
	/*
	 * Reports about all characters except 0-9, '.' and ','.
	 */
	CheckDigitsDotComma: function (input, msg)
	{
		this._check(input, /[^0-9,.]/gi, this._REPORT_ACTION, msg);
	},
	
	/*
	 * Reports about all characters except A-Z, a-z, 0-9, '_', '.' and '-'.
	 */
	CheckDomainCharacters: function (input, msg)
	{
		return this._check(input, /[^A-Za-z0-9_.-]/gi, this._REPORT_ACTION, msg);
	},
	
	/*
	 * Reports about all characters except A-Z, a-z, 0-9, '_', '.', '@' and '-'.
	 */
	CheckEmailCharacters: function (input, msg)
	{
		this._check(input, /[^A-Za-z0-9_.@-]/gi, this._REPORT_ACTION, msg);
	},
	
	/*
	 * Reports about all characters except A-Z, a-z, 0-9 and _.~#$()=+|><,.":;'-.
	 */
	CheckUserPasswordCharacters: function (input, msg)
	{
		return this._check(input, /[^A-Za-z0-9_.~#$()=+|><,."":;''-]/gi, this._REPORT_ACTION, msg);
	},

	CheckCommaSeparatedIP: function (input, msg)
	{
		try
		{
			Tip.Hide('');
			var value = input.value.trim();
			if (value != '') {
				var arrIP = new Array();
				var i=0;
				arrIP = value.split(',');
				if (arrIP.length > 0) {
					for(i=0; i < arrIP.length; i++) {
						if(!IsIPAddress(arrIP[i])) {
							Tip.Show(msg, input, '');
							break;
						}
					}
				}
			}
		}
		catch (errorMsg) {
			ShowError(errorMsg, '007');
		}
	},
	
	CheckDomain: function (input, msg)
	{
		Tip.Hide();
		var value = input.value.trim();
		if(!DomainNameIsOk(value)) {
			Tip.Show(msg, input, ''); 
		}
	},
	
	CheckEmail: function (input, msg)
	{
		Tip.Hide(); 
		if(!IsEmailAddress(input)) {
			Tip.Show('Email address is incorrect', input, ''); 
		}
	}
}

var Validator = {
	_COMMA_SEPARATED_IP_REPORT: 'Only IP Addresses or comma separated IP Addresses allowing here.',
	_INCORRECT_CHARACTERS_DOMAIN_MESSAGE: 'Incorrect characters in the domain name.',
	_INCORRECT_DOMAIN_MESSAGE: 'Your Domain name or IP address is incorrect.',
	_INCORRECT_CHARACTERS_EMAIL_MESSAGE: 'Incorrect characters in the email address.',
	_INCORRECT_EMAIL_MESSAGE: 'Email address is incorrect.',
	_INCORRECT_ADMIN_ACCOUNT_MESSAGE: 'Incorrect character for an User/Account Name.',
	_INCORRECT_USER_PASSWORD_MESSAGE: 'Incorrect characters in the password.',
	
	RegisterAllowNum: function (input)
	{
		if (null == input) return;
		input.onkeyup = function () { InputSymbols.ReplaceNonDigits(this); };
		input.onblur = function () { InputSymbols.ReplaceNonDigits(this); };
	},
	
	RegisterAllowCommaSeparatedNum: function (input)
	{
		if (null == input) return;
		input.onkeyup = function () { InputSymbols.ReplaceNonDigitsComma(this); };
		input.onblur = function () { InputSymbols.ReplaceNonDigitsComma(this); };
	},

	RegisterAllowFloat: function (input)
	{
		if (null == input) return;
		input.onkeyup = function () { InputSymbols.ReplaceNonFloatNumber(this); };
		input.onblur = function () { InputSymbols.ReplaceNonFloatNumber(this); };
	},
	
	RegisterAllowCommaSeparatedIP: function (input)
	{
		if (null == input) return;
		var obj = this;
		input.onkeyup = function () {
			InputSymbols.CheckDigitsDotComma(this, obj._COMMA_SEPARATED_IP_REPORT);
		};
		input.onblur = function () {
			InputSymbols.CheckCommaSeparatedIP(this, obj._COMMA_SEPARATED_IP_REPORT);
		};
	},
	
	RegisterAllowDomainSymbols: function (input)
	{
		if (null == input) return;
		var obj = this;
		input.onkeyup = function () {
			InputSymbols.CheckDomainCharacters(this, obj._INCORRECT_CHARACTERS_DOMAIN_MESSAGE);
		};
		input.onblur = function () {
			InputSymbols.CheckDomain(this, obj._INCORRECT_DOMAIN_MESSAGE);
		};
	},

	RegisterAllowEmailSymbols: function (input)
	{
		if (null == input) return;
		var obj = this;
		input.onkeyup = function () {
			InputSymbols.CheckEmailCharacters(this, obj._INCORRECT_CHARACTERS_EMAIL_MESSAGE);
		};
		input.onblur = function () {
			InputSymbols.CheckEmail(this, obj._INCORRECT_EMAIL_MESSAGE);
		};
	},
	
	RegisterAllowAdminsAccountSymbols: function (input)
	{
		if (null == input) return;
		var obj = this;
		input.onkeyup = function () {
			InputSymbols.CheckDomainCharacters(this, obj._INCORRECT_ADMIN_ACCOUNT_MESSAGE);
		};
		input.onblur = function () {
			InputSymbols.CheckDomainCharacters(this, obj._INCORRECT_ADMIN_ACCOUNT_MESSAGE);
		};
		input.onkeydown = function (ev) {
			if (isEnter(ev)) return false;
		};
	},
	
	RegisterAllowUserPasswordSymbols: function (input)
	{
		if (null == input) return;
		var obj = this;
		input.onkeyup = function () {
			InputSymbols.CheckUserPasswordCharacters(this, obj._INCORRECT_USER_PASSWORD_MESSAGE);
		};
		input.onblur = function () {
			InputSymbols.CheckUserPasswordCharacters(this, obj._INCORRECT_USER_PASSWORD_MESSAGE);
		};
		input.onkeydown = function (ev) {
			if (isEnter(ev)) return false;
		};
	}
};

function IsIPAddress(sIPValue)
{
	try
	{
		if (sIPValue == '0.0.0.0' || sIPValue == '255.255.255.255') return false;
		
		var ipPattern = /^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$/;
		var aIpArray = sIPValue.match(ipPattern);
		if (aIpArray == null) return false;
		
		var thisSegment;
		for (i = 1; i < 5; i++) { //because element aIpArray[0] - is the hole ip address!!!
			thisSegment = aIpArray[i];
			if (thisSegment > 255) return false;
		}
		return true;
	}
	catch (errorMsg) {
		ShowError(errorMsg, '017');
		return false;
	}
}

function IsDomainName(sDomainName)
{
	try
	{
		sDomainName = '' + sDomainName;
		sDomainName = sDomainName.trim();
		if (sDomainName == '') return false;

		var arrTest = new Array();
		arrTest = sDomainName.split('.');
		if(arrTest[0] == '' || arrTest[arrTest.length - 1] == '') return false;

		var temp_value = sDomainName.replace(/[^A-Za-z0-9_.-]/gi, '');
		if (temp_value != sDomainName) {
			return false;
		}
		else {
			return true;
		}
	}
	catch (errorMsg) {
		ShowError(errorMsg, '016');
		return false;
	}
}

function DomainNameIsOk(param)
{
	return IsDomainName(param) && IsIPAddress(param);
}

function IsDomainMaskOk(sDomainName)
{
	try {
		sDomainName = ('' + sDomainName).trim();
		if (sDomainName == '') return false;

		var arrTest = new Array();
		arrTest = sDomainName.split('.');
		if(arrTest[0] == '' || arrTest[arrTest.length - 1] == '') return false;
		
		var temp_value = sDomainName.replace(/[^A-Za-z0-9_.*-]/gi, '');
		if (temp_value != sDomainName) {
			return false;
		}
		else {
			return true;
		}
	}
	catch (errorMsg) {
		ShowError(errorMsg, '171');
		return false;
	}
}

function IsEmailAddress(input)
{
	try {
		emailStr = input.value.trim();
		if (emailStr == '') return true;
		var arrIP = emailStr.split('@');
		
		var leftPartOk = IsDomainMaskOk(arrIP[0]);
		var rightPartOk = (arrIP.length > 2) ? false : true;
		if (rightPartOk && arrIP.length == 2) rightPartOk = IsDomainMaskOk(arrIP[1]);
		if (leftPartOk && rightPartOk) {
			// if user type more than one
			input.value = ConvertDataToNormalView(emailStr);
			return true;
		}
		else {
			Tip.Show('Incorrect characters in the input field', input, '');
			return false;
		}
	}
	catch (errorMsg) {
		ShowError(errorMsg, '018');
		return false;
	}
}

function ConvertDataToNormalView(param)
{
	try {
		var i=0;
		var result = '';
		if(param.length < 2) {
			return param;
		}
		for(i=1; i < param.length; i++) {
			if(param.charAt(i-1) == '*' && param.charAt(i) == '*') {
				if(result.charAt(result.length-1) != '*') result += '*';
				i++;
			}
			else if(param.charAt(i-1) == '*' && param.charAt(i) != '*') {
				if(result.charAt(result.length-1) != '*') result += '*';
			}
			else {
				result += param.charAt(i-1);
			}
		}
		if(i != 0) result += param.charAt(i-1);
		return result;
	}
	catch (errorMsg) {
		ShowError(errorMsg, '172');
		return false;
	}
}
