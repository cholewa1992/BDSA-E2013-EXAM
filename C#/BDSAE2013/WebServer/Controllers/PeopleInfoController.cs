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
    class PersonInfoController : AbstractRequestController
    {
        //The constructor set the keyword to person info so PersonInfoController is able to identify it self. 
        public PersonInfoController()
        {
            Keyword = "PersonInfo";
        }

        /// <summary>
        /// This method returns a delegate that can be used to get person info in a given storage.
        /// The delegate only contain the id value. id is the only thing needed to get person info.
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to get person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessGet(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            NameValueCollection nameValueColletion = ConvertByteToDataTable(request.Data);

            #if DEBUG
            //Write til the console. Only for debugging.
            Console.WriteLine("Person info Get");
            #endif

            // The delegate with the id value is returned.
            return (storage => storage.Get<PersonInfo>(int.Parse(nameValueColletion["id"])));
        }

        
        /// <summary>
        /// This method returns a delegate that can be used to update person info in a given storage.
        /// The delegate contain a person info object.
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to get person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessPut(Request request)
        {

            // The Bytes(Data) in the request is converted to a table to retreive the values.
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Person info Put was invoked..."
                + " info: " + nameValueCollection["info"]
                + " note: " + nameValueCollection["note"]
                + " person_id: " + nameValueCollection["person_id"]
                + " type_id: " + nameValueCollection["type_id"]
                );

            // New person info object initialized with values from webserver.
            PersonInfo personInfo = new PersonInfo()
            {
                Info = nameValueCollection["info"],
                Note = nameValueCollection["note"],
                Person_Id = int.Parse(nameValueCollection["person_id"]),
                Type_Id = int.Parse(nameValueCollection["type_id"])
            };
             
            #if DEBUG
            //Write til the console. Only for debugging.
            Console.WriteLine("Person info Put");
            #endif

            // The delegate with the personInfo object is returned
            return (storage => storage.Update<PersonInfo>(personInfo));
        }


        /// <summary>
        /// This method returns a delegate that can be used to insert person info in a given storage.
        /// The delegate contain a person info object.
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns> A delegate that can be used to insert person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessPost(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Person info Post was invoked..."
                + " info: " + nameValueCollection["info"]
                + " note: " + nameValueCollection["note"]
                + " person_id: " + nameValueCollection["person_id"]
                + " type_id: " + nameValueCollection["type_id"]
                );

            // New person info object initialized with values from webserver.
            PersonInfo personInfo = new PersonInfo()
            {
                Info = nameValueCollection["info"],
                Note = nameValueCollection["note"],
                Person_Id = int.Parse(nameValueCollection["person_id"]),
                Type_Id = int.Parse(nameValueCollection["type_id"])
            };
            
            #if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person info Post");
            #endif

            // The delegate with the person info object is returned
            return (storage => storage.Add<PersonInfo>(personInfo));
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete person info in a given storage.
        /// The delegate only contain the id value. id is the only thing needed to delete person info.
        /// </summary>
        /// <param name="request">The original request recieved by the webserver</param>
        /// <returns>  A delegate that can be used to delete person info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessDelete(Request request)
        {
            // The Bytes(Data) in the request is converted to a table to retreive the values.
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

            #if DEBUG
            //Write til the console. Only for debugging
            Console.WriteLine("Person info Delete");
            #endif

            // The delegate with the id value is returned.
            return (storage => storage.Delete<PersonInfo>(int.Parse(nameValueCollection["id"])));
        }
    }
}
