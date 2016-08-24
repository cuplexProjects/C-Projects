var l = 0, t = 0;
var IE = document.all ? true : false;
var tooltip = document.createElement("div");
tooltip.id = 'tooltip';

var currentMainTab = 'ServerSettingsTabID';
var currentPanel;
var currentServerTab = "S_0";
//----------------------------------------------------------------------------------------------------------------------------------------------
function ShowError(errorMsg, errorCode)
{
	var ErrorPrefix = "Error on client side - Code:";
	MsgBox.Show(ErrorPrefix + ' ' + errorCode + '<br/>' + errorMsg, 2);
}
//----------------------------------------------------------------------------------------------------------------------------------------------
function getMouseXY(e)
{
    try {
	    if (IE) {
		    l = event.clientX + document.documentElement.scrollLeft;
		    t = event.clientY + document.documentElement.scrollTop;
	    }
	    else {
		    l = e.pageX;
		    t = e.pageY;
	    }  
	    tooltip.style.left = l + "px";
	    tooltip.style.top = t + "px";
	    return true;
    }
    catch(errorMsg) {
    	ShowError(errorMsg, 103);
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
var _seeToolTip = 0;
function AddToolTip(tooltip_text)
{
    try {
		if (window.event) getMouseXY(window.event);
		document.onmousemove = getMouseXY;
	    document.body.appendChild(tooltip);
	    tooltip.innerHTML = tooltip_text;
	    _seeToolTip++;
    }
    catch(errorMsg) {
    	ShowError(errorMsg, 104);
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
function RemoveToolTip() 
{
    try {
		document.onmousemove = '';
    	if (tooltip && _seeToolTip > 0) {
    		_seeToolTip--;
        	document.body.removeChild(tooltip);
        }
    }
    catch (errorMsg) {
        ShowError(errorMsg, 105);
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
function mainTabSwitch(id) 
{
    try
    {
	    if (id == 'mainTab1') 
	    {
		    document.getElementById(currentMainTab).className ='wm_accountslist_email';
		    document.getElementById(id).className='wm_accountslist_email_activ';
		    document.getElementById('ServerSettingsTabID').className='wm_settings';
		    document.getElementById('DomainsAndUsersTabID').className='hide';
		    viewPanel('new_domain_panel');
		    currentMainTab = document.getElementById(id).id;
	    } 
	    else 
	    {
		    document.getElementById(currentMainTab).className ='wm_accountslist_email';
		    document.getElementById(id).className='wm_accountslist_email_activ';
		    document.getElementById('DomainsAndUsersTabID').className='wm_settings'; 
		    document.getElementById('ServerSettingsTabID').className='hide'; 
		    viewPanel('new_domain_panel');
		    currentMainTab = document.getElementById(id).id;
	    }
    }
    catch(errorMsg)
    {
        ShowError(errorMsg, 106);
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
function toggle_sTab(x)
{
    try
    {
        ShowServerSettingsSaveButton();
	    if (x.className == 'wm_selected_settings_item') 
	    {
		    x.className = 'wm_settings_item'
	    }
	    else 
	    {
		    document.getElementById(currentServerTab).className = 'wm_settings_item';
		    x.className = 'wm_selected_settings_item';
		    currentServerTab = x.id;
	    }
    }
    catch(errorMsg)
    {
        ShowError(errorMsg, 107);
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
function viewPanel(Panel) 
{
    try
    {
	    if (currentPanel == undefined) 
	    {
		    currentPanel = 'server_tab_1';
	    }
        document.getElementById(currentPanel).className = 'hide';
        document.getElementById(Panel).className = 'wm_admin_center';
        currentPanel = Panel;
    }
    catch(errorMsg)
    {
        ShowError(errorMsg, 108);
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
function advSwitch (el) 
{
    try
    {
	    if (el.parentNode.className == 'expanded') 
	    {
		    el.parentNode.className = 'convoluted';
	    }	
	    else 
	    {
		    el.parentNode.className = 'expanded';
	    }
    }
    catch(errorMsg)
    {
        ShowError(errorMsg, 109);
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------