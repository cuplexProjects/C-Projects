using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.XPath;
using System.Collections;
using System.Globalization;

namespace WebMail
{
    public class XmlPacketReplyMessage
    {
        public string action = string.Empty;
        public int id = -1;
        public string uid = string.Empty;
        public XmlPacketFolder folder;
        
        public static XmlPacketReplyMessage CreateWithXPath(XPathNodeIterator xpathIterator)
        {
            XmlPacketReplyMessage result = new XmlPacketReplyMessage();

            string attrValue = xpathIterator.Current.GetAttribute("action", "");
            if (attrValue.Length > 0) result.action = attrValue;

            attrValue = xpathIterator.Current.GetAttribute("id", "");
            if (attrValue.Length > 0) result.id = Convert.ToInt32(attrValue);

            XPathNodeIterator uidIter = xpathIterator.Current.Select("uid");
            if (uidIter.MoveNext())
            {
                result.uid = Utils.DecodeHtml(uidIter.Current.Value);
            }
            else
            {
                result.uid = string.Empty;
            }
            XPathNodeIterator xpathFolderIter = xpathIterator.Current.Select("folder");
            if (xpathFolderIter.MoveNext())
            {
                result.folder = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
            }

            return result;
        }
    }
    
    public class XmlPacketAttachment
	{
		public int size = 0;
		public bool inline = false;
		public string temp_name = string.Empty;
		public string name = string.Empty;
		public string mime_type = string.Empty;

		public static XmlPacketAttachment CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketAttachment result = new XmlPacketAttachment();

			string attrValue = xpathIterator.Current.GetAttribute("size", "");
			if (attrValue.Length > 0) result.size = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("inline", "");
			if (attrValue.Length > 0) result.inline = (string.Compare(attrValue, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;

			XPathNodeIterator tempNameIter = xpathIterator.Current.Select("temp_name");
			if (tempNameIter.MoveNext()) result.temp_name = Utils.DecodeHtml(tempNameIter.Current.Value);

			XPathNodeIterator nameIter = xpathIterator.Current.Select("name");
			if (nameIter.MoveNext()) result.name = Utils.DecodeHtml(nameIter.Current.Value);

			XPathNodeIterator mimeTypeIter = xpathIterator.Current.Select("mime_type");
			if (mimeTypeIter.MoveNext()) result.mime_type = Utils.DecodeHtml(mimeTypeIter.Current.Value);

			return result;
		}
	}

	public class XmlPacketGroup
	{
		public int id = 0;
		public string name = string.Empty;
		public bool organization = false;
		public string email = string.Empty;
		public string company = string.Empty;
		public string street = string.Empty;
		public string city = string.Empty;
		public string state = string.Empty;
		public string zip = string.Empty;
		public string country = string.Empty;
		public string phone = string.Empty;
		public string fax = string.Empty;
		public string web = string.Empty;
		public XmlPacketContact[] contacts = null;
		public XmlPacketContact[] new_contacts = null;

		public static XmlPacketGroup CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketGroup result = new XmlPacketGroup();

			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("organization", "");
			if (attrValue.Length > 0) result.organization = (string.Compare(attrValue, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;

			XPathNodeIterator nameIter = xpathIterator.Current.Select("name");
			if (nameIter.MoveNext()) result.name = Utils.DecodeHtml(nameIter.Current.Value);

			XPathNodeIterator emailIter = xpathIterator.Current.Select("email");
			if (emailIter.MoveNext()) result.email = Utils.DecodeHtml(emailIter.Current.Value);

			XPathNodeIterator companyIter = xpathIterator.Current.Select("company");
			if (companyIter.MoveNext()) result.company = Utils.DecodeHtml(companyIter.Current.Value);

			XPathNodeIterator streetIter = xpathIterator.Current.Select("street");
			if (streetIter.MoveNext()) result.street = Utils.DecodeHtml(streetIter.Current.Value);

			XPathNodeIterator cityIter = xpathIterator.Current.Select("city");
			if (cityIter.MoveNext()) result.city = Utils.DecodeHtml(cityIter.Current.Value);

			XPathNodeIterator stateIter = xpathIterator.Current.Select("state");
			if (stateIter.MoveNext()) result.state = Utils.DecodeHtml(stateIter.Current.Value);

			XPathNodeIterator zipIter = xpathIterator.Current.Select("zip");
			if (zipIter.MoveNext()) result.zip = Utils.DecodeHtml(zipIter.Current.Value);

			XPathNodeIterator countryIter = xpathIterator.Current.Select("country");
			if (countryIter.MoveNext()) result.country = Utils.DecodeHtml(countryIter.Current.Value);

			XPathNodeIterator faxIter = xpathIterator.Current.Select("fax");
			if (faxIter.MoveNext()) result.fax = Utils.DecodeHtml(faxIter.Current.Value);

			XPathNodeIterator phoneIter = xpathIterator.Current.Select("phone");
			if (phoneIter.MoveNext()) result.phone = Utils.DecodeHtml(phoneIter.Current.Value);

			XPathNodeIterator webIter = xpathIterator.Current.Select("web");
			if (webIter.MoveNext()) result.web = Utils.DecodeHtml(webIter.Current.Value);

			ArrayList contacts = new ArrayList();
			XPathNodeIterator contactsIter = xpathIterator.Current.Select("contacts/contact");
			while (contactsIter.MoveNext())
			{
				XmlPacketContact contact = XmlPacketContact.CreateWithXPath(contactsIter);
				if (contact != null) contacts.Add(contact);
			}
			result.contacts = (XmlPacketContact[])contacts.ToArray(typeof(XmlPacketContact));

			XPathNodeIterator newContactsIter = xpathIterator.Current.Select("new_contacts/contact");
			contacts = new ArrayList();
			while (newContactsIter.MoveNext())
			{
				XmlPacketContact contact = XmlPacketContact.CreateWithXPath(newContactsIter);
				if (contact != null) contacts.Add(contact);
			}
			result.new_contacts = (XmlPacketContact[])contacts.ToArray(typeof(XmlPacketContact));

			return result;
		}
	}

