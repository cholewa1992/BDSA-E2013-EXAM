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
    public class PersonRequestController : AbstractRequestController
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
        public override Func<IStorageConnectionBridgeFacade, object> ProcessGet(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            NameValueCollection nameValueColletion = ConvertByteToDataTable(request.Data);

            #if DEBUG
            //Write til the console. Only for debugging.
            Console.WriteLine("People Get");
            #endif

            // The delegate with the id value is returned.
            return (storage => storage.Get<People>(int.Parse(nameValueColletion["id"])));
        }

        
        /// <summary>
        /// This method returns a delegate that can be used to update a person (People) in a given storage.
        /// The delegate contain a people object.
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to get a person from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, object> ProcessPut(Request request)
        {

            // The Bytes(Data) in the request is converted to a table to retreive the values.
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

            // New person object initialized with values from webserver.
            People person = new People()
            {
                Name = nameValueCollection["Name"],
                Gender = nameValueCollection["Gender"]
            };
             
            #if DEBUG
            //Write til the console. Only for debugging.
            Console.WriteLine("Person Put");
            #endif

            // The delegate with the people object is returned
            return (storage => storage.Update<People>(person));
        }


        /// <summary>
        /// This method returns a delegate that can be used to insert a person (People) in a given storage.
        /// The delegate contain a people object.
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to insert a person from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, object> ProcessPost(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

            // New person object initialized with values from webserver.
            People person = new People()
            {
                Name = nameValueCollection["Name"],
                Gender = nameValueCollection["Gender"]
            };
            
            #if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person Post");
            #endif

            // The delegate with the people object is returned
            return (storage => storage.Add<People>(person));
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete a person (People) in a given storage.
        /// The delegate only contain the id value. id is the only thing needed to get a person.
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to delete a person from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, object> ProcessDelete(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

            #if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person Delete");
            #endif

            // The delegate with the id value is returned.
            return (storage => storage.Delete<People>(int.Parse(nameValueCollection["id"])));
        }
    }
}
