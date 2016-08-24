<%@ Register Src="Copyright.ascx" TagPrefix="BaseWebmail" TagName="Copyright" %>
<%@ Register Src="flash-detect.ascx" TagPrefix="BaseWebmail" TagName="FlashDetect" %>
<%@ Register Src="Logo.ascx" TagPrefix="BaseWebmail" TagName="Logo" %>
<%@ Page language="c#" Codebehind="webmail.aspx.cs" AutoEventWireup="True" Inherits="WebMail.webmail" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html id="html">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="Content-Script-Type" content="text/javascript" />
	<link rel="shortcut icon" href="favicon.ico" />
	<meta http-equiv="Pragma" content="cache" />
	<meta http-equiv="Cache-Control" content="public" />
	<title><%=defaultTitle%></title>
	<link rel="stylesheet" href="skins/<%=defaultSkin%>/styles.css" type="text/css" id="skin" />
<%if (_stylesRtl) {%>
	<link rel="stylesheet" href="skins/<%=defaultSkin%>/styles-rtl.css" type="text/css" id="skin-rtl" />
<%} %>
<% if (check == "1")
   { %>
	<script type="text/javascript" src="langs-js.aspx?lang=<%=defaultLang%>"></script>
	<script type="text/javascript" src="js/common/defines.js"></script>
	<script type="text/javascript" src="js/common/common-helpers.js"></script>
	<script type="text/javascript" src="js/common/loaders.js"></script>
	<script type="text/javascript" src="js/common/functions.js"></script>
	<script type="text/javascript" src="js/common/popups.js"></script>
	<script type="text/javascript">
		var checkMail;
		var WebMailUrl = 'webmail.aspx?start=<%=start%>&to=<%=to%>';
		var LoginUrl = 'default.aspx';
		var CheckMailUrl = 'check-mail.aspx';
		var EmptyHtmlUrl = 'empty.html';
		var Browser = new CBrowser();

		function Init()
		{
			checkMail = new CCheckMail(1);
			checkMail.Start();
		}
		
		function SetCheckingAccountHandler(accountName)
		{
			checkMail.SetAccount(accountName);
		}
		
		function SetStateTextHandler(text) {
			checkMail.SetText(text);
		}
		
		function SetCheckingFolderHandler(folder, count) {
			checkMail.SetFolder(folder, count);
		}
		
		function SetRetrievingMessageHandler(number) {
			checkMail.SetMsgNumber(number);
		}
		
		function SetDeletingMessageHandler(number) {
			checkMail.DeleteMsg(number);
		}
		
		function SetUpdatedFolders() { }
		
		function EndCheckMailHandler(error) {
			if (error == 'session_error') {
				document.location = LoginUrl + '?error=1';
			} else {
				document.location = WebMailUrl;
			}
		}
		
		function CheckEndCheckMailHandler() {
			if (checkMail.started) {
				document.location = WebMailUrl;
			}
		}
	</script>
</head>
<body onload="Init();">
	<div align="center" id="content" class="wm_content">
		<BaseWebmail:Logo id="Logo_CheckMail" runat="server"></BaseWebmail:Logo>
	</div>
	<BaseWebmail:Copyright id="Copyright_CheckMail" runat="server"></BaseWebmail:Copyright>
</body>
<% }
   else
   { %>
	<script type="text/javascript">
        var JSLoadedCount = 1;
		var TotalJSFilesCount = 52;
		function JSFileLoaded()
		{
			JSLoadedCount++;
			var percent = Math.ceil((JSLoadedCount)*100/(TotalJSFilesCount + 1));
			if (percent >= 0)
			{
				var jsProgressLoaded = document.getElementById('jsProgressLoaded');
				if (jsProgressLoaded)
				{
					percent = (percent > 100) ? 100 : percent;
					jsProgressLoaded.style.width = percent + 'px';
				}
			}
		}

        function BodyLoaded()
        {
            if (JSLoadedCount >= TotalJSFilesCount) {
                Init();
                window.onresize = ResizeBodyHandler;
                document.onkeyup = EventBodyHandler;
            }
        }
	</script>
</head>

<body onload="BodyLoaded();">
<table class="wm_information wm_loading_information" style="right: auto; width: auto; top: 0px; left: 604px;" id="info_cont"><tr><td>
	<div class="wm_info_block">
		<div class="wm_shadow">
			<div class="a">&nbsp;</div>
		</div>
		<div class="wm_info_message" id="info_message">
			<span><%=_resMan.GetString("InfoWebMailLoading")%></span>
	        <div class="wm_progressbar">
	            <div class="wm_progressbar_used" style="width: 0px;" id="jsProgressLoaded"></div>
	        </div>
	    </div>
		<div class="a">&nbsp;</div>
		<div class="b">&nbsp;</div>
	</div>
</td></tr></table>
	<div id="spell_popup_menu" class="wm_hide"></div>
	<div align="center" id="content" class="wm_hide">
		<BaseWebmail:Logo id="Control_Logo" runat="server"></BaseWebmail:Logo>
	</div>
	<BaseWebmail:Copyright id="Control_Copyright" runat="server"></BaseWebmail:Copyright>
	<BaseWebmail:FlashDetect id="Control_FlasDetect" runat="server"></BaseWebmail:FlashDetect>
	<iframe name="session_saver" id="session_saver" src="session-saver.aspx" class="wm_hide"></iframe>
</body>
<script type="text/javascript">
		var WebMailSessId = '<%=Session.SessionID %>';
    var LoginUrl = 'default.aspx';
	var WebMailUrl = 'webmail.aspx';
	var BaseWebMailUrl = 'webmail.aspx';
	var ActionUrl = 'xml-processing.aspx';
	var EditAreaUrl = 'edit-area.aspx';
	var EmptyHtmlUrl = 'empty.html';
	var UploadUrl = 'upload.aspx';
	var ImportUrl = 'import.aspx';
	var HistoryStorageUrl = 'history-storage.aspx';
	var CheckMailUrl = 'check-mail.aspx';
	var LanguageUrl = 'langs-js.aspx';
	var SpellcheckerUrl = 'spellcheck.aspx';
	var CalendarUrl = 'calendar.aspx';
	var CalendarProcessingUrl = 'calendar/processing.aspx';
	var MiniWebMailUrl = 'mini-webmail.aspx';
	
	var Title = '<%=jsClearDefaultTitle%>';
	var SkinName = '<%=jsClearDefaultSkin%>';
	var Start = <%=jsClearStart%>;
	var ToAddr = '<%=jsClearToAddr%>';
	var XType = '0';
	var CSType = <%=CSType%>;
	var WmVersion = '<%=WmVersion%>';
	var RTL = <%=_rtl%>;
	var UseDb = true;
	var Browser;
	var WebMail, HistoryStorage, Pop3SpamButtonAdd = false;
	var UseDb = true;
	var UseImapSearchInDirectMode = true;
	var SwfUploader;
	var ViewMessage = null;
	var hMailServer = <%=hMailServer%>;
	
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
	var infoCont = document.getElementById('info_cont');
	infoCont.style.right = 'auto';
	var offsetWidth = infoCont.offsetWidth;
	infoCont.style.left = Math.round((GetWidth() - offsetWidth)/2) + 'px';
</script>
<script type="text/javascript">
	var copy = document.getElementById('copyright');
	if (copy) copy.className = 'wm_hide';
</script>
<script type="text/javascript" src="langs-js.aspx?v=<%=WmVersion%>&lang=<%=defaultLang%>"></script>
<script type="text/javascript" src="js/common/defines.js"></script>
<script type="text/javascript" src="js/common/calendar-screen.js"></script>
<script type="text/javascript" src="js/common/common-handlers.js"></script>
<script type="text/javascript" src="js/common/common-helpers.js"></script>
<script type="text/javascript" src="js/common/data-source.js"></script>
<script type="text/javascript" src="js/common/functions.js"></script>
<script type="text/javascript" src="js/common/loaders.js"></script>
<script type="text/javascript" src="js/common/page-switcher.js"></script>
<script type="text/javascript" src="js/common/popups.js"></script>
<script type="text/javascript" src="js/common/toolbar.js"></script>
<script type="text/javascript" src="js/common/variable-table.js"></script>
<script type="text/javascript" src="js/common/webmail.js"></script>

<script type="text/javascript" src="js/mail/autocomplete-recipients.js"></script>
<script type="text/javascript" src="js/mail/folders-pane.js"></script>
<script type="text/javascript" src="js/mail/html-editor.js"></script>
<script type="text/javascript" src="js/mail/mail-data.js"></script>
<script type="text/javascript" src="js/mail/mail-handlers.js"></script>
<script type="text/javascript" src="js/mail/message-headers.js"></script>
<script type="text/javascript" src="js/mail/message-info.js"></script>
<script type="text/javascript" src="js/mail/message-line.js"></script>
<script type="text/javascript" src="js/mail/message-list-prototype.js"></script>
<script type="text/javascript" src="js/mail/message-list-central-pane.js"></script>
<script type="text/javascript" src="js/mail/message-list-central-screen.js"></script>
<script type="text/javascript" src="js/mail/message-list-display.js"></script>
<script type="text/javascript" src="js/mail/message-list-top-screen.js"></script>
<script type="text/javascript" src="js/mail/new-message-screen.js"></script>
<script type="text/javascript" src="js/mail/message-reply-pane.js"></script>
<script type="text/javascript" src="js/mail/resizers.js"></script>
<script type="text/javascript" src="js/mail/swfupload.js"></script>
<script type="text/javascript" src="js/mail/view-message-screen.js"></script>

<script type="text/javascript" src="js/contacts/contact-line.js"></script>
<script type="text/javascript" src="js/contacts/contacts-data.js"></script>
<script type="text/javascript" src="js/contacts/contacts-handlers.js"></script>
<script type="text/javascript" src="js/contacts/contacts-screen.js"></script>
<script type="text/javascript" src="js/contacts/edit-contact.js"></script>
<script type="text/javascript" src="js/contacts/edit-group.js"></script>
<script type="text/javascript" src="js/contacts/import.js"></script>
<script type="text/javascript" src="js/contacts/view-contact.js"></script>

<script type="text/javascript" src="js/settings/account-list.js"></script>
<script type="text/javascript" src="js/settings/account-properties.js"></script>
<script type="text/javascript" src="js/settings/autoresponder.js"></script>
<script type="text/javascript" src="js/settings/calendar.js"></script>
<script type="text/javascript" src="js/settings/common.js"></script>
<script type="text/javascript" src="js/settings/defines-calendar.js"></script>
<script type="text/javascript" src="js/settings/filters.js"></script>
<script type="text/javascript" src="js/settings/folders.js"></script>
<script type="text/javascript" src="js/settings/mobile-sync.js"></script>
<script type="text/javascript" src="js/settings/settings-data.js"></script>
<script type="text/javascript" src="js/settings/signature.js"></script>
<script type="text/javascript" src="js/settings/user-settings-screen.js"></script>

<script type="text/javascript">
	function Init() {
		Browser = new CBrowser();
		if (Browser.IE && Browser.Version < 7) {
			try {
				document.execCommand('BackgroundImageCache', false, true);
			} catch(e) {}
		}
		HtmlEditorField.Build(false);
		var DataTypes = [
			new CDataType(TYPE_BASE, false, 0, false, { }, 'base' ),
			new CDataType(TYPE_ACCOUNT_BASE, false, 0, false, { IdAcct: 'id_acct', ChangeAcct: 'change_acct' }, 'account_base' ),
			new CDataType(TYPE_MESSAGES_BODIES, false, 0, false, { }, 'messages_bodies' ),
			new CDataType(TYPE_FOLDERS_BASE, false, 0, false, { }, 'folders_base' ),
			new CDataType(TYPE_SETTINGS_LIST, false, 0, false, { }, 'settings_list' ),
			new CDataType(TYPE_ACCOUNT_LIST, false, 0, false, { }, 'accounts' ),
			new CDataType(TYPE_FOLDER_LIST, true, 10, false, { IdAcct: 'id_acct', Sync: 'sync' }, 'folders_list' ),
			new CDataType(TYPE_MESSAGE_LIST, true, 20, false, { IdAcct: 'id_acct', Page: 'page', SortField: 'sort_field', SortOrder: 'sort_order' }, 'messages' ),
			new CDataType(TYPE_MESSAGES_OPERATION, false, 0, false, { }, '' ),
			new CDataType(TYPE_MESSAGE, true, 100, true, { Id: 'id', Charset: 'charset' }, 'message' ),
			new CDataType(TYPE_USER_SETTINGS, false, 0, false, { }, 'settings' ),
			new CDataType(TYPE_ACCOUNT_PROPERTIES, false, 0, false, { IdAcct: 'id_acct' }, 'account' ),
			new CDataType(TYPE_FILTERS, false, 0, false, { IdAcct: 'id_acct' }, 'filters' ),
			new CDataType(TYPE_FILTER_PROPERTIES, false, 0, false, { IdFilter: 'id_filter', IdAcct: 'id_acct' }, 'filter' ),
			new CDataType(TYPE_SIGNATURE, false, 0, false, { IdAcct: 'id_acct' }, 'signature' ),
			new CDataType(TYPE_AUTORESPONDER, false, 0, false, { IdAcct: 'id_acct' }, 'autoresponder' ),
			new CDataType(TYPE_MOBILE_SYNC, false, 0, false, { }, 'mobile_sync' ),
			new CDataType(TYPE_CONTACTS, true, 5, false, { Page: 'page', SortField: 'sort_field', SortOrder: 'sort_order' }, 'contacts_groups' ),
			new CDataType(TYPE_CONTACT, true, 20, false, { IdAddr: 'id_addr' }, 'contact' ),
			new CDataType(TYPE_GROUPS, false, 0, false, { }, 'groups' ),
			new CDataType(TYPE_GROUP, true, 10, false, { IdGroup: 'id_group' }, 'group' ),
			new CDataType(TYPE_SPELLCHECK, false, 0, false, { Word: 'word' }, 'spellcheck')
		];
		WebMail = new CWebMail(Title, SkinName);
		WebMail.DataSource = new CDataSource( DataTypes, ActionUrl, ErrorHandler, LoadHandler, TakeDataHandler, ShowLoadingInfoHandler );
		HistoryStorage = new CHistoryStorage(
			{
				Document: document,
				HistoryStorageObjectName: "HistoryStorage",
				PathToPageInIframe: HistoryStorageUrl,
				MaxLimitSteps: 50,
				Browser: Browser
			}
		);
		
		if (Start) {
			WebMail.SetStartScreen(Start);
		}
		
		WebMail.DataSource.Get(TYPE_BASE, { }, [], '');
	}
</script>
<% } %>
<!--<%=_WmVersion%>-->
</html>
