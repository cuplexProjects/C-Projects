var wmDbSettings = {
	_radioMySQL: null,
	_radioMSSQL: null,
	_radioMSAccess: null,
	
	_msg: null,
	
	_login: null,
	_password: null,
	_dbname: null,
	_dsn: null,
	_host: null,
	_accessfile: null,
	_create_database: null,
	
	_dsnSwitcher: null,
	_csSwitcher: null,
	_csSwitcherChild: null,
	
	_button: null,
	
	_isMSSQL: true,
	_isMySQL: true,
	_isODBC: true,
	
	_allGood: false,
	
	Init: function ()
	{
		var obj = this;
		this._radioMSSQL = $('intDbTypeMsSql');
		this._radioMySQL = $('intDbTypeMySql');
		this._radioMSAccess = $('intDbTypeMsAccess');
		
		this._msg = $('dbMessageDiv');
		
		this._login = $('txtSqlLogin');
		this._password = $('txtSqlPassword');
		this._dbname = $('txtSqlName');
		this._dsn = $('txtSqlDsn');
		this._host = $('txtSqlSrc');
		this._accessfile = $('txtAccessFile');
		
		this._create_database = $('create_database'); 

		this._buttonTest = $('test_btn');
		this._buttonSubmit = $('submit_btn');
		
		this._dsnSwitcher = $("useDSN");
		this._csSwitcher = $("useCS");
		this._csSwitcherChild = $("odbcConnectionString");
		
		this._allGood = (this._radioMySQL && this._radioMSSQL && this._msg &&
			this._login && this._password && this._dbname && this._dsn && this._host &&
			this._buttonTest && this._buttonSubmit && this._csSwitcher && this._csSwitcherChild  && this._dsnSwitcher);
			
		if (this._allGood) {
			this._dsnSwitcher.onclick = function() {
				if (obj._dsnSwitcher.checked) {
					obj._csSwitcher.checked = false;
				};
				obj.InitForm();
			};		
		
			this._csSwitcher.onclick = function() {
				if (obj._csSwitcher.checked) {
					obj._dsnSwitcher.checked = false;
				};
				obj.InitForm();
			};
			
			this._radioMySQL.onclick = function() {
				obj.InitForm();
			};
			
			this._radioMSSQL.onclick = function() {
				obj.InitForm();
			};
			
			this._radioMSAccess.onclick = function() {
				obj.InitForm();
			};

			this._buttonTest.form.onsubmit = function() { 
				return true;
			};
			
			this._buttonSubmit.form.onsubmit = function() { 
				obj._buttonSubmit.disabled = true;
				obj._buttonTest.disabled = true;
				return true;
			};
			
			this._dsn.onkeyup = function() {
				obj.InitForm();
			};
		}
		
		this.InitForm();
	},
	
	SetCsCheckBoxView: function(isHide)
	{
		if (this._allGood) {
			this._csSwitcher.className = (isHide) ? "wm_hide" : "wm_checkbox";
		}
	},
	
	SetInfo: function (msg)
	{
		if (this._allGood) {
			if (msg.length > 0) {
				this._msg.className = "wm_install_db_msg";
				this._msg.innerHTML = msg;
			} else {
				this._msg.className = "wm_install_db_msg_null";
				this._msg.innerHTML = '<br />';
			}
			
		}
	},
	
	InitForm: function ()
	{
			var _msg = '';
			var _useOdbc = false;
			
			SetDisabled(this._radioMySQL, false, true);
			SetDisabled(this._radioMSSQL, false, true);

			SetDisabled(this._csSwitcherChild, !(this._csSwitcher.checked));

			if (this._radioMSAccess.checked || this._radioMSSQL.checked)
			{
			    this._dsnSwitcher.checked = false;
			}
			
			SetDisabled(this._dsnSwitcher, (this._radioMSAccess.checked || this._radioMSSQL.checked), true);

			SetDisabled(this._dsn, !(this._dsnSwitcher.checked));
			var _isDsn = (!this._dsn.disabled && this._dsnSwitcher.checked);
						
			SetDisabled(this._login, (this._csSwitcher.checked || _isDsn || _useOdbc || this._radioMSAccess.checked), true);
			SetDisabled(this._password, (this._csSwitcher.checked || _isDsn || _useOdbc || this._radioMSAccess.checked), true);

			SetDisabled(this._dbname, (this._csSwitcher.checked || _isDsn || _useOdbc || this._radioMSAccess.checked), true);
			SetDisabled(this._host, (this._csSwitcher.checked || _isDsn || _useOdbc || this._radioMSAccess.checked), true);
			
			SetDisabled(this._accessfile, (this._csSwitcher.checked || _isDsn || _useOdbc || this._radioMSSQL.checked || this._radioMySQL.checked), true);
			
			if (this._csSwitcher.checked || _isDsn || _useOdbc || this._radioMSAccess.checked)
			{
			    this._create_database.disabled = true;
			}
			else
			{
			    this._create_database.disabled = false;
			}

			this.SetInfo(_msg);
	}
};

var SettingsObjects = Array();
SettingsObjects['db'] = wmDbSettings;