using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using CuplexLib;
using CLinq = CuplexLib.Linq;
using System.Web.UI;

public class StarVotingControl : Panel
{
    public double Rating { get; set; }
    public int LinkRef { get; set; }
    public int? UserRef { get; set; }
    public bool HasVoted { get; set; }
    private const int NumberOfStars = 5;    

    public StarVotingControl()
    {
        this.Rating = 0;
    }
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }
    protected override void OnPreRender(EventArgs e)
    {
        this.CssClass = "ratingStarBlock";
        if (this.UserRef != null)
        {
            this.CssClass += " userActive";
            registerScripts();
        }

        for (int i = 1; i <= NumberOfStars; i++)
        {
            HtmlGenericControl starContainer = new HtmlGenericControl("div");
            HtmlGenericControl starLink = new HtmlGenericControl("a");

            starContainer.Attributes.Add("class", "star");            
            starLink.Attributes.Add("href", "#" + i.ToString());

            if (Rating >= i)
            {
                starLink.Style.Add("width", "100%");
                starContainer.Attributes["class"] += " on";
            }
            else if (Math.Ceiling(Rating) == i)
            {
                int percentage = Convert.ToInt32(Math.Round((Rating - Math.Floor(Rating)) * 100, 0));
                starLink.Style.Add("width", percentage + "%");                
                starContainer.Attributes["class"] += " on";
            }
            else
                starLink.Style.Add("width", "100%");

            if (this.UserRef != null)
            {
                starLink.Attributes.Add("onclick", "CuplexService.RateLink(" + LinkRef.ToString() + "," + i.ToString() + ",RateLinkCallback); return false;");
                if (!HasVoted)
                    starLink.Attributes.Add("title", i.ToString() + " av 5");
            }
            else
                starLink.Attributes.Add("onclick", "return false");

            if (this.UserRef == null || HasVoted)
                starContainer.Attributes["class"] += " done";

            starContainer.Controls.Add(starLink);
            this.Controls.Add(starContainer);            
        }

        this.Attributes.Add("title", Math.Round(Rating, 2).ToString() + " av 5");
        
    }
    private void registerScripts()
    {
        string script = "RegisterStarVotingEvents();";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "StarVotingControl", script, true);
    }

    public static string RateLink(int LinkRef, int rating)
    {
        CuplexLib.User user = HttpContext.Current.Session["User"] as CuplexLib.User;
        if (user == null || rating < 1 || rating > 5)
            return "";

        using (CLinq.DataContext db = CLinq.DataContext.Create())
        {
            if (!db.UserToLinks.Where(utl => utl.LinkRef == LinkRef && utl.UserRef == user.UserRef).Any() && db.Links.Where(l => l.LinkRef == LinkRef).Any())
            {
                CLinq.UserToLink userToLink = new CuplexLib.Linq.UserToLink();
                userToLink.UserRef = user.UserRef;
                userToLink.LinkRef = LinkRef;
                userToLink.Rating = rating;
                db.UserToLinks.InsertOnSubmit(userToLink);
                db.SubmitChanges();

                //Recalculate rating on Link
                var linkRatingQuery =
                from utl in db.UserToLinks
                where utl.LinkRef == LinkRef
                select utl.Rating;

                var linkToUpdate = db.Links.Where(l => l.LinkRef == LinkRef).Single();
                linkToUpdate.Rating = (double)linkRatingQuery.Sum() / (double)linkRatingQuery.Count();
                db.SubmitChanges();
            }
        }
        return "";
    }
}
