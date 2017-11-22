using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apps.cuplex.se.Models
{
    public class AppsListViewModel
    {
        public List<AppViewModel> ApplicationsList { get; set; }
        public string ListDescription { get; set; }
    }
}