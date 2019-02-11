/*
 * Classes:
 *  CError
 *  CReport
 *  ReportPrototype
 *  CInformation
 *  CFadeEffect
 */

function PopUpWindow(url) {
	var shown = window.open(url, 'Popup',
		'left=(screen.width-700)/2,top=(screen.height-400)/2,'+
		'toolbar=no,location=no,directories=no,status=yes,scrollbars=yes,resizable=yes,'+
		'copyhistory=no,width=700,height=400');
	shown.focus();
	return false;
}
 
function CreateChild(parent, tagName)
{
	var node = document.createElement(tagName);
	parent.appendChild(node);
	return node;
}

function GetWidth()
{
	var width = 1024;
	if (document.documentElement && document.documentElement.clientWidth)
		width = document.documentElement.clientWidth;
	else if (document.body.clientWidth)
		width = document.body.clientWidth;
	else if (self.innerWidth)
		width = self.innerWidth;
	return width;
}

function GetBounds(object)
{
	if (object == null) 
		return {Left: 0, Top: 0, Width: 0, Height: 0};
	var left = object.offsetLeft;
	var top = object.offsetTop;
	for (var parent = object.offsetParent; parent; parent = parent.offsetParent) {
		left += parent.offsetLeft;
		top += parent.offsetTop;
	};
	return {Left: left, Top: top, Width: object.offsetWidth, Height: object.offsetHeight};
}

function CError(name) {
	this._name = name;
	this._containerObj = null;
	this._messageObj = null;
	this._controlObj = null;
	this._fadeObj = null;
	this._delay = 10000;

	this.Build = function() {
		var tbl, tr, td, aDiv, infoDiv, infoBlock, infoShadow, bDiv, div, closeImageDiv, obj;

		tbl = CreateChildWithAttrs(document.body, 'table',
[['class', 'wm_hide'],
['cellpadding', '0'],
['cellspacing', '0']]);

		tr = CreateChildWithAttrs(tbl, 'tr', [['style', 'position:relative;z-index:20']]);
		td = CreateChild(tr, 'td');
		
		infoBlock = CreateChildWithAttrs(td, 'div', [['class', 'wm_info_block']]);
	    infoShadow = CreateChildWithAttrs(infoBlock, 'div', [['class', 'wm_shadow']]);
	    infoDiv = CreateChildWithAttrs(infoBlock, 'div', [['class', 'wm_info_message'], ['id', 'info_message']]);

		aDiv = CreateChildWithAttrs(infoBlock, 'div', [['class', 'a']]);
		aDiv.innerHTML = '&nbsp;';
		bDiv = CreateChildWithAttrs(infoBlock, 'div', [['class', 'b']]);
		bDiv.innerHTML = '&nbsp;';
		CreateChildWithAttrs(infoDiv, 'div', [['class', 'wm_info_image']]);
		closeImageDiv = CreateChildWithAttrs(infoDiv, 'div', [['class', 'wm_close_info_image wm_control']]);
		obj = this;
		closeImageDiv.onclick = function() {
			obj.Hide();
		};
		this._containerObj = tbl;
		this._messageObj = CreateChild(infoDiv, 'span');
		this._controlObj = new CInformation(tbl, 'wm_error_information');
	};
}

function CReport(name) {
	this._name = name;
	this._containerObj = null;
	this._messageObj = null;
	this._controlObj = null;
	this._fadeObj = null;
	this._delay = 5000;

	this.Build = function() {
		var tbl, tr, td, aDiv, infoDiv, div, bDiv;

		tbl = CreateChildWithAttrs(document.body, 'table',
[['class', 'wm_hide'],
['cellpadding', '0'],
['cellspacing', '0']]);

		tr = CreateChildWithAttrs(tbl, 'tr', [['style', 'position:relative;z-index:20']]);
		td = CreateChild(tr, 'td');
		
		infoBlock = CreateChildWithAttrs(td, 'div', [['class', 'wm_info_block']]);
	    infoShadow = CreateChildWithAttrs(infoBlock, 'div', [['class', 'wm_shadow']]);
	    infoDiv = CreateChildWithAttrs(infoBlock, 'div', [['class', 'wm_info_message'], ['id', 'info_message']]);

		aDiv = CreateChildWithAttrs(infoBlock, 'div', [['class', 'a']]);
		aDiv.innerHTML = '&nbsp;';
		bDiv = CreateChildWithAttrs(infoBlock, 'div', [['class', 'b']]);
		bDiv.innerHTML = '&nbsp;';
		obj = this;
		this._containerObj = tbl;
		this._messageObj = CreateChild(infoDiv, 'span');
		this._controlObj = new CInformation(tbl, 'wm_report_information');
	};
}

