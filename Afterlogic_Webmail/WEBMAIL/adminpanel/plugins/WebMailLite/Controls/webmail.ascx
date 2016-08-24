<%@ Control Language="C#" AutoEventWireup="true" Inherits="webmail_settingsLite" Codebehind="webmail.ascx.cs" %>

<script type="text/javascript">
	function mailProtocolChange() 
	{
		// change port
		var select = document.getElementById('<%=intIncomingMailProtocol.ClientID%>');
		var port = document.getElementById('<%=intIncomingMailPort.ClientID%>');
		port.value = (select.value == 'POP3') ? '110' : '143';
	}
	
	function DirectModeIsDefaultChange() 
	{
		document.getElementById('<%=intDirectModeIsDefault.ClientID%>').disabled = !document.getElementById('<%=intAllowDirectMode.ClientID%>').checked;
	}

    function Run()
	{
		if (document.getElementById('<%=intEnableAttachmentSizeLimit.ClientID%>').checked)
		{
			document.getElementById('<%=intAttachmentSizeLimit.ClientID%>').readOnly = false;
			document.getElementById('<%=intAttachmentSizeLimit.ClientID%>').style.background = "#FFFFFF";
		}
		else
		{
			document.getElementById('<%=intAttachmentSizeLimit.ClientID%>').readOnly = true;
			document.getElementById('<%=intAttachmentSizeLimit.ClientID%>').style.background = "#EEEEEE";
		}
		
		if (document.getElementById('<%=intEnableMailboxSizeLimit.ClientID%>').checked)
		{
			document.getElementById('<%=intMailboxSizeLimit.ClientID%>').readOnly = false;
			document.getElementById('<%=intMailboxSizeLimit.ClientID%>').style.background = "#FFFFFF";
		}
		else
		{
			document.getElementById('<%=intMailboxSizeLimit.ClientID%>').readOnly = true;
			document.getElementById('<%=intMailboxSizeLimit.ClientID%>').style.background = "#EEEEEE";
		}
        DirectModeIsDefaultChange();
	}
</script>

