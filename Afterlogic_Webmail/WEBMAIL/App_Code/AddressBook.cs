using System;
namespace WebMail
{
	public struct AddressBookGroupContact
	{
		public long id;
		public string fullname;
		public string email;
		public bool isGroup;
	}

	/// <summary>
    /// This class provides properties and methods for managing Contacts in Address book 
	/// </summary>
	public class AddressBookContact
	{
		#region Fields

		private long _id_addr = 0;
		private int _id_user = 0;
		private string _email = string.Empty;
		private string _full_nm = string.Empty;
		private string _notes = string.Empty;
		private bool _use_friendly_nm = false;
		private string _h_street = string.Empty;
		private string _h_city = string.Empty;
		private string _h_state = string.Empty;
		private string _h_zip = string.Empty;
		private string _h_country = string.Empty;
		private string _h_phone = string.Empty;
		private string _h_fax = string.Empty;
		private string _h_mobile = string.Empty;
		private string _h_web = string.Empty;
		private string _b_email = string.Empty;
		private string _b_company = string.Empty;
		private string _b_street = string.Empty;
		private string _b_city = string.Empty;
		private string _b_state = string.Empty;
		private string _b_zip = string.Empty;
		private string _b_country = string.Empty;
		private string _b_job_title = string.Empty;
		private string _b_department = string.Empty;
		private string _b_office = string.Empty;
		private string _b_phone = string.Empty;
		private string _b_fax = string.Empty;
		private string _b_web = string.Empty;
		private byte _birthday_day = 0;
		private byte _birthday_month = 0;
		private short _birthday_year = 0;
		private string _other_email = string.Empty;
		private ContactPrimaryEmail _primary_email = ContactPrimaryEmail.Personal;
		private long _id_addr_prev = 0;
		private bool _tmp = false;
		private int _use_frequency = 0;
		private bool _auto_create = false;
		private string _str_id = string.Empty;
		private DateTime _date_modified = Constants.MinDate;

		private AddressBookGroup[] _groups = new AddressBookGroup[0];
		#endregion

		#region Properties

		public long IDAddr
		{
			get { return _id_addr; }
			set { _id_addr = value; }
		}

		public int IDUser
		{
			get { return _id_user; }
			set { _id_user = value; }
		}

		public string HEmail
		{
			get { return _email; }
			set { _email = value; }
		}

		public string FullName
		{
			get { return _full_nm; }
			set { _full_nm = value; }
		}

		public string Notes
		{
			get { return _notes; }
			set { _notes = value; }
		}

		public bool UseFriendlyName
		{
			get { return _use_friendly_nm; }
			set { _use_friendly_nm = value; }
		}

		public string HStreet
		{
			get { return _h_street; }
			set { _h_street = value; }
		}

		public string HCity
		{
			get { return _h_city; }
			set { _h_city = value; }
		}

		public string HState
		{
			get { return _h_state; }
			set { _h_state = value; }
		}

		public string HZip
		{
			get { return _h_zip; }
			set { _h_zip = value; }
		}

		public string HCountry
		{
			get { return _h_country; }
			set { _h_country = value; }
		}

		public string HPhone
		{
			get { return _h_phone; }
			set { _h_phone = value; }
		}

		public string HFax
		{
			get { return _h_fax; }
			set { _h_fax = value; }
		}

		public string HMobile
		{
			get { return _h_mobile; }
			set { _h_mobile = value; }
		}

		public string HWeb
		{
			get { return _h_web; }
			set { _h_web = value; }
		}

		public string BEmail
		{
			get { return _b_email; }
			set { _b_email = value; }
		}

		public string BCompany
		{
			get { return _b_company; }
			set { _b_company = value; }
		}

		public string BStreet
		{
			get { return _b_street; }
			set { _b_street = value; }
		}

		public string BCity
		{
			get { return _b_city; }
			set { _b_city = value; }
		}

		public string BState
		{
			get { return _b_state; }
			set { _b_state = value; }
		}

		public string BZip
		{
			get { return _b_zip; }
			set { _b_zip = value; }
		}

		public string BCountry
		{
			get { return _b_country; }
			set { _b_country = value; }
		}

		public string BJobTitle
		{
			get { return _b_job_title; }
			set { _b_job_title = value; }
		}

		public string BDepartment
		{
			get { return _b_department; }
			set { _b_department = value; }
		}

		public string BOffice
		{
			get { return _b_office; }
			set { _b_office = value; }
		}

		public string BPhone
		{
			get { return _b_phone; }
			set { _b_phone = value; }
		}

		public string BFax
		{
			get { return _b_fax; }
			set { _b_fax = value; }
		}

		public string BWeb
		{
			get { return _b_web; }
			set { _b_web = value; }
		}

		public byte BirthdayDay
		{
			get { return _birthday_day; }
			set { _birthday_day = value; }
		}

		public byte BirthdayMonth
		{
			get { return _birthday_month; }
			set { _birthday_month = value; }
		}

		public short BirthdayYear
		{
			get { return _birthday_year; }
			set { _birthday_year = value; }
		}