function CInfo(name) {
	this._name = name;
	this._containerObj = null;
	this._messageObj = null;
	this._controlObj = null;
	this._fadeObj = null;
	this._delay = 5000;

	this.Build = function () {
	var tbl, tr, td, aDiv, infoDiv, div, bDiv;

	tbl = CreateChildWithAttrs(document.body, 'table',
[['class', 'wm_hide'],
['cellpadding', '0'],
['cellspacing', '0']]);

		tr = CreateChildWithAttrs(tbl, 'tr', [['style', 'position:relative;z-index:20']]);
		td = CreateChild(tr, 'td');
		
		infoBlock = CreateChildWithAttrs(td, 'div', [['class', 'wm_info_block']]);
	    infoShadow = CreateChildWithAttrs(infoBlock, 'div', [['class', 'wm_shadow']]);
	    infoDiv = CreateChildWithAttrs(infoBlock, 'div', [['class', 'wm_info_message'], ['id', 'info_message']]);

		aDiv = CreateChildWithAttrs(infoBlock, 'div', [['class', 'a']]);
		aDiv.innerHTML = '&nbsp;';
		bDiv = CreateChildWithAttrs(infoBlock, 'div', [['class', 'b']]);
		bDiv.innerHTML = '&nbsp;';
		obj = this;
		this._containerObj = tbl;
		this._messageObj = CreateChild(infoDiv, 'span');
	this._controlObj = new CInformation(tbl, 'wm_information');

	};
}


var ReportPrototype = {
	Show: function (msg, priorDelay) {
		this._messageObj.innerHTML = msg;
		this._controlObj.Show();
		this._controlObj.Resize();
		if (null != this._fadeObj) {
			var interval = (priorDelay)
				? this._fadeObj.Go(this._containerObj, priorDelay)
				: this._fadeObj.Go(this._containerObj, this._delay);
			
			if (this._name) setTimeout(this._name + '.Hide()', interval);
		} else {
			if (this._name) {
				if (priorDelay){
					setTimeout(this._name + '.Hide()', priorDelay);
				} else {
					setTimeout(this._name + '.Hide()', this._delay);
				}
			}
		}
	},
	
	SetFade: function (fadeObj) {
		this._fadeObj = fadeObj;
	},
	
	Hide: function () {
		this._controlObj.Hide();
		if (null != this._fadeObj) this._fadeObj.SetOpacity(1, IsGoodIE());
	},
	
	Resize: function () {
		this._controlObj.Resize();		
	}
};

CInfo.prototype = ReportPrototype;
CReport.prototype = ReportPrototype;
CError.prototype = ReportPrototype;

/* for control placement and displaying of information block */
function CInformation(cont, cls) {
	this._mainContainer = cont;
	this._containerClass = cls;
}

CInformation.prototype = {
	Show: function () {
		this._mainContainer.className = this._containerClass;
	},
	
	Hide: function () {
		this._mainContainer.className = 'wm_hide';
	},

	Resize: function () {
		var tbl = this._mainContainer;
		tbl.style.width = 'auto';
		var offsetWidth = tbl.offsetWidth;
		var width = GetWidth();

		var tblLeft = Math.round(width / 2 - offsetWidth / 2);
		tbl.style.left =  tblLeft + 'px';
		tbl.style.top = this.GetScrollY() + 'px';
	},

	GetScrollY: function() {
		var scrollY = 0;
		if (document.body && typeof document.body.scrollTop != "undefined") {
			scrollY += document.body.scrollTop;
			if (scrollY == 0 && document.body.parentNode && typeof document.body.parentNode != "undefined") {
				scrollY += document.body.parentNode.scrollTop;
			}
		}
		else if (typeof window.pageXOffset != "undefined")  {
			scrollY += window.pageYOffset;
		};
		return scrollY;
	}
};

function CFadeEffect(name) {
	this._name = name;
	this._elem = null;
}

CFadeEffect.prototype = {
	Go: function (elem, delay) {
		this._elem = elem;
		var interval = 50;
		var iCount = 10;
		var diff = 1/iCount;
		var isIE = IsGoodIE() ? 'true' : 'false';
		for(var i=0; i<=iCount; i++) {
			setTimeout(this._name + '.SetOpacity('+ (1 - diff*i) + ', ' + isIE + ')', delay + interval*i);
		};
		return delay + interval*iCount;
	},
	
	SetOpacity: function (opacity, isIE) {
		var elem = this._elem;
		/* Internet Exploder 5.5+ */
		if (isIE)
		{
			opacity *= 100;
			var oAlpha = elem.filters['DXImageTransform.Microsoft.alpha'] || elem.filters.alpha;
			if (oAlpha) {
				oAlpha.opacity = opacity;
			} else {
				elem.style.filter += "progid:DXImageTransform.Microsoft.Alpha(opacity="+opacity+")";
			}
		} else {
			elem.style.opacity = opacity;		/* CSS3 compliant (Moz 1.7+, Safari 1.2+, Opera 9) */
			elem.style.MozOpacity = opacity;	/* Mozilla 1.6-, Firefox 0.8 */
			elem.style.KhtmlOpacity = opacity;	/* Konqueror 3.1, Safari 1.1 */
		}
	}
};

