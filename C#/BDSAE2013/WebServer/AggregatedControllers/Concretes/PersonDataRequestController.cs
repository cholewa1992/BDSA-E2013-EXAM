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
    /// An aggregated request controller that handle the rest methods GET
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
    public class PersonDataRequestController : AbstractAggregatedRequestController
    {
        /// <summary>
        /// The constructor defines the keyword associated with the controller on creation
        /// </summary>
        public PersonDataRequestController()
        {
            Keyword = "PersonData";

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
            int personId = int.Parse(GetUrlArgument(request.Method));

#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("PersonData Get was invoked... " + "personId: " + personId);
#endif

            //Return the delegate 
            return (storage => 
            {
                //Get the person details
                People person = storage.Get<People>(personId);

                //Initialize the list in which we store the strings to be parsed into json
                List<string> jsonInput = new List<string>();

                //Compute the information of the person info associated with the person
                //Get the list of person info associated with the person. Sort the results by type_id
                var personInfoList = person.PersonInfo.OrderBy(x => x.Type_Id);

                //Initialize an array of strings to contain the information we get by iteration through the information of each person info
                string[] personInfoStringArray = new string[personInfoList.Count() * 4];
                int personInfoStringArrayIndex = 0;

                //Set up an index to differentiate each person info
                int index = 0;
                //Set the current type of person info to an invalid number
                int currentType = -1;

                //Iterate through all associated movie info
                foreach (PersonInfo personInfo in personInfoList)
                {
                    if (personInfo.Type_Id != currentType)
                    {
                        //If the type id of the current person info is different from the type id, we reset the index.
                        index = 0;

                        //Furthermore we set the current type to be the type id of the current person info
                        currentType = personInfo.Type_Id;
                    }

                    //We add the attribute name and value of the current person info, based on it's type and the index of the current type
                    personInfoStringArray[personInfoStringArrayIndex++] = ("pi" + InfoTypes.GetTypeString((int)personInfo.Type_Id) + index + "Id");
                    personInfoStringArray[personInfoStringArrayIndex++] = ("" + personInfo.Id);
                    personInfoStringArray[personInfoStringArrayIndex++] = ("pi" + InfoTypes.GetTypeString((int)personInfo.Type_Id) + index + "Info");
                    personInfoStringArray[personInfoStringArrayIndex++] = ("" + personInfo.Info);

                    //Increment the index
                    index++;
                }

                //We concat the json input list with the array of found person info
                jsonInput = jsonInput.Concat(personInfoStringArray).ToList();

                //Compute the information of actors associated with the person
                //Get the list of participants associated with the person
                var participateList = person.Participate;

                //Initialize an array of strings to contain the information we get by iteration through the information of each movie
                string[] movieStringArray = new string[participateList.Count() * 16];
                int movieStringArrayIndex = 0;

                //Reset the index, used when assigning attribute names
                index = 0;

                foreach (Participate participate in participateList)
                {
                    //Get the movie associated with the participation entity
                    Movies movie = participate.Movies;

                     //If the movie does not exist in the database we skip the addition of its information
                    if (movie == null)
                        continue;
                    
                    movieStringArray[movieStringArrayIndex++] = ("m" + index + "Id");
                    movieStringArray[movieStringArrayIndex++] = ("" + movie.Id);
                    movieStringArray[movieStringArrayIndex++] = ("m" + index + "Title");
                    movieStringArray[movieStringArrayIndex++] = ("" + movie.Title);
                    movieStringArray[movieStringArrayIndex++] = ("m" + index + "Kind");
                    movieStringArray[movieStringArrayIndex++] = ("" + movie.Kind);
                    movieStringArray[movieStringArrayIndex++] = ("m" + index + "Year");
                    movieStringArray[movieStringArrayIndex++] = ("" + movie.Year);
                    movieStringArray[movieStringArrayIndex++] = ("m" + index + "CharacterName");
                    movieStringArray[movieStringArrayIndex++] = ("" + participate.CharName);
                    movieStringArray[movieStringArrayIndex++] = ("m" + index + "Role");
                    movieStringArray[movieStringArrayIndex++] = ("" + participate.Role);
                    movieStringArray[movieStringArrayIndex++] = ("m" + index + "Note");
                    movieStringArray[movieStringArrayIndex++] = ("" + participate.Note);
                    movieStringArray[movieStringArrayIndex++] = ("m" + index + "NrOrder");
                    movieStringArray[movieStringArrayIndex++] = ("" + participate.NrOrder);

                    //Increment the index
                    index++;
                }

                //We concat the json input list with the array of found person info
                jsonInput = jsonInput.Concat(movieStringArray).ToList();

                //Convert the object to json attributes
                string json = JSonParser.Parse(new string[]{
                    "id", "" + person.Id,
                    "name", "" + person.Name,
                    "gender", "" + person.Gender}.Concat(
                    jsonInput).ToArray()
                    );

                //return the json encoded as byte code
                return Encoder.Encode(json);
            }
            );
        }
    }
}