		public string OtherEmail
		{
			get { return _other_email; }
			set { _other_email = value; }
		}

		public ContactPrimaryEmail PrimaryEmail
		{
			get { return _primary_email; }
			set { _primary_email = value; }
		}

		public long IDAddrPrev
		{
			get { return _id_addr_prev; }
			set { _id_addr_prev = value; }
		}

		public bool Tmp
		{
			get { return _tmp; }
			set { _tmp = value; }
		}

		public AddressBookGroup[] Groups
		{
			get { return _groups; }
			set { _groups = value; }
		}

		public int UseFrequency
		{
			get { return _use_frequency; }
			set { _use_frequency = value; }
		}

		public bool AutoCreate
		{
			get { return _auto_create; }
			set { _auto_create = value; }
		}

		public string StrID
		{
			get { return _str_id; }
			set { _str_id = value; }
		}

		public DateTime DateModified
		{
			get { return _date_modified; }
			set { _date_modified = value; }
		}

		#endregion

		public AddressBookContact() {}

		public static AddressBookContact CreateContact(AddressBookContact contactToCreate)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				contactToCreate = dbMan.CreateAddressBookContact(contactToCreate);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return contactToCreate;
		}

		public static void DeleteContact(AddressBookContact contact)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.DeleteAddressBookContacts(new AddressBookContact[] {contact});
			}
			finally
			{
				dbMan.Disconnect();
			}
		}

		public void Update()
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.UpdateAddressBookContact(this);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}

        public bool IsOpen()
        {
            string Str = _b_city + _b_company + _b_country + _b_department + _b_fax + _b_job_title + _b_office + _b_phone + _b_state + _b_street + _b_web + _b_zip + _h_city + _h_country + _h_fax + _h_mobile + _h_phone + _h_state + _h_street + _h_web + _h_zip + _notes;
            if (Str.Trim().Length > 0)
            {
                return true;
            }

            if (_birthday_day + _birthday_month + _birthday_year > 0)
            {
                return true;
            }

            short cnt = 0;
            if (_email.Length > 0) cnt++;
            if (_b_email.Length > 0) cnt++;
            if (_other_email.Length > 0) cnt++;
            if (cnt > 1)
            {
                return true;
            }

			return false;	
        }

        public string GetPrimaryEmailAsString()
        {
            switch (_primary_email)
            {
                default:
            		return _email;
                case ContactPrimaryEmail.Business:
					return _b_email;
                case ContactPrimaryEmail.Other:
					return _other_email;
            }
        }
	}

    /// <summary>
    /// This class provides properties and methods for managing Groups in Address book 
    /// </summary>
    public class AddressBookGroup
    {
        #region Fields

        private int _id_group = -1;
        private int _id_user = -1;
        private string _group_nm = string.Empty;
        private int _use_frequency;
        private string _email = string.Empty;
        private string _company = string.Empty;
        private string _street = string.Empty;
        private string _city = string.Empty;
        private string _state = string.Empty;
        private string _zip = string.Empty;
        private string _country = string.Empty;
        private string _phone = string.Empty;
        private string _fax = string.Empty;
        private string _web = string.Empty;
        private bool _organization = false;
		private string _str_id = string.Empty;

        private AddressBookContact[] _contacts = new AddressBookContact[0];
        private AddressBookContact[] _newContacts = new AddressBookContact[0];

        #endregion

        #region Properties

        public int IDGroup
        {
            get { return _id_group; }
            set { _id_group = value; }
        }

        public int IDUser
        {
            get { return _id_user; }
            set { _id_user = value; }
        }

        public string GroupName
        {
            get { return _group_nm; }
            set { _group_nm = value; }
        }

        public AddressBookContact[] Contacts
        {
            get { return _contacts; }
            set { _contacts = value; }
        }

        public AddressBookContact[] NewContacts
        {
            get { return _newContacts; }
            set { _newContacts = value; }
        }

        public int UseFrequency
        {
            get { return _use_frequency; }
            set { _use_frequency = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string Web
        {
            get { return _web; }
            set { _web = value; }
        }

        public bool Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

		public string StrID
		{
			get { return _str_id; }
			set { _str_id = value; }
		}

        #endregion

        public AddressBookGroup() { }

        public void RenameGroup(string newName)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            try
            {
                dbMan.Connect();
                dbMan.UpdateAddressBookGroup(this);
            }
            finally
            {
                dbMan.Disconnect();
            }
        }

        public static AddressBookGroup CreateGroup(AddressBookGroup groupToCreate)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            try
            {
                dbMan.Connect();
                groupToCreate = dbMan.CreateAddressBookGroup(groupToCreate);
            }
            finally
            {
                dbMan.Disconnect();
            }
            return groupToCreate;
        }

        public static void DeleteGroup(AddressBookGroup groupToDelete)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            try
            {
                dbMan.Connect();
                dbMan.CreateAddressBookGroup(groupToDelete);
            }
            finally
            {
                dbMan.Disconnect();
            }
        }
    }
}
