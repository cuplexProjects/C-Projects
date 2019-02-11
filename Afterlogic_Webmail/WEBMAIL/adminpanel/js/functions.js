function OpenURL(strUrl)
{
	strUrl = Trim(strUrl);
	if (strUrl.length > 0) {
		var newWin, strProt;
		strProt = strUrl.substr(0,4);
		if (strProt != "http" && strProt != "ftp:")
			strUrl = "http://" + strUrl;
		newWin = window.open(encodeURI(strUrl), null,"toolbar=yes,location=yes,directories=yes,status=yes,scrollbars=yes,resizable=yes,copyhistory=yes")
		newWin.focus();
	}
}

function Trim(str) {
	if (str != null)
	{
		return str.replace(/^\s+/, '').replace(/\s+$/, '');
	}
	return str;
}

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

function GetHeight()
{
	var height = 768;
	if (self.innerHeight)
		height = self.innerHeight;
	else if (document.documentElement && document.documentElement.clientHeight)
		height = document.documentElement.clientHeight;
	else if (document.body.clientHeight)
		height = document.body.clientHeight;
	return height;
}

function CreateChild(parent, tagName)
{
	var node = document.createElement(tagName);
	parent.appendChild(node);
	return node;
}

function CreateTextChild(parent, text)
{
	var node = document.createTextNode(text);
	parent.appendChild(node);
	return node;
}
    
function CreateChildWithAttrs(parent, tagName, arAttrs)
{
	if (Browser.IE) {
		var strAttrs = '';
		var attrsLen = arAttrs.length;
		for (var i=attrsLen-1; i>=0; i--) {
			var t = arAttrs[i];
			var key = t[0];
			var val = t[1];
			strAttrs += ' ' + key + '="'+ val + '"';
		}
		tagName = '<' + tagName + strAttrs + '>';
		var node = document.createElement(tagName);
	} else {
		var node = document.createElement(tagName);
		var attrsLen = arAttrs.length;
		for (var i=attrsLen-1; i>=0; i--) {
			var t = arAttrs[i];
			var key = t[0];
			var val = t[1];
			node.setAttribute(key, val);
		}
	}
	parent.appendChild(node);
	return node;
}

function GetBounds(object)
{
	if (object == null) 
		return {Left: 0, Top: 0, Width: 0, Height: 0};
	var left = object.offsetLeft;
	var top = object.offsetTop;
	for (var parent = object.offsetParent; parent; parent = parent.offsetParent)
	{
		left += parent.offsetLeft;
		top += parent.offsetTop;
	}
	return {Left: left, Top: top, Width: object.offsetWidth, Height: object.offsetHeight};
}
