using System.Collections.Generic;

namespace MvcApplication2.Models
{
    public class HomeModels
    {
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Kind { get; set; }
        public string Year { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public string SeriesYear { get; set; }
        public int EpisodeOfId { get; set; }

        public Movie(string jsonString)
        {
            
        }
    }
}