	public class XmlPacketContact
	{
		public long id = 0;
		public short primary_email = 0;
		public short use_friendly_name = 0;
		public string fullname = string.Empty;
		public byte birthday_day = 0;
		public byte birthday_month = 0;
		public short birthday_year = 0;
		public string personal_email = string.Empty;
		public string personal_street = string.Empty;
		public string personal_city = string.Empty;
		public string personal_state = string.Empty;
		public string personal_zip = string.Empty;
		public string personal_country = string.Empty;
		public string personal_fax = string.Empty;
		public string personal_phone = string.Empty;
		public string personal_mobile = string.Empty;
		public string personal_web = string.Empty;
		public string business_email = string.Empty;
		public string business_company = string.Empty;
		public string business_job_title = string.Empty;
		public string business_department = string.Empty;
		public string business_office = string.Empty;
		public string business_street = string.Empty;
		public string business_city = string.Empty;
		public string business_state = string.Empty;
		public string business_zip = string.Empty;
		public string business_country = string.Empty;
		public string business_fax = string.Empty;
		public string business_phone = string.Empty;
		public string business_web = string.Empty;
		public string other_email = string.Empty;
		public string other_notes = string.Empty;
		public XmlPacketGroup[] groups = null;

		public static XmlPacketContact CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketContact result = new XmlPacketContact();

			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt64(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("primary_email", "");
			if (attrValue.Length > 0) result.primary_email = Convert.ToInt16(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("use_friendly_nm", "");
			if (attrValue.Length > 0) result.use_friendly_name = Convert.ToInt16(attrValue);

			XPathNodeIterator fullnameIter = xpathIterator.Current.Select("fullname");
			if (fullnameIter.MoveNext()) result.fullname = Utils.DecodeHtml(fullnameIter.Current.Value);

			XPathNodeIterator birthdayIter = xpathIterator.Current.Select("birthday");
			if (birthdayIter.MoveNext())
			{
				attrValue = birthdayIter.Current.GetAttribute("day", "");
				if (attrValue.Length > 0) result.birthday_day = Convert.ToByte(attrValue, CultureInfo.InvariantCulture);
				
				attrValue = birthdayIter.Current.GetAttribute("month", "");
				if (attrValue.Length > 0) result.birthday_month = Convert.ToByte(attrValue, CultureInfo.InvariantCulture);

				attrValue = birthdayIter.Current.GetAttribute("year", "");
				if (attrValue.Length > 0) result.birthday_year = Convert.ToInt16(attrValue, CultureInfo.InvariantCulture);
			}

			XPathNodeIterator personalEmailIter = xpathIterator.Current.Select("personal/email");
			if (personalEmailIter.MoveNext()) result.personal_email = Utils.DecodeHtml(personalEmailIter.Current.Value);

			XPathNodeIterator personalStreetIter = xpathIterator.Current.Select("personal/street");
			if (personalStreetIter.MoveNext()) result.personal_street = Utils.DecodeHtml(personalStreetIter.Current.Value);

			XPathNodeIterator personalCityIter = xpathIterator.Current.Select("personal/city");
			if (personalCityIter.MoveNext()) result.personal_city = Utils.DecodeHtml(personalCityIter.Current.Value);

			XPathNodeIterator personalStateIter = xpathIterator.Current.Select("personal/state");
			if (personalStateIter.MoveNext()) result.personal_state = Utils.DecodeHtml(personalStateIter.Current.Value);

			XPathNodeIterator personalZipIter = xpathIterator.Current.Select("personal/zip");
			if (personalZipIter.MoveNext()) result.personal_zip = Utils.DecodeHtml(personalZipIter.Current.Value);

			XPathNodeIterator personalCountryIter = xpathIterator.Current.Select("personal/country");
			if (personalCountryIter.MoveNext()) result.personal_country = Utils.DecodeHtml(personalCountryIter.Current.Value);

			XPathNodeIterator personalFaxIter = xpathIterator.Current.Select("personal/fax");
			if (personalFaxIter.MoveNext()) result.personal_fax = Utils.DecodeHtml(personalFaxIter.Current.Value);

			XPathNodeIterator personalPhoneIter = xpathIterator.Current.Select("personal/phone");
			if (personalPhoneIter.MoveNext()) result.personal_phone = Utils.DecodeHtml(personalPhoneIter.Current.Value);

			XPathNodeIterator personalMobileIter = xpathIterator.Current.Select("personal/mobile");
			if (personalMobileIter.MoveNext())
			{
				result.personal_mobile = Utils.DecodeHtml(personalMobileIter.Current.Value);
			}

			XPathNodeIterator personalWebIter = xpathIterator.Current.Select("personal/web");
			if (personalWebIter.MoveNext()) result.personal_web = Utils.DecodeHtml(personalWebIter.Current.Value);

			XPathNodeIterator businessEmailIter = xpathIterator.Current.Select("business/email");
			if (businessEmailIter.MoveNext()) result.business_email = Utils.DecodeHtml(businessEmailIter.Current.Value);

			XPathNodeIterator businessCompanyIter = xpathIterator.Current.Select("business/company");
			if (businessCompanyIter.MoveNext()) result.business_company = Utils.DecodeHtml(businessCompanyIter.Current.Value);

			XPathNodeIterator businessJobTitleIter = xpathIterator.Current.Select("business/job_title");
			if (businessJobTitleIter.MoveNext()) result.business_job_title = Utils.DecodeHtml(businessJobTitleIter.Current.Value);

			XPathNodeIterator businessDepartmentIter = xpathIterator.Current.Select("business/department");
			if (businessDepartmentIter.MoveNext()) result.business_department = Utils.DecodeHtml(businessDepartmentIter.Current.Value);

			XPathNodeIterator businessOfficeIter = xpathIterator.Current.Select("business/office");
			if (businessOfficeIter.MoveNext()) result.business_office = Utils.DecodeHtml(businessOfficeIter.Current.Value);

			XPathNodeIterator businessStreetIter = xpathIterator.Current.Select("business/street");
			if (businessStreetIter.MoveNext()) result.business_street = Utils.DecodeHtml(businessStreetIter.Current.Value);

			XPathNodeIterator businessCityIter = xpathIterator.Current.Select("business/city");
			if (businessCityIter.MoveNext()) result.business_city = Utils.DecodeHtml(businessCityIter.Current.Value);

			XPathNodeIterator businessStateIter = xpathIterator.Current.Select("business/state");
			if (businessStateIter.MoveNext()) result.business_state = Utils.DecodeHtml(businessStateIter.Current.Value);

			XPathNodeIterator businessZipIter = xpathIterator.Current.Select("business/zip");
			if (businessZipIter.MoveNext()) result.business_zip = Utils.DecodeHtml(businessZipIter.Current.Value);

			XPathNodeIterator businessCountryIter = xpathIterator.Current.Select("business/country");
			if (businessCountryIter.MoveNext()) result.business_country = Utils.DecodeHtml(businessCountryIter.Current.Value);

			XPathNodeIterator businessFaxIter = xpathIterator.Current.Select("business/fax");
			if (businessFaxIter.MoveNext()) result.business_fax = Utils.DecodeHtml(businessFaxIter.Current.Value);

			XPathNodeIterator businessPhoneIter = xpathIterator.Current.Select("business/phone");
			if (businessPhoneIter.MoveNext()) result.business_phone = Utils.DecodeHtml(businessPhoneIter.Current.Value);

			XPathNodeIterator businessWebIter = xpathIterator.Current.Select("business/web");
			if (businessWebIter.MoveNext()) result.business_web = Utils.DecodeHtml(businessWebIter.Current.Value);

			XPathNodeIterator otherEmailIter = xpathIterator.Current.Select("other/email");
			if (otherEmailIter.MoveNext()) result.other_email = Utils.DecodeHtml(otherEmailIter.Current.Value);

			XPathNodeIterator otherNotesIter = xpathIterator.Current.Select("other/notes");
			if (otherNotesIter.MoveNext()) result.other_notes = Utils.DecodeHtml(otherNotesIter.Current.Value);

			ArrayList groups = new ArrayList();
			XPathNodeIterator groupsIter = xpathIterator.Current.Select("groups/group");
			while (groupsIter.MoveNext())
			{
                XmlPacketGroup group = XmlPacketGroup.CreateWithXPath(groupsIter.Clone());
				groups.Add(group);
			}
			result.groups = (XmlPacketGroup[])groups.ToArray(typeof(XmlPacketGroup));

			return result;
		}
	}

