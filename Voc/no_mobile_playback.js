function detectmob() { 
 if( navigator.userAgent.match(/Android/i)
 || navigator.userAgent.match(/webOS/i)
 || navigator.userAgent.match(/iPhone/i)
 || navigator.userAgent.match(/iPad/i)
 || navigator.userAgent.match(/iPod/i)
 || navigator.userAgent.match(/BlackBerry/i)
 || navigator.userAgent.match(/Windows Phone/i)
 ){
    return true;
  }
 else {
    return false;
  }
}

function htmlUnescape(value){
    return String(value)
        .replace(/&quot;/g, '"')
        .replace(/&#39;/g, "'")
        .replace(/&lt;/g, '<')
        .replace(/&gt;/g, '>')
        .replace(/&amp;/g, '&');
}

window.onload = function() {
	if (detectmob()) {
		var html = htmlUnescape(document.getElementById('silverlightControlHost').innerHTML);
		
		//Find media source in html
		html = html.match(/<MediaSource>.+<\/MediaSource>/gm);
		if (html)
			html = html[0];
		else
			return;
			
		html = html.replace(/<MediaSource>|<\/MediaSource>/gm,'');
		html = html.replace(/\..+/gm,'');				
		html = ('<video width="852" height="480" controls><source src="{0}_480.mp4" type="video/mp4"><object data="{0}_480.mp4" width="852" height="480"></object></video>').replace(/\{0\}/g,html);		
		document.getElementById('silverlightControlHost').innerHTML = html;
	}
};