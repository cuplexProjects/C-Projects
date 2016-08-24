using System;
using System.Collections.Specialized;

namespace WebMail.classic
{

	/// <summary>
	/// Summary description for jsbuilder.
	/// </summary>
	public class jsbuilder
	{
		const string CRLF = "\r\n";
		private StringCollection _jsFiles = new StringCollection();
		private StringCollection _jsText = new StringCollection();
		private StringCollection _jsInitFunctionText = new StringCollection();

		public jsbuilder()
		{
			this.AddFile("langs-js.aspx");
			this.AddFile("js/common/defines.js");
			this.AddFile("js/common/functions.js");
			this.AddFile("js/common/common-helpers.js");
			this.AddFile("js/common/popups.js");
		}

		public void AddFile(string text)
		{
			text = text.Trim();
			if (text.Length > 0)
			{
				if (!this._jsFiles.Contains(text))
				{
					this._jsFiles.Add(text);
				}
			}
		}

		public void AddText(string text)
		{
			text = text.Trim();
			if (text.Length > 0)
			{
				this._jsText.Add(text);
			}
		}

		public void AddInitText(string text)
		{
			text = text.Trim();
			if (text.Length > 0)
			{
				this._jsInitFunctionText.Add(text);
			}
		}
		
		public string FilesToHtml()
		{
			string returString = "";
			foreach (string fileString in this._jsFiles)
			{
				returString += string.Format(@"<script language=""JavaScript"" type=""text/javascript"" src=""{0}""></script>" + jsbuilder.CRLF, fileString);
			}
			return returString;
		}

		public string TextToHtml()
		{
			string returString = "";
			if (this._jsText.Count > 0)
			{
				returString += @"<script language=""JavaScript"" type=""text/javascript"">" + jsbuilder.CRLF;
				foreach (string textString in this._jsText)
				{
					returString += textString + jsbuilder.CRLF;
				}
				returString += jsbuilder.CRLF + "</script>" + jsbuilder.CRLF;
			}
			return returString;
		}

		public string InitTextToHtml()
		{
			string returString = @"<script language=""JavaScript"">
function Init()
{
	Browser = new CBrowser();
	PopupMenus = new CPopupMenus();
";
			foreach (string initString in this._jsInitFunctionText)
			{
				returString += initString + jsbuilder.CRLF;
			}

			returString += @"ResizeElements(""all"");
}
Init();
</script>";
			
			return returString;
		}

		public string ToHtml()
		{
			return jsbuilder.CRLF + this.FilesToHtml() + this.TextToHtml();
		}

	}
}