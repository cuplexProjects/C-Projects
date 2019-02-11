function ParseCId(idstring)
{
	var IdArray = idstring.split('_');
	return (IdArray.length > 1) ?
		{type: IdArray[0], cid: IdArray[1]} : null;
	//return (IdArray.length > 3) ? {type: IdArray[0], cid: IdArray[1], c_name: IdArray[2], c_email: IdArray[3]} : null;
}

function Init_list(ListId)
{
	var list = document.getElementById(ListId);
	var tr_arr = list.getElementsByTagName("tr");
	
	Selection = new CListSelection();
	
	for (i=0; i < tr_arr.length; i++)
	{
		this.line = null;
		
		if ((tr_arr[i].id != "contact_list_headers")&&(tr_arr[i].id != "none")&&(tr_arr[i].id != ""))
		    this.line = document.getElementById(tr_arr[i].id);
		    
		if (this.line)
		{
			Selection.AddLine(new CListSelectionPart(tr_arr[i]));
	
    		this.line.onclick = function(e)
			{
				e = e ? e : window.event;
				if(e.ctrlKey) {
					Selection.CheckCtrlLine(this.id);
				} else if (e.shiftKey) {
					Selection.CheckShiftLine(this.id);
				} else {
				
				    if (Browser.Mozilla) {var elem = e.target;}
					else {var elem = e.srcElement;}
						
					if (!elem || elem.id == "none") {
						return false;
					}			
						
					var loverTag = elem.tagName.toLowerCase();
			
					if (loverTag == "input") {
						Selection.CheckCBox(this.id);
					} else {
						Selection.CheckLine(this.id);
						ViewAddressRecord(this.id);
					}
				}
			}
			
			this.line.ondblclick = function(e)
			{
			    Selection.CheckCtrlLine(this.id);
				e = e ? e : window.event;
				if (Browser.Mozilla) {var elem = e.target;}
				else {var elem = e.srcElement;}
					
				if (!elem || elem.id == "none" || elem.tagName.toLowerCase() == "input") {
					return false;
				}
			}
		}
	}
}

function CListSelection()
{
	this.lines = Array();
	this.length = 0;
	this.prev = -1;
	
	this.AllCheckBox = document.getElementById("allcheck"); 
}

CListSelection.prototype = 
{
	AddLine: function (line)
	{
		this.lines.push(line);
		this.length = this.lines.length;
	},
	
	GetCheckedLines: function ()
	{
		var idArray = Array();
		for (var i = this.length-1; i >= 0; i--) {
			var line = this.lines[i];
			if (line.Checked == true) {
				idArray.push(line.Id);
			}
		}
		return idArray;
	},
	
	CheckCtrlLine: function(id)
	{

		for (var i = this.length-1; i >= 0; i--) {
			var line = this.lines[i];
			if (line.Id == id){
				if (line.Checked == false) {
					line.Check();
					this.prev = i;
				} else {
					line.Uncheck();
				}
			}
		}
		this.ReCheckAllBox();
	},
	
	CheckLine: function(id)
	{

		for (var i = this.length-1; i >= 0; i--) {
			var line = this.lines[i];
			if (line.Id == id){
				line.Check();
				this.prev = i;
			} else {
				line.Uncheck();
			}
		}
		this.ReCheckAllBox();
	},
	
	CheckShiftLine: function(id)
	{

		if (this.prev == -1) {
			this.CheckLine(id);
		} else {
			var isChecking = false;
			var prev = this.prev;
			for (var i = 0; i < this.length; i++) {
				var line = this.lines[i];
				if (this.prev == i || line.Id == id)
					isChecking = isChecking ? false : true;
				if (line.Id == id)
					prev = i;
				if (isChecking || this.prev == i || line.Id == id) {
					line.Check();
				} else {
					line.Uncheck();
				}
			}
			//this.prev = prev;
		}
		this.ReCheckAllBox();
	},
	
	UncheckAll: function ()
	{
		for (var i = this.length-1; i >= 0; i--) {
			this.lines[i].Uncheck();
		}
		this.prev = -1;
	},
	
	ReCheckAllBox: function()
	{
		var isAllCheck = true;

		for (var i = this.length-1; i >= 0; i--) {
			if (this.lines[i].Checked == false) { isAllCheck = false;}
		}
		if (this.length == 0) { isAllCheck = false;}
		
		if (this.AllCheckBox)
		{
			this.AllCheckBox.checked = isAllCheck;
		}		
	},
	
	CheckCBox: function(id)
	{
		for (var i = this.length-1; i >= 0; i--) {
			var line = this.lines[i];
			if (line.Id == id){
				if (line.Checked == false) {
					line.Check();
					this.prev = i;
				} else {
					line.Uncheck();
				}
			}
		}

		this.ReCheckAllBox();
	},
		
	CheckAllBox: function(objCheckbox)
	{
		for (var i = this.length-1; i >= 0; i--) {
			var line = this.lines[i];
			if (objCheckbox.checked) {
				line.Check();
			} else {
				line.Uncheck();
			}
		}		
	}
}

function CListSelectionPart(tr)
{
	tr.onmousedown = function() {return false;}//don't select content in Opera
	tr.onselectstart = function() {return false;}//don't select content in IE
	tr.onselect = function() {return false;}//don't select content in IE
	this._tr = tr;
	this._className = tr.className;
	this.Id = tr.id;
	this.Checked = false;
	
	var collection = this._tr.getElementsByTagName('td');
	if (collection.length > 1) {
		this._checkTd = collection[0];
		var checkboxcoll = this._checkTd.getElementsByTagName('input');
		if (checkboxcoll.length > 0) {
				this._checkbox = checkboxcoll[0];
		}
	} 
	this.ApplyClassName();
}

CListSelectionPart.prototype = {
	Check: function()
	{
		this.Checked = true;
		this.ApplyClassName();
		this.AppleCheckBox();
	},

	Uncheck: function()
	{
		this.Checked = false;
		this.ApplyClassName();
		this.AppleCheckBox();
	},
	
	ApplyClassName: function ()
	{
		if (this.Checked)
			this._tr.className = this._className + '_select';
		else
			this._tr.className = this._className;
	},
	
	AppleCheckBox: function ()
	{
		if (this._checkbox) this._checkbox.checked = (this.Checked);
	} 
}
