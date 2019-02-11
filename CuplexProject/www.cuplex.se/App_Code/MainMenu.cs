using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using CLinq = CuplexLib.Linq;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for MainMenu
/// </summary>

namespace CustomUserControls
{
    public class MainMenu : Panel
    {
        private List<MenuItem> _menuItemList = null;
        public MainMenu()
        {

        }
        protected override void OnLoad(EventArgs e)
        {
            _menuItemList = MenuItem.GetRootMenuItemList();
            base.OnLoad(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            if (_menuItemList != null)
            {
                this.Controls.Clear();

                foreach(MenuItem mi in _menuItemList)
                {
                    HtmlGenericControl menuItemDiv = new HtmlGenericControl("div");
                    menuItemDiv.Attributes.Add("class", "menuItem");
                    menuItemDiv.InnerHtml = mi.MenuText;

                    if (!string.IsNullOrEmpty(mi.PageUrl))
                    {
                        HtmlGenericControl pageLink = new HtmlGenericControl("a");
                        pageLink.Attributes.Add("href", FormatUrl(mi.PageUrl));
                        pageLink.Controls.Add(menuItemDiv);
                        this.Controls.Add(pageLink);

                    }
                    else
                        this.Controls.Add(menuItemDiv);
                }
            }
            base.OnPreRender(e);
        }
        private string FormatUrl(string url)
        {
            return cms.Current.GetRootPath + url.Replace("{home}", "");
        }

        public class MenuItem
        {
            public int MenuItemRef { get; set; }
            public int MenuLevel { get; set; }
            public int? ParentRef { get; set; }
            public int? SortOrder { get; set; }
            public string MenuText { get; set; }
            public string ResourceKey { get; set; }
            public string PageUrl { get; set; }
            public bool Visible { get; set; }

            private List<MenuItem> childNodes = null;
            private MenuItem parentNode = null;

            public static List<MenuItem> GetRootMenuItemList()
            {
                List<MenuItem> mainMenuList = HttpRuntime.Cache["MainMenuList"] as List<MenuItem>;
                if (mainMenuList == null)
                {
                    mainMenuList = new List<MenuItem>();
                    using (var db = CLinq.DataContext.Create())
                    {
                        var menuQuery =
                        from m in db.MenuItems
                        where m.Visible
                        orderby m.SortOrder
                        select m;

                        var menuList = menuQuery.ToList();
                        var rootItems = menuList.Where(x => x.MenuLevel == 0 && x.ParentRef == null).ToList();

                        foreach (var item in rootItems)
                        {
                            MenuItem mi = new MenuItem();
                            mi.MenuItemRef = item.MenuItemRef;
                            mi.MenuLevel = item.MenuLevel;
                            mi.MenuText = item.MenuText;
                            mi.ParentRef = item.ParentRef;
                            mi.ResourceKey = item.ResourceKey;
                            mi.SortOrder = item.SortOrder;
                            mi.PageUrl = item.PageUrl;
                            mi.Visible = true;

                            mi.childNodes = GetChildMenuItems(1, mi.MenuItemRef, menuList);

                            mainMenuList.Add(mi);
                        }
                    }

                    HttpRuntime.Cache.Insert("MainMenuList", mainMenuList, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                return mainMenuList;
            }
            private static List<MenuItem> GetChildMenuItems(int menuLevel, int menuItemRef, List<CLinq.MenuItem> menuItemList)
            {
                List<MenuItem> menuItemListRet = null;

                var childNodes = menuItemList.Where(x => x.MenuLevel == menuLevel && x.ParentRef == menuItemRef).ToList();
                if (childNodes.Count > 0)
                {
                    menuItemListRet = new List<MenuItem>();
                    foreach (var node in childNodes)
                    {
                        MenuItem mi = new MenuItem();
                        mi.MenuItemRef = node.MenuItemRef;
                        mi.MenuLevel = node.MenuLevel;
                        mi.MenuText = node.MenuText;
                        mi.ParentRef = menuItemRef;
                        mi.ResourceKey = node.ResourceKey;
                        mi.SortOrder = node.SortOrder;
                        mi.PageUrl = node.PageUrl;
                        mi.Visible = true;

                        mi.childNodes = GetChildMenuItems(menuLevel + 1, mi.MenuItemRef, menuItemList);
                        menuItemListRet.Add(mi);
                    }
                }

                return menuItemListRet;
            }
        }
    }
}