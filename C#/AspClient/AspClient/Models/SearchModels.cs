using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspClient.Models
{
    public class SearchResults
    {
        public string SearchString{ get; set; }

        public Dictionary<int, MovieResult> Results{ get; set; }
    }

    public class MovieResult
    {
        public int Id;
        public string Title;
        public string Url;
    }

    public class PersonResult
    {
        public int Id;
        public string Name;
        public string Url;
    }
}