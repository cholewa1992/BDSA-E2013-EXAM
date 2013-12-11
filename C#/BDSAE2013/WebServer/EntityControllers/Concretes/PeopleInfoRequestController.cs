using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Specialized;
using CommunicationFramework;
using EntityFrameworkStorage;
using Storage;
using Utils;

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
    public class PeopleInfoRequestController : AbstractEntityRequestController
    {
        //The constructor set the keyword to person info so PersonInfoController is able to identify it self. 
        public PeopleInfoRequestController()
        {
            Keyword = "PersonInfo";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        /// <summary>
        /// This method returns a delegate that can be used to get person info in a given storage.
        /// The delegate only contain the id value. id is the only thing needed to get person info.
        /// @pre request != null
        /// @pre request.Method != null
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to get person info from a given storage, based on the contents of the request </returns>
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
            //Write til the console. Only for debugging.
            Console.WriteLine("Person info Get");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Get the object from the database
                PersonInfo personInfo = storage.Get<PersonInfo>(id);

                //Convert the object to json attributes
                string json = JSonParser.Parse(
                    "id", "" + personInfo.Id,
                    "personId", "" + personInfo.Person_Id,
                    "typeId", "" + personInfo.Type_Id,
                    "personInfoId", "" + personInfo.PersonInfoId,
                    "info", "" + personInfo.Info,
                    "note", "" + personInfo.Note
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }

        
        /// <summary>
        /// This method returns a delegate that can be used to update person info in a given storage.
        /// The id of the people info is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to get person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string,string> values = GetRequestValues(request.Data);

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("personId") || !values.ContainsKey("typeId") || !values.ContainsKey("info"))
                throw new InvalidDataException("The data parsed to PeopleInfoRequestController post method did not contain enough information to create a PersonInfo");

#if DEBUG
            //Print the values to the console
            Console.WriteLine("Person info Put was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                // New person info initialized with the vital values from the request.
                PersonInfo personInfo = new PersonInfo()
                {
                    Info = values["info"],
                    Person_Id = int.Parse(values["personId"]),
                    Type_Id = int.Parse(values["typeId"])
                };

                //Add additional information if it is in the request
                if (values.ContainsKey("note"))
                    personInfo.Note = values["note"];
                else
                    personInfo.Note = "";

                //Add the person info to the database
                storage.Add<PersonInfo>(personInfo);

                //Set the json response message
                string json = JSonParser.Parse(
                    "response", "The PersonInfo was successfully added"
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }


        /// <summary>
        /// This method returns a delegate that can be used to insert person info in a given storage.
        /// The id of the person info to update, as well as the parameters to be updated is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to insert person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string,string> values = GetRequestValues(request.Data);
            
            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to PeopleInfoRequestController Put method did not the required id");

#if DEBUG
            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Person info Post was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Get the PersonInfo to be updated from the database
                PersonInfo personInfo = storage.Get<PersonInfo>(int.Parse(values["id"]));

                //Update any other information given through the request
                if (values.ContainsKey("info"))
                    personInfo.Info = values["info"];

                if (values.ContainsKey("note"))
                    personInfo.Note = values["note"];

                if (values.ContainsKey("personId"))
                    personInfo.Person_Id = int.Parse(values["personId"]);

                if (values.ContainsKey("typeId"))
                    personInfo.Type_Id = int.Parse(values["typeId"]);

                if (values.ContainsKey("personInfoId"))
                    personInfo.PersonInfoId = int.Parse(values["personInfoId"]);

                //Update the person info in the database
                storage.Update<PersonInfo>(personInfo);

                //Set the json response string
                string json = JSonParser.Parse(
                    "response", "The PersonInfo was successfully updated"
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete person info in a given storage.
        /// The id of the person info is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to delete person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string,string> values = GetRequestValues(request.Data);

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to PeopleInfoRequestController Delete method did not the required id");

            #if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person info Delete");
            #endif

            //Return the delegate 
            return (storage =>
            {
                //Delete the person from the storage
                storage.Delete<PersonInfo>(int.Parse(values["id"]));

                //Set the json response message
                string json = JSonParser.Parse(
                    "response", "The PersonInfo was successfully deleted"
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }
    }
}
