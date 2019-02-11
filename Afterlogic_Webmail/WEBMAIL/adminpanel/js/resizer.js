
function ResizeElements(mode)
{
	List.ResizeBody();
} 

function CList()
{
	this._logo = document.getElementById('logo');
	this._accountsBar = document.getElementById('accountslist');
	this._toolBar = document.getElementById('toolbar');
	this._lowToolBar = document.getElementById('lowtoolbar');
	// this._copyright	= document.getElementById('copyright');

	// this._copyrightMargin = 30;
	// logo + accountslist + toolbar + lowtoolbar + copyright
	// this._externalHeight = 58 + 32 + 27 + 28 + 40 + this._copyrightMargin;
	this._externalHeight = 58 + 32 + 27 + 20;
	this._contactsHeadersWidth = 175;
	
	this._mainDiv = document.getElementById('main_contacts');
	this._leftDiv = document.getElementById('contacts');
	this._rightDiv = document.getElementById('contacts_viewer');

	this._contactListTbl = document.getElementById('list');
	this._contactListDiv = document.getElementById('contact_list_div');
	this._contactListHeaders = document.getElementById('contact_list_headers');
	this._emailObj = document.getElementById('emailobj');
	this._pageSwitcher = PageSwitcher;

	this._cardTable = document.getElementById('wm_contacts_card');
	
	this.minListHeight = 300;
}

CList.prototype =
{
	ResizeBody: function(mode)
	{
	    if (!Browser.IE || Browser.Version >= 7)
	    {
		    var listBorderHeight = 1;
		    var height = GetHeight() - this.GetExternalHeight();
		    if (height < this.minListHeight) height = this.minListHeight;
		    var tableHeight = this._contactListHeaders.offsetHeight + this._contactListTbl.offsetHeight;
		    var cardHeight = 0;
		    if (this._cardTable != null) cardHeight = this._cardTable.offsetHeight;
		    if (height < tableHeight) height = tableHeight;
		    if (height < cardHeight) height = cardHeight;
            this._mainDiv.style.height = height + 'px';
            this._contactListDiv.style.height = height - listBorderHeight + 'px';
            
            this.ResizeListTable(this._leftDiv.offsetWidth);
		    if (this._cardTable != null) 
		    {
		        this._cardTable.style.width = 'auto';
		        var cardWidth = this._cardTable.offsetWidth;
		        var rightWidth = this._rightDiv.offsetWidth;
		        if (cardWidth < rightWidth) cardWidth = rightWidth;
		        this._cardTable.style.width = cardWidth - 1 + 'px';
		    }
	    }
	    else
	    {
            this._mainDiv.style.width = ((document.documentElement.clientWidth || document.body.clientWidth) < 850) && (this._cardTable != null) ? '850px' : '100%';
		    var listWidth = this._leftDiv.offsetWidth;
		    this.ResizeListTable(listWidth);

            if (this._cardTable != null)
            {
		        var width = GetWidth();
		        if (width < 850) width = 850;
		        this._cardTable.style.width = width - listWidth - 4 + 'px';
		    }
	    }
	    this._pageSwitcher.Replace(this._contactListHeaders);
	},
	
	ResizeListTable: function (listWidth)
	{
		var emailWidth = listWidth - this._contactsHeadersWidth;
	    if (this._emailObj != null && emailWidth > 0) {
		    this._emailObj.style.width = emailWidth + 'px';
	    }
    },
    
	GetExternalHeight: function()
	{
		var res = 0;
		var offsetHeight = this._logo.offsetHeight;    if (offsetHeight) { res += offsetHeight; };
		offsetHeight = this._accountsBar.offsetHeight; if (offsetHeight) { res += offsetHeight; } else { return this._externalHeight; }
		offsetHeight = this._toolBar.offsetHeight;     if (offsetHeight) { res += offsetHeight; } else { return this._externalHeight; }
		offsetHeight = this._lowToolBar.offsetHeight;  if (offsetHeight) { res += offsetHeight; } else { return this._externalHeight; }
//		offsetHeight = this._copyright.offsetHeight;   if (offsetHeight) { res += offsetHeight; } else { return this._externalHeight; }
//		this._externalHeight = res + this._copyrightMargin;
		this._externalHeight = res;
		return this._externalHeight;
	}
}
