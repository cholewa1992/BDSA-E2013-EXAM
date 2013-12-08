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
    /// A request controller that handle the rest methods GET, POST, PUT and DELETE.
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
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
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that gets a person and all their associated data from the database </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
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
                List<PersonInfo> personInfoList = storage.Get<PersonInfo>().Where(pi => pi.Person_Id == person.Id).OrderBy(x => x.Type_Id).ToList();

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
                        currentType = (int)personInfo.Type_Id;
                    }

                    //We add the attribute name and value of the current person info, based on it's type and the index of the current type
                    jsonInput.Add("pi" + personInfo.Type_Id + "," + index);
                    jsonInput.Add("" + personInfo.Info);
                    
                    //Increment the index
                    index++;
                }

                //Compute the information of actors associated with the person
                //Get the list of participants associated with the person
                List<Participate> participateList = storage.Get<Participate>().Where(p => p.Person_Id == person.Id).ToList();

                //Reset the index, used when assigning attribute names
                index = 0;

                foreach (Participate participate in participateList)
                {
                    //TODO differentiate between actors and directors
                    //TODO clean up data to avoid duplicates
                    
                    //Attempt to get the movie associated with the participate entity
                    //If the movie does not exist in the database, the participate entity is faulty and should be deleted
                    try
                    {
                        //Get the movie associated with the participation entity
                        Movies movie = storage.Get<Movies>((int)participate.Movie_Id);

                        //Add all relevant information of the movie using the movie and the participant entities
                        jsonInput.Add("m" + index + "Id");
                        jsonInput.Add("" + movie.Id);
                        jsonInput.Add("m" + index + "Title");
                        jsonInput.Add("" + movie.Title);
                        jsonInput.Add("m" + index + "Kind");
                        jsonInput.Add("" + movie.Kind);
                        jsonInput.Add("m" + index + "Year");
                        jsonInput.Add("" + movie.Year);
                        jsonInput.Add("m" + index + "CharacterName");
                        jsonInput.Add("" + participate.CharName);
                        jsonInput.Add("m" + index + "Role");
                        jsonInput.Add("" + participate.Role);
                        jsonInput.Add("m" + index + "Note");
                        jsonInput.Add("" + participate.Note);
                        jsonInput.Add("m" + index + "NrOrder");
                        jsonInput.Add("" + participate.NrOrder);

                        //Increment the index
                        index++;
                    }
                    catch (InvalidOperationException e)
                    {
                        //Deletes the faulty entity from the database
                        //storage.Delete<Participate>(participate.Id);
                    }
                }

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
