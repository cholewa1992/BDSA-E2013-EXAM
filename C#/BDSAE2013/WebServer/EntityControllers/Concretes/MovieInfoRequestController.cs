using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkStorage;
using Utils;
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
        /// @pre request != null
        /// @pre request.Method != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that get movie info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Method == null)
                throw new ArgumentNullException("Incoming request method must not be null");

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
                    "note", "" + movieInfo.Note,
                    "typeId", "" + movieInfo.Type_Id
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to post movie info to a given storage.
        /// The parameters of the movie info is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that posts movie info to a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            //Get the values from the request
            Dictionary<string,string> values = GetRequestValues(request.Data);

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("movieId") || !values.ContainsKey("typeId") || !values.ContainsKey("info"))
                throw new InvalidDataException("The data parsed to MovieInfoRequestController post method did not contain enough information to create MovieInfo");

#if DEBUG
            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Movie info Post was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Create the object to be added to the database using all the required information of the request
                MovieInfo movieInfo = new MovieInfo()
                {
                    Info = values["info"],
                    Movie_Id = int.Parse(values["movieId"]),
                    Type_Id = int.Parse(values["typeId"])
                };

                //If the request also contains information about the note we add the information as well
                if (values.ContainsKey("note"))
                    movieInfo.Note = values["note"];
                else
                    movieInfo.Note = "";

                //Add the movie info to the database
                storage.Add<MovieInfo>(movieInfo);

                //Compute the json with the response
                string json = JSonParser.Parse(
                    "response", "The MovieInfo was successfully added"
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to update movie info in a given storage.
        /// The id of the movie info to update, as well as the parameters to be updated is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that updates movie info in a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            //Get the values from the request
            Dictionary<string,string> values = GetRequestValues(request.Data);

            //Check for any vital information in the request
            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to MovieInfoRequestController put did not contain the required id");

#if DEBUG
            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Movie info Post was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Get the object to be updated from the database
                MovieInfo movieInfo = storage.Get<MovieInfo>(int.Parse(values["id"]));

                if(values.ContainsKey("info"))
                    movieInfo.Info = values["info"];

                if(values.ContainsKey("note"))
                    movieInfo.Note = values["note"];

                if(values.ContainsKey("movieId"))
                    movieInfo.Movie_Id = int.Parse(values["movieId"]);

                if(values.ContainsKey("typeId"))
                    movieInfo.Type_Id = int.Parse(values["typeId"]);

                //Update the movie info entity in the database
                storage.Update<MovieInfo>(movieInfo);

                //Set the response string as json
                string json = JSonParser.Parse(
                    "response", "The MovieInfo was successfully updated"
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete movie info from a given storage.
        /// The id of the movie info is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that deletes movie info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            Dictionary<string,string> values = GetRequestValues(request.Data);

            //Check for any vital information in the request
            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to MovieInfoRequestController delete did not contain the required id");

#if DEBUG
            Console.WriteLine("Movie info Delete was invoked... " + "id: " + values["id"]);
#endif

            //Return the delegate 
            return (storage =>
            {
                var id = int.Parse(values["id"]);

                storage.Delete<MovieInfo>(id);

                string json = JSonParser.Parse(
                    "response", "The MovieInfo was successfully deleted"
                    );

                return Encoder.Encode(json);
            });
        }

    }
}
