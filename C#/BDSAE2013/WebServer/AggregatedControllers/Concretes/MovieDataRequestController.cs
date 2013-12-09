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
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that gets a movie and all its associated data from the database </returns>
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
                //Get the movie details
                Movies movie = storage.Get<Movies>(movieIndex);

                //Initialize the list in which we store the strings to be parsed into json
                List<string> jsonInput = new List<string>();

                //Compute the information of the movie info associated with the movie
                //Get the list of movie info associated with the movie. Sort the results by type_id
                List<MovieInfo> movieInfoList = storage.Get<MovieInfo>().Where(mi => mi.Movie_Id == movie.Id).OrderBy(x => x.Type_Id).ToList();

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
                    jsonInput.Add("mi" + InfoTypes.GetTypeString((int)movieInfo.Type_Id) + index);
                    jsonInput.Add("" + movieInfo.Info);

                    //Increment the index
                    index++;
                    
                }

                //Compute the information of actors associated with the movie
                //Get the list of participants associated with the movie
                List<Participate> participateList = storage.Get<Participate>().Where(p => p.Movie_Id == movie.Id).ToList();

                //Reset the index, used when assigning attribute names
                index = 0;

                //Iterate through all participants
                foreach (Participate participate in participateList)
                {
                    //TODO differentiate between actors and directors
                    //TODO clean up data to avoid duplicates

                    //Attempt to get the person associated with the participate entity
                    //If the person does not exist in the database, the participate entity is faulty and should be deleted
                    try
                    {
                        //Get the person associated with the participation entity
                        People person = storage.Get<People>((int)participate.Person_Id);

                        //Add all relevant information of the person using the person and the participant entities
                        jsonInput.Add("a" + index + "Id");
                        jsonInput.Add("" + person.Id);
                        jsonInput.Add("a" + index + "Name");
                        jsonInput.Add("" + person.Name);
                        jsonInput.Add("a" + index + "Gender");
                        jsonInput.Add("" + person.Gender);
                        jsonInput.Add("a" + index + "CharacterName");
                        jsonInput.Add("" + participate.CharName);
                        jsonInput.Add("a" + index + "Role");
                        jsonInput.Add("" + participate.Role);
                        jsonInput.Add("a" + index + "Note");
                        jsonInput.Add("" + participate.Note);
                        jsonInput.Add("a" + index + "NrOrder");
                        jsonInput.Add("" + participate.NrOrder);

                        //Increment the index
                        index++;
                    }
                    catch(InvalidOperationException e)
                    {
                        //Deletes the faulty entity from the database
                        //storage.Delete<Participate>(participate.Id);
                    }
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
    }
}
