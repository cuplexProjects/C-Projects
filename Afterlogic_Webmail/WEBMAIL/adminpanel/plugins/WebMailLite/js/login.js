
function CHideLoginItem(id, radioId, divId)
{
	this.Id = id;
    this._radio = $(radioId);
    this._div = document.getElementById(divId);
    
    this.Checked = function ()
    {
    	if (this._radio == null) return false;
    	return this._radio.checked;
    };
    
    this.MakeActive = function ()
    {
    	if (this._div == null) return;
    	this._SetDisabled(false);
    };

    this.MakePassive = function ()
    {
    	if (this._div == null) return;
    	this._SetDisabled(true);
    };
    
    this._SetDisabled = function (bValue)
    {
    	this._div.className = (bValue) ? '' : 'activ';
    	var inputs = this._div.getElementsByTagName("INPUT");
    	var selects = this._div.getElementsByTagName("SELECT");
    	var i, c;
    	for (i = 1, c = inputs.length; i < c; i++)
    	{
			 inputs[i].disabled = bValue;   	
    	}
    	for (i = 0, c = selects.length; i < c; i++)
    	{
			 selects[i].disabled = bValue;   	
    	}
    }

	if (this._radio == null) return;
    this._radio.onclick = function ()
    {
    	if (this.checked)
    		wmLoginSettings.SetMode(id);
    };
}

var wmLoginSettings = {
	Constants: {
		StandardLogin: 0,
		HideLogin: 1,
		HideEmail: 2
	},
	_mode: 0,
	_items: Array(),
	
	Init: function ()
	{
		var consts = this.Constants;
	    this._items[consts.StandardLogin] = new CHideLoginItem(consts.StandardLogin, 'standartLoginRadio', 'hideLoginDiv1');
	    this._items[consts.HideLogin] = new CHideLoginItem(consts.HideLogin, 'hideLoginRadio', 'hideLoginDiv2');
	    this._items[consts.HideEmail] = new CHideLoginItem(consts.HideEmail, 'hideEmailRadio', 'hideLoginDiv3');
		
		var iCount = this._items.length;
		for (var i=0; i<iCount; i++) {
			var item = this._items[i];
			if (item.Checked()) {
				item.MakeActive();
				this._mode = item.Id;
			}
			else
			{
				item.MakePassive();
			}
		}
	},
	
	SetMode: function (mode)
	{
		var iCount = this._items.length;
		for (var i=0; i<iCount; i++) {
			var item = this._items[i];
			if (item.Id == mode) {
				item.MakeActive();
			}
			else {
				item.MakePassive();
			}
		}
	}
};
