using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspClient.Models
{
    public class SearchResults
    {
        public string SearchString{ get; set; }

        public Dictionary<int, MovieResult> MovieResults{ get; set; }
        public Dictionary<int, PersonResult> PersonResults { get; set; }
    }

    public struct MovieResult
    {
        public int Id;
        public string Title;
        public string Url;
        public string Plot;
    }

    public struct PersonResult
    {
        public int Id;
        public string Name;
        public string Url;
        public string Biography;
    }
}