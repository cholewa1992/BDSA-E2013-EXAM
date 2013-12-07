using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;
using EntityFrameworkStorage;
using System.Collections.Specialized;
using Utils;

namespace WebServer
{
    /// <summary>
    /// A request controller that handle the rest methods GET, POST, PUT and DELETE.
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    public class MovieDataRequestController : AbstractAggregatedRequestController
    {
        private readonly int searchLimit = 5;
        private Dictionary<int, string> movieInfoTypeTable;

        /// <summary>
        /// The constructor defines the keyword associated with the controller on creation
        /// </summary>
        public MovieDataRequestController()
        {
            Keyword = "MovieData";

            SetupMovieInfoTypeTable();

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        /// <summary>
        /// This method returns a delegate that can be used to get a movie from a given storage.
        /// The id of the movie is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that gets a movie from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Get the request value of the url
            int movieIndex = int.Parse(GetUrlArgument(request.Method));

#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("MovieData Get was invoked... " + "movieIndex: " + movieIndex);
#endif
            //Return the delegate 
            return (storage => 
            {
                Movies movie = storage.Get<Movies>(movieIndex);

                List<MovieInfo> movieInfoList = storage.Get<MovieInfo>().Where(mi => mi.Movie_Id == movie.Id).OrderBy(x => x.Type_Id).ToList();

                List<string> jsonInput = new List<string>();

                int index = 0;
                int currentType = -1;

                foreach (MovieInfo movieInfo in movieInfoList)
                {
                    if (movieInfo.Type_Id != currentType)
                    {
                        currentType = (int)movieInfo.Type_Id;
                        index = 0;
                    }

                    jsonInput.Add("mi"+movieInfo.Type_Id+","+index);
                    jsonInput.Add(""+movieInfo.Info);
                    
                    index++;
                }

                //Convert the object to json attributes
                string json = JSonParser.Parse(new string[]{
                    "id", "" + movie.Id,
                    "title", "" + movie.Title,
                    "year", "" + movie.Year,
                    "kind", "" + movie.Kind,
                    "seasonNumber", "" + movie.SeasonNumber,
                    "seriesYear", "" + movie.SeriesYear,
                    "episodeNumber", "" + movie.EpisodeNumber,
                    "episodeOfId", "" + movie.EpisodeOf_Id}.Concat(
                    jsonInput).ToArray()
                    );

                //return the json encoded as byte code
                return Encoder.Encode(json);
            }
            );
        }

        private void SetupMovieInfoTypeTable()
        {
            movieInfoTypeTable = new Dictionary<int, string>();

            movieInfoTypeTable.Add(2, "displayType");
            movieInfoTypeTable.Add(3, "genre");
            movieInfoTypeTable.Add(4, "language");

        }
    }
}