<table class="wm_settings_common" width="550" border="0">
	<tr>
		<td width="150"></td>
		<td width="160"></td>
		<td></td>
	</tr>
	<tr>
		<td class="wm_settings_list_select" colspan="3"><b>Common Settings</b></td>
	</tr>
	<tr>
		<td colspan="2"><br>
		</td>
	</tr>
	<tr>
		<td align="right">Site name:</td>
		<td colspan="2"><input type="text" id="txtSiteName" size="50" runat="server" class="wm_input" maxlength="100" /></td>
	</tr>
	<tr>
		<td colspan="3"><br />
		</td>
	</tr>
	<tr>
		<td class="wm_settings_list_select" colspan="3"><b>Default Mail Server Settings</b></td>
	</tr>
	<tr>
		<td colspan="3"><br />
		</td>
	</tr>
	<tr>
		<td align="right">Incoming mail:
		</td>
		<td><input type="text" id="txtIncomingMail" maxlength="100" runat="server" class="wm_input"></td>
		<td><nobr>Port:&nbsp;<input type="text" id="intIncomingMailPort" maxlength="4" size="3" runat="server" class="wm_input">
				&nbsp;<select id="intIncomingMailProtocol" runat="server" onchange="mailProtocolChange()" class="wm_input">
					<option value="POP3" selected>POP3</option>
					<option value="IMAP4">IMAP4</option>
				</select></nobr>
		</td>
	</tr>
	<tr>
		<td align="right">Outgoing mail:
		</td>
		<td><input type="text" id="txtOutgoingMail" maxlength="100" runat="server" class="wm_input"></td>
		<td>Port:&nbsp;<input type="text" id="intOutgoingMailPort" maxlength="4" size="3" runat="server" class="wm_input">
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colspan="2"><input type="checkbox" id="intReqSmtpAuthentication" class="wm_checkbox" runat="server">
			<label for="<%=intReqSmtpAuthentication.ClientID%>">Requires SMTP authentication</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colspan="2"><input type="checkbox" id="intAllowDirectMode" class="wm_checkbox" runat="server" onclick="DirectModeIsDefaultChange();">
		    <label for="<%=intAllowDirectMode.ClientID%>">Allow direct mode</label> (<a target="_blank" href="http://www.afterlogic.com/kb/articles/synchronization-modes-how-do-they-work">What's this?</a>)
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colspan="2"><input type="checkbox" id="intDirectModeIsDefault" class="wm_checkbox" Runat="server">
			<label for="<%=intDirectModeIsDefault.ClientID%>">Direct mode is default</label>
		</td>
	</tr>
	<tr>
		<td align="right">Attachment size limit:
		</td>
		<td colspan="2"><input type="text" id="intAttachmentSizeLimit" maxlength="10" runat="server" class="wm_input"
				style="WIDTH: 85px"> KB&nbsp;&nbsp;&nbsp; <input type="checkbox" id="intEnableAttachmentSizeLimit" class="wm_checkbox" runat="server" onclick="Run()">
				<label for="<%=intEnableAttachmentSizeLimit.ClientID%>">Enable attachment size limit</label>
		</td>
	</tr>
	<tr>
		<td align="right">Mailbox size limit:
		</td>
		<td colspan="2"><input type="text" id="intMailboxSizeLimit" maxlength="10" runat="server" class="wm_input"
				style="WIDTH: 85px"> KB&nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" id="intEnableMailboxSizeLimit" class="wm_checkbox" runat="server"	onclick="Run()">
				<label for="<%=intEnableMailboxSizeLimit.ClientID%>">Enable mailbox size limit</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colspan="2"><input type="checkbox" id="intTakeImapQuota" class="wm_checkbox" runat="server">
		        <label for="<%=intTakeImapQuota.ClientID%>">Take quota value from IMAP server</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colspan="2"><input type="checkbox" id="intAllowUsersChangeEmailSettings" class="wm_checkbox" runat="server">
		        <label for="<%=intAllowUsersChangeEmailSettings.ClientID%>">Allow new users to change email settings</label>
		</td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
        <td colspan="2">
            <input id="intAllowUsersAddNewAccounts" class="wm_checkbox" type="checkbox" disabled="disabled" value="1" runat="server"/>
            <label for="<%=intAllowUsersAddNewAccounts.ClientID%>">
                <font color="#aaaaaa">Allow users to add new email accounts (Pro only)</font>
            </label>
        </td>
	</tr>
	<tr class="wm_hide">
		<td align="right">&nbsp;</td>
		<td colspan="2"><input type="checkbox" id="intAllowUsersChangeAccountsDef" class="wm_checkbox" runat="server">
		    <label for="<%=intAllowUsersChangeAccountsDef.ClientID%>">Allow users to change accounts which can be used to log in</label>
		</td>
	</tr>
	<tr>
		<td colspan="2"><br>
		</td>
	</tr>
	<tr>
		<td class="wm_settings_list_select" colspan="3"><b>Internationalization Support</b></td>
	</tr>
	<tr>
		<td colspan="3"><br>
		</td>
	</tr>
	<tr>
		<td align="right">Default user charset
		</td>
		<td colspan="2"><select id="txtDefaultUserCharset" runat="server" class="wm_input" style="WIDTH: 320px">
				<option Value="0" selected>Default</option>
				<option Value="950">Chinese Traditional</option>
				<option Value="949">Korean (EUC)</option>
				<option Value="50225">Korean (ISO)</option>
				<option Value="50220">Japanese</option>
				<option Value="932">Japanese (Shift-JIS)</option>
				<option Value="28591">Western Alphabet (ISO)</option>
				<option Value="28592">Central European Alphabet (ISO)</option>
				<option Value="28593">Latin 3 Alphabet (ISO)</option>
				<option Value="28594">Baltic Alphabet (ISO)</option>
				<option Value="28595">Cyrillic Alphabet (ISO)</option>
				<option Value="28596">Arabic Alphabet (ISO)</option>
				<option Value="28597">Greek Alphabet (ISO)</option>
				<option Value="28598">Hebrew Alphabet (ISO)</option>
				<option Value="20866">Cyrillic Alphabet (KOI8-R)</option>
				<option Value="65000">Universal Alphabet (UTF-7)</option>
				<option Value="65001">Universal Alphabet (UTF-8)</option>
				<option Value="1250">Central European Alphabet (Windows)</option>
				<option Value="1251">Cyrillic Alphabet (Windows)</option>
				<option Value="1252">Western Alphabet (Windows)</option>
				<option Value="1253">Greek Alphabet (Windows)</option>
				<option Value="1254">Turkish Alphabet</option>
				<option Value="1255">Hebrew Alphabet (Windows)</option>
				<option Value="1256">Arabic Alphabet (Windows)</option>
				<option Value="1257">Baltic Alphabet (Windows)</option>
				<option Value="1258">Vietnamese Alphabet (Windows)</option>
			</select></td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colspan="2">
			<input type="checkbox" id="intAllowUsersChangeCharset" class="wm_checkbox" runat="server">
			<label for="<%=intAllowUsersChangeCharset.ClientID%>">Allow users to change charset</label>
		</td>
	</tr>
	<tr>
		<td align="right">Default user time offset</td>
		<td colspan="2"><select id="txtDefaultTimeZone" runat="server" class="wm_input" style="WIDTH: 320px">
				<option Value="0" selected>Default</option>
				<option Value="1">(GMT -12:00) Eniwetok, Kwajalein, Dateline Time</option>
				<option Value="2">(GMT -11:00) Midway Island, Samoa</option>
				<option Value="3">(GMT -10:00) Hawaii</option>
				<option Value="4">(GMT -09:00) Alaska</option>
				<option Value="5">(GMT -08:00) Pacific Time (US &amp; Canada); Tijuana</option>
				<option Value="6">(GMT -07:00) Arizona</option>
				<option Value="7">(GMT -07:00) Mountain Time (US &amp; Canada)</option>
				<option Value="8">(GMT -06:00) Central America</option>
				<option Value="9">(GMT -06:00) Central Time (US &amp; Canada)</option>
				<option Value="10">(GMT -06:00) Mexico City, Tegucigalpa</option>
				<option Value="11">(GMT -06:00) Saskatchewan</option>
				<option Value="12">(GMT -05:00) Indiana (East)</option>
				<option Value="13">(GMT -05:00) Eastern Time (US &amp; Canada)</option>
				<option Value="14">(GMT -05:00) Bogota, Lima, Quito</option>
				<option Value="15">(GMT -04:00) Santiago</option>
				<option Value="16">(GMT -04:00) Caracas, La Paz</option>
				<option Value="17">(GMT -04:00) Atlantic Time (Canada)</option>
				<option Value="18">(GMT -03:30) Newfoundland</option>
				<option Value="19">(GMT -03:00) Greenland</option>
				<option Value="20">(GMT -03:00) Buenos Aires, Georgetown</option>
				<option Value="21">(GMT -03:00) Brasilia</option>
				<option Value="22">(GMT -02:00) Mid-Atlantic</option>
				<option Value="23">(GMT -01:00) Cape Verde Is.</option>
				<option Value="24">(GMT -01:00) Azores</option>
				<option Value="25">(GMT) Casablanca, Monrovia</option>
				<option Value="26">(GMT) Dublin, Edinburgh, Lisbon, London</option>
				<option Value="27">(GMT +01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna</option>
				<option Value="28">(GMT +01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague</option>
				<option Value="29">(GMT +01:00) Brussels, Copenhagen, Madrid, Paris</option>
				<option Value="30">(GMT +01:00) Sarajevo, Skopje, Sofija, Vilnius, Warsaw, Zagreb</option>
				<option Value="31">(GMT +01:00) West Central Africa</option>
				<option Value="32">(GMT +02:00) Athens, Istanbul, Minsk</option>
				<option Value="33">(GMT +02:00) Bucharest</option>
				<option Value="34">(GMT +02:00) Cairo</option>
				<option Value="35">(GMT +02:00) Harare, Pretoria</option>
				<option Value="36">(GMT +02:00) Helsinki, Riga, Tallinn</option>
				<option Value="37">(GMT +02:00) Israel, Jerusalem Standard Time</option>
				<option Value="38">(GMT +03:00) Baghdad</option>
				<option Value="39">(GMT +03:00) Arab, Kuwait, Riyadh</option>
				<option Value="40">(GMT +03:00) Moscow, St. Petersburg, Volgograd</option>
				<option Value="41">(GMT +03:00) East Africa, Nairobi</option>
				<option Value="42">(GMT +03:30) Tehran</option>
				<option Value="43">(GMT +04:00) Abu Dhabi, Muscat</option>
				<option Value="44">(GMT +04:00) Baku, Tbilisi, Yerevan</option>
				<option Value="45">(GMT +04:30) Kabul</option>
				<option Value="46">(GMT +05:00) Ekaterinburg</option>
				<option Value="47">(GMT +05:00) Islamabad, Karachi, Sverdlovsk, Tashkent</option>
				<option Value="48">(GMT +05:30) Calcutta, Chennai, Mumbai, New Delhi, India 
					Standard Time</option>
				<option Value="49">(GMT +05:45) Kathmandu, Nepal</option>
				<option Value="50">(GMT +06:00) Almaty, Novosibirsk, North Central Asia</option>
				<option Value="51">(GMT +06:00) Astana, Dhaka</option>
				<option Value="52">(GMT +06:00) Sri Jayewardenepura, Sri Lanka</option>
				<option Value="53">(GMT +06:30) Rangoon</option>
				<option Value="54">(GMT +07:00) Bangkok, Hanoi, Jakarta</option>
				<option Value="55">(GMT +07:00) Krasnoyarsk</option>
				<option Value="56">(GMT +08:00) Beijing, Chongqing, Hong Kong SAR, Urumqi</option>
				<option Value="57">(GMT +08:00) Irkutsk, Ulaan Bataar</option>
				<option Value="58">(GMT +08:00) Kuala Lumpur, Singapore</option>
				<option Value="59">(GMT +08:00) Perth, Western Australia</option>
				<option Value="60">(GMT +08:00) Taipei</option>
				<option Value="61">(GMT +09:00) Osaka, Sapporo, Tokyo</option>
				<option Value="62">(GMT +09:00) Seoul, Korea Standard time</option>
				<option Value="63">(GMT +09:00) Yakutsk</option>
				<option Value="64">(GMT +09:30) Adelaide, Central Australia</option>
				<option Value="65">(GMT +09:30) Darwin</option>
				<option Value="66">(GMT +10:00) Brisbane, East Australia</option>
				<option Value="67">(GMT +10:00) Canberra, Melbourne, Sydney, Hobart</option>
				<option Value="68">(GMT +10:00) Guam, Port Moresby</option>
				<option Value="69">(GMT +10:00) Hobart, Tasmania</option>
				<option Value="70">(GMT +10:00) Vladivostok</option>
				<option Value="71">(GMT +11:00) Magadan, Solomon Is., New Caledonia</option>
				<option Value="72">(GMT +12:00) Auckland, Wellington</option>
				<option Value="73">(GMT +12:00) Fiji Islands, Kamchatka, Marshall Is.</option>
				<option Value="74">(GMT +13:00) Nuku'alofa, Tonga</option>
			</select></td>
	</tr>
	<tr>
		<td align="right">&nbsp;</td>
		<td colspan="2">
			<input type="checkbox" id="intAllowUsersChangeTimeOffset" class="wm_checkbox" runat="server"/>
			<label for="<%=intAllowUsersChangeTimeOffset.ClientID%>">Allow users to change time offset</label>
		</td>
	</tr>
    <!-- hr -->
	<tr>
		<td colspan="3">
			<hr size="1" />
		</td>
	</tr>
	<tr>
		<td></td>
		<td colspan="2" align="right">
		    <asp:Button id="SaveButton" style="FONT-WEIGHT: bold" runat="server" text="Save" Width="100" onclick="SaveButton_Click" />
		</td>
	</tr>
</table>
<script type="text/javascript">
	Run();
</script>
