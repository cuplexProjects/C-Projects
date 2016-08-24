namespace WebMail
{
	public class TempRow
	{
		private long _id_temp;
		private int _id_acct;
		private string _data_val;

		public long ID
		{
			get { return _id_temp; }
			set { _id_temp = value; }
		}

		public int IDAcct
		{
			get { return _id_acct; }
			set { _id_acct = value; }
		}

		public string DataVal
		{
			get { return _data_val; }
			set { _data_val = value; }
		}

		public TempRow() { }

		public TempRow(long id_temp, int id_acct, string data_val)
		{
			_id_temp = id_temp;
			_id_acct = id_acct;
			_data_val = data_val;
		}
	}
}
