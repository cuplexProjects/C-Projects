namespace WebMail
{
	/// <summary>
	/// Class: MSSQL Server Storage.
	/// </summary>
	public class MsSqlStorage : DbStorage
	{
		public MsSqlStorage(Account account) : base(account) {}
	}

	public class MySqlStorage : DbStorage
	{
		public MySqlStorage(Account account) : base(account) {}
	}

	public class MsAccessStorage : DbStorage
	{
		public MsAccessStorage(Account account) : base(account) {}
	}

    public class PostgreSqlStorage : DbStorage
    {
        public PostgreSqlStorage(Account account) : base(account) { }
    }
}
