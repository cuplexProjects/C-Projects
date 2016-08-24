/* Google Analytics */
var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-38050167-1']);
_gaq.push(['_trackPageview']);

(function() {
var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();

/* Remove the current search field and button*/
$j('#suggestions_query').remove();
$j('#suggestion_submit').remove();

var postbackSearch = $j('input#query').size()>0;
if (postbackSearch) {
	$j('input#query').remove();
	$j('#buttonsubmit.button.search.primary').remove();
}

/* Add the new search field and button*/
var new_form = '<input id="suggestions_query" name="suggestions_query" type="text" placeholder="Search"><input class="button search primary" id="suggestion_submit" name="commit" type="submit" value="">';

if (postbackSearch) {
	new_form = new_form.replace('placeholder="Search"','placeholder="Search in this category"');
	$j('#searchform').prepend(new_form);
}
else
	$j('#suggest_form').prepend(new_form);

/* Place the search field after pinned topics and before forum tabs*/
$j('#solution_suggestion').detach().insertBefore($j('#contentcolumn').find('.forum_tabs:first'));

//Change "pinned_topics_title" to "Looking for answers?".
$j('h2#pinned_topics_title').html('Looking for answers?');

var footer = '<div class="voc_footer"><a href="javascript:void(0);" onclick="showFooterPopup(3);">&copy; 1998-2013 Volvo Cars Corporation</a><a href="javascript:void(0);" onclick="showFooterPopup(1);">@{Privacy_Cookies}</a><a href="javascript:void(0);" onclick="showFooterPopup(2);">@{Terms_of_Use}</a><div class="applink_button_wrapper"><a href="http://www.windowsphone.com/sv-se/store/app/volvo-on-call/d367904a-070d-42ef-9dd1-7fae68b318a2" target="_blank" class="app_link"><img src="@{Hosting_Base_URL}/images/btn_Appstore_Windows.png" alt="Windows phone link" /></a><a href="https://play.google.com/store/apps/details?id=se.volvo.vcc&feature=search_result#?t=W251bGwsMSwxLDEsInNlLnZvbHZvLnZjYyJd" target="_blank" class="app_link"><img src="@{Hosting_Base_URL}/images/btn_Appstore_Google.png" alt="Google play link" /></a><a href="https://itunes.apple.com/se/app/volvo-on-call/id439635293?mt=8" target="_blank" class="app_link"><img src="@{Hosting_Base_URL}/images/btn_Appstore_iOS.png" alt="Appstore link" /></a></div><a href="/anonymous_requests/new">@{Contact_Us}</a><a href="/access/unauthenticated">@{Login}</a></div>';

if (!currentUser.isAnonymous) {
	footer = footer.replace('<a href="/access/unauthenticated">@{Login}</a>','<a href="/access/logout">@{Sign_out}</a><a id="agent_link" href="/agent">@{Agent_View}</a>');
}

$j('#footer').empty().hide();

var sidebar = $j('#sidebar');
var ie8orLower = parseInt($j.browser.version, 10) <= 8;
var isOnFirstPage = false;

//Custom center function
jQuery.fn.center = function () {
    this.css("position", "absolute");
    this.css("top", ($j(window).height() - this.height()) / 2 + $j(window).scrollTop() + "px");
    this.css("left", ($j(window).width() - this.width()) / 2 + $j(window).scrollLeft() + "px");
    return this;
};

/* Load Volvo Sans */
WebFontConfig = {
	custom: { families: ['volvo_sans_pro'],
	urls: [ '@{Hosting_Base_URL}/fonts/fontstyles.css' ] }
};
(function() {
	var wf = document.createElement('script');
	wf.src = ('https:' == document.location.protocol ? 'https' : 'http') +
		'://ajax.googleapis.com/ajax/libs/webfont/1/webfont.js';
	wf.type = 'text/javascript';
	wf.async = 'true';
	var s = document.getElementsByTagName('script')[0];
	s.parentNode.insertBefore(wf, s);
  })();

/* Add home to breadcrumb  */  
//$j('h2.forums').prepend('<a href="/home">Home</a><span class="delim">/</span>');

//Insert search form
var searchForm = '<form action="/categories/search" id="searchform" method="get"><input id="suggestions_query" name="suggestions_query" type="text" placeholder="@{Search_this_category}"><input class="button search primary" id="suggestion_submit" name="commit" type="submit" value=""><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&amp;#x2713;"></div><input id="for_search" name="for_search" type="hidden" value="1"></form>';

if ($j('#top:visible').size()>0) {	
	$j(window).scroll(function () {
		var top_header_height = $j('.top_header').height();
		if (sidebar.children().size() > 0 && sidebar.height() < $j(window).height()) {
			var y = $j(this).scrollTop() - top_header_height;
			sidebar.stop();
			if (y < 0)y = 0;			
			sidebar.animate( {
				top: y + 'px'
			}, 1000, 'swing');
		}
	});
	
	$j('a:contains("edit")').bind('click',function() { window.setTimeout("$j('#container').trigger('resize');", 250); });	
	$j('#container').resize(function() {		
		if ((sidebar.position().top + sidebar.height()) > $j('#container').height()) {
			var y = $j('#container').height() - sidebar.height();
			if (y<0) y=0;
			sidebar.stop();
			sidebar.css('top', y + 'px');
		}
	});	
}

//Change New request header
Event.observe(window, 'load', function() {
if ($j('#container #ticketform h2').size() > 0){
   $j('#container #ticketform h2')[0].update('@{Submit_a_request_for_assistance}');   
   $j('#container #sidebar .side-box-content')[0].update('@{NewRequest_Sidebox_HTML}');
}});
  
$j(document).ready(function () {
	var flashMsg = $j('#flash > #notice');
	flashMsg.delay(2000).fadeIn(1000);	
	if (flashMsg && flashMsg.html())
		flashMsg.html(flashMsg.html().replace(/Request #\d+ ".+" created/g,'Your request has been created and a confirmation email has been sent to you. If you would like to add any details, respond to this email'));
	
	if (Zendesk !== null && Zendesk.tab == "home") {
		$j("div#@{category_20085466} .column ul li.fade_truncation_outer.articles,div#@{category_20085466} .column ul li.fade_truncation_outer.questions").hide();
		$j("div#@{category_20085466} .column").css({"width":"auto","height":"auto"});
		
		$j('#@{category_20085466}').parent().addClass('faq_column');
		$j('#@{category_20074573}').parent().addClass('using_voc');
		sidebar.hide();
		
		$j("h2#pinned_topics_title").replaceWith('<h2>@{Popular_topics}</h2>');
		isOnFirstPage = true;		
	}	
	else {
		if (ie8orLower) {
			sidebar.css('left',sidebar.position().left + 'px');
			sidebar.css('position','absolute');
		}
		else {
			sidebar.css('position','relative');		
		}
	}
	
	if (Zendesk !== null) {
		if (Zendesk.tab == "new") {
			$j('#ticket_header').prepend('<a href="/home">@{Home}</a>&nbsp;/');
		} else if (Zendesk.tab == "") {
			var pattern = '/access/unauthenticated$';
			if (document.location.toString().match(pattern)) {
				var elem = $j('.content.content_grey').find("h2:contains('Log in to Volvo On Call Self Service')");
				if (elem.size() > 0)
					elem.html(elem.html().replace(/Log in to Volvo On Call Self Service/,'<a href="/home">@{Home}</a>&nbsp;/&nbsp;Log in to Volvo On Call Self Service'));
			}
		}
	}
	
	if ($j("#top:visible").size() > 0) {		
		$j('#top').html('<div class="top_header"><div class="volvo_home_lnk" onclick="document.location=\'/home\'"></div><div class="header_links"><ul><li class="hide_on_home"><a href="/home">@{Home}</a></li><li><a href="@{WhatIsVoc_URL}">@{WhatIsVoc_LinkText}</a></li><li><a href="@{GetStartedWithVoc_LinkURL}">@{Get_Started}</a></li><li><a href="@{User_Guides_URL}">@{User_Guides}</a></li><li><a href="@{FAQ_Forums_URL}">@{FAQ_Forums_LinkText}</a></li></ul></div><div class="top_search hide_on_home"><form action="/categories/search" id="menu_searchform" method="get"><input id="menu_suggestions_query" name="suggestions_query" type="text" placeholder="@{Top_Header_Search}"><input class="button menu_search menu" id="menu_suggestion_submit" name="commit" type="submit" value=""><div style="margin:0;padding:0;display:inline"><input name="utf8" type="hidden" value="&amp;#x2713;"></div>    <input id="for_search" name="for_search" type="hidden" value="1"> </form></div></div>');
		$j('#container').append(footer);		
		$j('#flash_messages').addClass('flash_messages_class');
		
		if ($j.browser.msie  && parseInt($j.browser.version, 10) <= 7) {
			$j('.applink_button_wrapper').css('display','inline');
			$j('.voc_footer').css('width','1000px');
			if (parseInt($j.browser.version, 10) <= 6) {
				$j('.faded_truncation').hide();
			}
		}
		
		if(isOnFirstPage) {
			$j('.hide_on_home').hide();
		}
		
		//Change Add article link to Add new topic
		$j("p.button-item>.button.small:contains('Add Article')").text('Add new topic');
		
		jQuery.ajax({
			url:"/users/current.xml",
			async:true,
			dataType :"xml",
			success: function( data, textStatus, jqXHR) {
				var role = parseInt($j(data).find("roles").text(),10);
				if (role == 2 || role == 4)
					$j('#agent_link').css('display','inline-block');
			}
		});
	}
	
	var ticketDeflect = $j('#ticket-deflect');
	if (ticketDeflect.size() > 0) {		
		var ticketDeflectTitle = ticketDeflect.prev();
		ticketDeflectTitle.removeAttr('style');
		
		var ticketDeflectBox = $j('<div class="ticket_deflect_box contact_us"><h2><a href="/anonymous_requests/new">@{Contact_Us}</a></h2></div>');
		ticketDeflectBox.insertAfter(ticketDeflectTitle);	
		ticketDeflectTitle.addClass('caps');
		ticketDeflectTitle.detach();	
		ticketDeflect.remove();
		
		ticketDeflectBox.prepend(ticketDeflectTitle);		
		$j('.frame').css('padding-bottom','5px');
	}
		
	//Fix strong header margins
	$j('div.frame').find('strong:only-child').parent('p').addClass('pstrong');
		
		
	awaitAjaxUpdate($j('#voting_control'));
	$j('img.image_zoom').bind('click',function() {showModalImage($j(this).attr('src'));});	
});

//Poll function awaiting async data with 50 ms intervals.
function awaitAjaxUpdate(srcElem) {	
	_awaitAjaxUpdate(new ajaxUpdateDataHolder(srcElem, srcElem.html(),replaceVotingCtrlText));
}

function _awaitAjaxUpdate(ajax_data) {
	if (ajax_data.srcElem.html() != ajax_data.orgData)	
		ajax_data.onComplete();
	else
		setTimeout(function() {
			_awaitAjaxUpdate(ajax_data);
	},50);
}

function ajaxUpdateDataHolder(srcElem, orgData, onComplete) {
	this.orgData=orgData;
	this.srcElem=srcElem;	
	this.onComplete=onComplete;
}

function replaceVotingCtrlText() {
	var votingControl = $j('#voting_control');
	if(votingControl.text().indexOf('@{zero_people_found_this_useful}')>=0) {
		votingControl.html(votingControl.html().replace('@{zero_people_found_this_useful}','@{Found_this_useful}'));
		var lnk = votingControl.find("a:contains(' - ')");
		if (lnk.size()>0)
			lnk.html("<b>@{Let_us_know}</b>");
	}
}

function showFooterPopup(pageNo) {
	var url = '';
	switch(pageNo) {
		case 1:
			url = '@{Hosting_Base_URL}/cookies_and_privacy.html';
		break;		
		case 2:
			url = '@{Hosting_Base_URL}/terms_of_use.html';
		break;
		case 3:
			url = '@{Hosting_Base_URL}/copyright.html';
		break;
		default:
			return;		
	}
	var iFrame = $j('<div class="iframepopup"><div class="modal_iframe_head"><img src="@{Hosting_Base_URL}/images/popup_close.gif" onclick="$j(\'.modal_bg\').trigger(\'click\');"> </div><div class="iframepopup_bg"></div><iframe src="' + url + '" frameborder="0" scrolling="yes" width="100%" height="100%" seamless></iframe></div>');
	var modal_bg = $j('<div class="modal_bg"></div>').bind('click',function() { $j(this).unbind().remove(); $j('.iframepopup').fadeOut().remove(); }).insertBefore($j('#page'));
	iFrame.hide();
	iFrame.insertAfter(modal_bg).center().fadeIn();	
}

function showModalImage(imgSrc) {	
	var modal_bg = $j('<div class="modal_bg"></div>').bind('click',function() { $j(this).unbind().remove(); $j('.modal_img').fadeOut().remove(); }).insertBefore($j('#page'));
	var imgbox = $j('<div class="modal_img"><div class="modal_img_bg"></div><div class="modal_img_head"><img src="@{Hosting_Base_URL}/images/popup_close.gif" onclick="$j(\'.modal_bg\').trigger(\'click\');"> </div><img src="' + imgSrc + '" class="modal_img_frame"></div>');
	imgbox.find('.modal_img_frame').load(function() {rescaleImage(this);});
	imgbox.hide();
	imgbox.insertAfter(modal_bg).center().fadeIn();
}

function rescaleImage(imgDomElement) {
	var winHeight=parseInt($j(window).height()*0.9,10);
	var winWidth=parseInt($j(window).width()*0.9,10);	
	var img=$j(imgDomElement);
	var imgWidth=img.width();
	var imgHeight=img.height();
	var imgProportions=(imgWidth+1.0)/(imgHeight+1.0);
	
	//Resize and maintain aspect ratio.
	if(imgWidth>winWidth||imgHeight>winHeight){
		imgWidth=Math.min(imgWidth,winWidth);
		imgHeight=Math.min(imgHeight,winHeight);
		
		//Scale x or y?
		if(imgWidth/imgHeight>imgProportions)
			imgWidth=parseInt(imgWidth/((imgWidth/imgHeight)/imgProportions),10);
		else
			imgHeight=parseInt(imgHeight/(imgProportions/(imgWidth/imgHeight)),10);
		
		img.attr('width',imgWidth+'px');
		img.attr('height',imgHeight+'px');		
		img.parent().center();
	}
}

function setLanguageLink(locale) {	
	if (locale.id == 1)
		$j('#contentcolumn').prepend('<div class="lang_link"><a href="https://volvooncall-help-se.volvocars.com/home" rel="nofollow">@{Swedish}</a></div>');		
	else
		$j('#contentcolumn').prepend('<div class="lang_link"><a href="https://volvooncall-help.volvocars.com/home" rel="nofollow">@{English}</a></div>');
}