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
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to get person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Get the request value of the url
            int id = GetUrlArgument(request.Method);
            
            #if DEBUG
            //Write til the console. Only for debugging.
            Console.WriteLine("Person info Get");
            #endif

            //Return the delegate 
            return (storage =>
            {
                PersonInfo personInfo = storage.Get<PersonInfo>(id);

                string json = JSonParser.Parse(
                    "id", "" + personInfo.Id,
                    "personId", "" + personInfo.Person_Id,
                    "typeId", "" + personInfo.Type_Id,
                    "personInfoId", "" + personInfo.PersonInfoId,
                    "info", "" + personInfo.Info,
                    "note", "" + personInfo.Note
                    );

                return Encoder.Encode(json);
            });
        }

        
        /// <summary>
        /// This method returns a delegate that can be used to update person info in a given storage.
        /// The delegate contain a person info byte[].
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to get person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {

            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string,string> values = GetRequestValues(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Person info Put was invoked..."
                + " info: " + values["info"]
                + " note: " + values["note"]
                + " person_id: " + values["personId"]
                + " typeId: " + values["typeId"]
                );

            // New person info byte[] initialized with values from webserver.
            PersonInfo personInfo = new PersonInfo()
            {
                Info = values["info"],
                Note = values["note"],
                Person_Id = int.Parse(values["personId"]),
                Type_Id = int.Parse(values["typeId"]),
                PersonInfoId = int.Parse(values["personInfoId"])
            };
             
            #if DEBUG
            //Write til the console. Only for debugging.
            Console.WriteLine("Person info Put");
            #endif

            //Return the delegate 
            return (storage =>
            {
                storage.Add<PersonInfo>(personInfo);

                string json = JSonParser.Parse(
                    "response", "The Person Info was successfully added"
                    );

                return Encoder.Encode(json);
            });
        }


        /// <summary>
        /// This method returns a delegate that can be used to insert person info in a given storage.
        /// The delegate contain a person info byte[].
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to insert person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string,string> values = GetRequestValues(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Person info Post was invoked..."
                + " info: " + values["info"]
                + " note: " + values["note"]
                + " personId: " + values["personId"]
                + " typeId: " + values["typeId"]
                );

            // New person info byte[] initialized with values from webserver.
            PersonInfo personInfo = new PersonInfo()
            {
                Id = int.Parse(values["id"]),
                Info = values["info"],
                Note = values["note"],
                Person_Id = int.Parse(values["personId"]),
                Type_Id = int.Parse(values["typeId"]),
                PersonInfoId = int.Parse(values["personInfoId"])
            };
            
            #if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person info Post");
            #endif

            //Return the delegate 
            return (storage =>
            {
                storage.Update<PersonInfo>(personInfo);

                string json = JSonParser.Parse(
                    "response", "The Person Info was successfully updated"
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete person info in a given storage.
        /// The delegate only contain the id value. id is the only thing needed to delete person info.
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to delete person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            Dictionary<string,string> values = GetRequestValues(request.Data);

            #if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person info Delete");
            #endif

            //Return the delegate 
            return (storage =>
            {
                storage.Delete<PersonInfo>(int.Parse(values["id"]));

                string json = JSonParser.Parse(
                    "response", "The Person Info was successfully deleted"
                    );

                return Encoder.Encode(json);
            });
        }
    }
}
