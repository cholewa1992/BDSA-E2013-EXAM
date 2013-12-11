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
    public class PersonRequestController : AbstractEntityRequestController
    {
        //The constructor set the keyword to person so PersonRequestController is able to identify it self. 
        public PersonRequestController()
        {
            Keyword = "Person";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        /// <summary>
        /// This method returns a delegate that can be used to get a person (People) in a given storage.
        /// The delegate only contain the id value. id is the only thing needed to get a person.
        /// @pre request != null
        /// @pre request.Method != null
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to get a person from a given storage, based on the contents of the request </returns>
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
            Console.WriteLine("People Get");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Get the person from the database
                People person = storage.Get<People>(id);

                //Conver the object attributes to json
                string json = JSonParser.Parse(
                    "id", "" + person.Id,
                    "name", "" + person.Name,
                    "gender", "" + person.Gender
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to insert a person (People) in a given storage.
        /// The id of the people is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to insert a person from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Data == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string, string> values = GetRequestValues(request.Data);

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("name") || !values.ContainsKey("gender"))
                throw new InvalidDataException("The data parsed to PeopleRequestController post method did not contain enough information to create a Person");

#if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person Post");
#endif

            //Return the delegate 
            return (storage =>
            {
                // Create a new instance of the People entity using the vital information
                People person = new People()
                {
                    Name = values["name"],
                    Gender = values["gender"]
                };

                //Add the person to the database
                storage.Add<People>(person);

                //Set the json response
                string json = JSonParser.Parse(
                    "response", "The Person was successfully added"
                    );

                //Returns the json as encoded bytes
                return Encoder.Encode(json);
            });
        }
        
        /// <summary>
        /// This method returns a delegate that can be used to update a person (People) in a given storage.
        /// The id of the person to update, as well as the parameters to be updated is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to get a person from a given storage, based on the contents of the request </returns>
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
                throw new InvalidDataException("The data parsed to PeopleRequestController put method did not contain the required id");

            #if DEBUG
            //Write til the console. Only for debugging.
            Console.WriteLine("Person Put");
            #endif

            //Return the delegate 
            return (storage =>
            {
                People person = storage.Get<People>(int.Parse(values["id"]));

                //Add additional information if it is in the request
                if (values.ContainsKey("name"))
                    person.Name = values["name"];

                if (values.ContainsKey("gender"))
                    person.Gender = values["gender"];

                //Update the Person in the database
                storage.Update<People>(person);

                //Set the json response
                string json = JSonParser.Parse(
                    "response", "The Person was successfully updated"
                    );

                //Return the response as encoded bytes
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete a person (People) in a given storage.
        /// The id of the person is determined by the parsed request
        /// @pre request != null
        /// @pre request.Data != null
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to delete a person from a given storage, based on the contents of the request </returns>
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
            Console.WriteLine("Person Delete");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Delete the entity in the database
                storage.Delete<People>(int.Parse(values["id"]));

                //Set the json response
                string json = JSonParser.Parse(
                    "response", "The Person was successfully deleted"
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }
    }
}
