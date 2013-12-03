using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to get a person from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Get the request value of the url
            int id = GetUrlArgument(request.Method);
            
            #if DEBUG
            //Write til the console. Only for debugging.
            Console.WriteLine("People Get");
            #endif

            //Return the delegate 
            return (storage =>
            {
                People person = storage.Get<People>(id);

                string json = JSonParser.Parse(
                    "id", "" + person.Id,
                    "name", "" + person.Name,
                    "gender", "" + person.Gender
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to insert a person (People) in a given storage.
        /// The delegate contain a people byte[].
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to insert a person from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string, string> values = GetRequestValues(request.Data);

            // New person byte[] initialized with values from webserver.
            People person = new People()
            {
                Name = values["name"],
                Gender = values["gender"]
            };

#if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person Post");
#endif

            //Return the delegate 
            return (storage =>
            {
                storage.Add<People>(person);

                string json = JSonParser.Parse(
                    "response", "The Person was successfully added"
                    );

                return Encoder.Encode(json);
            });
        }
        
        /// <summary>
        /// This method returns a delegate that can be used to update a person (People) in a given storage.
        /// The delegate contain a people byte[].
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to get a person from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {

            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string,string> values = GetRequestValues(request.Data);

            // New person byte[] initialized with values from webserver.
            People person = new People()
            {
                Id = int.Parse(values["id"]),
                Name = values["name"],
                Gender = values["gender"]
            };
             
            #if DEBUG
            //Write til the console. Only for debugging.
            Console.WriteLine("Person Put");
            #endif

            //Return the delegate 
            return (storage =>
            {
                storage.Update<People>(person);

                string json = JSonParser.Parse(
                    "response", "The Person was successfully updated"
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete a person (People) in a given storage.
        /// The delegate only contain the id value. id is the only thing needed to get a person.
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to delete a person from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string,string> values = GetRequestValues(request.Data);

            #if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person Delete");
            #endif

            //Return the delegate 
            return (storage =>
            {
                storage.Delete<People>(int.Parse(values["id"]));

                string json = JSonParser.Parse(
                    "response", "The Person was successfully deleted"
                    );

                return Encoder.Encode(json);
            });
        }
    }
}
