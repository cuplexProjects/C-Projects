using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace WebMail
{
	/// <summary>
	/// Summary description for import.
	/// </summary>
	public partial class import : Page
	{
		protected int _jsErrorCode = 0;
		protected int _jsContactsImported = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			Account acct = Session[Constants.sessionAccount] as Account;
			if (acct != null)
			{
				int fileType = (string.Compare(Request.Form["file_type"], "0", true, CultureInfo.InvariantCulture) == 0) ? 0 : 1;
				HttpFileCollection files = Request.Files;
				if ((files != null) && (files.Count > 0))
				{
					HttpPostedFile file = files[0];
					if (file != null)
					{
						byte[] buffer = null;
						using (Stream uploadStream = file.InputStream)
						{
							buffer = new byte[uploadStream.Length];
							long numBytesToRead = uploadStream.Length;
							long numBytesRead = 0;
							while (numBytesToRead > 0)
							{
								int n = uploadStream.Read(buffer, (int)numBytesRead, (int)numBytesToRead);
								if (n==0)
									break;
								numBytesRead += n;
								numBytesToRead -= n;
							}
						}
						if (buffer != null)
						{
							try
							{
								string csvText = Encoding.Default.GetString(buffer);
								CsvParser parser = new CsvParser(csvText, true);
								DataTable dt = parser.Parse();
								if (dt.Rows.Count == 0)
								{
									WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
									Log.WriteLine("import Page_Load", "Error: No contacts for import");
									_jsErrorCode = 2;
									_jsContactsImported = 0;
									Session[Constants.sessionReportText] = string.Format(@"{0}", resMan.GetString("ErrorNoContacts"));
									return;
								}

								ArrayList contacts = new ArrayList();
								for (int rowsIndex = 0; rowsIndex < dt.Rows.Count; rowsIndex++)
								{
									bool contactInitialized = false;
									AddressBookContact contact = new AddressBookContact();
									contact.IDUser = acct.UserOfAccount.ID;
									string firstName = string.Empty;
									string lastName = string.Empty;
									string nickname = string.Empty;
									string displayName = string.Empty;
									string birthday = string.Empty;
									DataRow dr = dt.Rows[rowsIndex];
									for (int columnsIndex = 0; columnsIndex < dt.Columns.Count; columnsIndex++)
									{
										if (dr[columnsIndex] != DBNull.Value)
										{
											if (dr[columnsIndex] as string != null)
											{
                                                switch (dt.Columns[columnsIndex].ColumnName.ToLower())
                                                {
                                                    case "first name":
                                                    case "firstname":
                                                        firstName = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "last name":
                                                    case "lastname":
                                                        lastName = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "notes":
                                                        contact.Notes = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "home street":
                                                    case "homestreet":
                                                    case "homeaddress":
                                                        contact.HStreet = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "home city":
                                                    case "homecity":
                                                        contact.HCity = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "home postal code":
                                                    case "zip":
                                                        contact.HZip = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "home state":
                                                    case "homestate":
                                                        contact.HState = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "home country":
                                                    case "homecountry":
                                                    case "home country/region":
                                                        contact.HCountry = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "home phone":
                                                    case "homephone":
                                                        contact.HPhone = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "home fax":
                                                    case "homefax":
                                                        contact.HFax = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "mobile phone":
                                                    case "mobilephone":
                                                        contact.HMobile = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "web page":
                                                    case "webpage":
                                                    case "personal web page":
                                                    case "personalwebpage":
                                                        contact.HWeb = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "company":
                                                        contact.BCompany = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "business street":
                                                    case "businessstreet":
                                                        contact.BStreet = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "business city":
                                                    case "businesscity":
                                                        contact.BCity = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "business state":
                                                    case "businessstate":
                                                        contact.BState = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "business postal code":
                                                        contact.BZip = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "business country":
                                                    case "business country/region":
                                                        contact.BCountry = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "job title":
                                                    case "jobtitle":
                                                        contact.BJobTitle = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "department":
                                                        contact.BDepartment = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "office location":
                                                    case "officelocation":
                                                        contact.BOffice = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "business phone":
                                                    case "businessphone":
                                                        contact.BOffice = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "business fax":
                                                    case "businessfax":
                                                        contact.BFax = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "business web page":
                                                    case "businesswebpage":
                                                        contact.BWeb = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "e-mail address":
                                                    case "e-mailaddress":
                                                    case "emailaddress":
                                                    case "e-mail":
                                                    case "email":
                                                        contact.HEmail = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "e-mail display name":
                                                    case "e-maildisplayname":
                                                    case "emaildisplayname":
                                                    case "name":
                                                        displayName = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "birthday":
                                                        birthday = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                    case "nickname":
                                                        nickname = dr[columnsIndex] as string;
                                                        contactInitialized = true;
                                                        break;
                                                }
                                            }
										}
									}
									if (!contactInitialized)
									{
										WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
										Log.WriteLine("import Page_Load", "Error: CSV file has invalid format");
										_jsErrorCode = 3;
										_jsContactsImported = 0;
										Session[Constants.sessionReportText] = string.Format(@"{0}", resMan.GetString("ErrorInvalidCSV"));
										break;
									}
									string fullName;
									if (nickname != string.Empty)
									{
										fullName = string.Format("{0} \"{1}\" {2}", firstName, nickname, lastName);
									}
									else
									{
										fullName = string.Format("{0} {1}", firstName, lastName);
									}
									contact.FullName = fullName;
									try
									{
										DateTime birthdayDate = DateTime.Parse(birthday);
										contact.BirthdayDay = (byte)birthdayDate.Day;
										contact.BirthdayMonth = (byte)birthdayDate.Month;
										contact.BirthdayYear = (short)birthdayDate.Year;
									}
									catch {}
									contacts.Add(contact);
								}
								DbStorage storage = DbStorageCreator.CreateDatabaseStorage(acct);
								try
								{
									storage.Connect();
									foreach (AddressBookContact contact in contacts)
									{
										contact.AutoCreate = true;

                                        try
                                        {
                                            storage.CreateAddressBookContact(contact);
                                            _jsContactsImported++;
                                        }
                                        catch { }
									}
									_jsErrorCode = 1;
									WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
                                    Session[Constants.sessionReportText] = string.Format(@"{0} {1} {2}", resMan.GetString("InfoHaveImported"), _jsContactsImported, resMan.GetString("InfoNewContacts"));
								}
								finally
								{
									storage.Disconnect();
								}
							}
							catch
							{
								Log.WriteLine("import Page_Load", "Error while importing contacts");
								_jsErrorCode = 0;
								_jsContactsImported = 0;
							}
						}
					}
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new EventHandler(this.Page_Load);
		}
		#endregion

	}
}
