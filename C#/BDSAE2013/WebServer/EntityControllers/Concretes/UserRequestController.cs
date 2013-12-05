using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using CommunicationFramework;
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
                //Get the object from the database
                UserAcc userAcc = storage.Get<UserAcc>(id);

                //Convert the object attributes to json
                string json = JSonParser.Parse(
                    "id", "" + userAcc.Id,
                    "firstname", "" + userAcc.Firstname,
                    "lastname", "" + userAcc.Lastname,
                    "username", "" + userAcc.Username,
                    "password", "" + userAcc.Password,
                    "email", "" + userAcc.Email
                    );

                //Return the json as encoded bytes
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

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("username") || !values.ContainsKey("password"))
                throw new InvalidDataException("The data parsed to UserRequestController post method did not contain enough information to create a User");

#if DEBUG
            //Post the values to the console
            Console.WriteLine("User Post was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Create e user account entity using the vital information
                UserAcc userAcc = new UserAcc()
                {
                    Username = values["username"],
                    Password = values["password"]
                };

                //Add any other information given through the request
                if (values.ContainsKey("firstname"))
                    userAcc.Firstname = values["firstname"];

                if (values.ContainsKey("lastname"))
                    userAcc.Lastname = values["lastname"];

                if (values.ContainsKey("email"))
                    userAcc.Email = values["email"];

                //Add the user to the database
                storage.Add<UserAcc>(userAcc);

                //Set the json response
                string json = JSonParser.Parse(
                    "response", "The User was successfully added"
                    );

                //Return the json as encoded bytes
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

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to UserRequestController put method did not contain the required id");

#if DEBUG
            //Post the values to the console
            Console.WriteLine("User Put was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                UserAcc userAcc = storage.Get<UserAcc>(int.Parse(values["id"]));

                //Add any other information given through the request
                if (values.ContainsKey("username"))
                    userAcc.Username = values["username"];

                if (values.ContainsKey("password"))
                    userAcc.Password = values["password"];

                if (values.ContainsKey("firstname"))
                    userAcc.Firstname = values["firstname"];

                if (values.ContainsKey("lastname"))
                    userAcc.Lastname = values["lastname"];

                if (values.ContainsKey("email"))
                    userAcc.Email = values["email"];

                //Update the user account in the database
                storage.Update<UserAcc>(userAcc);

                //Set the response json
                string json = JSonParser.Parse(
                    "response", "The User was successfully updated"
                    );

                //Return the json as encoded bytes
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

            //Check for all vital information in the request. If one information is missing we throw an exception
            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to UserRequestController delete method did not contain the required id");

            Console.WriteLine("Useracc Delete was invoked... ");

            //Return the delegate 
            return (storage =>
            {
                //Delete the user account from the database
                storage.Delete<UserAcc>(int.Parse(values["id"]));

                //Set the response json
                string json = JSonParser.Parse(
                    "response", "The User was successfully deleted"
                    );

                //Return the json as encoded bytes
                return Encoder.Encode(json);
            });
        }
    }
}