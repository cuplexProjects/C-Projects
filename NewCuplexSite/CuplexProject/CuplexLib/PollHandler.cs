using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLinq = CuplexLib.Linq;

namespace CuplexLib
{
    public class PollHandler
    {
        public static string VoteOnPoll(int pollOptionRef, User user)
        {
            using (var db = CLinq.DataContext.Create())
            {
                //Verify that the poll is active and exists
                var poll = db.Polls.Where(p => p.IsActive && p.PollOptions.Any(po => po.PollOptionRef == pollOptionRef)).SingleOrDefault();
                if (poll == null) return "0";

                //Verify that the user have'nt already voted.
                if (db.UserToPollOptions.Any(x => x.UserRef == user.UserRef && x.PollOption.PollRef == poll.PollRef))
                    return "0";

                CLinq.UserToPollOption utpo = new CuplexLib.Linq.UserToPollOption();
                utpo.UserRef = user.UserRef;
                utpo.PollOptionRef = pollOptionRef;
                utpo.RateDate = DateTime.Now;
                db.UserToPollOptions.InsertOnSubmit(utpo);
                db.SubmitChanges();

                //Recalculate score
                int pollOptionRatings = db.UserToPollOptions.Where(x => x.PollOption.PollRef == poll.PollRef).Count();
                var pollOptionList = poll.PollOptions.ToList();
                foreach (var po in pollOptionList)
                    po.Rating = (double)po.UserToPollOptions.Count / (double)pollOptionRatings;

                db.SubmitChanges();
                return poll.PollRef.ToString();
            }
        }
        public static PollResult GetPollResult(int pollRef)
        {
            PollResult pollResult = new PollResult();

            using (var db = CLinq.DataContext.Create())
            {
                var poll = db.Polls.Where(p => p.PollRef == pollRef).SingleOrDefault();
                if (poll == null) return null;

                pollResult.PollRef = poll.PollRef;
                pollResult.PollDescription = poll.Description;

                var pollOptionList = poll.PollOptions.ToList();
                foreach (var po in pollOptionList)
                {
                    PollResultOption pro = new PollResultOption();
                    pro.PollOptionName = po.OptionName;
                    pro.PollOptionRef = po.PollOptionRef;
                    pro.Rating = po.Rating;
                    pro.Ratings = po.UserToPollOptions.Count;
                    pro.SortOrder = po.SortOrder;

                    pollResult.PollResultOptionList.Add(pro);
                }
            }
            pollResult.PollResultOptionList.Sort();
            return pollResult;
        }
    }

    public class PollResult
    {
        public int PollRef { get; set; }
        public string PollDescription { get; set; }
        public List<PollResultOption> PollResultOptionList { get; private set; }

        public PollResult()
        {
            PollResultOptionList = new List<PollResultOption>();
        }
    }
    public class PollResultOption : IComparable<PollResultOption>
    {
        public int PollOptionRef { get; set; }
        public string PollOptionName { get; set; }
        public double Rating { get; set; }
        public int Ratings { get; set; }
        public int? SortOrder { get; set; }
        
        public int CompareTo(PollResultOption other)
        {
            if (this.SortOrder != null && other.SortOrder != null)
            {
                if (this.SortOrder > other.SortOrder)
                    return 1;
                else if (this.SortOrder < other.SortOrder)
                    return -1;
                else
                    return 0;
            }
            else
                return this.PollOptionName.CompareTo(other.PollOptionName);
        }
    }
}