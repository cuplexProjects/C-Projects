var ErrorPrefix = "Error on client side - Code:";
var Report;
var Tip;
var ErrorMsg;
var FadeEffect;
var GlobalInfo = {};
var CurrentType = "";
var CurrentCustomDomainIndex = -1;
var CurrentDomainIndex = -1;
var CurrentUserIndex = -1;
//----------------------------------------------------------------------------------------------------------------------------------------------
var currentControl = "NEW_DOMAIN";
var selectedDomain = "";
var selectedCustomDomain = "";
var selectedUser = "";
var selectedUserPassword = "";
var selectedUserType = "";

var selectedDomainIndex = -1;
var selectedCustomDomainIndex = -1;
var selectedUserIndex = -1;

var domainsActiveForm = "";
//----------------------------------------------------------------------------------------------------------------------------------------------
function CBrowser()
{
	this.Init = function()
	{
		var len = this.Profiles.length;
		for (var i = 0; i < len; i++) {
			if (this.Profiles[i].Criterion) {
				this.Name = this.Profiles[i].Id;
				this.Version = this.Profiles[i].Version();
				this.Allowed = this.Version >= this.Profiles[i].AtLeast;
     			break;
			}
   		};
		this.IE = (this.Name == 'Microsoft Internet Explorer');
		this.Opera = (this.Name == 'Opera');
		this.Mozilla = (this.Name == 'Mozilla' || this.Name == 'Firefox' || this.Name == 'Netscape' || this.Name == 'Chrome');
		this.Safari = (this.Name == 'Safari');
		this.Gecko = (this.Opera || this.Mozilla);
	};

	this.Profiles = [
		{
			Id: 'Opera',
			Criterion: window.opera,
			AtLeast: 8,
			Version: function() {
				var r = navigator.userAgent;
				var start1 = r.indexOf('Opera/');
				var start2 = r.indexOf('Opera ');
				if (-1 == start1) {
					var start = start2 + 6;
					var end = r.length;
				}
				else {
					var start = start1 + 6;
					var end = r.indexOf(' ');
				};
				r = parseFloat(r.slice(start, end));
				return r;
			}
		},
		{
			Id: 'Chrome',
			Criterion:
			(
				(navigator.appCodeName.toLowerCase() == 'mozilla') &&
				(navigator.appName.toLowerCase() == 'netscape') &&
				(navigator.product.toLowerCase() == 'gecko') &&
				(navigator.userAgent.toLowerCase().indexOf('chrome') != -1)
			),
			AtLeast: 0,
			Version: function() {
				return parseFloat(navigator.userAgent.split('Chrome/').reverse().join('Chrome/'));
			}
		},
		{
			Id: 'Safari',
			Criterion:
			(
				(navigator.appCodeName.toLowerCase() == 'mozilla') &&
				(navigator.appName.toLowerCase() == 'netscape') &&
				(navigator.product.toLowerCase() == 'gecko') &&
				(navigator.userAgent.toLowerCase().indexOf('safari') != -1)
			),
			AtLeast: 1.2,
			Version: function() {
				var r = navigator.userAgent;
				return parseFloat(r.split('Version/').reverse().join(' '));
			}
		},
		{
			Id: 'Firefox',
			Criterion:
			(
				(navigator.appCodeName.toLowerCase() == 'mozilla') &&
				(navigator.appName.toLowerCase() == 'netscape') &&
				(navigator.product.toLowerCase() == 'gecko') &&
				((navigator.userAgent.toLowerCase().indexOf('firefox') != -1) ||
				(navigator.userAgent.toLowerCase().indexOf('iceweasel') != -1))
			),
			AtLeast: 1,
			Version: function() {
				var userAgent = navigator.userAgent.toLowerCase();
				if (userAgent.indexOf('firefox/') != -1) {
					return parseFloat(userAgent.split('firefox/').reverse().join('firefox/'));
				}
				if (userAgent.indexOf('iceweasel/') != -1) {
					return parseFloat(userAgent.split('iceweasel/').reverse().join('iceweasel/'));
				}
				return 0;
			}
		},
		{
			Id: 'Netscape',
			Criterion:
			(
				(navigator.appCodeName.toLowerCase() == 'mozilla') &&
				(navigator.appName.toLowerCase() == 'netscape') &&
				(navigator.product.toLowerCase() == 'gecko') &&
				(navigator.userAgent.toLowerCase().indexOf('netscape') != -1)
			),
			AtLeast: 7,
			Version: function() {
				var r = navigator.userAgent.split(' ').reverse().join(' ');
				r = parseFloat(r.slice(r.indexOf('/')+1,r.indexOf(' ')));
				return r;
			}
		},
		{
			Id: 'Mozilla',
			Criterion:
			(
				(navigator.appCodeName.toLowerCase() == 'mozilla') &&
				(navigator.appName.toLowerCase() == 'netscape') &&
				(navigator.product.toLowerCase() == 'gecko') &&
				(navigator.userAgent.toLowerCase().indexOf('mozilla') != -1)
			),
			AtLeast: 1,
			Version: function() {
				var r = navigator.userAgent;
				return parseFloat(r.split('Firefox/').reverse().join('Firefox/'));
			}
		},
		{
			Id: 'Microsoft Internet Explorer',
			Criterion:
			(
				(navigator.appName.toLowerCase() == 'microsoft internet explorer') &&
				(navigator.appVersion.toLowerCase().indexOf('msie') != 0) &&
				(navigator.userAgent.toLowerCase().indexOf('msie') != 0) &&
				(!window.opera)
			),
			AtLeast: 5,
			Version: function() {
				var r = navigator.userAgent.toLowerCase();
				r = parseFloat(r.slice(r.indexOf('msie')+4,r.indexOf(';',r.indexOf('msie')+4)));
				return r;
			}
		}
	];

	this.Init();
}

