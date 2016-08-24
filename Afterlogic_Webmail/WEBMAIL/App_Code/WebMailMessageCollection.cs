using System;
using System.Collections;
using MailBee.Mime;

namespace WebMail
{
	/// <summary>
	/// Summary description for WebMailMessageCollection.
	/// </summary>
	public class WebMailMessageCollection : CollectionBase
	{
		public WebMailMessageCollection()
		{
		}

		public WebMailMessageCollection(Account acct, MailMessageCollection messageCollection, bool isUidStr, Folder fld)
		{
			foreach (MailMessage msg in messageCollection)
			{
				WebMailMessage webMsg = new WebMailMessage(acct);
				webMsg.Init(msg, isUidStr, fld);
				Add(webMsg);
			}
		}

		public WebMailMessage this[ int index ]  
		{
			get { return( (WebMailMessage) List[index] ); }
			set { List[index] = value; }
		}

		public int Add(WebMailMessage value)
		{
			return( List.Add( value ) );
		}

		public int IndexOf(WebMailMessage value )
		{
			return( List.IndexOf( value ) );
		}

		public void Insert(int index, WebMailMessage value )
		{
			List.Insert(index, value );
		}

		public void Remove(WebMailMessage value )
		{
			List.Remove(value );
		}

		public bool Contains(WebMailMessage value )
		{
			// If value is not of type WebMailMessage, this will return false.
			return( List.Contains(value ) );
		}

		public object[] ToUidsCollection(bool isStrUid)
		{
			object[] result = new object[List.Count];
			for (int i = 0; i < List.Count; i++)
			{
				WebMailMessage msg = List[i] as WebMailMessage;
				if (msg != null)
				{
					object temp = (isStrUid) ? (object)msg.StrUid : (object)msg.IntUid;
					if (temp.ToString() != "-1")
					{
						result[i] = temp;
					}
				}
			}
			return result;
		}

		public object[] ToIDsCollection()
		{
			object[] result = new object[List.Count];
			for (int i = 0; i < List.Count; i++)
			{
				WebMailMessage msg = List[i] as WebMailMessage;
				if (msg != null)
				{
					result[i] = msg.IDMsg;
				}
			}
			return result;
		}

		protected override void OnInsert(int index, Object value )
		{
			// Insert additional code to be run only when inserting values.
		}

		protected override void OnRemove(int index, Object value )
		{
			// Insert additional code to be run only when removing values.
		}

		protected override void OnSet(int index, Object oldValue, Object newValue )
		{
			// Insert additional code to be run only when setting values.
		}

		protected override void OnValidate(Object value )
		{
			if ( value.GetType() != typeof(WebMailMessage) )
				throw new ArgumentException("value must be of type WebMailMessage.", "value" );
		}

	}
}
