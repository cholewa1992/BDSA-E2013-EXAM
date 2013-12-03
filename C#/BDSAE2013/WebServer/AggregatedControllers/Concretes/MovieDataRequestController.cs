using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;
using EntityFrameworkStorage;
using System.Collections.Specialized;

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
        /// <summary>
        /// The constructor defines the keyword associated with the controller on creation
        /// </summary>
        public MovieDataRequestController()
        {
            Keyword = "MovieData";

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
            //Get the values of the given request.
            Dictionary<string,string> values = GetRequestValues(request.Data);

#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("Movie Get was invoked... " + "id: " + values["id"]);
#endif

            int movieId = 2;

            //Return the delegate 
            return (storage => 
            {
                //MovieDataDto movieDataDto = new MovieDataDto();

                
                IEnumerable<Movies> movies = storage.Get<Movies>();
                movies.First((m) => m.Id == movieId);

                IEnumerable<MovieInfo> movieInfo = storage.Get<MovieInfo>();
                movieInfo.Select((mi) => mi.Movie_Id == movieId);


                //return storage.Get<Movies>(int.Parse(values["id"]));

                return new byte[0];
            }
            );
        }
    }
}
