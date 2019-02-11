namespace WebMail.classic
{
	using System;

	/// <summary>
	///		Summary description for AccountsList.
	/// </summary>
	public partial class AccountsList : System.Web.UI.UserControl
	{
		protected string _skin = Constants.defaultSkinName;
		protected WebmailResourceManager _manager = null;
		protected string AcctList = string.Empty;
		//protected int _accountsCount = 1;

		public string Skin
		{
			get { return _skin; }
			set { _skin = value; }
		}

		//public int AccountsCount
		//{
		//    get { return _accountsCount; }
		//}

		private string _webMailMode;

		private string _accountsListLink = "webmail.aspx?scr=default";
        protected string _settingsLink = "webmail.aspx?scr=settings_common";
        protected string _contactsLink = "webmail.aspx?scr=contacts";
        protected string _logoutLink = "default.aspx?mode=logout";

		protected string _ContactsDiv = String.Empty;
		protected string _CalendarDiv = String.Empty;

		protected string mailActiveClassName = string.Empty;
		protected string contactsActiveClassName = string.Empty;
		protected string settingsActiveClassName = string.Empty;

        public string WebMailMode
        {
            get { return _webMailMode; }
            set
            {
                _webMailMode = value;
                _accountsListLink = "#\" onclick=\"parent.HideCalendar('account'); return false;";
                _settingsLink = "#\" onclick=\"parent.HideCalendar('settings'); return false;";
                _contactsLink = "#\" onclick=\"parent.HideCalendar('contacts'); return false;";
                _logoutLink = "#\" onclick=\"parent.HideCalendar('logout'); return false;";
            }
        }

		protected void SetActiveClassNames()
		{
			if (_webMailMode == "ajax") return;

			string screen = null;
			if (Request.QueryString.Get("scr") != null)
			{
				screen = Request.QueryString.Get("scr").ToLower();
			}
			switch (screen)
			{
				case "settings_common":
				case "settings_accounts_properties":
				case "settings_accounts_filters":
				case "settings_accounts_signature":
				case "settings_accounts_folders":
				case "settings_contacts":
					settingsActiveClassName = " wm_active_tab";
					break;
				case "contacts":
				case "contacts_view":
				case "contacts_add":
				case "contacts_import":
					contactsActiveClassName = " wm_active_tab";
					break;
				default:
					mailActiveClassName = " wm_active_tab";
					break;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			SetActiveClassNames();
            WebMail.Account acct = (WebMail.Account)Session[Constants.sessionAccount];
			Skin = acct.UserOfAccount.Settings.DefaultSkin;
            
			if (acct == null)
			{
				Response.Redirect("default.aspx", true);
			}
			else
			{
				try
				{
					_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();

					_CalendarDiv = @"<div class=""wm_accountslist_contacts wm_active_tab"">
										<a href=""#"" onclick=""return false;"">" + _manager.GetString("Calendar") + @"</a>
									</div>";
					WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
					if (settings.AllowContacts)
					{
						_ContactsDiv = @"<div class=""wm_accountslist_contacts" + contactsActiveClassName + @""">
											<a href=""" + _contactsLink + @""">" + _manager.GetString("Contacts") + @"</a>
										</div>";
					}

					BaseWebMailActions bwa = new BaseWebMailActions(acct, Page);

					AccountCollection Accounts = bwa.GetAccounts(acct.IDUser);
					//_accountsCount = Accounts.Length;
					if(Accounts.Count > 1)
					{
						AcctList = @"
						<div id=""popup_replace_1"" class=""wm_accountslist_email" + mailActiveClassName + @""">
							<a href="""+_accountsListLink+@""">
								" + acct.Email + @"
							</a>
						</div>
						<div class=""wm_accountslist_selection wm_control"" id=""popup_control_1""
							 onmouseover=""this.className='wm_accountslist_selection_over wm_control';"" 
							 onmousedown=""this.className='wm_accountslist_selection_press wm_control';""
							 onmouseup=""this.className='wm_accountslist_selection_over wm_control';"" 
							 onmouseout=""this.className='wm_accountslist_selection wm_control';"">
						</div>
					";

						string temp = String.Empty;

						for(int i = 0; Accounts.Count > i; i++)
						{
							if(Accounts[i].ID != acct.ID)
							{
								temp += @"
							<div>
							<div id=""" + Accounts[i].ID + @""" onmouseout=""this.className='wm_account_item';"" onmouseover=""this.className='wm_account_item_over';"" onclick=""javascript:ChangeAccount(this.id);"" class=""wm_account_item"">
								" + Accounts[i].Email + @"
							</div>
							</div>
						";
							}
						}

						temp = @"
						<div id=""popup_menu_1"" class=""wm_hide"">
							" + temp + @"
						</div>
					";

						AcctList = temp + AcctList;
					}
					else
					{
						AcctList = @"
						<div class=""wm_accountslist_email""" + mailActiveClassName + @">
							<a href=""" + _accountsListLink + @""">
								" + acct.Email + @"
							</a>
						</div>
						<div class=""wm_accountslist_selection_none""></div>
					";
					}
				}
				catch (Exception)
				{
//					((basewebmail)Page).OutputException(ex);
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
	}
}
