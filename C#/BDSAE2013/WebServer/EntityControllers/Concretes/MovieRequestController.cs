using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;
using EntityFrameworkStorage;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;


namespace WebServer
{
    /// <summary>
    /// A request controller that handle the rest methods GET, POST, PUT and DELETE.
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    public class MovieRequestController : AbstractEntityRequestController
    {
        /// <summary>
        /// The constructor defines the keyword associated with the controller on creation
        /// </summary>
        public MovieRequestController()
        {
            Keyword = "Movie";

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
            //TODO make checks

            //Get the request value of the url
            int id = int.Parse(GetUrlArgument(request.Method));
            
            #if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("Movie Get was invoked... " + "id: " + id);
            #endif 

            //Return the delegate 
            return (storage =>
            {
                Movies movie = storage.Get<Movies>(id);

                string json = JSonParser.Parse(
                    "id", "" + movie.Id,
                    "title", "" + movie.Title,
                    "year", "" + movie.Year,
                    "kind", "" + movie.Kind,
                    "seasonNumber", "" + movie.SeasonNumber,
                    "seriesYear", "" + movie.SeriesYear,
                    "episodeNumber", "" + movie.EpisodeNumber,
                    "episodeOfId", "" + movie.EpisodeOf_Id
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to post a movie to a given storage.
        /// The parameters of the movie is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that posts a movie to a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);
            
            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Movie Post was invoked..."
                + " title: " + values["title"]
                + " kind: " + values["kind"]
                + " year: " + values["year"]
                + " seasonNumber: " + values["seasonNumber"]
                + " episodeNumber: " + values["episodeNumber"]
                + " seriesYear: " + values["seriesYear"]
                + " episodeOfId: " + values["episodeOfId"]
                );

            //(Should we create an byte[] to parse or parse parameters?)
            Movies movie = new Movies()
            {
                Title = values["title"],
                Kind = values["kind"],
                Year = int.Parse(values["year"]),
                SeasonNumber = int.Parse(values["seasonNumber"]),
                EpisodeNumber = int.Parse(values["episodeNumber"]),
                SeriesYear = values["seriesYear"],
                EpisodeOf_Id = int.Parse(values["episodeOfId"])
            };

            //Return the delegate 
            return (storage =>
            {
                storage.Add<Movies>(movie);

                string json = JSonParser.Parse(
                    "response", "The movie was successfully added"
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to update a movie in a given storage.
        /// The id of the movie to update, as well as the parameters to be updated is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that updates a movie in a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);
            
            //Print the values to the console (should be deleted before release)
            Console.WriteLine("Movie Put was invoked..."
                + " id: " + values["id"]
                + " title: " + values["title"]
                + " kind: " + values["kind"]
                + " year: " + values["year"]
                + " seasonNumber: " + values["seasonNumber"]
                + " episodeNumber: " + values["episodeNumber"]
                + " seriesYear: " + values["seriesYear"]
                + " episodeOfId: " + values["episodeOfId"]
                );

            //Should we create an byte[] to parse or parse parameters?
            Movies movie = new Movies()
            {
                Id = int.Parse(values["id"]),
                Title = values["title"],
                Kind = values["kind"],
                Year = int.Parse(values["year"]),
                SeasonNumber = int.Parse(values["seasonNumber"]),
                EpisodeNumber = int.Parse(values["episodeNumber"]),
                SeriesYear = values["seriesYear"],
                EpisodeOf_Id = int.Parse(values["episodeOfId"])
            };

            //Return the delegate 
            return (storage =>
            {
                storage.Update<Movies>(movie);

                string json = JSonParser.Parse(
                    "response", "The movie was successfully updated"
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete a movie from a given storage.
        /// The id of the movie is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that deletes a movie from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);
            
            //Print the values to the console (should be deleted before release)
            Console.WriteLine("Movie Delete was invoked... " + "id: " + values["id"]);

            var id = int.Parse(values["id"]);

            //Return the delegate 
            return (storage =>
            {
                storage.Delete<Movies>(id);

                string json = JSonParser.Parse(
                    "response", "The movie was successfully Delete"
                    );

                return Encoder.Encode(json);
            });
        }

    }
}
