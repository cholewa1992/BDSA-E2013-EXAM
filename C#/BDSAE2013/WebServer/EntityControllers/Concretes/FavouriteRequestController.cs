using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;
using EntityFrameworkStorage;
using System.Collections.Specialized;

namespace WebServer
{
    /// <summary>
    /// A request controller that handle the rest methods GET, POST, PUT and DELETE.
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    public class FavouriteRequestController : AbstractEntityRequestController
    {
        public FavouriteRequestController()
        {
            Keyword = "Favourite";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        /// <summary>
        /// This method returns a delegate that can be used to get a favourite list from a given storage.
        /// The id of the favourite list is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that get a favourite list from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Get the request value of the url
            int id = int.Parse(GetUrlArgument(request.Method));
            
#if DEBUG
            //Print the incoming data to the console
            Console.WriteLine("Favourites List Get was invoked... " + "id: " + id);
#endif

            //Return the delegate 
            return (storage =>
            {
                //Get the favourite list from the storage
                FavouriteList favouriteList = storage.Get<FavouriteList>(id);

                //Convert the attributes of the favourite list to json
                string json = JSonParser.Parse(
                    "id", "" + favouriteList.Id,
                    "title", "" + favouriteList.Title,
                    "userAccId", "" + favouriteList.UserAccId
                    );

                //Encode the json to bytes and return them
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to post a favourites list to a given storage.
        /// The parameters of the favourites list is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that posts a favourite list to a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            if (!values.ContainsKey("title") || !values.ContainsKey("userAccId"))
                throw new InvalidDataException("The data parsed to favourite controller put method did not contain either a title or a user account id");

#if DEBUG
            //Print the values to the console
            Console.WriteLine("Favourites List Post was invoked..."
                + " title: " + values["title"]
                + " userAccId: " + values["userAccId"]
                );
#endif

            //Create the Favourite List object to be created in the database
            FavouriteList favouriteList = new FavouriteList()
            {
                Title = values["title"],
                UserAccId = int.Parse(values["userAccId"])
            };

            //Return the delegate 
            return (storage =>
            {
                //Add the Favourite List to the database
                storage.Add<FavouriteList>(favouriteList);

                //set the response message in json
                string json = JSonParser.Parse(
                    "response", "The Favourite List was successfully added"
                    );

                //Encode the json to bytes and return them
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to update a favourite list in a given storage.
        /// The id of the favourite list to update, as well as the parameters to be updated is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that updates a favourites list in a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to favourite controller put method did not contain an id");

#if DEBUG
            //Print the values to the console
            Console.WriteLine("Favourites List Put was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Get the Favourite List object to be updated from the database
                FavouriteList favouriteList = storage.Get<FavouriteList>(int.Parse(values["id"]));

                if (values.ContainsKey("title"))
                    favouriteList.Title = values["title"];

                if (values.ContainsKey("userAccId"))
                    favouriteList.UserAccId = int.Parse(values["userAccId"]);

                //Updated the favourite list in the database
                storage.Update<FavouriteList>(favouriteList);

                //Set the response message
                string json = JSonParser.Parse(
                    "response", "The Favourite List was successfully updated"
                    );

                //Encode the json to bytes and return them
                return Encoder.Encode(json);
            });
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete a favourite list from a given storage.
        /// The id of the favourite list is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that deletes a favourite list from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request)
        {
            //Get the values of the request
            Dictionary<string, string> values = GetRequestValues(request.Data);

            if (!values.ContainsKey("id"))
                throw new InvalidDataException("The data parsed to favourite controller delete method did not contain an id");

#if DEBUG
            //Print the values to the console
            Console.WriteLine("Favourites List Delete was invoked...");
#endif

            //Return the delegate 
            return (storage =>
            {
                //Delete the favourite list from the database
                storage.Delete<FavouriteList>(int.Parse(values["id"]));

                //Se the response message as json
                string json = JSonParser.Parse(
                    "response", "The Favourite List was successfully deleted"
                    );

                //Encode the json to bytes and return them
                return Encoder.Encode(json);
            });
        }
    }
}