function SysInit()
{
    try
    {
        Tip = new CTip();
        FadeEffect = new CFadeEffect('FadeEffect');
        Report = new CReport('Report');
        Report.Build();	
        ErrorMsg = new CError('ErrorMsg');
        ErrorMsg.Build();	
        ErrorMsg.SetFade(FadeEffect);
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 031');
    }
}

function ClearDDL(objName)
{
    try
    {
        var obj = document.getElementById(objName);
        for(i=0; i < obj.length; i++)
        {
            obj.remove(0);
            i=-1;
        }
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 058');
    }
}

function ClearInputText(objName)
{
    try
    {
        var obj = document.getElementById(objName);
        if (typeof (obj) == 'undefined')
        {
            Tip.Show("Object is not defined", obj, '');
            return;
        }
        obj.value = "";
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 059');
    }
}   

function AutoHeight()
{
    try
    {
    
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 063');
    }
}

function $(id) 
{
	var elementId = '';
	if (typeof(id) == 'string') elementId = id;
	if (typeof(ID_PREFIX) == 'string') elementId = ID_PREFIX + elementId;
	if (elementId == '') {
		MsgBox.Show('Error on client side: Incorrect element ID - "' + elementId + '".', 2);
		return null;
	}
	var element = document.getElementById(elementId);
	if (!element) {
    	element = document.getElementById(id);
        	if (!element) {
		        MsgBox.Show('Error on client side: No element with ID "' + id + '".', 2);
		        return null;
		    }
	}
	return element;
}

/*
function SetDisabled(obj, isDisabled) {
	if (obj) {
		if (isDisabled) {
			if (!obj.type || obj.type == 'checkbox' || obj.type == 'radio') {}
			else {
				obj.style.background = "#ddd";
			}
			obj.disabled = true;
		} else {
			obj.disabled = false;
			if (!obj.type || obj.type == 'checkbox' || obj.type == 'radio') {}
			else {
				obj.style.background = "#fff";
			}
		}
	}
}
*/

function SetDisabled(obj, isDisabled, withLabel) {
	if (obj) {
		
		if (isDisabled) {
			if (!obj.type || obj.type == 'checkbox' || obj.type == 'radio') {}
			else {
				obj.style.background = "#ddd";
			}
			obj.disabled = true;
		} else {
			obj.disabled = false;
			if (!obj.type || obj.type == 'checkbox' || obj.type == 'radio') {}
			else {
				obj.style.background = "#fff";
			}
		}
		
		withLabel = (withLabel == undefined) ? false : withLabel;
		if (withLabel) {
			var _l = $(obj.id + "_label"); 
			if (_l) {
				_l.style.color = (isDisabled) ? "#aaaaaa" : "#000000"; 
			}
		}
	}
}

function DeleteThisDomainAlert()
{
  if (confirm("Are you sure you want to delete this domain?")) 
  {
    return true;
  }
  else
  {
    return false;
  }
}

function DeleteThisUserAlert()
{
  if (confirm("Are you sure you want to delete this user?")) 
  {
    return true;
  }
  else
  {
    return false;
  }
}

function DeleteThisCustomDomainAlert()
{
  if (confirm("Are you sure you want to delete this custom domain?")) 
  {
    return true;
  }
  else
  {
    return false;
  }
}