	public class XmlPacketSignature
	{
		public int type = 0;
		public int opt = 0;
		public string str = string.Empty;

		public static XmlPacketSignature CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketSignature result = null;
			XPathNodeIterator xpathSignatureIter = xpathIterator.Current.Select(@"signature");
			if (xpathSignatureIter.MoveNext())
			{
				result = new XmlPacketSignature();

				string attrValue = xpathSignatureIter.Current.GetAttribute("type", "");
				if (attrValue.Length > 0) result.type = Convert.ToInt32(attrValue);

				attrValue = xpathSignatureIter.Current.GetAttribute("opt", "");
				if (attrValue.Length > 0) result.opt = Convert.ToInt32(attrValue);

				result.str = Utils.DecodeHtml(xpathSignatureIter.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacketAccount
	{
		public int id_acct;
		public bool def_acct;
		public IncomingMailProtocol mail_protocol;
		public int mail_inc_port;
		public int mail_out_port;
		public bool mail_out_auth;
		public bool use_friendly_name;
		public short mails_on_server_days;
		public MailMode mail_mode;
		public bool getmail_at_login;
		public int inbox_sync_type;
		public string friendly_nm;
		public string email;
		public string mail_inc_host;
		public string mail_inc_login;
		public string mail_inc_pass;
		public string mail_out_host;
		public string mail_out_login;
		public string mail_out_pass;

		public XmlPacketAccount()
		{
			id_acct = -1;
			def_acct = false;
			mail_protocol = IncomingMailProtocol.Pop3;
			mail_inc_port = 110;
			mail_out_port = 25;
			mail_out_auth = false;
			use_friendly_name = false;
			mails_on_server_days = 0;
			mail_mode = MailMode.LeaveMessagesOnServer;
			getmail_at_login = false;
			inbox_sync_type = 0;
			friendly_nm = string.Empty;
			email = string.Empty;
			mail_inc_host = string.Empty;
			mail_inc_login = string.Empty;
			mail_inc_pass = string.Empty;
			mail_out_host = string.Empty;
			mail_out_login = string.Empty;
			mail_out_pass = string.Empty;
		}

		public static XmlPacketAccount CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketAccount result = null;
			XPathNodeIterator xpathAccoutIter = xpathIterator.Current.Select(@"account");
			if (xpathAccoutIter.MoveNext())
			{
				result = new XmlPacketAccount();

				string attrValue = xpathAccoutIter.Current.GetAttribute("id", "");
				if (attrValue.Length > 0) result.id_acct = Convert.ToInt32(attrValue);

				result.def_acct = (string.Compare(xpathAccoutIter.Current.GetAttribute("def_acct", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;
				result.mail_protocol = (IncomingMailProtocol) Convert.ToInt32(xpathAccoutIter.Current.GetAttribute("mail_protocol", ""));

				attrValue = xpathAccoutIter.Current.GetAttribute("mail_inc_port", "");
				if (attrValue.Length > 0) result.mail_inc_port = Convert.ToInt32(attrValue);

				attrValue = xpathAccoutIter.Current.GetAttribute("mail_out_port", "");
				if (attrValue.Length > 0) result.mail_out_port = Convert.ToInt32(attrValue);

				result.mail_out_auth = (string.Compare(xpathAccoutIter.Current.GetAttribute("mail_out_auth", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;

				result.use_friendly_name = (string.Compare(xpathAccoutIter.Current.GetAttribute("use_friendly_nm", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;

				attrValue = xpathAccoutIter.Current.GetAttribute("mails_on_server_days", "");
				if (attrValue.Length > 0) result.mails_on_server_days = Convert.ToInt16(attrValue);

				attrValue = xpathAccoutIter.Current.GetAttribute("mail_mode", "");
				switch (attrValue)
				{
					case "0":
						result.mail_mode = MailMode.DeleteMessagesFromServer;
						break;
					case "1":
						result.mail_mode = MailMode.LeaveMessagesOnServer;
						break;
					case "2":
						result.mail_mode = MailMode.KeepMessagesOnServer;
						break;
					case "3":
						result.mail_mode = MailMode.DeleteMessageWhenItsRemovedFromTrash;
						break;
					case "4":
                        result.mail_mode = MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash;
						break;
				}

				result.getmail_at_login = (string.Compare(xpathAccoutIter.Current.GetAttribute("getmail_at_login", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;

				attrValue = xpathAccoutIter.Current.GetAttribute("inbox_sync_type", "");
				if (attrValue.Length > 0) result.inbox_sync_type = Convert.ToInt32(attrValue);

				XPathNodeIterator friendlyIter = xpathAccoutIter.Current.Select("friendly_nm");
				if (friendlyIter.MoveNext())
				{
					result.friendly_nm = Utils.DecodeHtml(friendlyIter.Current.Value);
				}
				else
				{
					result.friendly_nm = string.Empty;
				}

				XPathNodeIterator emailIter = xpathAccoutIter.Current.Select("email");
				if (emailIter.MoveNext())
				{
					result.email = Utils.DecodeHtml(emailIter.Current.Value);
				}
				else
				{
					result.email = string.Empty;
				}

				XPathNodeIterator mailIncHostIter = xpathAccoutIter.Current.Select("mail_inc_host");
				if (mailIncHostIter.MoveNext())
				{
					result.mail_inc_host = Utils.DecodeHtml(mailIncHostIter.Current.Value);
				}
				else
				{
					result.mail_inc_host = string.Empty;
				}

				XPathNodeIterator mailIncLoginIter = xpathAccoutIter.Current.Select("mail_inc_login");
				if (mailIncLoginIter.MoveNext())
				{
					result.mail_inc_login = Utils.DecodeHtml(mailIncLoginIter.Current.Value);
				}
				else
				{
					result.mail_inc_login = string.Empty;
				}

				XPathNodeIterator mailIncPassIter = xpathAccoutIter.Current.Select("mail_inc_pass");
				if (mailIncPassIter.MoveNext())
				{
					result.mail_inc_pass = Utils.DecodeHtml(mailIncPassIter.Current.Value);
				}
				else
				{
					result.mail_inc_pass = string.Empty;
				}

				XPathNodeIterator mailOutHostIter = xpathAccoutIter.Current.Select("mail_out_host");
				if (mailOutHostIter.MoveNext())
				{
					result.mail_out_host = Utils.DecodeHtml(mailOutHostIter.Current.Value);
				}
				else
				{
					result.mail_out_host = string.Empty;
				}

				XPathNodeIterator mailOutLoginIter = xpathAccoutIter.Current.Select("mail_out_login");
				if (mailOutLoginIter.MoveNext())
				{
					result.mail_out_login = Utils.DecodeHtml(mailOutLoginIter.Current.Value);
				}
				else
				{
					result.mail_out_login = string.Empty;
				}

				XPathNodeIterator mailOutPassIter = xpathAccoutIter.Current.Select("mail_out_pass");
				if (mailOutPassIter.MoveNext())
				{
					result.mail_out_pass = Utils.DecodeHtml(mailOutPassIter.Current.Value);
				}
				else
				{
					result.mail_out_pass = string.Empty;
				}
			}
			return result;
		}
	}

	public class XmlPacketFilters
	{
		public XmlPacketFilter[] newFilterArray;
		public XmlPacketFilter[] updatedFilterArray;
		public XmlPacketFilter[] removedFilterArray;
		public int id_acct;

		public static XmlPacketFilters CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketFilters result = new XmlPacketFilters();
			XPathNodeIterator xpathFiltersIter = xpathIterator.Current.Select("filters");
			if (xpathFiltersIter.MoveNext())
			{
				string attrValue = xpathFiltersIter.Current.GetAttribute("id_acct", "");
				if (attrValue.Length > 0) result.id_acct = Convert.ToInt32(attrValue);

				ArrayList newFilters = new ArrayList();
				ArrayList updatedFilters = new ArrayList();
				ArrayList removedFilters = new ArrayList();
				XPathNodeIterator xpathFilterIter = xpathFiltersIter.Current.Select("filter");
				while (xpathFilterIter.MoveNext())
				{
					XmlPacketFilter xmlFlt = XmlPacketFilter.CreateWithXPath(xpathFilterIter);
					if (xmlFlt != null)
					{
						switch (xmlFlt.status)
						{
							case Constants.FilterStatus.New:
								newFilters.Add(xmlFlt);
								break;
							case Constants.FilterStatus.Updated:
								updatedFilters.Add(xmlFlt);
								break;
							case Constants.FilterStatus.Removed:
								removedFilters.Add(xmlFlt);
								break;
						}
					}
				}
				result.newFilterArray = (XmlPacketFilter[])newFilters.ToArray(typeof(XmlPacketFilter));
				result.updatedFilterArray = (XmlPacketFilter[])updatedFilters.ToArray(typeof(XmlPacketFilter));
				result.removedFilterArray = (XmlPacketFilter[])removedFilters.ToArray(typeof(XmlPacketFilter));
			}
			return result;
		}
	}
	
	public class XmlPacketFilter
	{
		public string status;
		public int id_filter;
		public byte field;
		public byte condition;
		public string filter;
		public byte action;
		public long id_folder;
		public bool applied;

		public XmlPacketFilter()
		{
			status = Constants.FilterStatus.New;
			id_filter = -1;
			field = 0;
			condition = 0;
			filter = string.Empty;
			action = 0;
			id_folder = -1;
			applied = true;
		}

		public static XmlPacketFilter CreateWithXPath(XPathNodeIterator xpathFilterIter)
		{
			XmlPacketFilter result = new XmlPacketFilter();

			string attrValue = xpathFilterIter.Current.GetAttribute("status", "");
			if (attrValue.Length > 0) result.status = attrValue;

			attrValue = xpathFilterIter.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id_filter = Convert.ToInt32(attrValue);

			if (result.status != Constants.FilterStatus.Removed)
			{
				attrValue = xpathFilterIter.Current.GetAttribute("field", "");
				if (attrValue.Length > 0) result.field = Convert.ToByte(attrValue);

				attrValue = xpathFilterIter.Current.GetAttribute("condition", "");
				if (attrValue.Length > 0) result.condition = Convert.ToByte(attrValue);

				attrValue = xpathFilterIter.Current.GetAttribute("action", "");
				if (attrValue.Length > 0) result.action = Convert.ToByte(attrValue);

				attrValue = xpathFilterIter.Current.GetAttribute("id_folder", "");
				if (attrValue.Length > 0) result.id_folder = Convert.ToInt32(attrValue);

				attrValue = xpathFilterIter.Current.GetAttribute("applied", "");
				if (attrValue.Length > 0) 
					result.applied = (string.Compare(attrValue, "1", true, CultureInfo.InvariantCulture) == 0)
						? true
						: false;
			}

			result.filter = Utils.DecodeHtml(xpathFilterIter.Current.Value);

			return result;
		}
	}

	public class XmlPacketMessages
	{
        public int getmsg;
        public XmlPacketLookFor look_for;
		public XmlPacketFolder to_folder;
		public XmlPacketFolder folder;
		public XmlPacketMessage[] messages;

		public XmlPacketMessages()
		{
            getmsg = 0;
            look_for = null;
			to_folder = null;
			messages = new XmlPacketMessage[0];
		}

		public static XmlPacketMessages CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketMessages result = null;
			XPathNodeIterator xpathMessagesIter = xpathIterator.Current.Select(@"messages");
			if (xpathMessagesIter.MoveNext())
			{
				result = new XmlPacketMessages();

                string attrValue = xpathMessagesIter.Current.GetAttribute("getmsg", "");
                if (attrValue.Length > 0) result.getmsg = Convert.ToInt32(attrValue);

				result.look_for = XmlPacketLookFor.CreateWithXPath(xpathMessagesIter.Clone());

				XPathNodeIterator xpathFolderIter = xpathMessagesIter.Current.Select("folder");
				if (xpathFolderIter.MoveNext())
				{
					result.folder = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
				}

				XmlPacketFolder to_folder = null;
				xpathFolderIter = xpathMessagesIter.Current.Select("to_folder");
				if (xpathFolderIter.MoveNext())
				{
					to_folder = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
				}
				result.to_folder = to_folder;
				ArrayList messages = new ArrayList();
				XPathNodeIterator xpathMessageIter = xpathMessagesIter.Current.Select("message");
				while (xpathMessageIter.MoveNext())
				{
					XmlPacketMessage xmlMsg = XmlPacketMessage.CreateWithXPath(xpathMessageIter);
					if (xmlMsg != null)
					{
						messages.Add(xmlMsg);
					}
				}
				result.messages = (XmlPacketMessage[])messages.ToArray(typeof (XmlPacketMessage));
			}
			return result;
		}
	}

	public class XmlPacketMessage
	{
		public int id = 0;
		public int charset = -1;
        public int size = -1;
        public int priority = 3;
        public int sensivity = 0;
        public bool saveMail = true;
        public string from = string.Empty;
		public int fromAcctId = -1;
		public string to = string.Empty;
		public string cc = string.Empty;
		public string bcc = string.Empty;
		public string subject = string.Empty;
		public string mailConfirmation = string.Empty;
		public XmlPacketGroup[] groups;
		public bool bodyIsHtml = false;
		public string bodyText = string.Empty;
		public XmlPacketAttachment[] attachments;
        public XmlPacketReplyMessage reply_message;

		public string uid = string.Empty;
		public XmlPacketFolder folder;

		public static XmlPacketMessage CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketMessage result = new XmlPacketMessage();
			
			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("from_acct_id", "");
			if (attrValue.Length > 0) result.fromAcctId = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("charset", "");
			if (attrValue.Length > 0) result.charset = Convert.ToInt32(attrValue);

            attrValue = xpathIterator.Current.GetAttribute("size", "");
            if (attrValue.Length > 0) result.size = Convert.ToInt32(attrValue);

            attrValue = xpathIterator.Current.GetAttribute("priority", "");
			if (attrValue.Length > 0) result.priority = Convert.ToInt32(attrValue);

            attrValue = xpathIterator.Current.GetAttribute("sensivity", "");
            if (attrValue.Length > 0) result.sensivity = Convert.ToInt32(attrValue);

            attrValue = xpathIterator.Current.GetAttribute("save_mail", "");
            if (attrValue.Length > 0) result.saveMail = (string.Compare(attrValue, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;

            XPathNodeIterator uidIter = xpathIterator.Current.Select("uid");
			if (uidIter.MoveNext())
			{
				result.uid = Utils.DecodeHtml(uidIter.Current.Value);
			}
			else
			{
				result.uid = string.Empty;
			}
			XPathNodeIterator xpathFolderIter = xpathIterator.Current.Select("folder");
			if (xpathFolderIter.MoveNext())
			{
				result.folder = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
			}

			XPathNodeIterator fromIter = xpathIterator.Current.Select("headers/from");
			if (fromIter.MoveNext()) result.from = Utils.DecodeHtml(fromIter.Current.Value);

			XPathNodeIterator toIter = xpathIterator.Current.Select("headers/to");
			if (toIter.MoveNext()) result.to = Utils.DecodeHtml(toIter.Current.Value);

			XPathNodeIterator ccIter = xpathIterator.Current.Select("headers/cc");
			if (ccIter.MoveNext()) result.cc = Utils.DecodeHtml(ccIter.Current.Value);

			XPathNodeIterator bccIter = xpathIterator.Current.Select("headers/bcc");
			if (bccIter.MoveNext()) result.bcc= Utils.DecodeHtml(bccIter.Current.Value);

			XPathNodeIterator subjectIter = xpathIterator.Current.Select("headers/subject");
			if (subjectIter.MoveNext()) result.subject = Utils.DecodeHtml(subjectIter.Current.Value);

			XPathNodeIterator mailConfirmationIter = xpathIterator.Current.Select("headers/mailconfirmation");
			if (mailConfirmationIter.MoveNext()) result.mailConfirmation = Utils.DecodeHtml(mailConfirmationIter.Current.Value);
			
			ArrayList groups = new ArrayList();
			XPathNodeIterator groupsIter = xpathIterator.Current.Select("headers/groups/group");
			while (groupsIter.MoveNext())
			{
				XmlPacketGroup group = XmlPacketGroup.CreateWithXPath(groupsIter);
				if (group != null)
				{
					groups.Add(group);
				}
			}
			if (groups.Count > 0) result.groups = (XmlPacketGroup[])groups.ToArray(typeof (XmlPacketGroup));

			XPathNodeIterator bodyIter = xpathIterator.Current.Select("body");
			if (bodyIter.MoveNext())
			{
				attrValue = bodyIter.Current.GetAttribute("is_html", "");
				if (attrValue.Length > 0) result.bodyIsHtml = (string.Compare(attrValue, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;

				result.bodyText = bodyIter.Current.Value;
			}

			ArrayList attachments = new ArrayList();
			XPathNodeIterator attachmentIter = xpathIterator.Current.Select("attachments/attachment");
			while (attachmentIter.MoveNext())
			{
				XmlPacketAttachment attach = XmlPacketAttachment.CreateWithXPath(attachmentIter);
				if (attach != null)
				{
					attachments.Add(attach);
				}
			}
			result.attachments = (XmlPacketAttachment[])attachments.ToArray(typeof(XmlPacketAttachment));

            XPathNodeIterator replyIter = xpathIterator.Current.Select("reply_message");
            if (replyIter.MoveNext())
            {
                result.reply_message = XmlPacketReplyMessage.CreateWithXPath(replyIter);
            }

			return result;
		}
	}

	public class XmlPacketSkin
	{
		public bool def;
		public string skin;

		public static XmlPacketSkin CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketSkin result = null;
			XPathNodeIterator xpathSkinIter = xpathIterator.Current.Select("skin");
			if (xpathSkinIter.MoveNext())
			{
				result = new XmlPacketSkin();
				result.def = (string.Compare(xpathSkinIter.Current.GetAttribute("def", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;
				result.skin = Utils.DecodeHtml(xpathSkinIter.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacketLang
	{
		public bool def;
		public string lang;

		public static XmlPacketLang CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketLang result = null;
			XPathNodeIterator xpathLangIter = xpathIterator.Current.Select("lang");
			if (xpathLangIter.MoveNext())
			{
				result = new XmlPacketLang();
				result.def = (string.Compare(xpathLangIter.Current.GetAttribute("def", ""), "0", true, CultureInfo.InvariantCulture) == 0) ? false : true;
				result.lang = Utils.DecodeHtml(xpathLangIter.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacketSettings
	{
		public short msgs_per_page;
		public short contacts_per_page;
		public bool allow_dhtml_editor;
		public string def_skin;
		public int def_charset_inc;
		public int def_charset_out;
		public short def_timezone;
		public string def_lang;
		public string def_date_fmt;
        public TimeFormats def_time_fmt;
		public byte view_mode;
        public int auto_checkmail_interval;

		public static XmlPacketSettings CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketSettings result = null;
			XPathNodeIterator xpathSettingsIter = xpathIterator.Current.Select(@"settings");
			if (xpathSettingsIter.MoveNext())
			{
				result = new XmlPacketSettings();
				
				string attrValue = xpathSettingsIter.Current.GetAttribute("msgs_per_page", "");
				if (attrValue.Length > 0) result.msgs_per_page = Convert.ToInt16(attrValue);

				attrValue = xpathSettingsIter.Current.GetAttribute("contacts_per_page", "");
				if (attrValue.Length > 0) result.contacts_per_page = Convert.ToInt16(attrValue);

				result.allow_dhtml_editor = (string.Compare(xpathSettingsIter.Current.GetAttribute("allow_dhtml_editor", ""), "1", false, CultureInfo.InvariantCulture) == 0) ? true : false;
				
				attrValue = xpathSettingsIter.Current.GetAttribute("def_charset_inc", "");
				if (attrValue.Length > 0) result.def_charset_inc = Convert.ToInt32(attrValue);
				
				attrValue = xpathSettingsIter.Current.GetAttribute("def_charset_out", "");
				if (attrValue.Length > 0) result.def_charset_out = Convert.ToInt32(attrValue);
				
				attrValue = xpathSettingsIter.Current.GetAttribute("def_timezone", "");
				if (attrValue.Length > 0) result.def_timezone = Convert.ToInt16(attrValue);
				
				attrValue = xpathSettingsIter.Current.GetAttribute("view_mode", "");
				if (attrValue.Length > 0) result.view_mode = Convert.ToByte(attrValue);

                attrValue = xpathSettingsIter.Current.GetAttribute("auto_checkmail_interval", "");
                if (attrValue.Length > 0) result.auto_checkmail_interval = Convert.ToInt32(attrValue);

                attrValue = xpathSettingsIter.Current.GetAttribute("time_format", "");
                result.def_time_fmt = (attrValue.Length > 0 && attrValue == "1") ? TimeFormats.F12 : TimeFormats.F24;

				XPathNodeIterator defSkinIter = xpathSettingsIter.Current.Select("def_skin");
				if (defSkinIter.MoveNext())
				{
					result.def_skin = Utils.DecodeHtml(defSkinIter.Current.Value);
				}
				else
				{
					result.def_skin = string.Empty;
				}
				XPathNodeIterator defLangIter = xpathSettingsIter.Current.Select("def_lang");
				if (defLangIter.MoveNext())
				{
					result.def_lang = Utils.DecodeHtml(defLangIter.Current.Value);
				}
				else
				{
					result.def_lang = string.Empty;
				}
				XPathNodeIterator defDateFmtIter = xpathSettingsIter.Current.Select("def_date_fmt");
				if (defDateFmtIter.MoveNext())
				{
					result.def_date_fmt = Utils.DecodeHtml(defDateFmtIter.Current.Value);
				}
				else
				{
					result.def_date_fmt = string.Empty;
				}
			}
			return result;
		}
	}

	public class XmlPacketFolders
	{
		public XmlPacketFolder[] folderArray;
	
		public static XmlPacketFolders CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketFolders result = new XmlPacketFolders();
			XPathNodeIterator xpathFoldersIter = xpathIterator.Current.Select("folders");
			if (xpathFoldersIter.MoveNext())
			{
				ArrayList folders = new ArrayList();
				XPathNodeIterator xpathFolderIter = xpathFoldersIter.Current.Select("folder");
				while (xpathFolderIter.MoveNext())
				{
					XmlPacketFolder xmlFld = XmlPacketFolder.CreateWithXPath(xpathFolderIter);
					if (xmlFld != null)
					{
						folders.Add(xmlFld);
					}
				}
				result.folderArray = (XmlPacketFolder[])folders.ToArray(typeof (XmlPacketFolder));
			}
			return result;
		}
	}

	public class XmlPacketMessagesBodiesFolders
	{
		public XmlPacketMessagesBodiesFolder[] messagesBodiesFolders;

		public static XmlPacketMessagesBodiesFolder[] CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketMessagesBodiesFolders result = new XmlPacketMessagesBodiesFolders();
			XPathNodeIterator xpathFolderIter = xpathIterator.Current.Select("folder");
			ArrayList folders = new ArrayList();
			while (xpathFolderIter.MoveNext())
			{
				XmlPacketMessagesBodiesFolder xmlFld = XmlPacketMessagesBodiesFolder.CreateWithXPath(xpathFolderIter);
				if (xmlFld != null)
				{
					folders.Add(xmlFld);
				}
			}
			result.messagesBodiesFolders = (XmlPacketMessagesBodiesFolder[])folders.ToArray(typeof(XmlPacketMessagesBodiesFolder));
			return result.messagesBodiesFolders;
		}
	}

	public class XmlPacketMessagesBodiesFolder
	{
		public long id = 0;
		public string full_name = string.Empty;
		public XMLPacketMessagesBody[] messages;
		public int[] id_msgs;
        public object[] uid_msgs;

		public static XmlPacketMessagesBodiesFolder CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketMessagesBodiesFolder result = new XmlPacketMessagesBodiesFolder();
			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt64(attrValue);

			XPathNodeIterator fullNameIter = xpathIterator.Current.Select("full_name");
			if (fullNameIter.MoveNext())
			{
				result.full_name = Utils.DecodeHtml(fullNameIter.Current.Value);
			}
			else
			{
				result.full_name = Utils.DecodeHtml(xpathIterator.Current.Value);
			}

			XPathNodeIterator xpathMessagesIter = xpathIterator.Current.Select("message");
			ArrayList messages = new ArrayList();
			ArrayList id_msgs = new ArrayList();
            ArrayList uid_msgs = new ArrayList();
			while (xpathMessagesIter.MoveNext())
			{
				XMLPacketMessagesBody xmlMsg = XMLPacketMessagesBody.CreateWithXPath(xpathMessagesIter);
				if (xmlMsg != null)
				{
					messages.Add(xmlMsg);
					id_msgs.Add(xmlMsg.id);
                    uid_msgs.Add(xmlMsg.uid);
				}
			}
			result.messages = (XMLPacketMessagesBody[])messages.ToArray(typeof(XMLPacketMessagesBody));
			result.id_msgs = (int[])id_msgs.ToArray(typeof(int));
            result.uid_msgs = (object[])uid_msgs.ToArray(typeof(object));
			return result;
		}
	}

	public class XMLPacketMessagesBody
	{
		public int id = 0;
		public int charset = -1;
        public int size = 0;
        public string uid = string.Empty;

		public static XMLPacketMessagesBody CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XMLPacketMessagesBody result = new XMLPacketMessagesBody();

			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("charset", "");
			if (attrValue.Length > 0) result.charset = Convert.ToInt32(attrValue);

            attrValue = xpathIterator.Current.GetAttribute("size", "");
            if (attrValue.Length > 0) result.size = Convert.ToInt32(attrValue);
            
            XPathNodeIterator uidIter = xpathIterator.Current.Select("uid");
			if (uidIter.MoveNext())
			{
				result.uid = Utils.DecodeHtml(uidIter.Current.Value);
			}
			else
			{
				result.uid = string.Empty;
			}
			return result;
		}
	}

	public class XmlPacketFolder
	{
		public long id;
		public int sync_type;
        public int type;
        public int hide;
		public short fld_order;
		public string name;
		public string full_name;

		public static XmlPacketFolder CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketFolder result = new XmlPacketFolder();
			string attrValue = xpathIterator.Current.GetAttribute("id", "");
			if (attrValue.Length > 0) result.id = Convert.ToInt64(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("sync_type", "");
			if (attrValue.Length > 0) result.sync_type = Convert.ToInt32(attrValue);

            attrValue = xpathIterator.Current.GetAttribute("type", "");
            if (attrValue.Length > 0) result.type = Convert.ToInt32(attrValue);
            
            attrValue = xpathIterator.Current.GetAttribute("hide", "");
			if (attrValue.Length > 0) result.hide = Convert.ToInt32(attrValue);

			attrValue = xpathIterator.Current.GetAttribute("fld_order", "");
			if (attrValue.Length > 0) result.fld_order = Convert.ToInt16(attrValue);

			XPathNodeIterator nameIter = xpathIterator.Current.Select("name");
			if (nameIter.MoveNext())
			{
				result.name = Utils.DecodeHtml(nameIter.Current.Value);
			}

			XPathNodeIterator fullNameIter = xpathIterator.Current.Select("full_name");
			if (fullNameIter.MoveNext())
			{
				result.full_name = Utils.DecodeHtml(fullNameIter.Current.Value);
			}
			else
			{
				result.full_name = Utils.DecodeHtml(xpathIterator.Current.Value);
			}
			return result;
		}
	}

	public class XmlPacketLookFor
	{
		public int fields;
		public string search_query;
		public int type;

		public static XmlPacketLookFor CreateWithXPath(XPathNodeIterator xpathIterator)
		{
			XmlPacketLookFor result = null;
			XPathNodeIterator xpathLookForIter = xpathIterator.Current.Select("look_for");
			if (xpathLookForIter.MoveNext())
			{
				result = new XmlPacketLookFor();
				string attrValue = xpathLookForIter.Current.GetAttribute("fields", "");
				if (attrValue.Length > 0) result.fields = Convert.ToByte(attrValue);
				result.search_query = Utils.DecodeHtml(xpathLookForIter.Current.Value);

				attrValue = xpathLookForIter.Current.GetAttribute("type", "");
				if (attrValue.Length > 0) result.type = Convert.ToInt32(attrValue);
				result.search_query = Utils.DecodeHtml(xpathLookForIter.Current.Value);
			}
			return result;
		}
	}

    public class XmlPacketAutoresponder
    {
        public bool enable;
        public string message;
        public string subject;

        public static XmlPacketAutoresponder CreateWithXPath(XPathNodeIterator xpathIterator)
        {
            XmlPacketAutoresponder result = null;
            XPathNodeIterator xpathAutoresponderIter = xpathIterator.Current.Select("autoresponder");
            if (xpathAutoresponderIter.MoveNext())
            {
                result = new XmlPacketAutoresponder();
                result.enable = (string.Compare(xpathAutoresponderIter.Current.GetAttribute("enable", ""), "1", false, CultureInfo.InvariantCulture) == 0) ? true : false;                    
                XPathNodeIterator xpathMessageIter = xpathAutoresponderIter.Current.Select("message");
                if (xpathMessageIter.MoveNext())
                {
                    result.message = Utils.DecodeHtml(xpathMessageIter.Current.Value);
                }
                XPathNodeIterator xpathSubjectIter = xpathAutoresponderIter.Current.Select("subject");
                if (xpathSubjectIter.MoveNext())
                {
                    result.subject = Utils.DecodeHtml(xpathSubjectIter.Current.Value);
                }
            }
            return result;
        }
    }
    
    public class XmlPacket
	{
		public string action;
		public string request;
		public string confirmation;
		public string subject;
		public Hashtable parameters;
		public XmlPacketMessages messages;
		public XmlPacketSettings settings;
		public XmlPacketFolder folder;
		public XmlPacketLookFor look_for;
		public XmlPacketAccount account;
		public XmlPacketFilters filters;
		public XmlPacketSignature signature;
		public XmlPacketFolders folders;
		public XmlPacketGroup group;
		public XmlPacketContact contact;
		public XmlPacketContact[] contacts;
		public XmlPacketGroup[] groups;
		public XmlPacketMessage message;
        public XmlPacketAutoresponder autoresponder;
        public UserColumn[] columns;
		public XmlPacketMessagesBodiesFolder[] messagesBodiesFolders;

		public XmlPacket()
		{
			action = string.Empty;
			request = string.Empty;
            parameters = new Hashtable(StringComparer.OrdinalIgnoreCase);
            messages = null;
			settings = null;
			folder = null;
			look_for = null;
			account = null;
			filters = null;
			signature = null;
			folders = null;
			group = null;
			contact = null;
			contacts = null;
			message = null;
            autoresponder = null;
			messagesBodiesFolders = null;
		}
	}
}
