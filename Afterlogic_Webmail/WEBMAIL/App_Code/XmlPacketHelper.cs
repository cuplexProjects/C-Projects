using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;
using System.Xml;

namespace WebMail
{
	public static class XmlPacketHelper
	{
		internal static System.Xml.XmlAttribute CreateInboxSyncAttr(XmlElement webmailNode, Account acct)
		{
			XmlAttribute inboxSyncTypeAttr = webmailNode.OwnerDocument.CreateAttribute("inbox_sync_type");
			FolderSyncType inboxSyncType = FolderSyncType.NewHeadersOnly;
			MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
			try
			{
				mp.Connect();
				Folder fld = mp.GetFolder(FolderType.Inbox);
				if (fld != null) inboxSyncType = fld.SyncType;
			}
			finally
			{
				mp.Disconnect();
			}
			inboxSyncTypeAttr.Value = ((short)inboxSyncType).ToString(CultureInfo.InvariantCulture);

			return inboxSyncTypeAttr;
		}

		internal static System.Xml.XmlAttribute CreateMail_ModeAttr(XmlElement webmailNode, Account acct)
		{
			int mail_mode = 1;
			if (acct.MailMode == MailMode.DeleteMessagesFromServer) mail_mode = 0;
			if (acct.MailMode == MailMode.LeaveMessagesOnServer) mail_mode = 1;
			if (acct.MailMode == MailMode.KeepMessagesOnServer) mail_mode = 2;
			if (acct.MailMode == MailMode.DeleteMessageWhenItsRemovedFromTrash) mail_mode = 3;
			if (acct.MailMode == MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash) mail_mode = 4;
			XmlAttribute mailModeAttr = webmailNode.OwnerDocument.CreateAttribute("mail_mode");
			mailModeAttr.Value = mail_mode.ToString(CultureInfo.InvariantCulture);

			return mailModeAttr;
		}
	}
}