var MsgBox = {
	_fadeObj: null,
	_errorObj: null,
	_reportObj: null,
	_infoObj: null,
	_skin: 'adminpanel',
	
	Init: function () {
		this._skin = _apPath;
		if (this._fadeObj == null) this._fadeObj = new CFadeEffect('MsgBox._fadeObj');
		if (this._errorObj == null) {
			this._errorObj = new CError('MsgBox._errorObj');
			this._errorObj.Build(this._skin);
			this._errorObj.SetFade(this._fadeObj);
		}
		if (this._reportObj == null) {
			this._reportObj = new CReport('MsgBox._reportObj');
			this._reportObj.Build();
			this._reportObj.SetFade(this._fadeObj);
		}
		if (this._infoObj == null) {
			this._infoObj = new CInfo('MsgBox._infoObj');
			this._infoObj.Build();
			this._infoObj.SetFade(this._fadeObj);
		}
	},
	
	/*
	 * type = 0 - info
	 * type = 1 - report
	 * type = 2 - error
	 */
	Show: function (msg, type, delay) {
		this.Init();
		if (!type) type = 0;
		switch (type) {
			case 0: this._infoObj.Show(msg, delay);	break;
			case 1: this._reportObj.Show(msg, delay); break;
			case 2:	this._errorObj.Show(msg, delay); break;
		}
	}
};

function OnlineMsgError(text)
{
	MsgBox.Show(text, 2);
}

function OnlineMsgInfo(text)
{
	MsgBox.Show(text, 1);
}

var IFrame;
function CTip()
{
    if (navigator.userAgent.indexOf("MSIE") >= 0)
    {
        IFrame = CreateChild(document.body, 'iframe');
        IFrame.style.position = 'absolute';
        IFrame.className = 'wm_hide';
    }
    
    this._container = CreateChild(document.body, 'table');
    this._container.className = 'wm_hide';
    var tr = this._container.insertRow(0);
    var td = tr.insertCell(0);
    td.className = 'wm_tip_arrow';
    this._message = tr.insertCell(1);
    this._message.className = 'wm_tip_info';
    this._base = '';
}

CTip.prototype = {
    SetMessageText: function(text)
    {
        this._message.innerHTML = text;
    },
    
    SetCoord: function(element)
    {
        var bounds = GetBounds(element);
        this._container.style.top = (bounds.Top + bounds.Height/2 - 16) + 'px';
        this._container.style.left = (bounds.Left + bounds.Width - 5) + 'px';
    },
    
    Show: function(text, element, base)
    {
        this.SetMessageText(text);
        this.SetCoord(element);
        this._base = base;
        this._container.id = 'tipID';
        this._container.className = 'wm_tip';
        
        if (navigator.userAgent.indexOf("MSIE") >= 0)
        {
            IFrame.style.top = document.getElementById('tipID').style.top;
            IFrame.style.left = 20 + parseInt(document.getElementById('tipID').style.left, 10) + 'px';
            IFrame.style.width = this._container.offsetWidth - 20 + 'px';   
            IFrame.style.height = this._container.offsetHeight + 'px';   
            IFrame.style.border = 'none';
            IFrame.style.display = 'block';
        }
    },
    
    Hide: function(base)
    { 
        if (navigator.userAgent.indexOf("MSIE") >= 0)
        {      
            IFrame.style.display = 'none';
        }
        if (this._base == base || this._base == '')
                this._container.className = 'wm_hide';
    }
};

var PopupDiv = 
{
	_container: null,
	_initialized: false,
	
	_init: function ()
	{
		if (this._initialized) {
			return;
		}
		this._container = $('popup_restart_help');
		if (this._container) {
			this._container.className = 'wm_hide';
		}
		this._initialized = true;
	},

    SetCoord: function()
    {
    	if (this._container) {
		    var height = GetHeight();
	        var width = GetWidth();
	        
	        height = (height/2 - 160/2);
	        width = (width/2 - 420/2);
			height = (height < 50) ? 50 : Math.round(height); 
			width = (width < 50) ? 50 :  Math.round(width); ;
	        
	        this._container.style.top = height + 'px';
	        this._container.style.left = width + 'px';
		}
    },
    
    Show: function()
    {
    	this._init();
		this.SetCoord();
		if (this._container) {
        	this._container.className = 'wm_restart_div';
        }
    },
    
    Hide: function(base)
    {
    	this._init();
    	if (this._container) {
        	this._container.className = 'wm_hide';
        }
    }
};