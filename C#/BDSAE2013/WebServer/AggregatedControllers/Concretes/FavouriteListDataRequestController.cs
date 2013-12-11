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
    /// A aggregated request controller that handle the rest methods GET
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    public class FavouriteListDataRequestController : AbstractAggregatedRequestController
    {
        /// <summary>
        /// The constructor defines the keyword associated with the controller on creation
        /// </summary>
        public FavouriteListDataRequestController()
        {
            Keyword = "FavouriteListData";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        /// <summary>
        /// This method returns a delegate that can be used to get a complete set of data about a specified person
        /// The id of the person is determined by the parsed request
        /// @pre request != null
        /// @pre request.Method != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that gets a person and all their associated data from the database </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Method == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            //Get the request value of the url
            int favouriteListId = int.Parse(GetUrlArgument(request.Method));

#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("FavouriteListData Get was invoked... " + "favouriteListId: " + favouriteListId);
#endif

            //Return the delegate 
            return (storage => 
            {
                //Get the favourite list details
                FavouriteList favouriteList = storage.Get<FavouriteList>(favouriteListId);

                //Initialize the list in which we store the strings to be parsed into json
                List<string> jsonInput = new List<string>();
                
                //Compute the information of the person info associated with the person
                //Get the list of person info associated with the person. Sort the results by type_id
                List<FavouritedMovie> favouritedMoviesList = storage.Get<FavouritedMovie>().Where(fm => fm.FavList_Id == favouriteList.Id).ToList();

                int index = 0;

                //Iterate through all associated movie info
                foreach (FavouritedMovie favouritedMovie in favouritedMoviesList)
                {
                    //Attempt to get the movies associated with the favourite movie entity
                    //If the person does not exist in the database, the participate entity is faulty and should be deleted
                    if (favouritedMovie.Movies == null)
                        continue;

                    //Get the movie associated with the participation entity
                    Movies movie = favouritedMovie.Movies;

                    //Add all relevant information of the movie using the movie
                    jsonInput.Add("m" + index + "Id");
                    jsonInput.Add("" + movie.Id);
                    jsonInput.Add("m" + index + "Title");
                    jsonInput.Add("" + movie.Title);
                    jsonInput.Add("m" + index + "Kind");
                    jsonInput.Add("" + movie.Kind);
                    jsonInput.Add("m" + index + "Year");
                    jsonInput.Add("" + movie.Year);

                    //Increment the index
                    index++;
                }

                //Convert the object to json attributes
                string json = JSonParser.Parse(new string[]{
                    "id", "" + favouriteList.Id,
                    "title", "" + favouriteList.Title,
                    "userAccountId", "" + favouriteList.UserAcc_Id}.Concat(
                    jsonInput).ToArray()
                    );

                //return the json encoded as byte code
                return Encoder.Encode(json);
            }
            );
        }
    }
}
