using System;
using System.Collections;
using System.Globalization;
using System.IO;
using MailBee;
using MailBee.ImapMail;
using MailBee.Security;
using System.Collections.Generic;
using MailBee.Mime;

namespace WebMail
{
	public class EnvelopeComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Envelope xEnvelope = x as Envelope;
			Envelope yEnvelope = y as Envelope;
			if (xEnvelope != null && yEnvelope != null)
				return xEnvelope.Uid.CompareTo(yEnvelope.Uid);
			return 0;
		}
	}

    /// <summary>
    /// This class provides properties and methods for connecting to an IMAP4 server, 
    /// downloading, searching, and managing folders and e-mail messages in a user account. 
    /// </summary>
	public class ImapStorage : MailServerStorage
	{
		protected Imap _imapObj;
		protected MailBee.ImapMail.FolderCollection _imapFolders = null;
		protected MailBee.ImapMail.FolderCollection _subscribedImapFolders = null;
		protected FolderCollection _folderTree = null;

		public ImapStorage(Account account) : base(account)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			try
			{
                
                
                Imap.LicenseKey  = settings.LicenseKey;
                
				_imapObj = new Imap();
				_imapObj.EnvelopeDownloaded += new ImapEnvelopeDownloadedEventHandler(_imapObj_EnvelopeDownloaded);

                _imapObj.Timeout = 600000;
                
                if (settings.EnableLogging)
				{
					_imapObj.Log.Enabled = true;
					string dataFolderPath = Utils.GetDataFolderPath();
					if (dataFolderPath != null)
					{
						_imapObj.Log.Filename = Path.Combine(dataFolderPath, Constants.logFilename);
					}
				}
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailException(ex.Message);
			}
		}

		public override void Connect()
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			try
			{
                if (_account.MailIncomingPort == 993)
                {
                    _imapObj.SslMode = SslStartupMode.OnConnect;
                    _imapObj.SslProtocol = SecurityProtocol.Auto;
                    _imapObj.SslCertificates.AutoValidation = CertificateValidationFlags.None;
                }
                _imapObj.Connect(_account.MailIncomingHost, _account.MailIncomingPort);
				_imapObj.Login(_account.MailIncomingLogin, _account.MailIncomingPassword, AuthenticationMethods.Auto, AuthenticationOptions.PreferSimpleMethods, null);
			}
			catch (MailBeeConnectionException ex)
			{
                Log.WriteException(ex);
                throw new WebMailException(resMan.GetString("ErrorIMAP4Connect"));
			}
			catch (MailBeeImapLoginBadCredentialsException ex)
			{
                Log.WriteException(ex);
                throw new WebMailException(resMan.GetString("ErrorPOP3IMAP4Auth"));
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
		}

		public override bool IsConnected()
		{
			return _imapObj.IsConnected;
		}

		public override void Disconnect()
		{
			try
			{
				if (IsConnected()) _imapObj.Disconnect();
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
		}

		public override long GetMailStorageSize()
		{
			try
			{
				if (_imapObj.GetExtension("QUOTA") != null)
				{
                    FolderQuota fq = _imapObj.GetFolderQuota(Constants.FolderNames.Inbox);
					if (fq.MaxStorageSize > -1)
					{
						return fq.MaxStorageSize;
					}
				}
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
//                throw new WebMailMailBeeException(ex);
			}
			return 0;
		}

        public bool IsQuotaSupported()
        {
            try
            {
                if (_imapObj.GetExtension("QUOTA") != null)
                {
                    return true;
                }
            }
            catch (MailBeeException ex)
            {
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
            }
            return false;
        }
        
        public override int GetFolderMessageCount(string fullPath)
		{
			FolderStatus fs = GetFolderStatus(fullPath);
            if (fs != null) return fs.MessageCount;
            return 0;
		}

		public override int GetFolderUnreadMessageCount(string fullPath)
		{
			FolderStatus fs = GetFolderStatus(fullPath);
			if (fs != null && fs.UnseenCount >= 0) return fs.UnseenCount;
            return 0;
		}

		public override Folder CreateFolder(Folder fld)
        {
            try
            {
                _imapObj.CreateFolder(fld.FullPath);
            }
            catch (MailBeeException ex)
            {
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
            }
            try
            {
                if (fld.Hide)
                {
                    _imapObj.UnsubscribeFolder(fld.FullPath);
                }
                else
                {
                    _imapObj.SubscribeFolder(fld.FullPath);
                }
            }
            catch (MailBeeException)
            {

            }
            return fld;
        }

		public override void DeleteFolder(Folder fld)
		{
			try
			{
				if (GetImapFolder(fld.FullPath) != null)
				{
					_imapObj.DeleteFolder(fld.FullPath);
				}
			}
			catch (MailBeeException ex)
			{
				// if non empty folder
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
		}

		public override void UpdateFolder(Folder fld)
        {
            try
            {
                if (string.Compare(fld.Name, fld.UpdateName, true, CultureInfo.InvariantCulture) != 0)
                {
                    if (!string.IsNullOrEmpty(fld.UpdateFullPath))
                    {
                        _imapObj.RenameFolder(fld.FullPath, fld.UpdateFullPath);
                    }
                }
            }
            catch (MailBeeException ex)
            {
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
            }
            try
            {
                if (fld.Hide)
                {
                    _imapObj.UnsubscribeFolder(fld.FullPath);
                }
                else
                {
                    _imapObj.SubscribeFolder(fld.FullPath);
                }
            }
            catch (MailBeeException)
            {

            }
        }

        private static ArrayList ReSetType(FolderCollection fc, bool hasInbox, bool hasSent, bool hasDrafts, bool hasTrash, bool hasSpam)
        {
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            
            for (int i = 0; i < fc.Count; i++)
            {
                if (fc[i].Type == FolderType.Inbox)
                {
					if (hasInbox) fc[i].Type = FolderType.Custom;
					else fc[i].Hide = false;
                    hasInbox = true;
                }
                else if (fc[i].Type == FolderType.SentItems)
                {
                    if (hasSent) fc[i].Type = FolderType.Custom;
					else fc[i].Hide = false;
					hasSent = true;
                }
                else if (fc[i].Type == FolderType.Drafts)
                {
                    if (hasDrafts) fc[i].Type = FolderType.Custom;
					else fc[i].Hide = false;
					hasDrafts = true;
                }
                else if (fc[i].Type == FolderType.Trash && settings.Imap4DeleteLikePop3)
                {
                    if (hasTrash) fc[i].Type = FolderType.Custom;
                    else fc[i].Hide = false;
                    hasTrash = true;
                }
                else if (fc[i].Type == FolderType.Spam)
                {
                    if (hasSpam) fc[i].Type = FolderType.Custom;
                    else fc[i].Hide = false;
                    hasSpam = true;
                }
                else
                {
                    fc[i].Type = FolderType.Custom;
                }

                if (fc[i].SubFolders.Count > 0)
                {
                    if (fc[i].Type == FolderType.Inbox)
                    {
                        ArrayList vars = ReSetType(fc[i].SubFolders, hasInbox, hasSent, hasDrafts, hasTrash, hasSpam);
                        if (vars.Count == 5)
                        {
                            hasInbox = (bool) vars[0];
                            hasSent = (bool) vars[1];
                            hasDrafts = (bool) vars[2];
                            hasTrash = (bool)vars[3];
                            hasSpam = (bool)vars[4];
                        }
                    }
                    else
                    {
                        ReSetType(fc[i].SubFolders, true, true, true, true, true);
                    }
                }
            }

            ArrayList ArrayResp = new ArrayList();
            ArrayResp.Add(hasInbox);
            ArrayResp.Add(hasSent);
            ArrayResp.Add(hasDrafts);
            ArrayResp.Add(hasTrash);
            ArrayResp.Add(hasSpam);

            return ArrayResp;
        }

        public override FolderCollection GetFolders()
		{
			try
			{
				if (_folderTree == null)
				{
					MailBee.ImapMail.FolderCollection subscribedFolderCollection = DownloadSubscribedImapFolders();
					MailBee.ImapMail.FolderCollection mailBeeFolderCollection = DownloadImapFolders();

					FolderCollection tmp_col = FolderCollection.CreateImapFolderTreeFromList(mailBeeFolderCollection, subscribedFolderCollection);
					ReSetType(tmp_col, false, false, false, false, false);
					_folderTree = tmp_col;
				}
				return _folderTree;
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
		}

		public override Folder GetFolder(string folderFullName)
		{
			try
			{
				FolderCollection fc = GetFolders();
				foreach (Folder fld in fc)
				{
					if (fld.FullPath == folderFullName) return fld;
				}
				return null;
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
		}

        public byte[] GetEnvelopeItem(Envelope env, byte[] mimePartHeader, byte[] mimePartBody)
        {
            int mimePartHeaderLength = ((mimePartHeader != null) ? mimePartHeader.Length : 0);
            int mimePartBodyLength = ((mimePartBody != null) ? mimePartBody.Length : 0);

            // Build MIME part data from header data and body data.
            byte[] mimePartData = new byte[mimePartHeaderLength + mimePartBodyLength];
            if (mimePartHeader != null)
            {
                Buffer.BlockCopy(mimePartHeader, 0, mimePartData, 0, mimePartHeaderLength);
            }
            if (mimePartBody != null)
            {
                Buffer.BlockCopy(mimePartBody, 0, mimePartData, mimePartHeaderLength, mimePartBodyLength);
            }
            mimePartHeader = null;
            mimePartBody = null;

            return mimePartData;
        }
    

        public override WebMailMessage LoadMessage(object index, Folder fld)
        {
        
            return LoadMessage(index, fld, false, MessageMode.None);
        }

        public override WebMailMessage LoadMessage(object index, Folder fld, bool body_strucrure, MessageMode mode)
		{
			long lngUid;
			try
			{
				lngUid = Convert.ToInt64(index);
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("InvalidUid"));
			}
			try
			{
				if (fld != null) 
				{
                    if (ExamineImapFolder(fld.FullPath))
					{
						WebMailMessage result = new WebMailMessage(_account);

                        if (body_strucrure)
                        {
                            EnvelopeCollection envs = _imapObj.DownloadEnvelopes(lngUid.ToString(), true,
                                EnvelopeParts.BodyStructure, 0, null, null);
                            
                            MailMessageCollection msgs = new MailMessageCollection();

                            if ((mode & MessageMode.Headers) > 0)
                            {
                                msgs = _imapObj.DownloadMessageHeaders(lngUid.ToString(), true);
                            }
                            else
                            {
                                msgs = _imapObj.DownloadMessageHeaders(lngUid.ToString(), true);
                            }

                            foreach (MailMessage msg in msgs)
                            {
                                List<string> HtmlPartsRequest = new List<string>();
                                List<string> HtmlPartsResponse = new List<string>();
                                List<string> PlainPartsRequest = new List<string>();
                                List<string> PlainPartsResponse = new List<string>();

                                List<string> AttachmentsParts = new List<string>();
                                List<string> AttachmentsHeaderPartsRequest = new List<string>();
                                List<string> AttachmentsHeaderPartsResponse = new List<string>();
                                List<int> AttachmentsPartsSize = new List<int>();

                                List<string> AllParts = new List<string>();

                                long uid = 0;
                                Envelope _env = envs[0];
                                if (_env.IsValid)
                                {
                                    ImapBodyStructureCollection parts = _env.BodyStructure.GetAllParts();
                                    foreach (ImapBodyStructure part in parts)
                                    {
                                        // Attachments Part
                                        if (((mode & MessageMode.Attachments) > 0) && ((part.Disposition != null &&
                                            part.Disposition.ToLower() == "attachment") ||
                                            (part.Filename != null && part.Filename != string.Empty) ||
                                            (part.ContentType != null && (part.ContentType.ToLower() == "message/rfc822" ||
                                            part.ContentType.ToLower().StartsWith("image/")))))
                                        {
                                            int size = part.Size;
                                            if (part.MailEncodingName.ToLower() == "base64")
                                            {
                                                size = (int)Math.Round(size * 0.75, 0);
                                            }
                                            else if (part.MailEncodingName.ToLower() == "quoted-printable")
                                            {
                                                size = (int)Math.Round(size * 0.44, 0);
                                            }
                                            AttachmentsHeaderPartsRequest.Add("BODY.PEEK[" + part.PartID + ".MIME]");
                                            AttachmentsHeaderPartsResponse.Add("BODY[" + part.PartID + ".MIME]");
                                            AttachmentsParts.Add(part.PartID);
                                            AttachmentsPartsSize.Add(size);
                                        }

                                        if (part.Disposition == null)
                                        {
                                            // Plain Part
                                            if (part.ContentType.ToLower() == "text/plain")
                                            {
                                                PlainPartsRequest.Add("BODY.PEEK[" + part.PartID + ".MIME]");
                                                PlainPartsRequest.Add("BODY.PEEK[" + part.PartID + "]");

                                                PlainPartsResponse.Add("BODY[" + part.PartID + ".MIME]");
                                                PlainPartsResponse.Add("BODY[" + part.PartID + "]");
                                            }

                                            // Html Part
                                            if (part.ContentType.ToLower() == "text/html")
                                            {
                                                HtmlPartsRequest.Add("BODY.PEEK[" + part.PartID + ".MIME]");
                                                HtmlPartsRequest.Add("BODY.PEEK[" + part.PartID + "]");

                                                HtmlPartsResponse.Add("BODY[" + part.PartID + ".MIME]");
                                                HtmlPartsResponse.Add("BODY[" + part.PartID + "]");
                                            }
                                        }
                                    }
                                    AllParts.AddRange(PlainPartsRequest);
                                    AllParts.AddRange(HtmlPartsRequest);
                                    AllParts.AddRange(AttachmentsHeaderPartsRequest);
                                }
                                uid = _env.Uid;

                                if (uid > 0)
                                {
                                    envs = _imapObj.DownloadEnvelopes(uid.ToString(), true,
                                        EnvelopeParts.Uid, 0, null, (string[])AllParts.ToArray());

                                    foreach (Envelope env in envs)
                                    {
                                        if (env.IsValid)
                                        {
                                            if ((mode & MessageMode.HtmlBody) > 0)
                                            {
                                                for (int i = 0; i < HtmlPartsResponse.Count; i += 2)
                                                {
                                                    byte[] mimePartHeader = (byte[])env.GetEnvelopeItem(HtmlPartsResponse[i], true);
                                                    byte[] mimePartBody = (byte[])env.GetEnvelopeItem(HtmlPartsResponse[i + 1], true);
                                                    byte[] mimePartData = GetEnvelopeItem(env, mimePartHeader, mimePartBody);

                                                    TextBodyPart tbPart = new TextBodyPart(MimePart.Parse(mimePartData));
                                                    msg.BodyParts.Html.Text += tbPart.Text;
                                                    msg.BodyParts.Html.Charset = tbPart.Charset;
                                                }
                                            }

                                            if ((mode & MessageMode.PlainBody) > 0 ||
                                                (HtmlPartsResponse.Count == 0) || 
                                                (msg.BodyHtmlText.Length == 0))
                                            {
                                                for (int i = 0; i < PlainPartsResponse.Count; i += 2)
                                                {
                                                    byte[] mimePartHeader = (byte[])env.GetEnvelopeItem(PlainPartsResponse[i], true);
                                                    byte[] mimePartBody = (byte[])env.GetEnvelopeItem(PlainPartsResponse[i + 1], true);
                                                    byte[] mimePartData = GetEnvelopeItem(env, mimePartHeader, mimePartBody);

                                                    TextBodyPart tbPart = new TextBodyPart(MimePart.Parse(mimePartData));
                                                    msg.BodyParts.Plain.Text += tbPart.Text;
                                                    msg.BodyParts.Plain.Charset = tbPart.Charset;
                                                }
                                                if (((mode & MessageMode.HtmlBody) == 0) && 
                                                    (msg.BodyParts.Plain.Text.Length > 0))
                                                {
                                                    msg.BodyParts.Html.Text = " ";
                                                }
                                                else if (msg.BodyParts.Plain.Text.Length == 0)
                                                {
                                                    for (int i = 0; i < HtmlPartsResponse.Count; i += 2)
                                                    {
                                                        byte[] mimePartHeader = (byte[])env.GetEnvelopeItem(HtmlPartsResponse[i], true);
                                                        byte[] mimePartBody = (byte[])env.GetEnvelopeItem(HtmlPartsResponse[i + 1], true);
                                                        byte[] mimePartData = GetEnvelopeItem(env, mimePartHeader, mimePartBody);

                                                        TextBodyPart tbPart = new TextBodyPart(MimePart.Parse(mimePartData));
                                                        msg.BodyParts.Html.Text += tbPart.Text;
                                                        msg.BodyParts.Html.Charset = tbPart.Charset;
                                                    }
                                                }
                                            }

                                            for (int i = 0; i < AttachmentsHeaderPartsResponse.Count; i++)
                                            {
                                                byte[] mimePartHeader = (byte[])env.GetEnvelopeItem(AttachmentsHeaderPartsResponse[i], true);
                                                byte[] mimePartBody = new byte[AttachmentsPartsSize[i]];
                                                byte[] mimePartData = GetEnvelopeItem(env, mimePartHeader, mimePartBody);
                                                
                                                Attachment att = new Attachment(MimePart.Parse(mimePartData));
                                                att.Headers.Add("X-PartID", AttachmentsParts[i], true);
                                                msg.Attachments.Add(att);
                                            }
                                            if (msg.Charset == string.Empty)
                                            {
                                                if (msg.BodyParts.Plain.Charset != string.Empty)
                                                {
                                                    msg.Charset = msg.BodyParts.Plain.Charset;
                                                }
                                                else if (msg.BodyParts.Html.Charset != string.Empty)
                                                {
                                                    msg.Charset = msg.BodyParts.Html.Charset;
                                                }
                                            }
//                                            msg.Builder.Apply();
                                        }
                                    }
                                }
                                result.Init(msg, false, fld);
                            }
                        }
                        else
                        {
                            SystemMessageFlags oldFlags = GetSystemMessageFlags(lngUid);

                            result.Init(_imapObj.DownloadEntireMessage(lngUid, true), false, fld);

                            SystemMessageFlags newFlags = GetSystemMessageFlags(lngUid);

                            if ((newFlags & SystemMessageFlags.Seen) > 0 && (oldFlags & SystemMessageFlags.Seen) <= 0)
                            {
                                _imapObj.SetMessageFlags(lngUid.ToString(), true, SystemMessageFlags.Seen, MessageFlagAction.Remove);
                            }
                        }

                        result.Flags = GetSystemMessageFlags(lngUid);

                        return result;
					}
				}
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBoxException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_MSG_HAS_DELETED"));
            }
			return null;
		}

		public override WebMailMessageCollection LoadMessageHeaders(int pageNumber, Folder fld)
		{
			WebMailMessageCollection coll = null;
			if (fld != null)
			{
                try
                {
					if (ExamineImapFolder(fld.FullPath))
                    {
                        if (_imapObj.MessageCount > 0)
                        {
                            coll = LoadMessageHeaders(GetIndexSetFromPageNumber(pageNumber), false, fld);
                        }
                        else
                        {
                            coll = new WebMailMessageCollection();
                        }
                    }
                }
                catch (MailBeeException)
                {
                }
                finally
                {
                    if (fld.SyncType == FolderSyncType.DirectMode)
				    {
					    CollectionBase cb = coll as CollectionBase;
					    if (cb != null)
					    {
						    Utils.ReverseCollection(ref cb);
					    }
				    }
                }
				
			}
			return coll;
		}

		public override WebMailMessageCollection LoadMessageHeaders(Folder fld)
		{
			return LoadMessageHeaders(Imap.AllMessages, false, fld);
		}

		public override WebMailMessageCollection LoadMessageHeaders(object[] messageIndexSet, Folder fld)
		{
			return LoadMessageHeaders(messageIndexSet, fld);
		}

        public override WebMailMessageCollection LoadMessages(Folder fld)
        {
            return LoadMessages(Imap.AllMessages, fld);
        }

        public override WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld)
        {
            return LoadMessages(messageIndexSet, fld, false, MessageMode.None);
        }

        public override WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld, bool body_structure, MessageMode mode, XMLPacketMessagesBody[] messages)
        {
            if (messageIndexSet.Length > 0)
            {
                UidCollection messageIndexSetColl = CreateMessageIndexSet(messageIndexSet);

                return LoadMessages(messageIndexSetColl, fld, body_structure, mode, messages);
            }
            return null;
        }

        public override WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld, bool body_structure, MessageMode mode)
		{
			if (messageIndexSet.Length > 0)
			{
				UidCollection messageIndexSetColl = CreateMessageIndexSet(messageIndexSet);

                return LoadMessages(messageIndexSetColl, fld, body_structure, mode);
			}
			return null;
		}

        public override byte[] LoadAttachmentPart(object uid, Folder fld, string partID)
        {
            if (SelectImapFolder(fld.FullPath))
            {
                List<string> AllPartsRequest = new List<string>();
                List<string> AllPartsResponse = new List<string>();

                AllPartsRequest.Add("BODY.PEEK[" + partID + ".MIME]");
                AllPartsRequest.Add("BODY.PEEK[" + partID + "]");
                
                AllPartsResponse.Add("BODY[" + partID + ".MIME]");
                AllPartsResponse.Add("BODY[" + partID + "]");
                EnvelopeCollection envs = _imapObj.DownloadEnvelopes(uid.ToString(), true,
                    EnvelopeParts.Uid, 0, null, (string[])AllPartsRequest.ToArray());

                foreach (Envelope env in envs)
                {
                    if (env.IsValid)
                    {
                        // Get MIME part header data.
                        byte[] mimePartHeader = (byte[])env.GetEnvelopeItem(AllPartsResponse[0], true);

                        // Get MIME part body data.
                        byte[] mimePartBody = (byte[])env.GetEnvelopeItem(AllPartsResponse[1], true);

                        // Build MIME part data from header data and body data.
                       byte[] mimePartData = new byte[mimePartHeader.Length + mimePartBody.Length];
                       Buffer.BlockCopy(mimePartHeader, 0, mimePartData, 0, mimePartHeader.Length);
                       Buffer.BlockCopy(mimePartBody, 0, mimePartData, mimePartHeader.Length, mimePartBody.Length);
                       mimePartHeader = null;
                       mimePartBody = null;

                        Attachment att = new Attachment(MimePart.Parse(mimePartData));
                        return att.GetData();
                    }
                }
            }
            return null;
        }

		public override void MoveMessages(object[] messageIndexSet, Folder fromFolder, Folder toFolder)
		{
			if (messageIndexSet.Length > 0)
			{
				try
				{
					if (GetImapFolder(toFolder.FullPath) != null)
					{
						if (SelectImapFolder(fromFolder.FullPath))
						{
                            _imapObj.MoveMessages(CreateMessageIndexSet(messageIndexSet).ToString(), true, toFolder.FullPath);
						}
					}
				}
				catch (MailBeeException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailMailBeeException(ex);
				}
			}
		}

		public override void DeleteMessages(object[] messageIndexSet, Folder fld)
		{
			if (messageIndexSet.Length > 0)
			{
                string messageIndexSetString = CreateMessageIndexSet(messageIndexSet).ToString();
				try
				{
					if (SelectImapFolder(fld.FullPath))
					{
						OnMessageDeleted(new DeleteMessageEventArgs(-1, fld.FullPath));
						_imapObj.DeleteMessages(messageIndexSetString, true);
						_imapObj.Expunge();
					}
				}
				catch (MailBeeException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailMailBeeException(ex);
				}
			}
		}

		public override void SaveMessage(WebMailMessage message, Folder fld)
		{
			if (message.MailBeeMessage != null)
			{
				try
				{
					SystemMessageFlags flags = SystemMessageFlags.None;
					if (message.Seen) flags = SystemMessageFlags.Seen;
					_imapObj.UploadMessage(message.MailBeeMessage, fld.FullPath, flags);
				}
				catch (MailBeeException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailMailBeeException(ex);
				}
			}
		}

		public override void SaveMessages(WebMailMessageCollection messages, Folder fld)
		{
			foreach (WebMailMessage message in messages)
			{
				SaveMessage(message, fld);
			}
		}

		public override void SetMessagesFlags(object[] messageIndexSet, SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			if (messageIndexSet.Length > 0)
			{
                string messageIndexSetString = CreateMessageIndexSet(messageIndexSet).ToString();

				try
				{
					if (SelectImapFolder(fld.FullPath))
					{
						_imapObj.SetMessageFlags(messageIndexSetString, true, flags, flagsAction);
					}
				}
				catch (MailBeeException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailMailBeeException(ex);
				}
			}
		}

		public override void SetMessagesFlags(SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			try
			{
				if (SelectImapFolder(fld.FullPath))
				{
					_imapObj.SetMessageFlags(Imap.AllMessages, true, flags, flagsAction);
				}
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
		}


        public override Dictionary<long, string> Synchronize(FolderCollection foldersTree)
		{
			DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_account);

			FolderCollection folders = new FolderCollection();
			FolderCollection.CreateFolderListFromTree(ref folders, foldersTree);

			try
			{
				dbStorage.Connect();
				foreach (Folder fld in folders)
				{
					if ((fld.SyncType == FolderSyncType.DontSync) || 
                        (fld.SyncType == FolderSyncType.DirectMode) ||
						fld.Hide)
					{
                        if ((fld.SyncType == FolderSyncType.DirectMode) && 
                            (!_updatedFolders.ContainsKey(fld.ID)))
                        {
                            _updatedFolders.Add(fld.ID, fld.FullPath);
                        }
                        continue;
					}

					if (SelectImapFolder(fld.FullPath))
					{
						_folderName = Utils.GetLocalizedFolderNameByType(fld);
						bool synchronizeOldMsgs = true;
						if ((fld.SyncType == FolderSyncType.NewEntireMessages) || 
                            (fld.SyncType == FolderSyncType.NewHeadersOnly))
						{
							synchronizeOldMsgs = false;
						}

                        // download envelopes from the mail server and sort messages headers
                        EnvelopeCollection unsortedSrvMsgs = new EnvelopeCollection();
                        if (GetFolderMessageCount(fld.FullPath) > 0)
                        {
                            unsortedSrvMsgs = _imapObj.DownloadEnvelopes(Imap.AllMessages, false, EnvelopeParts.Flags | EnvelopeParts.Uid, 0);
                        }

                        ArrayList srvMsgsArrayList = new ArrayList();
						for (int i = 0; i < unsortedSrvMsgs.Count; i++)
						{
							srvMsgsArrayList.Add(unsortedSrvMsgs[i]);
						}
						Envelope[] srvMsgs = (Envelope[])srvMsgsArrayList.ToArray(typeof(Envelope));
						Array.Sort(srvMsgs, new EnvelopeComparer());
						
						// get sorted messages headers from database
						WebMailMessageCollection dbMsgs = dbStorage.LoadMessages(_account.ID, fld, synchronizeOldMsgs);

						if (srvMsgs.Length == 0)
						{
							if (dbMsgs.Count > 0 && synchronizeOldMsgs)
							{
								// delete all messages from a folder in database
								DeleteMessagesFromDb(dbStorage, fld, dbMsgs, 0, dbMsgs.Count - 1, true);
							}
							continue;
						}

						if (dbMsgs.Count == 0)
						{
							// download all messages from the mail server
							DownloadMessagesFromServer(dbStorage, fld, srvMsgs[0].Uid,
								srvMsgs[srvMsgs.Length - 1].Uid);
							continue;
						}

						int srvIndex = 0;
						int lastSrvIndex = srvMsgs.Length - 1;
						long srvUid = srvMsgs[srvIndex].Uid;
						long startUid = srvUid;
						long endUid = srvUid;

						int dbIndex = 0;
						int lastDbIndex = dbMsgs.Count - 1;
						long dbUid = dbMsgs[dbIndex].IntUid;

						bool msgsChunkDownloaded = true;
						/**
						 * pass on the server uids and database uids is based 
						 * on the principle of merging arrays. Both collections srvMsgs 
						 * and dbMsgs must be sorted in the field uid ascending.
						 */
						while (srvIndex < srvMsgs.Length || dbIndex < dbMsgs.Count)
						{
							if (srvUid > dbUid && dbIndex == dbMsgs.Count)
							{
								startUid = srvUid;
								endUid = srvMsgs[lastSrvIndex].Uid;
								// download messages from the mail server from startUid to endUid
								DownloadMessagesFromServer(dbStorage, fld, startUid, endUid);
								msgsChunkDownloaded = true;

								srvIndex = srvMsgs.Length;
							}
							else if (srvUid < dbUid)
							{
								endUid = srvUid;
								if (srvIndex == srvMsgs.Length)
								{
									if (!msgsChunkDownloaded)
									{
										// download messages from the mail server from startUid to endUid
										DownloadMessagesFromServer(dbStorage, fld, startUid, endUid);
										msgsChunkDownloaded = true;
									}

									// remove messages from the database from dbIndex to lastDbIndex
									DeleteMessagesFromDb(dbStorage, fld, dbMsgs, dbIndex, lastDbIndex,
										synchronizeOldMsgs);

									dbIndex = dbMsgs.Count;
								}
								else
								{
									if (msgsChunkDownloaded)
									{
										startUid = srvUid;
										msgsChunkDownloaded = false;
									}
									srvIndex++;
									if (srvIndex <= lastSrvIndex)
									{
										srvUid = srvMsgs[srvIndex].Uid;
									}
								}
							}
							else
							{
								if (!msgsChunkDownloaded)
								{
									// download messages from the mail server from startUid to endUid
									DownloadMessagesFromServer(dbStorage, fld, startUid, endUid);
									msgsChunkDownloaded = true;
								}
								if (srvUid == dbUid)
								{
									// synchronize messages flags from dbIndex to srvIndex
                                    bool synchronized = false;
                                    try
                                    {
                                        synchronized = SynchronizeFlags(dbStorage, dbMsgs[dbIndex], srvMsgs[srvIndex],
                                            synchronizeOldMsgs);
                                    }
                                    catch
                                    {
                                        synchronized = false;
                                    }
                                    if (synchronized && !_updatedFolders.ContainsKey(fld.ID))
                                    {
                                        _updatedFolders.Add(fld.ID, fld.FullPath);
                                    }
									srvIndex++;
									if (srvIndex <= lastSrvIndex)
									{
										srvUid = srvMsgs[srvIndex].Uid;
									}
									dbIndex++;
									if (dbIndex <= lastDbIndex)
									{
										dbUid = dbMsgs[dbIndex].IntUid;
									}
								}
								else
								{
									// remove message from the database with dbIndex
									DeleteMessagesFromDb(dbStorage, fld, dbMsgs, dbIndex, dbIndex,
										synchronizeOldMsgs);
									dbIndex++;
									if (dbIndex <= lastDbIndex)
									{
										dbUid = dbMsgs[dbIndex].IntUid;
									}
								}
							}
						}
					} // if (SelectImapFolder(fld.FullPath))
				} // foreach (Folder fld in folders)
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
			finally
			{
				dbStorage.Disconnect();
			}
            return _updatedFolders;
        }

		protected void DownloadMessagesFromServer(DbStorage dbStorage, Folder fld, long firstUid, long lastUid)
		{
			_msgsCount = (int)(lastUid - firstUid + 1);
			if (_msgsCount > _imapObj.MessageCount) _msgsCount = _imapObj.MessageCount;
			_msgNumber = 0;
			bool downloadAllMessage = true;
			if ((fld.SyncType == FolderSyncType.AllHeadersOnly) || 
                (fld.SyncType == FolderSyncType.NewHeadersOnly))
			{
				downloadAllMessage = false;
			}

			long startUid = firstUid;
			bool needToDownload = true;
			EnvelopeCollection srvMsgs = new EnvelopeCollection();
			do
			{
				long endUid = startUid + Constants.DownloadChunk;
				if (endUid > lastUid)
				{
					endUid = lastUid;
					needToDownload = false;
				}
				try
				{
					int bodyPreviewSize = (downloadAllMessage) ? -1 : 0;
					srvMsgs = DownloadEnvelopes(startUid, endUid, EnvelopeParts.MessagePreview | EnvelopeParts.Flags | EnvelopeParts.Uid | EnvelopeParts.Rfc822Size, bodyPreviewSize);
				}
				finally
				{
					WebMailMessageCollection msgs = new WebMailMessageCollection();
					foreach (Envelope env in srvMsgs)
					{
						WebMailMessage msg = new WebMailMessage(_account);
						msg.Init(env.MessagePreview, false, fld);
						msg.Flags = env.Flags.SystemFlags;
						msg.Downloaded = downloadAllMessage;
						msg.Size = (long)env.Size;
						msgs.Add(msg);
					}
					if (msgs.Count > 0)
					{
                        if (!_updatedFolders.ContainsKey(fld.ID))
                        {
                            _updatedFolders.Add(fld.ID, fld.FullPath);
                        }
                        ApplyXSpam(msgs);
					}
					if (msgs.Count > 0)
					{
						ArrayList arr = new ArrayList();
						ApplyFilters(msgs, dbStorage, fld, ref arr);
					}
				}
				startUid = endUid + 1;
			}
			while (needToDownload);
		}

		protected static bool SynchronizeFlags(DbStorage dbStorage, WebMailMessage dbMsg, Envelope srvMsg,
			bool synchronizeOldMsgs)
		{
			if (!synchronizeOldMsgs)
			{
				return false;
			}
			bool srvSeen = ((srvMsg.Flags.SystemFlags & SystemMessageFlags.Seen) > 0) ? true : false;
			bool srvFlagged = ((srvMsg.Flags.SystemFlags & SystemMessageFlags.Flagged) > 0) ? true : false;
			bool srvDeleted = ((srvMsg.Flags.SystemFlags & SystemMessageFlags.Deleted) > 0) ? true : false;
			bool srvReplied = ((srvMsg.Flags.SystemFlags & SystemMessageFlags.Answered) > 0) ? true : false;
			if (srvSeen != dbMsg.Seen || srvFlagged != dbMsg.Flagged || srvDeleted != dbMsg.Deleted || 
				srvReplied != dbMsg.Replied)
			{
				dbMsg.Flags = srvMsg.Flags.SystemFlags;
				dbStorage.UpdateMessage(dbMsg);
                return true;
			}
            return false;
		}

		protected void DeleteMessagesFromDb(DbStorage dbStorage, Folder fld, WebMailMessageCollection dbMsgs,
			int startIndex, int endIndex, bool synchronizeOldMsgs)
		{
			if (!synchronizeOldMsgs)
			{
				return;
			}
			int msgsCount = endIndex - startIndex + 1;
			object[] idsMsgs = new object[msgsCount];
			for (int i = 0; i < msgsCount; i++)
			{
				idsMsgs[i] = dbMsgs[i + startIndex].IDMsg;
			}
			dbStorage.DeleteMessages(idsMsgs, fld);
            if (msgsCount > 0 && !_updatedFolders.ContainsKey(fld.ID))
            {
                _updatedFolders.Add(fld.ID, fld.FullPath);
            }
        }

        public static UidCollection CreateMessageIndexSet(object[] messageIndexSet)
		{
			UidCollection uc = new UidCollection();
			try
			{
				foreach (object s in messageIndexSet)
				{
					uc.Add(Convert.ToInt64(s));
				}
			}
			catch (InvalidCastException ex)
			{
                Log.WriteException(ex);
                throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("InvalidUid"));
			}
			return uc;
		}

		private EnvelopeCollection DownloadEnvelopes(long startUid, long endUid, EnvelopeParts parts, int bodyPreviewSize)
		{
			_imapObj.EnableLastDownloaded = true;
			EnvelopeCollection ec = new EnvelopeCollection();
			bool finished = false;
			string endUidStr = (endUid > 0) ? endUid.ToString() : "*";

			do
			{
				try
				{
					_imapObj.DownloadEnvelopes(string.Format("{0}:{1}", startUid, endUidStr), true, parts, bodyPreviewSize);
					ec.Add(_imapObj.LastDownloadedEnvelopes);
					finished = true;
				}
				catch (MailBeeException ex)
				{
                    Log.WriteException(ex);
                    if ((_imapObj.LastDownloadedEnvelopes == null) || 
                        (_imapObj.LastDownloadedEnvelopes.Count == 0))
					{
						throw new WebMailMailBeeException(ex);
					}
					ec.Add(_imapObj.LastDownloadedEnvelopes);
					startUid = _imapObj.LastDownloadedEnvelopes[_imapObj.LastDownloadedEnvelopes.Count - 1].Uid + 1;
				}
			}
			while (!finished);

			_imapObj.EnableLastDownloaded = false;
			return ec;
		}

		private WebMailMessageCollection LoadMessages(string messageIndexSet, Folder fld)
		{
			WebMailMessageCollection returnColl = new WebMailMessageCollection();
			EnvelopeCollection envColl = null;
			try
			{
				if (fld != null)
				{
					if (SelectImapFolder(fld.FullPath))
					{
                        envColl = _imapObj.DownloadEnvelopes(messageIndexSet, true, EnvelopeParts.All, -1);
					}
				}
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
			if (envColl != null)
			{
				foreach (Envelope env in envColl)
				{
					WebMailMessage webMsg = new WebMailMessage(_account);
					webMsg.Init(env.MessagePreview, false, fld);
					webMsg.Flags = env.Flags.SystemFlags;
					returnColl.Add(webMsg);
				}
			}
			return returnColl;
		}

        private WebMailMessageCollection LoadMessages(UidCollection messageIndexSet, Folder fld, bool body_structure, MessageMode mode, XMLPacketMessagesBody[] messages)
        {
            WebMailMessageCollection returnColl = new WebMailMessageCollection();
            try
            {
                if (fld != null)
                {
                    if (SelectImapFolder(fld.FullPath))
                    {
                        foreach (XMLPacketMessagesBody message in messages)
                        {
                            
                            bool _body_structure = (body_structure && message.size > Constants.IMAP_OPT_MAIL_SIZE) ? true : false;
                            WebMailMessage _msg = LoadMessage(message.uid, fld, _body_structure, mode);
                            if (_msg != null)
                            {
                                returnColl.Add(_msg);
                            }
                        }
                    }
                }
            }
            catch (MailBeeException ex)
            {
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
            }

            return returnColl;
        }

        private WebMailMessageCollection LoadMessages(UidCollection messageIndexSet, Folder fld, bool body_structure, MessageMode mode)
        {
            WebMailMessageCollection returnColl = new WebMailMessageCollection();
            try
            {
                if (fld != null)
                {
                    if (SelectImapFolder(fld.FullPath))
                    {
                        foreach (object _uid in messageIndexSet)
                        {
                            WebMailMessage _msg = LoadMessage(_uid, fld, body_structure, mode);
                            if (_msg != null)
                            {
                                returnColl.Add(_msg);
                            }
                        }
                    }
                }
            }
            catch (MailBeeException ex)
            {
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
            }
           
            return returnColl;
        }

        public WebMailMessageCollection SearchMessages(int page, string condition, Folder fld,
            bool inHeadersOnly, out int searchMessagesCount)
        {
            UidCollection uidsForLoad = new UidCollection();
            UidCollection uids = SearchMessagesUids(condition, fld, inHeadersOnly);
            searchMessagesCount = uids.Count;
            if (uids.Count > 0)
            {
                int msgsOnPage = Account.UserOfAccount.Settings.MsgsPerPage;
                int firstUidIndex = (page - 1) * msgsOnPage;
                int lastUidIndex = page * msgsOnPage - 1;
                if (lastUidIndex >= uids.Count) lastUidIndex = uids.Count - 1;
                if (firstUidIndex <= lastUidIndex)
                {
                    for (int i = firstUidIndex; i <= lastUidIndex; i++)
                    {
                        uidsForLoad.Add(uids[i]);
                    }
                }
            }
            if (uidsForLoad.Count > 0)
            {
                return LoadMessages(uidsForLoad.ToString(), fld);
            }
            return new WebMailMessageCollection();
        }

        public UidCollection SearchMessagesUids(string condition, Folder fld, bool inHeadersOnly)
        {
            UidCollection uids = new UidCollection();
            try
            {
                if (fld != null)
                {
                    if (ExamineImapFolder(fld.FullPath))
                    {
                        string quotedCond = ImapUtils.ToQuotedString(condition);
                        quotedCond = System.Text.Encoding.Default.GetString(System.Text.Encoding.UTF8.GetBytes(quotedCond));

                        if (inHeadersOnly)
                        {
                            uids = (UidCollection)_imapObj.Search(true,
                                "OR (OR FROM " + quotedCond + " TO " + quotedCond + ") SUBJECT " + quotedCond,
                                System.Text.Encoding.UTF8.WebName.ToUpper());
                        }
                        else
                        {
                            uids = (UidCollection)_imapObj.Search(true,
                                "OR (OR (OR FROM " + quotedCond + " TO " + quotedCond + ") SUBJECT " + quotedCond + ") BODY " + quotedCond,
                                System.Text.Encoding.UTF8.WebName.ToUpper());
                        }
                    }
                }
            }
            catch (MailBeeException ex)
            {
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
            }
            return uids;
        }
        
        private WebMailMessageCollection LoadMessageHeaders(string messageIndexSet, bool indexAsUid, Folder fld)
		{
			WebMailMessageCollection returnColl = new WebMailMessageCollection();
			EnvelopeCollection envColl = null;
			try
			{
				if (fld != null)
				{
					if (ExamineImapFolder(fld.FullPath))
					{
                        envColl = _imapObj.DownloadEnvelopes(messageIndexSet, indexAsUid, EnvelopeParts.MessagePreview | EnvelopeParts.Flags | EnvelopeParts.Uid | EnvelopeParts.Rfc822Size | EnvelopeParts.BodyStructure, 0);
					}
				}
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
			if (envColl != null)
			{
				foreach (Envelope env in envColl)
				{
					WebMailMessage webMsg = new WebMailMessage(_account);
					webMsg.Init(env.MessagePreview, false, fld);
					webMsg.Flags = env.Flags.SystemFlags;
					returnColl.Add(webMsg);
				}
			}
			return returnColl;
		}

		private FolderStatus GetFolderStatus(string fullPath)
		{
            try
            {
                return _imapObj.GetFolderStatus(fullPath);
            }
            catch (MailBeeException ex)
            {
                Log.WriteException(ex);
                return null;
            }
		}

		private string GetIndexSetFromPageNumber(int pageNumber)
		{
			int msgPerPage = _account.UserOfAccount.Settings.MsgsPerPage;
			int msgCount = _imapObj.MessageCount;
			int startIndex = msgCount - (pageNumber * msgPerPage);
			if (startIndex < 0)
			{
				startIndex = 0;
			}
			startIndex++;
            int endIndex = msgCount - ((pageNumber - 1) * msgPerPage);
            if (endIndex > msgCount)
            {
                endIndex = msgCount;
            }
            string result = string.Format("{0}:{1}", startIndex, endIndex);
			return result;
		}

		private MailBee.ImapMail.FolderCollection DownloadImapFolders()
		{
			if (_imapFolders == null)
			{
				_imapFolders = _imapObj.DownloadFolders();
			}
			return _imapFolders;
		}

		private MailBee.ImapMail.FolderCollection DownloadSubscribedImapFolders()
		{
			if (_imapFolders == null)
			{
				_subscribedImapFolders = _imapObj.DownloadFolders(true);
			}
			return _subscribedImapFolders;
		}

		private MailBee.ImapMail.Folder GetImapFolder(string folderName)
		{
			MailBee.ImapMail.FolderCollection fc = DownloadImapFolders();
			foreach (MailBee.ImapMail.Folder f in fc)
			{
				if (string.Compare(f.Name, folderName, false, CultureInfo.InvariantCulture) == 0)
				{
					return f;
				}
			}
			return null;
		}

		private bool CanSelectFolder(string folderName)
		{
            MailBee.ImapMail.Folder fld = GetImapFolder(folderName);
			if (fld != null)
			{
				if ((fld.Flags & FolderFlags.Noselect) > 0)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		private bool SelectImapFolder(string folderName)
		{
			try
			{
				_imapObj.SelectFolder(folderName);
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                if (!CanSelectFolder(folderName))
				{
					return false;
				}
				throw;
			}
			return true;
		}

        private bool ExamineImapFolder(string folderName)
		{
			try
			{
                _imapObj.ExamineFolder(folderName);
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                if (!CanSelectFolder(folderName))
				{
					return false;
				}
				throw;
			}
			return true;
		}

        public string GetNamespace()
        {
            string result = string.Empty;
            try
            {
                if (_imapObj.GetExtension("NAMESPACE") != null)
                {
                    ImapNamespaceCollectionSet namespaceSet = _imapObj.GetNamespaces();
                    foreach (ImapNamespace ns in namespaceSet.Personal)
                    {
                        if (ns != null)
                        {
                            result = ns.Prefix;
                        }
                    }
                }
            }
            catch (MailBeeException ex)
            {
                Log.WriteException(ex);
            }
            return result;
        }

        private MailBee.ImapMail.SystemMessageFlags GetSystemMessageFlags(long lngUid)
        {
            MailBee.ImapMail.SystemMessageFlags _Flags = MailBee.ImapMail.SystemMessageFlags.None;
            EnvelopeCollection _envs = _imapObj.DownloadEnvelopes(lngUid.ToString(), true,
                EnvelopeParts.Flags, 0, null, null);

            if (_envs[0] != null && _envs[0].IsValid)
            {
                _Flags = _envs[0].Flags.SystemFlags;
            }

            return _Flags;
        }


		private void _imapObj_EnvelopeDownloaded(object sender, ImapEnvelopeDownloadedEventArgs e)
		{
			_msgNumber++;// = e.MessageNumber;
			if ((e != null) && (e.DownloadedEnvelope != null))
			{
				if (e.DownloadedEnvelope.MessagePreview != null)
				{
					OnMessageDownloaded(new CheckMailEventArgs(_folderName, _msgsCount, _msgNumber));
				}
			}
		}
	}
}
