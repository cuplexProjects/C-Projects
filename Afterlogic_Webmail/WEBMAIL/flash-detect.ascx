<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="flash-detect.ascx.cs" Inherits="WebMail.flash_detect" %>
<script type="text/javascript">
	var FLASH_INSTALLED = 2
	var FLASH_NOT_INSTALLED = 1;
	var FLASH_UNKNOWN = 0;
	
	var hasMimeTypes = (navigator.mimeTypes && navigator.mimeTypes.length);
	var MSDetect = (hasMimeTypes) ? "false" : "true";
	
	var flashInstalled = FLASH_UNKNOWN;
	if (hasMimeTypes) {
		var flashMType = navigator.mimeTypes['application/x-shockwave-flash'];
		flashInstalled = (flashMType && flashMType.enabledPlugin) ? FLASH_INSTALLED : FLASH_NOT_INSTALLED;
	}
</script>
<script type="text/vbscript">
	If MSDetect = "true" Then
		flashInstalled = FLASH_NOT_INSTALLED
		On error resume next
		If (IsObject(CreateObject("ShockwaveFlash.ShockwaveFlash"))) Then
			flashInstalled = FLASH_INSTALLED
		End If
		If Err.Number<>0 Then
			flashInstalled = FLASH_NOT_INSTALLED
		End If
	End If
</script>
<script type="text/javascript">
	// flashInstalled = FLASH_NOT_INSTALLED;
</script>