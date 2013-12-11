using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspClient.Models
{
    public interface DataModel
    {
        string SearchString{ get; set; }
        int Id{ get; set; }
    }

    public class MovieModel : DataModel
    {
        public string SearchString{ get; set;  }

        public int Id{ get; set; }
        public string Title{ get; set; }
        public string Year{ get; set; }
        public string Kind{ get; set; }
        public string SeasonNumber{ get; set; }
        public string SeriesYear{ get; set; }
        public string EpisodeNumber{ get; set; }
        public string EpisodeOfId{ get; set; }

        public List<ActorModel> cast;
    }

    public class PersonMovieModel : DataModel
    {
        public string SearchString{ get; set; }

        public int Id{ get; set; }
        public string Title{ get; set; }
        public string Kind{ get; set; }
        public string Year{ get; set; }
        public string CharacterName{ get; set; }
        public string Role{ get; set; }
        public string Note{ get; set; }
        public int NumberOrder{ get; set; }
    }

    public class PersonModel : DataModel
    {
        public string SearchString{ get; set; }
        
        public int Id{ get; set; }
        public string Name{ get; set; }
        public string Gender{ get; set; }
        public string BirthDate{ get; set; }

        public List<PersonMovieModel> starring;
    }

    public class ActorModel : DataModel
    {
        public string SearchString{ get; set; }

        public int Id{ get; set; }
        public string Name{ get; set; }
        public string Gender{ get; set; }
        public string CharacterName{ get; set; }
        public string Role{ get; set; }
        public string Note{ get; set; }
        public int NumberOrder{ get; set; }
    }
}