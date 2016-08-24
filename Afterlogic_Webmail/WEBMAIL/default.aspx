<%@ Register Src="Copyright.ascx" TagPrefix="BaseWebmail" TagName="Copyright" %>
<%@ Register Src="Logo.ascx" TagPrefix="BaseWebmail" TagName="Logo" %>
<%@ Page language="c#" CodeBehind="default.aspx.cs" AutoEventWireup="True" Inherits="WebMail._default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta http-equiv="Content-Script-Type" content="text/javascript" />
	<link rel="shortcut icon" href="favicon.ico" />
	<meta http-equiv="Pragma" content="cache" />
	<meta http-equiv="Cache-Control" content="public" />
	<title><%=settings.SiteName%></title>
	<link rel="stylesheet" href="skins/<%=defaultSkin%>/styles.css" type="text/css" id="skin" />
<%if (_stylesRtl) {%>
	<link rel="stylesheet" href="skins/<%=defaultSkin%>/styles-rtl.css" type="text/css" id="skin-rtl" />
<%} %>
	<script type="text/javascript">
		var WebMailUrl = 'webmail.aspx';
		var LoginUrl = 'default.aspx';
		var ActionUrl = 'xml-processing.aspx';
		var Title = '<%=defaultTitle%>';
		var SkinName = '<%=defaultSkin%>';
    	var DefLang = '<%=defaultLang%>';
		var HideLoginMode = <%=defaultHideLoginMode%>;
		var DomainOptional = '<%=defaultDomainOptional%>';
		var AllowAdvancedLogin = <%=defaultAllowAdvancedLogin%>;
		var AdvancedLogin = '<%=advancedLogin%>';
		var EmptyHtmlUrl = 'empty.html';
		var CheckMailUrl = 'check-mail.aspx';
		var LanguageUrl = 'langs-js.aspx';
		var RTL = <%=_rtl%>;
		var NeedToSubmit = false;
		var WmVersion = '<%=WmVersion%>';
		var PdaUrl = 'PDA/default.aspx';
	</script>
	<script type="text/javascript" src="langs-js.aspx?v=<%=WmVersion%>&lang=<%=defaultLang%>"></script>
	<script type="text/javascript" src="js/common/defines.js"></script>
	<script type="text/javascript" src="js/common/common-helpers.js"></script>
	<script type="text/javascript" src="js/common/loaders.js"></script>
	<script type="text/javascript" src="js/common/functions.js"></script>
	<script type="text/javascript" src="js/common/popups.js"></script>
	<script type="text/javascript" src="js/login/login-screen.js"></script>
	<script type="text/javascript">
		function ChangeLang(object) {
			if (object && object.name && object.name.length > 4 && object.name.substr(0, 4) == 'lng_') {
				document.location = LoginUrl + '?lang=' + object.name.substr(4);
			}
		}
		
		function defaultInit(){
            if (LoginDemoLangClass) LoginDemoLangClass.CheckLang('lng_' + DefLang);
		}
	</script>

	<!--for demo google-->
	
	
</head>

<body onload="Init();" id="mbody">
<div class="wm_hide" id="info">
	<div class="wm_shadow">
		<div class="a"></div>
	</div>
	<div class="wm_info_message" id="info_message"></div>
	<div class="a"></div>
	<div class="b"></div>
