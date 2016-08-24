using System;
using System.IO;
using System.Web;
using System.Web.UI;

namespace WebMail
{
	/// <summary>
	/// Summary description for upload.
	/// </summary>
	public partial class upload : Page
	{
		protected bool errorOccured = false;
		protected string name = string.Empty;
		protected string tmp_name = string.Empty;
		protected int size = 0;
		protected string mime_type = string.Empty;
		protected string error = string.Empty;
		protected string inlineImage = "false";
        protected string url = "";
		protected bool flashUpload = true;

		protected void Page_Load(object sender, EventArgs e)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			inlineImage = (Request["inline_image"] == "1") ? "true" : "false";
			flashUpload = (Request["flash_upload"] == "0") ? false : true;

			HttpFileCollection files = Request.Files;
			if ((files != null) && (files.Count > 0))
			{
				HttpPostedFile file = files[0];
				if (file != null)
				{
					name = Utils.EncodeHtml(Path.GetFileName(file.FileName)).Replace(@"'", @"\'");
					size = file.ContentLength;
					mime_type = Utils.EncodeHtml(file.ContentType);
					if ((file.ContentLength < settings.AttachmentSizeLimit) || (settings.EnableAttachmentSizeLimit == false))
					{
						try
						{
							byte[] buffer;
							using (Stream uploadStream = file.InputStream)
							{
								buffer = new byte[uploadStream.Length];
								long numBytesToRead = uploadStream.Length;
								long numBytesRead = 0;
								while (numBytesToRead > 0)
								{
									int n = uploadStream.Read(buffer, (int)numBytesRead, (int)numBytesToRead);
									if (n == 0)
										break;
									numBytesRead += n;
									numBytesToRead -= n;
								}
							}
							if (buffer != null)
							{
								string tempFolder = Utils.GetTempFolderName(Session);
								string filename = Utils.CreateTempFilePath(tempFolder, file.FileName);
								tmp_name = Path.GetFileName(filename);
								url = String.Format("download-view-attachment.aspx?download=0&temp_filename={0}", tmp_name);
								using (FileStream fs = File.Open(filename, FileMode.Create, FileAccess.Write))
								{
									fs.Write(buffer, 0, buffer.Length);
								}
							}
						}
						catch (IOException ex)
						{
							errorOccured = true;
							error = (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("UnknownUploadError");
							Log.WriteException(ex);
						}
					}
					else
					{
						errorOccured = true;
						error = (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("FileIsTooBig");
						Log.WriteLine("upload", error);
					}
				}
				else
				{
					errorOccured = true;
					error = (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("NoFileUploaded");
					Log.WriteLine("upload", error);
				}
			}
			else
			{
				errorOccured = true;
				error = (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("NoFileUploaded");
				Log.WriteLine("upload", error);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new EventHandler(this.Page_Load);
		}
		#endregion
	}
}
