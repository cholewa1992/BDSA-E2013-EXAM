using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using CommunicationFramework;
using Storage;

namespace WebServer
{
    /// <summary>
    /// A request controller that handle the rest methods GET, POST, PUT and DELETE.
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    public class MovieInfoRequestController : AbstractEntityRequestController
    {
        /// <summary>
        /// The constructor defines the keyword associated in controller on creation
        /// </summary>
        public MovieInfoRequestController()
        {
            Keyword = "MovieInfo";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        /// <summary>
        /// This method returns a delegate that can be used to get movie info from a given storage.
        /// The id of the movie info is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that get movie info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Get the request value of the url
            int id = int.Parse(GetUrlArgument(request.Method));
            
#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("Movie info Get was invoked... " + "id: " + id);
#endif

            //Return the delegate 
            return (storage =>
            {
                MovieInfo movieInfo = storage.Get<MovieInfo>(id);

                string json = JSonParser.Parse(
                    "id", "" + movieInfo.Id,
                    "info", "" + movieInfo.Info,
                    "movieId", "" + movieInfo.Movie_Id,
                    "movieInfoId", "" + movieInfo.MovieInfoId,
                    "note", "" + movieInfo.Note,
                    "typeId", "" + movieInfo.Type_Id
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to post movie info to a given storage.
        /// The parameters of the movie info is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that posts movie info to a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            //Get the values from the request
            Dictionary<string,string> values = GetRequestValues(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Movie info Post was invoked..."
                + " info: " + values["info"]
                + " note: " + values["note"]
                + " movieId: " + values["movieId"]
                + " typeId: " + values["typeId"]
                );

            //(Should we create an object to parse or parse parameters?)
            MovieInfo movieInfo = new MovieInfo()
            {
                Info= values["info"],
                Note = values["note"],
                Movie_Id = int.Parse(values["movieId"]),
                Type_Id = int.Parse(values["typeId"])
            };

            //Return the delegate 
            return (storage =>
            {
                storage.Add<MovieInfo>(movieInfo);

                string json = JSonParser.Parse(
                    "response", "The movie info was successfully added"
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to update movie info in a given storage.
        /// The id of the movie info to update, as well as the parameters to be updated is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that updates movie info in a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {
            //Get the values from the request
            Dictionary<string,string> values = GetRequestValues(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Movie info Post was invoked..."
                + " info: " + values["info"]
                + " note: " + values["note"]
                + " movieId: " + values["movieId"]
                + " typeId: " + values["typeId"]
                );

            //(Should we create an object to parse or parse parameters?)
            MovieInfo movieInfo = new MovieInfo()
            {
                Id = int.Parse(values["id"]),
                Info = values["info"],
                Note = values["note"],
                Movie_Id = int.Parse(values["movieId"]),
                Type_Id = int.Parse(values["typeId"])
            };

            //Return the delegate 
            return (storage =>
            {
                storage.Update<MovieInfo>(movieInfo);

                string json = JSonParser.Parse(
                    "response", "The movie info was successfully updated"
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete movie info from a given storage.
        /// The id of the movie info is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that deletes movie info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            Dictionary<string,string> values = GetRequestValues(request.Data);
            Console.WriteLine("Movie info Delete was invoked... " + "id: " + values["id"]);

            var id = int.Parse(values["id"]);

            //Return the delegate 
            return (storage =>
            {
                storage.Delete<MovieInfo>(id);

                string json = JSonParser.Parse(
                    "response", "The movie info was successfully deleted"
                    );

                return Encoder.Encode(json);
            });
        }

    }
}
