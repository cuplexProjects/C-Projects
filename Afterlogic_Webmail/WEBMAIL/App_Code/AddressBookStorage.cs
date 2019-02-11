using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MailBee.Mime;

namespace WebMail
{
    public class AddressBookStorageCreator
    {
        public static AddressBookStorage CreateAddressBookStorage(Account acct)
        {
            AddressBookStorageType type = AddressBookStorageType.DataBase;
            
            switch (type)
            {
                case AddressBookStorageType.DataBase:
                    return new AddressBookDBStorage(acct);
                default:
                    return null;
            }
        }
    }

    public abstract class AddressBookStorage
    {
        protected Account _account = null;

        public Account Account
        {
            get { return _account; }
            set { _account = value; }
        }

        public AddressBookStorage(Account account) { _account = account; }

        public virtual void Connect() { }

        public virtual bool IsConnected() { return false; }

        public virtual void Disconnect() { }

        public virtual AddressBookContact CreateAddressBookContact(AddressBookContact contact) { return null; }

        public virtual AddressBookGroupContact[] LoadAddressBookContactsGroups(int page, short sort_field, short sort_order, int id_group, string look_for, int look_for_type) { return null; }

        public virtual AddressBookContact GetAddressBookContact(long id_addr) { return null; }

        public virtual AddressBookContact GetAddressBookContact(string email) { return null; }

        public virtual void DeleteAddressBookContactsGroups(AddressBookContact[] contacts, AddressBookGroup[] groups) { }

        public virtual void UpdateAddressBookContact(AddressBookContact contact) { }

        public virtual void AddContactsToGroup(int id_group, AddressBookContact[] contacts) { }

        public virtual AddressBookGroup CreateAddressBookGroup(AddressBookGroup group) { return null; }

        public virtual AddressBookGroup GetAddressBookGroup(int id_group) { return null; }

        public virtual AddressBookGroup[] GetAddressBookGroups() { return null; }

        public virtual int GetAddressBookContactsCount(string look_for, int look_for_type) { return 0; }

        public virtual int GetAddressBookGroupsCount(string look_for, int look_for_type) { return 0; }

        public virtual void UpdateAddressBookGroup(AddressBookGroup group) { }

		public virtual AddressBookGroup GetAddressBookGroup(string group_uid) { return null; }

		public virtual AddressBookContact GetAddressBookContact(int user_id, string str_id) { return null; }
	}
}
