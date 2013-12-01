using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class UserRequestController : AbstractRequestController
    {
        public UserRequestController()
        {
            Keyword = "User";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        public override Func<IStorageConnectionBridge, object> ProcessGet(Request request)
        {
            //Get the values of the given request
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

#if DEBUG
            //Print the incoming data to the console
            Console.WriteLine("User Get was invoked... " + "id: " + nameValueCollection["id"]);
#endif
            //Return the delegate
            return (storage => storage.Get<UserAcc>(int.Parse(nameValueCollection["id"])));
        }

        /// <summary>
        /// This method returns a delegate that can be usd to post a user to a given storage.
        /// The parameters of the user is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server.</param>
        /// <returns> A delegate that posts a user to a given storage, based on the contents of the request.</returns>
        public override Func<IStorageConnectionBridge, object> ProcessPost(Request request)
        {
            //Get the values from the request
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

#if DEBUG
            //Post the values to the console
            Console.WriteLine("User Post was invoked..."
                + " Id: " + nameValueCollection["Id"]
                + " Username: " + nameValueCollection["Username"]
                + " Email: " + nameValueCollection["Email"]
                + " Password: " + nameValueCollection["Password"]
                + " Firstname: " + nameValueCollection["Firstname"]
                + " Lastname: ");
#endif

            //(Should we create an object to parse or parse parameters?)
            UserAcc useracc = new UserAcc()
            {
                Id = int.Parse(nameValueCollection["Id"]),
                Username = nameValueCollection["Username"],
                Email = nameValueCollection["Email"],
                Firstname = nameValueCollection["Firstname"],
                Lastname = nameValueCollection["Lastname"],
                Password = nameValueCollection["Password"]
            };

            //Cannot return proper delegate since Post moethod returns void
            //return (storage => storage.Add<UserAcc>(useracc));
            return (storage => "Not Yet Implemented");
        }



        /// <summary>
        /// This method returns a delegate that can be used to update a useracc in a given storage.
        /// The id of the useracc to update, as well as the parameters to be updated is determined by the parsed request
        /// </summary>
        /// <param name="request">The original request received by the web server</param>
        /// <returns> A delegate that updates a movie in a given storage, based on the contents of the request</returns>
        public override Func<IStorageConnectionBridge, object> ProcessPut(Request request)
        {
            //Get the values of the request
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

#if DEBUG
            //Post the values to the console
            Console.WriteLine("User Post was invoked..."
                + " Id: " + nameValueCollection["Id"]
                + " Username: " + nameValueCollection["Username"]
                + " Email: " + nameValueCollection["Email"]
                + " Password: " + nameValueCollection["Password"]
                + " Firstname: " + nameValueCollection["Firstname"]
                + " Lastname: ");
#endif

            //(Should we create an object to parse or parse parameters?)
            UserAcc useracc = new UserAcc()
            {
                Id = int.Parse(nameValueCollection["Id"]),
                Username = nameValueCollection["Username"],
                Email = nameValueCollection["Email"],
                Firstname = nameValueCollection["Firstname"],
                Lastname = nameValueCollection["Lastname"],
                Password = nameValueCollection["Password"]
            };

            //Cannot return proper delegate since Post moethod returns void
            //return (storage => storage.Add<UserAcc>(useracc));
            return (storage => "Not Yet Implemented");
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete a useracc from a given storage.
        /// The id of the useracc is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server.</param>
        /// <returns> A delegate that deletes a usreacc from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessDelete(Request request)
        {
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);
            Console.WriteLine("Useracc Delete was invoked... " + "id: " + nameValueCollection["id"]);

            //Should we create an object to parse or parse parameters?
            UserAcc useracc = new UserAcc()
            {
                Id = int.Parse(nameValueCollection["id"]),
            };

            //Cannot return proper delegate, since Delete method returns void
            return (storage => storage.Delete<UserAcc>(int.Parse(nameValueCollection["id"])));

            Console.WriteLine("Useracc Delete");
            //return (storage => "Not Yet Implemented");
        }
    }
}