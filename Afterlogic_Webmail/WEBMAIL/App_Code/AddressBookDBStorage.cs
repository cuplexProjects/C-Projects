using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WebMail
{
    public class AddressBookDBStorage : AddressBookStorage
    {
        protected DbStorage _dbStorage = null;

        public AddressBookDBStorage(Account account)
            : base(account)
        {
            _dbStorage = DbStorageCreator.CreateDatabaseStorage(account);
        }

        public override void Connect()
        {
            _dbStorage.Connect();
        }

        public override bool IsConnected()
        {
            return _dbStorage.IsConnected();
        }

        public override void Disconnect()
        {
            _dbStorage.Disconnect();
        }

        public override AddressBookContact CreateAddressBookContact(AddressBookContact contact)
        {
            return _dbStorage.CreateAddressBookContact(contact);
        }

        public override AddressBookGroupContact[] LoadAddressBookContactsGroups(int page, short sort_field, short sort_order, int id_group, string look_for, int look_for_type)
        {
            return _dbStorage.LoadAddressBookContactsGroups(page, sort_field, sort_order, id_group, look_for, look_for_type);
        }

        public override AddressBookContact GetAddressBookContact(long id_addr)
        {
            return _dbStorage.GetAddressBookContact(id_addr);
        }

        public override AddressBookContact GetAddressBookContact(string email)
        {
            return _dbStorage.GetAddressBookContact(email);
        }

        public override void DeleteAddressBookContactsGroups(AddressBookContact[] contacts, AddressBookGroup[] groups) 
        {
            _dbStorage.DeleteAddressBookContactsGroups(contacts, groups);
        }

        public override void UpdateAddressBookContact(AddressBookContact contact) 
        { 
            _dbStorage.UpdateAddressBookContact(contact);
        }

        public override void AddContactsToGroup(int id_group, AddressBookContact[] contacts) 
        {
            _dbStorage.AddContactsToGroup(id_group, contacts);
        }

        public override AddressBookGroup CreateAddressBookGroup(AddressBookGroup group) 
        {
            return _dbStorage.CreateAddressBookGroup(group);
        }

        public override AddressBookGroup GetAddressBookGroup(int id_group) 
        {
            return _dbStorage.GetAddressBookGroup(id_group);
        }

        public override AddressBookGroup[] GetAddressBookGroups() 
        {
            return _dbStorage.GetAddressBookGroups();
        }

        public override int GetAddressBookContactsCount(string look_for, int look_for_type) 
        {
            return _dbStorage.GetAddressBookContactsCount(look_for, look_for_type);
        }

        public override int GetAddressBookGroupsCount(string look_for, int look_for_type) 
        {
            return _dbStorage.GetAddressBookGroupsCount(look_for, look_for_type);
        }

        public override void UpdateAddressBookGroup(AddressBookGroup group) 
        {
            _dbStorage.UpdateAddressBookGroup(group);
        }

		public override AddressBookGroup GetAddressBookGroup(string group_uid)
		{
			return _dbStorage.GetAddressBookGroup(group_uid);
		}

		public override AddressBookContact GetAddressBookContact(int user_id, string str_id)
		{
			return _dbStorage.GetAddressBookContact(user_id, str_id);
		}
    }
}
