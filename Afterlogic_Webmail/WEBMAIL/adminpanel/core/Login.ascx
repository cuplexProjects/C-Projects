<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="WebMail.adminpanel.Login" %>

	<div align="center" class="wm_content">
		<div id="login_screen" class="wm_login">
			<div id="login_error" class="<%=_errorClass%>">
				<div class="wm_login_error_icon">
				</div>
				<div class="wm_login_error_message" id="login_error_message"><%=_errorMessage%></div>
			</div>
			<div align="center">
				<div class="wm_logo" id="logo">
				</div>
				<div class="wm_login">
				    <div class="a top">
				    </div>
				    <div class="b top">
				    </div>
				    <div class="login_table" style="margin: 0px;">
					    <div class="wm_login_header">
						    Administration Login</div>
                        <div class="wm_login_content">						
					        <table id="login_table" border="0" cellspacing="0" cellpadding="10">
						        <tr>
							        <td class="wm_title" style="font-size: 12px; width: 70px">
								        Login:
							        </td>
							        <td>
								        <input class="wm_input" size="20" type="text" id="LoginID" name="Login" style="width: 99%;
									        font-size: 16px;" onfocus="this.style.background = '#FFF9B2';" onblur="this.style.background = 'white';"
									        runat="server" />
							        </td>
						        </tr>
						        <tr>
							        <td class="wm_title" style="font-size: 12px; width: 70px">
								        Password:
							        </td>
							        <td>
								        <input class="wm_input" type="password" size="20" id="PasswordID" name="Password"
									        style="width: 99%; font-size: 16px;" onfocus="this.style.background = '#FFF9B2';"
									        onblur="this.style.background = 'white';" runat="server" />
							        </td>
						        </tr>
						        <tr>
							        <td colspan="2">
								        <span class="wm_login_button">
									        <asp:Button ID="EnterID" OnClick="Enter_OnClick" CssClass="wm_button" runat="server" Text="Login" />
								        </span>
							        </td>
						        </tr>
					        </table>
                        </div>					
				    </div>
				    <div class="b">
				    </div>
				    <div class="a">
				    </div>
			    </div>
	        </div>
		</div>
	</div>
	<div class="wm_copyright" id="copyright">
		Copyright © 2002-2010 <a href="http://www.afterlogic.com/" target="_blank">AfterLogic
			Corporation</a>
	</div>