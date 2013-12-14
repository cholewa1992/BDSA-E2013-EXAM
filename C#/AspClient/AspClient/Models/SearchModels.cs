using System.Collections.Generic;
namespace AspClient.Models
{
    public class SearchResults
    {
        public Dictionary<int, MovieResult> MovieResults{ get; set; }
        public Dictionary<int, PersonResult> PersonResults { get; set; }
    }

    public struct MovieResult
    {
        public int Id;
        public string Title;
        public string Plot;
    }

    public struct PersonResult
    {
        public int Id;
        public string Name;
        public string Biography;
    }
}