</div>
<div align="center" class="wm_content">
	<div id="content">
		<BaseWebmail:Logo id="Control_Logo" runat="server"></BaseWebmail:Logo>
	</div>
	<div id="login_screen">
		<div class="<%=errorClass%>" id="login_error">
		    <div class="wm_login_error_icon"></div>
		    <div id="login_error_message" class="wm_login_error_message"><%=errorDesc%></div>
		</div>
		<form action="default.aspx?mode=submit" method="post" id="login_form" name="login_form" onsubmit="NeedToSubmit = true; return false;">
			<input type="hidden" name="advanced_login" value="<%=advancedLogin%>" />
			<div class="wm_login">
				<div class="a top"></div>
				<div class="b top"></div>
				<div class="login_table" style="margin: 0px;">
					<div id="lang_LoginInfo" class="wm_login_header"><%=_resMan.GetString("LANG_LoginInfo")%></div>
					<div class="wm_login_content">
					<table id="login_table" border="0" cellspacing="0" cellpadding="10">
						<tr id="email_cont"<%=emailClass%>>
							<td class="wm_title" style="font-size: 12px; width: 70px;" id="lang_Email"><%=_resMan.GetString("LANG_Email")%>:</td>
							<td>
								<input style="width: 224px; font-size: 16px;" class="wm_input" type="text" value="" id="email" name="email" maxlength="255" 
									onfocus="this.className = 'wm_input_focus';" onblur="this.className = 'wm_input';" tabindex="<%=emailTabIndex%>" />
							</td>
						</tr>
						<tr id="login_cont"<%=loginClass%>>
							<td class="wm_title" style="font-size:12px; width:70px;" id="lang_Login"><%=_resMan.GetString("LANG_Login")%>:</td>
							<td id="login_parent"><nobr>
								<input style="width: <%=loginWidth%>;font-size:16px;" class="wm_input" type="text" value="" id="login" name="login" maxlength="255" 
									onfocus="this.className = 'wm_input_focus';" onblur="this.className = 'wm_input';" tabindex="<%=loginTabIndex%>" />
								<span id="domain"><%=domainContent%></span>
							</nobr></td>
						</tr>
						<tr>
							<td class="wm_title" style="font-size:12px; width:70px;" id="lang_Password"><%=_resMan.GetString("LANG_Password")%>:</td>
							<td>
								<input style="width: 224px;font-size:16px;" class="wm_input wm_password_input" type="password" value="" id="password" name="password" maxlength="255" 
									onfocus="this.className = 'wm_input_focus wm_password_input';" onblur="this.className = 'wm_input wm_password_input';" />
							</td>
						</tr>
					</table>
						<% if (settings.AllowAdvancedLogin) { %>
						<div id="advanced_fields" style="margin: 0px; height: 95px; display: <%=advancedDisplay%>; overflow:hidden; padding:0px;">
						<table cellspacing="0" cellpadding="6">
						<tr id="incoming">
							<td class="wm_title" id="lang_IncServer"><%=_resMan.GetString("LANG_IncServer")%>:</td>
							<td>
								<input class="wm_advanced_input" type="text" value="<%=defaultIncServer%>" id="inc_server" name="inc_server" maxlength="255"
									onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';" />
							</td>
							<td>
								<select class="wm_advanced_input" id="inc_protocol" name="inc_protocol" 
									onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';">
									<option value="<%=POP3_PROTOCOL%>" <%=pop3Selected%>><%=_resMan.GetString("LANG_PopProtocol")%></option>
									<option value="<%=IMAP4_PROTOCOL%>" <%=imap4Selected%>><%=_resMan.GetString("LANG_ImapProtocol")%></option>
								</select>
							</td>
							<td class="wm_title" id="lang_IncPort"><%=_resMan.GetString("LANG_IncPort")%>:</td>
							<td>
								<input class="wm_advanced_input" type="text" value="<%=defaultIncPort%>" id="inc_port" name="inc_port" maxlength="5"
									onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';" />
							</td>
						</tr>
						<tr id="outgoing">
							<td class="wm_title" id="lang_OutServer"><%=_resMan.GetString("LANG_OutServer")%>:</td>
							<td colspan="2">
								<input class="wm_advanced_input" type="text" value="<%=defaultOutServer%>" id="out_server" name="out_server" maxlength="255"
									onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';" />
							</td>
							<td class="wm_title" id="lang_OutPort"><%=_resMan.GetString("LANG_OutPort")%>:</td>
							<td align="right">
								<input class="wm_advanced_input" type="text" value="<%=defaultOutPort%>" id="out_port" name="out_port" maxlength="5"
									onfocus="this.className = 'wm_advanced_input_focus';" onblur="this.className = 'wm_advanced_input';" />
							</td>
						</tr>
						<tr id="authentication">
							<td colspan="5">
								<input class="wm_checkbox" type="checkbox" checked="checked" value="1" id="smtp_auth" name="smtp_auth"<%=smtpAuthChecked%>>
								<label for="smtp_auth" id="lang_UseSmtpAuth"><%=_resMan.GetString("LANG_UseSmtpAuth")%></label>
							</td>
						</tr>
						</table>
						</div>
						<% } %>
						<table>
						<tr>
							<td colspan="5">
								<input class="wm_checkbox" type="checkbox" value="1" id="sign_me" name="sign_me"<%=signMeChecked%>>
								<label for="sign_me" id="lang_SignMe"><%=_resMan.GetString("LANG_SignMe")%></label>
							</td>
						</tr>
						<tr>
							<td colspan="5">
								<span class="wm_login_button">
									<input class="wm_button" type="submit" id="submit" name="submit" onclick="NeedToSubmit = true; return false;" value="<%=_resMan.GetString("LANG_Enter")%>" />
								</span>
							<% if (settings.AllowAdvancedLogin) { %>
								<span class="wm_login_switcher">
									<a class="wm_reg" href="<%=switcherHref%>" id="login_mode_switcher" onclick="return false;"><%=switcherText%></a>
								</span>
							<% } %>
							<% if (settings.AllowLanguageOnLogin) { %>
                                <span class="wm_language_place">
									<a id="langs_selected" href="#" class="wm_reg" onclick="return false;" style="padding-right: 0px;"><span><%=defaultLangName%></span><font>&nbsp;</font><span class="wm_login_lang_switcher">&nbsp;</span></a>
									<input type="hidden" value="" id="language" name="language">
									<br />
									<div id="langs_collection">
									    <%=_languageOptions%>
									</div>
								</span>	
							<% } %>
                            </td>
						</tr>
					</table>
					</div>
				</div>
				<div class="b"></div>
				<div class="a"></div>
			</div>
		</form>
	</div>
	
	<div id="dummy"></div>
</div>
<BaseWebmail:Copyright id="Control_Copyright" runat="server"></BaseWebmail:Copyright>
<!--<%=_WmVersion%>-->
</body>
</html>