function CPopupMenu(popup_menu, popup_control, menu_class, popup_move, popup_title, move_class, move_press_class, title_class, title_over_class)
{
	this.popup = popup_menu;
	this.control = popup_control;
	this.move = popup_move;
	this.title = popup_title;
	this.menu_class = menu_class;
	this.move_class = move_class;
	this.move_press_class = move_press_class;
	this.title_class = title_class;
	this.title_over_class = title_over_class;
}

function CPopupMenus()
{
	this.items = Array();
	this.isShown = 0;
}

CPopupMenus.prototype = {
	getLength: function()
	{
		return this.items.length;
	},
	
	addItem: function(popup_menu, popup_control, menu_class, popup_move, popup_title, move_class, move_press_class, title_class, title_over_class)
	{
		this.items.push(new CPopupMenu(popup_menu, popup_control, menu_class, popup_move, popup_title, move_class, move_press_class, title_class, title_over_class));
		this.hideItem(this.getLength() - 1);
	},
	
	showItem: function(item_id)
	{
		this.hideAllItems();
		var item = this.items[item_id];
		var bounds = GetBounds(this.items[item_id].move);
		if (!window.RTL) {
    		item.popup.style.left = bounds.Left + 'px';
		}
		item.popup.style.top = bounds.Top + bounds.Height + 'px';

		item.popup.className = item.menu_class;
		if (item.title_class && item.title_class != '') {
			item.control.className = item.title_class;
			item.title.className = item.title_class;
		}
		if (item.move_press_class && item.move_press_class != '')
			item.move.className = item.move_press_class;
		var obj = this;
		item.control.onclick = function() {
			obj.hideItem(item_id);
		};
		var borders = 1;
		if (item.title_over_class != '') {
			item.control.onmouseover = function(){};
			item.control.onmouseout = function(){};
			item.title.onmouseover = function(){};
			item.title.onmouseout = function(){};
			borders = 2;
		}
		this.isShown = 2;
		item.popup.style.width = 'auto';
		var pOffsetWidth = item.popup.offsetWidth;
		var cOffsetWidth = item.control.offsetWidth;
		var tOffsetWidth = (item.control == item.title) ? 0 : item.title.offsetWidth;
		if (pOffsetWidth < (cOffsetWidth + tOffsetWidth - borders)) {
			item.popup.style.width = (cOffsetWidth + tOffsetWidth - borders) + 'px';
		}
		else {
			item.popup.style.width = (pOffsetWidth + borders) + 'px';
		}
		if (window.RTL) {
    		/* rtl */
	    	item.popup.style.left = (bounds.Left + bounds.Width - item.popup.offsetWidth) + 'px';
		}

		item.popup.style.height = 'auto';
		var pOffsetHeight = item.popup.offsetHeight;
		var height = GetHeight();
		if (pOffsetHeight > height*2/3) {
			item.popup.style.height = Math.round(height*2/3) + 'px';
			item.popup.style.overflowY = 'auto';
		}
		else {
			item.popup.style.overflowY = 'hidden';
		}
		
	},
	
	hideItem: function(item_id)
	{
		this.items[item_id].popup.className = 'wm_hide';
		if (this.items[item_id].move_class && this.items[item_id].move_class != '' && this.items[item_id].move.className != 'wm_hide')
			this.items[item_id].move.className = this.items[item_id].move_class;
		var obj = this;
		this.items[item_id].control.onclick = function() {
			obj.showItem(item_id);
		};
		if (obj.items[item_id].title_over_class != ''){
			this.items[item_id].control.onmouseover = function() {
				obj.items[item_id].title.className = obj.items[item_id].title_over_class; 
				obj.items[item_id].control.className = obj.items[item_id].title_over_class;
			};
			this.items[item_id].control.onmouseout = function() {
				obj.items[item_id].title.className = obj.items[item_id].title_class; 
				obj.items[item_id].control.className = obj.items[item_id].title_class; 
			};
			this.items[item_id].title.onmouseover = function() {
				obj.items[item_id].title.className = obj.items[item_id].title_over_class; 
			};
			this.items[item_id].title.onmouseout = function() {
				obj.items[item_id].title.className = obj.items[item_id].title_class; 
			}
		}
	},
	
	hideAllItems: function()
	{
		for (var i = this.getLength() - 1; i >= 0; i--) {
			this.hideItem(i);
		};
		this.isShown = 0;
	},
	
	checkShownItems: function()
	{
		if (this.isShown == 1) {
			this.hideAllItems()
		};
		if (this.isShown == 2) {
			this.isShown = 1;
		}
	}
};


