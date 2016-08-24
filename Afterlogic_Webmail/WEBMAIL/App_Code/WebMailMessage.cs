using System;
using System.Text;
using MailBee;
using MailBee.ImapMail;
using MailBee.Mime;

namespace WebMail
{
	/// <summary>
	/// Summary description for WebMailMessage.
	/// </summary>
	public class WebMailMessage
	{
		#region Fields
        private Account _account = null;
		protected MailMessage _message = null;
		protected long _id = -1;
		protected int _id_msg = -1;
		protected int _id_acct = -1;
		protected long _id_folder_srv = -1;
		protected long _id_folder_db = -1;
		protected string _str_uid = string.Empty;
		protected long _int_uid = -1;
		protected EmailAddress _from_msg = null;
		protected EmailAddressCollection _to_msg = null;
		protected EmailAddressCollection _cc_msg = null;
		protected EmailAddressCollection _bcc_msg = null;
		protected string _subject = string.Empty;
		protected DateTime _msg_date = Constants.MinDate;
		protected bool _attachments = false;
		protected long _size = 0;
		protected bool _seen = false;
		protected bool _flagged = false;
		protected MailPriority _priority = MailPriority.None;
        protected WebMailSensitivity _sensitivity = WebMailSensitivity.None;
        protected bool _downloaded = false;
		protected bool _x_spam = false;
        protected bool _x_virus = false;
        protected bool _rtl = false;
		protected bool _deleted = false;
		protected bool _is_full = false;
		protected bool _replied = false;
		protected bool _forwarded = false;
		protected SystemMessageFlags _flags = SystemMessageFlags.None;
		protected string _body_text = string.Empty;
		protected bool _grayed = false;
		protected int _charset = -1;
		protected byte[] _rawMsg = null;
		protected byte _safety = 0;

		protected string _folder_full_name = string.Empty;

		private string _dataFolder = string.Empty;

		#endregion

		#region Properties

		public long ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public int IDMsg
		{
			get { return _id_msg; }
			set { _id_msg = value; }
		}

		public MailMessage MailBeeMessage
		{
			get { return _message; }
			set { _message = value; }
		}

		public int IDAcct
		{
			get { return _id_acct; }
			set { _id_acct = value; }
		}

		public long IDFolderSrv
		{
			get { return _id_folder_srv; }
			set { _id_folder_srv = value; }
		}

		public long IDFolderDB
		{
			get { return _id_folder_db; }
			set { _id_folder_db = value; }
		}

		public string StrUid
		{
			get { return _str_uid; }
			set { _str_uid = value; }
		}

		public long IntUid
		{
			get { return _int_uid; }
			set { _int_uid = value; }
		}

		public EmailAddress FromMsg
		{
			get { return _from_msg; }
			set { _from_msg = value; }
		}

		public EmailAddressCollection ToMsg
		{
			get { return _to_msg; }
			set { _to_msg = value; }
		}

		public EmailAddressCollection CcMsg
		{
			get { return _cc_msg; }
			set { _cc_msg = value; }
		}

		public EmailAddressCollection BccMsg
		{
			get { return _bcc_msg; }
			set { _bcc_msg = value; }
		}

		public string Subject
		{
			get { return _subject; }
			set { _subject = value; }
		}

		public DateTime MsgDate
		{
			get { return _msg_date; }
			set { _msg_date = value; }
		}

		public bool Attachments
		{
			get { return _attachments; }
			set { _attachments = value; }
		}

		public long Size
		{
			get { return _size; }
			set { _size = value; }
		}

		public bool Seen
		{
			get { return _seen; }
			set
			{
				_seen = value;
			}
		}

		public bool Flagged
		{
			get { return _flagged; }
			set { _flagged = value; }
		}

		public MailPriority Priority
		{
			get { return _priority; }
			set { _priority = value; }
		}

        public WebMailSensitivity Sensitivity
        {
            get { return _sensitivity; }
            set { _sensitivity = value; }
        }
        
        public bool Downloaded
		{
			get { return _downloaded; }
			set {
				_downloaded = value;
			}
		}

		public bool XSpam
		{
			get { return _x_spam; }
			set { _x_spam = value; }
		}

        public bool XVirus
        {
            get { return _x_virus; }
            set { _x_virus = value; }
        }
        
        public bool Rtl
		{
			get { return _rtl; }
			set { _rtl = value; }
		}

		public bool Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public bool IsFull
		{
			get { return _is_full; }
			set { _is_full = value; }
		}

		public bool Replied
		{
			get { return _replied; }
			set { _replied = value; }
		}

		public bool Forwarded
		{
			get { return _forwarded; }
			set { _forwarded = value; }
		}

