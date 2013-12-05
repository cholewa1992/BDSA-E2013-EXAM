using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspClient.Models
{
    public class SearchResults
    {
        public string SearchString{ get; set; }

        public Dictionary<String, String> Results{ get; set; }
    }
}