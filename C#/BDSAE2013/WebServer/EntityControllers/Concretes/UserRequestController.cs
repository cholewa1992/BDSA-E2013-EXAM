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
    public class UserRequestController : AbstractEntityRequestController
    {
        public UserRequestController()
        {
            Keyword = "User";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Get the request value of the url
            int id = int.Parse(GetUrlArgument(request.Method));
            
#if DEBUG
            //Print the incoming data to the console
            Console.WriteLine("User Get was invoked... " + "id: " + id);
#endif

            //Return the delegate 
            return (storage =>
            {
                UserAcc userAcc = storage.Get<UserAcc>(id);

                string json = JSonParser.Parse(
                    "id", "" + userAcc.Id,
                    "firstname", "" + userAcc.Firstname,
                    "lastname", "" + userAcc.Lastname,
                    "username", "" + userAcc.Username,
                    "password", "" + userAcc.Password,
                    "email", "" + userAcc.Email
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be usd to post a user to a given storage.
        /// The parameters of the user is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server.</param>
        /// <returns> A delegate that posts a user to a given storage, based on the contents of the request.</returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);
            
#if DEBUG
            //Post the values to the console
            Console.WriteLine("User Post was invoked..."
                + " Username: " + values["username"]
                + " Email: " + values["email"]
                + " Password: " + values["password"]
                + " Firstname: " + values["firstname"]
                + " Lastname: ");
#endif

            //(Should we create an byte[] to parse or parse parameters?)
            UserAcc userAcc = new UserAcc()
            {
                Firstname = values["firstname"],
                Lastname = values["lastname"],
                Username = values["username"],
                Password = values["password"],
                Email = values["email"]
            };

            //Return the delegate 
            return (storage =>
            {
                storage.Add<UserAcc>(userAcc);

                string json = JSonParser.Parse(
                    "response", "The User was successfully added"
                    );

                return Encoder.Encode(json);
            });
        }



        /// <summary>
        /// This method returns a delegate that can be used to update a useracc in a given storage.
        /// The id of the useracc to update, as well as the parameters to be updated is determined by the parsed request
        /// </summary>
        /// <param name="request">The original request received by the web server</param>
        /// <returns> A delegate that updates a movie in a given storage, based on the contents of the request</returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);
            
#if DEBUG
            //Post the values to the console
            Console.WriteLine("User Post was invoked..."
                + " Id: " + values["id"]
                + " Username: " + values["username"]
                + " Email: " + values["email"]
                + " Password: " + values["password"]
                + " Firstname: " + values["firstname"]
                + " Lastname: ");
#endif

            //(Should we create an byte[] to parse or parse parameters?)
            UserAcc userAcc = new UserAcc()
            {
                Id = int.Parse(values["id"]),
                Username = values["username"],
                Email = values["email"],
                Firstname = values["firstname"],
                Lastname = values["lastname"],
                Password = values["password"]
            };

            //Return the delegate 
            return (storage =>
            {
                storage.Update<UserAcc>(userAcc);

                string json = JSonParser.Parse(
                    "response", "The User was successfully updated"
                    );

                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete a useracc from a given storage.
        /// The id of the useracc is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server.</param>
        /// <returns> A delegate that deletes a usreacc from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            Console.WriteLine("Useracc Delete was invoked... " + "id: " + values["id"]);

            //Return the delegate 
            return (storage =>
            {
                storage.Delete<UserAcc>(int.Parse(values["id"]));

                string json = JSonParser.Parse(
                    "response", "The User was successfully deleted"
                    );

                return Encoder.Encode(json);
            });
        }
    }
}