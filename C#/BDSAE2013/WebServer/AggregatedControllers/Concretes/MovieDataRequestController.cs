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
        /// This method returns a delegate that can be used to get a complete set of data about a specified moive
        /// The id of the movie is determined by the parsed request
        /// @pre request != null
        /// @pre request.Method != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that gets a movie and all its associated data from the database </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Method == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            //Get the request value of the url
            int movieIndex = int.Parse(GetUrlArgument(request.Method));

#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("MovieData Get was invoked... " + "movieIndex: " + movieIndex);
#endif

            //Return the delegate 
            return (storage => 
            {
                //Get the movie details
                Movies movie = storage.Get<Movies>(movieIndex);

                //Initialize the list in which we store the strings to be parsed into json
                List<string> jsonInput = new List<string>();

                //Compute the information of the movie info associated with the movie
                //Get the list of movie info associated with the movie. Sort the results by type_id
                var movieInfoList = storage.Get<MovieInfo>().Where(mi => mi.Movie_Id == movie.Id).OrderBy(x => x.Type_Id);

                //Initialize an array of strings to contain the information we get by iteration through the information of each movie info
                string[] movieInfoStringArray = new string[movieInfoList.Count()*4];
                int movieInfoStringArrayIndex = 0;

                //Set up an index to differentiate each movie info
                int index = 0;
                //Set the current type of movie info to an invalid number
                int currentType = -1;

                //Iterate through all associated movie info
                foreach (MovieInfo movieInfo in movieInfoList)
                {
                    //Check if the type id is null - if it is, we skip the addition of the current information
                    if (movieInfo.Type_Id == null)
                        continue;

                    if (movieInfo.Type_Id != currentType)
                    {
                        //If the type id of the current movie info is different from the type id, we reset the index.
                        index = 0;

                        //Furthermore we set the current type to be the type id of the current movie info
                        currentType = (int)movieInfo.Type_Id;
                    }

                    //We add the attribute name and value of the current movie info, based on it's type and the index of the current type
                    movieInfoStringArray[movieInfoStringArrayIndex++] = ("mi" + InfoTypes.GetTypeString((int)movieInfo.Type_Id) + index + "Id");
                    movieInfoStringArray[movieInfoStringArrayIndex++] = ("" + movieInfo.Id);
                    movieInfoStringArray[movieInfoStringArrayIndex++] = ("mi" + InfoTypes.GetTypeString((int)movieInfo.Type_Id) + index + "Info");
                    movieInfoStringArray[movieInfoStringArrayIndex++] = ("" + movieInfo.Info);

                    //Increment the index
                    index++;
                }

                //We concat the json input list with the array of found movie info
                jsonInput = jsonInput.Concat(movieInfoStringArray).ToList();

                //Compute the information of actors associated with the movie
                //Get the list of participants associated with the movie
                var participateList = storage.Get<Participate>().Where(p => p.Movie_Id == movie.Id);

                //Initialize an array of strings to contain the information we get by iteration through the information of each participant link
                string[] personStringArray = new string[participateList.Count() * 14];
                int personStringArrayIndex = 0;

                //Reset the index, used when assigning attribute names
                index = 0;

                //Iterate through all participants
                foreach (Participate participate in participateList)
                {
                    //TODO differentiate between actors and directors
                    //TODO clean up data to avoid duplicates

                    //Attempt to get the person associated with the participate entity
                    //If the person does not exist in the database, the participate entity is faulty and should be deleted
                    if(participate.People == null)
                        continue;

                    //Get the person associated with the participation entity
                    People person = participate.People;
                        
                    //Add all relevant information of the person using the person and the participant entities
                    personStringArray[personStringArrayIndex++] = ("p" + index + "Id");
                    personStringArray[personStringArrayIndex++] = ("" + person.Id);
                    personStringArray[personStringArrayIndex++] = ("p" + index + "Name");
                    personStringArray[personStringArrayIndex++] = ("" + person.Name);
                    personStringArray[personStringArrayIndex++] = ("p" + index + "Gender");
                    personStringArray[personStringArrayIndex++] = ("" + person.Gender);
                    personStringArray[personStringArrayIndex++] = ("p" + index + "CharacterName");
                    personStringArray[personStringArrayIndex++] = ("" + participate.CharName);
                    personStringArray[personStringArrayIndex++] = ("p" + index + "Role");
                    personStringArray[personStringArrayIndex++] = ("" + participate.Role);
                    personStringArray[personStringArrayIndex++] = ("p" + index + "Note");
                    personStringArray[personStringArrayIndex++] = ("" + participate.Note);
                    personStringArray[personStringArrayIndex++] = ("p" + index + "NrOrder");
                    personStringArray[personStringArrayIndex++] = ("" + participate.NrOrder);

                    //Increment the index
                    index++;
                }

                //We concat the json input list with the array of found person info
                jsonInput = jsonInput.Concat(personStringArray).ToList();


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
    }
}
