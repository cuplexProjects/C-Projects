using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for CuplexService
/// </summary>
[WebService(Namespace = "http://www.cuplex.se/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

[System.Web.Script.Services.ScriptService]
public class CuplexService : System.Web.Services.WebService
{

    public CuplexService()
    {

    }

    [WebMethod(EnableSession = true)]
    public string RateLink(int LinkRef, int rating)
    {
        return StarVotingControl.RateLink(LinkRef, rating);
    }
    [WebMethod(EnableSession = true)]
    public string VoteOnPoll(int pollOptionRef)
    {
        CuplexLib.User user = HttpContext.Current.Session["User"] as CuplexLib.User;
        if (user == null) return "0";
        return CuplexLib.PollHandler.VoteOnPoll(pollOptionRef, user);
    }
    [WebMethod(EnableSession = true)]
    public string GetPollResult(int pollRef)
    {
        CuplexLib.PollResult pr = CuplexLib.PollHandler.GetPollResult(pollRef);
        if (pr == null) return "";

        return PollResultRender.RenderPollResult(pr);
    }
}