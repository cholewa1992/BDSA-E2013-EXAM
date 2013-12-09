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
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that gets a person and all their associated data from the database </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
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
                List<FavouritedMovie> favouritedMoviesList = storage.Get<FavouritedMovie>().Where(fm => fm.FLId == favouriteList.Id).ToList();

                int index = 0;

                //Iterate through all associated movie info
                foreach (FavouritedMovie favouritedMovie in favouritedMoviesList)
                {
                    //Check if the movie id is null - if it is, we skip the addition of the current information
                    if (favouritedMovie.MovieId == null)
                        continue;

                    //Attempt to get the movie associated with the participate entity
                    //If the movie does not exist in the database, the favouritedMovie entity is faulty and should be deleted
                    try
                    {
                        //Get the movie associated with the participation entity
                        Movies movie = storage.Get<Movies>((int)favouritedMovie.MovieId);

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
                    catch (InvalidOperationException e)
                    {
                        //Deletes the faulty entity from the database
                        //storage.Delete<FavouritedMovie>(favouriteList.Id);
                    }
                }

                //Convert the object to json attributes
                string json = JSonParser.Parse(new string[]{
                    "id", "" + favouriteList.Id,
                    "title", "" + favouriteList.Title,
                    "userAccountId", "" + favouriteList.UserAccId}.Concat(
                    jsonInput).ToArray()
                    );

                //return the json encoded as byte code
                return Encoder.Encode(json);
            }
            );
        }
    }
}
