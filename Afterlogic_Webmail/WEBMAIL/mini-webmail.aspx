<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mini-webmail.aspx.cs" Inherits="WebMail.mini_webmail" %>
<%@ Register Src="flash-detect.ascx" TagPrefix="BaseWebmail" TagName="FlashDetect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" id="html" >
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="Content-Script-Type" content="text/javascript" />
	<meta http-equiv="Pragma" content="cache" />
	<meta http-equiv="Cache-Control" content="public" />
	<link rel="shortcut icon" href="favicon.ico" />
	<title><%=defaultTitle%></title>
	<link rel="stylesheet" href="skins/<%=defaultSkin%>/styles.css" type="text/css" id="skin" />
<%if (_stylesRtl) {%>
	<link rel="stylesheet" href="skins/<%=defaultSkin%>/styles-rtl.css" type="text/css" id="skin-rtl" />
<%} %>
	<script type="text/javascript">
		var WebMailSessId = '<%=Session.SessionID %>';
		var OpenMode = '<%=openMode %>';
		var ToAddr = '<%=jsClearToAddr %>';
		var ActionUrl = 'xml-processing.aspx';
		var LoginUrl = 'default.aspx';
		var EmptyHtmlUrl = 'empty.html';
		var Browser;
		var PreviewPane, NewMessageScreen;
		var UseDb = true;
		var UploadUrl = 'upload.aspx';
		var EditAreaUrl = 'edit-area.aspx';
		var WebMail = {
			_title: '<%=jsClearDefaultTitle %>',
			Settings: {
				AllowContacts: false,
				AllowDhtmlEditor: <%=allowDhtmlEditor %>,
				ShowTextLabels: <%=showTextLabels %>
			}
		}
		var CurrentAccount = {
			Id: <%=acct.ID %>,
			UseFriendlyNm: <%=acct.UseFriendlyName ? "true" : "false" %>,
			FriendlyNm: '<%=acctFriendlyNm %>',
			Email: '<%=acctEmail %>',
			MailProtocol: <%=(int)acct.MailIncomingProtocol %>,
			SignatureOpt: <%=(int)acct.SignatureOptions %>,
			SignatureType: <%=(int)acct.SignatureType %>,
			Signature: '<%=acctSignature %>'
		};
	</script>
</head>

<body onload="BodyLoaded();">
	<table class="wm_information wm_loading_information" cellpadding="0" cellspacing="0" style="right: auto; width: auto; top: 0px; left: 272px;" id="info_cont">
		<tr style="position:relative;z-index:20">
			<td class="wm_shadow" style="width:2px;font-size:1px;"></td>
			<td>
				<div class="wm_info_message" id="info_message">
					<span><%=_resMan.GetString("Loading")%></span>
				</div>
				<div class="a">&nbsp;</div>
				<div class="b">&nbsp;</div>
			</td>
			<td class="wm_shadow" style="width:2px;font-size:1px;"></td>
		</tr>
		<tr>
			<td colspan="3" class="wm_shadow" style="height:2px;background:none;">
				<div class="a">&nbsp;</div>
				<div class="b">&nbsp;</div>
			</td>
		</tr>
		<tr style="position:relative;z-index:19">
			<td colspan="3" style="height:2px;">
				<div class="a wm_shadow" style="margin:0px 2px;height:2px; top:-4px; position:relative; border:0px;background:#555;">&nbsp;</div>
			</td>
		</tr>
	</table>
	<BaseWebmail:FlashDetect id="Control_FlasDetect" runat="server"></BaseWebmail:FlashDetect>
</body>
<script type="text/javascript" src="langs-js.aspx?v=<%=WmVersion%>&lang=<%=defaultLang%>"></script>
<script type="text/javascript" src="js/common/common-helpers.js"></script>
<script type="text/javascript" src="js/common/common-handlers.js"></script>
<script type="text/javascript" src="js/common/data-source.js"></script>
<script type="text/javascript" src="js/common/defines.js"></script>
<script type="text/javascript" src="js/common/functions.js"></script>
<script type="text/javascript" src="js/common/loaders.js"></script>
<script type="text/javascript" src="js/common/popups.js"></script>
<script type="text/javascript" src="js/common/toolbar.js"></script>
<script type="text/javascript" src="js/common/webmail.js"></script>
<script type="text/javascript" src="js/mail/autocomplete-recipients.js"></script>
<script type="text/javascript" src="js/mail/html-editor.js"></script>
<script type="text/javascript" src="js/mail/mail-data.js"></script>
<script type="text/javascript" src="js/mail/mail-handlers.js"></script>
<script type="text/javascript" src="js/mail/message-headers.js"></script>
<script type="text/javascript" src="js/mail/message-info.js"></script>
<script type="text/javascript" src="js/mail/message-list-prototype.js"></script>
<script type="text/javascript" src="js/mail/new-message-screen.js"></script>
<script type="text/javascript" src="js/mail/message-reply-pane.js"></script>
<script type="text/javascript" src="js/mail/mini-webmail-window.js"></script>
<script type="text/javascript" src="js/mail/resizers.js"></script>
<script type="text/javascript" src="js/mail/swfupload.js"></script>
<script type="text/javascript" src="js/mail/view-message-screen.js"></script>
<script type="text/javascript" src="js/contacts/contacts-data.js"></script>
<script type="text/javascript">
	<%=jsMessageInit %>
</script>
</html>