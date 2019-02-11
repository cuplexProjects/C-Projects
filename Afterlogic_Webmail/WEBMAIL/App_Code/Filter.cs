using System.Globalization;
using MailBee.Mime;

namespace WebMail
{
	/// <summary>
    /// This class provides properties and methods for managing users filters
	/// </summary>
	public class Filter
	{
		private int _id_filter;
		private int _id_acct;
		private FilterField _field;
		private FilterCondition _condition;
		private string _filter;
		private FilterAction _action;
		private long _id_folder;
		private bool _applied;

		private Account _acct;

		public int IDFilter
		{
			get { return _id_filter; }
			set { _id_filter = value; }
		}

		public int IDAcct
		{
			get { return _id_acct; }
			set { _id_acct = value; }
		}

		public FilterField Field
		{
			get { return _field; }
			set { _field = value; }
		}

		public FilterCondition Condition
		{
			get { return _condition; }
			set { _condition = value; }
		}

		public string FilterStr
		{
			get { return _filter; }
			set { _filter = value; }
		}

		public FilterAction Action
		{
			get { return _action; }
			set { _action = value; }
		}

		public long IDFolder
		{
			get { return _id_folder; }
			set { _id_folder = value; }
		}

		public bool Applied
		{
			get { return _applied; }
			set { _applied = value; }
		}

		public Filter(Account acct)
		{
			_id_filter = -1;
			_id_acct = -1;
			_field = FilterField.From;
			_condition = FilterCondition.ContainSubstring;
			_filter = string.Empty;
			_action = FilterAction.DoNothing;
			_id_folder = -1;
			_applied = true;
			_acct = acct;
		}

		public Filter(int id_filter, int id_acct, FilterField field, FilterCondition condition, string filter,
			FilterAction action, long id_folder, bool applied)
		{
			_id_filter = id_filter;
			_id_acct = id_acct;
			_field = field;
			_condition = condition;
			_filter = filter;
			_action = action;
			_id_folder = id_folder;
			_applied = applied;
		}

		public FilterAction GetActionToApply(WebMailMessage msg)
		{
			string str = null;
			switch (_field)
			{
				case FilterField.From:
					{
						str = msg.FromMsg.ToString();
					}
					break;
				case FilterField.To:
					{
						str = msg.ToMsg.ToString();
					}
					break;
				case FilterField.Subject:
					{
						str = msg.Subject;
					}
					break;
				case FilterField.XSpamHeader:
					{
						HeaderCollection spamHeaders = msg.MailBeeMessage.Headers.Items("X-Spam-Header");
						if (spamHeaders != null && spamHeaders.Count > 0)
						{
							str = spamHeaders[0].Value;
						}
					}
					break;
                case FilterField.XVirusHeader:
                    {
                        HeaderCollection virusHeaders = msg.MailBeeMessage.Headers.Items("X-Virus-Header");
                        if (virusHeaders != null && virusHeaders.Count > 0)
                        {
                            str = virusHeaders[0].Value;
                        }
                    }
                    break;
            }
			if (str != null)
			{
				return ProcessMessage(str);
			}
			return FilterAction.DoNothing;
		}

		private FilterAction ProcessMessage(string str)
		{
			FilterAction action = FilterAction.DoNothing;
			bool needToProcess = false;
			switch (_condition)
			{
				case FilterCondition.ContainSubstring:
					if (str.IndexOf(_filter) >= 0)
					{
						needToProcess = true;
					}
					break;
				case FilterCondition.ContainExactPhrase:
					if (string.Compare(str, _filter, false, CultureInfo.InvariantCulture) == 0)
					{
						needToProcess = true;
					}
					break;
				case FilterCondition.NotContainSubstring:
					if (str.IndexOf(_filter) < 0)
					{
						needToProcess = true;
					}
					break;
				case FilterCondition.BeginsWithSubstring:
                    if (str.Substring(0, 4).ToLower() == _filter || str.Substring(0, 8).ToLower() == _filter)
					{
						needToProcess = true;
					}
					break;
			}
			if (needToProcess)
			{
				switch (_action)
				{
					case FilterAction.DeleteFromServerImmediately:
						action = FilterAction.DeleteFromServerImmediately;
						break;
					case FilterAction.MarkGrey:
						action = FilterAction.MarkGrey;
						break;
					case FilterAction.MoveToFolder:
						action = FilterAction.MoveToFolder;
						break;
				}
			}
			return action;
		}
	}
}
