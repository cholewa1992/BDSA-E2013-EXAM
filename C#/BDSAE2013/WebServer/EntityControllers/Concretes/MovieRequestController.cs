using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;
using EntityFrameworkStorage;
using System.Collections.Specialized;
using System.IO;


namespace WebServer
{
    /// <summary>
    /// A request controller that handle the rest methods GET, POST, PUT and DELETE.
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// Morten Rosenmeier (morr@itu.dk)
    /// </author>
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
        /// @pre request != null
        /// @pre request.Method != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that gets a movie from a given storage, based on the contents of the request </returns>
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
            Console.WriteLine("Movie Get was invoked... " + "id: " + id);
            #endif 

            //Return the delegate 
            return (storage =>
            {
                //Get the object from the database
                Movies movie = storage.Get<Movies>(id);

                //Convert the object to json attributes
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

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to post a movie to a given storage.
        /// The parameters of the movie is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that posts a movie to a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("title") || !values.ContainsKey("year") || !values.ContainsKey("kind"))
                throw new InvalidDataException("The data parsed to MovieRequestController post method did not contain enough information to create Movie");

#if DEBUG
            //Post the values to the console
            Console.WriteLine("Movie Post was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Create the object using the vital information from the request
                Movies movie = new Movies()
                {
                    Title = values["title"],
                    Year = int.Parse(values["year"]),
                    Kind = values["kind"]
                };

                //Add any other information given through the request
                if (values.ContainsKey("seasonNumber"))
                    movie.SeasonNumber = int.Parse(values["seasonNumber"]);

                if (values.ContainsKey("episodeNumber"))
                    movie.EpisodeNumber = int.Parse(values["episodeNumber"]);

                if (values.ContainsKey("seriesYear"))
                    movie.SeriesYear = values["seriesYear"];
                else
                    movie.SeriesYear = "";

                if (values.ContainsKey("episodeOfId"))
                    movie.EpisodeOf_Id = int.Parse(values["episodeOfId"]);

                //Add the movie to the database
                storage.Add<Movies>(movie);

                //Set the response as json
                string json = JSonParser.Parse(
                    "response", "The Movie was successfully added"
                    );

                //Return the byte encoded json
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to update a movie in a given storage.
        /// The id of the movie to update, as well as the parameters to be updated is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that updates a movie in a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to MovieRequestController put method did not contain an id");

#if DEBUG
            //Print the values to the console (should be deleted before release)
            Console.WriteLine("Movie Put was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Get the movie to update from the database
                Movies movie = storage.Get<Movies>(int.Parse(values["id"]));

                //Update any other information given through the request
                if (values.ContainsKey("title"))
                    movie.Title = values["title"];

                if (values.ContainsKey("year"))
                    movie.Year = int.Parse(values["year"]);

                if (values.ContainsKey("kind"))
                    movie.Kind = values["kind"];

                if (values.ContainsKey("seasonNumber"))
                    movie.SeasonNumber = int.Parse(values["seasonNumber"]);

                if (values.ContainsKey("episodeNumber"))
                    movie.EpisodeNumber = int.Parse(values["seasonNumber"]);

                if (values.ContainsKey("seriesYear"))
                    movie.SeriesYear = values["seriesYear"];

                if (values.ContainsKey("episodeOfId"))
                    movie.EpisodeOf_Id = int.Parse(values["episodeOfId"]);

                //Update the movie in the database
                storage.Update<Movies>(movie);

                //Set the json response message
                string json = JSonParser.Parse(
                    "response", "The Movie was successfully updated"
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete a movie from a given storage.
        /// The id of the movie is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that deletes a movie from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to MovieRequestController delete method did not contain an id");

            //Print the values to the console (should be deleted before release)
            Console.WriteLine("Movie Delete was invoked... " + "id: " + values["id"]);

            //Return the delegate 
            return (storage =>
            {
                var id = int.Parse(values["id"]);
                
                //Delete the movie from the database
                storage.Delete<Movies>(id);

                //Set the json response message
                string json = JSonParser.Parse(
                    "response", "The Movie was successfully deleted"
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }

    }
}