		public SystemMessageFlags Flags
		{
			get { return _flags; }
			set 
            { 
                _flags = value;
				if (_account.MailIncomingProtocol != IncomingMailProtocol.Pop3)
				{
					_seen = ((value & SystemMessageFlags.Seen) > 0) ? true : false;
					_flagged = ((value & SystemMessageFlags.Flagged) > 0) ? true : false;
					_deleted = ((value & SystemMessageFlags.Deleted) > 0) ? true : false;
					_replied = ((value & SystemMessageFlags.Answered) > 0) ? true : false;
				}
            }
		}

		public string BodyText
		{
			get { return _body_text; }
			set { _body_text = value; }
		}

		public string FolderFullName
		{
			get { return _folder_full_name; }
			set { _folder_full_name = value; }
		}

		public bool Grayed
		{
			get { return _grayed; }
			set { _grayed = value; }
		}

		public int OverrideCharset
		{
			get { return _charset; }
			set { _charset = value; }
		}

		public byte[] RawMsg
		{
			get { return _rawMsg; }
		}
		
		public byte Safety
		{
			get{
				return _safety;
			}
			set{
				_safety = value;
			}
		}

        public Account Account
        {
            get { return _account; }
            set { _account = value; }
        }
		#endregion

		public WebMailMessage(Account acct, string dataFolder) : this(acct)
		{
			_dataFolder = dataFolder;
		}

		public WebMailMessage(Account acct)
		{
			_account = acct;
			_id_acct = acct.ID;
		}

		public void Init(MailMessage msg, bool isUidStr, Folder fld)
		{
			if (isUidStr)
			{
				string uid = msg.UidOnServer as string;
				if (uid != null) _str_uid = uid;
			}
			else
			{
				if ((msg.UidOnServer != null) && ((Convert.ToInt64(msg.UidOnServer) != -1)))
				{
					_int_uid = Convert.ToInt64(msg.UidOnServer);
				}
			}
			if (fld != null)
			{
				_id_folder_db = fld.ID;
				_folder_full_name = fld.FullPath;
			}
			InitFromMailMessage(msg);
			if ((_account.MailIncomingProtocol == IncomingMailProtocol.WMServer && fld == null) || 
				(fld != null && (fld.SyncType == FolderSyncType.AllHeadersOnly || fld.SyncType == FolderSyncType.NewHeadersOnly)))
            {
                _downloaded = false;
            }
			else if (fld != null && fld.SyncType != FolderSyncType.DontSync)
			{
				_downloaded = msg.IsEntire;
			}
			else
			{
				_downloaded = true;
			}
		}

		private void InitFromMailMessage(MailMessage msg)
		{
            msg.Parser.DatesAsUtc = true;
            if ((_account != null) && (_account.UserOfAccount != null) && (_account.UserOfAccount.Settings != null))
			{
                if (string.IsNullOrEmpty(msg.Charset) && (!string.IsNullOrEmpty(msg.BodyHtmlText) || !string.IsNullOrEmpty(msg.BodyPlainText)))
				{
					Encoding defEnc = Encoding.Default;
					try
					{
						if (_dataFolder != string.Empty)
						{
							defEnc = Utils.GetIncCharset(_account.UserOfAccount.Settings, _dataFolder);
						}
						else
						{
							defEnc = Utils.GetIncCharset(_account.UserOfAccount.Settings);
						}
					}
					catch {}
                    msg.Parser.EncodingDefault = defEnc;
					if (_charset > 0)
					{
                        msg.Parser.EncodingOverride = Utils.GetEncodingByCodePage(_charset);
					}
                    msg.Parser.Apply();
				}
			}

            DateTime _dateReceivedUtc = new DateTime(msg.DateReceived.Ticks);
            DateTime _dateUtc = new DateTime(msg.Date.Ticks);

            _size = (msg.SizeOnServer > 0) ? msg.SizeOnServer : msg.Size;
            _rawMsg = msg.GetMessageRawData();
            _from_msg = msg.From;
            _to_msg = msg.To;
            _cc_msg = msg.Cc;
            _bcc_msg = msg.Bcc;
            _subject = msg.Subject;

			DateTime dt;
			try
			{
                dt = (_dateReceivedUtc != DateTime.MinValue) ? _dateReceivedUtc : _dateUtc;
			}
			catch (MailBeeDateParsingException)
			{
				dt = DateTime.Now;
			}
			_msg_date = (dt < Constants.MinDate) ? Constants.MinDate : dt;
            _attachments = msg.HasAttachments;
            _priority = msg.Priority;
            _sensitivity = Utils.ToWebMailSensitivity(msg.Sensitivity);

            _body_text = msg.BodyPlainText.ToLower();
			_message = msg;
		}
	}
}
