<%@ Page language="c#" Codebehind="print-msg.aspx.cs" AutoEventWireup="True" Inherits="WebMail.print_msg" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<html>
	<head>
		<link rel="shortcut icon" href="favicon.ico" />
		<link rel="stylesheet" href="./skins/<%=defaultSkin%>/styles.css" type="text/css" />
	<%if (_stylesRtl) {%>
		<link rel="stylesheet" href="skins/<%=defaultSkin%>/styles-rtl.css" type="text/css">
	<%} %>
	</head>
		<body class="wm_body" onload="window.print();"><div align="center" class="wm_space_before">
		<table class="wm_print">
					<tr>
						<td class="wm_print_title">
							<%=_manager.GetString("From")%>
						</td>
						<td class="wm_print_value">
							<asp:Literal ID="LabelFrom" Runat="server"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td class="wm_print_title">
						<%=_manager.GetString("To")%>
						</td>
						<td class="wm_print_value">
							<asp:Literal ID="LabelTo" Runat="server"></asp:Literal>
						</td>
					</tr>
					<% if (CcAddr != string.Empty) { %>
					<tr>
						<td class="wm_print_title">
						<%=_manager.GetString("Cc")%>
						</td>
						<td class="wm_print_value">
							<asp:Literal ID="LabelCc" Runat="server"></asp:Literal>
						</td>
					</tr>
					<% } %>
					<tr>
						<td class="wm_print_title">
							<%=_manager.GetString("Date")%>
						</td>
						<td class="wm_print_value">
							<asp:Literal ID="LabelDate" Runat="server"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td class="wm_print_title">
							<%=_manager.GetString("Subject")%>
						</td>
						<td class="wm_print_value">
							<asp:Literal ID="LabelSubject" Runat="server"></asp:Literal>
						</td>
					</tr>
					<% if (Attachments != string.Empty) { %>
					<tr>
						<td class="wm_print_title">
						<%=_manager.GetString("Attachments")%>
						</td>
						<td class="wm_print_value">
							<asp:Literal ID="LabelAttachments" Runat="server"></asp:Literal>
						</td>
					</tr>
					<% } %>
					<tr>
						<td colspan="2" class="wm_print_body">
							<div class="wm_space_before">
								<asp:Literal ID="MessageBody" Runat="server"></asp:Literal>
							</div>
						</td>
					</tr>
				</table>
			</div>
		</body>
	</html>
