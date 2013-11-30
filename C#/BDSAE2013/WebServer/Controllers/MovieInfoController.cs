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
    class MovieInfoController : AbstractRequestController
    {
        /// <summary>
        /// The constructor defines the keyword associated in controller on creation
        /// </summary>
        public MovieInfoController()
        {
            Keyword = "MovieInfo";
        }

        /// <summary>
        /// This method returns a delegate that can be used to get movie info from a given storage.
        /// The id of the movie info is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that get movie info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessGet(Request request)
        {
            //Get the values of the given request.
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("Movie info Get was invoked... " + "id: " + nameValueCollection["id"]);
#endif

            //Return the delegate 
            return (storage => storage.Get<MovieInfo>(int.Parse(nameValueCollection["id"])));
        }

        /// <summary>
        /// This method returns a delegate that can be used to post movie info to a given storage.
        /// The parameters of the movie info is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that posts movie info to a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessPost(Request request)
        {
            //Get the values from the request
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Movie info Post was invoked..."
                + " info: " + nameValueCollection["info"]
                + " note: " + nameValueCollection["note"]
                + " movie_id: " + nameValueCollection["movie_id"]
                + " type_id: " + nameValueCollection["type_id"]
                );

            //(Should we create an object to parse or parse parameters?)
            MovieInfo movieinfo = new MovieInfo()
            {
                Info= nameValueCollection["info"],
                Note = nameValueCollection["note"],
                Movie_Id = int.Parse(nameValueCollection["movie_id"]),
                Type_Id = int.Parse(nameValueCollection["type_id"])
            };

            return (storage => storage.Add<MovieInfo>(movieinfo));
        }

        /// <summary>
        /// This method returns a delegate that can be used to update movie info in a given storage.
        /// The id of the movie info to update, as well as the parameters to be updated is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that updates movie info in a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessPut(Request request)
        {
            //Get the values from the request
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Movie info Post was invoked..."
                + " info: " + nameValueCollection["info"]
                + " note: " + nameValueCollection["note"]
                + " movie_id: " + nameValueCollection["movie_id"]
                + " type_id: " + nameValueCollection["type_id"]
                );

            //(Should we create an object to parse or parse parameters?)
            MovieInfo movieinfo = new MovieInfo()
            {
                Info = nameValueCollection["info"],
                Note = nameValueCollection["note"],
                Movie_Id = int.Parse(nameValueCollection["movie_id"]),
                Type_Id = int.Parse(nameValueCollection["type_id"])
            };

            return (storage => storage.Add<MovieInfo>(movieinfo));
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete movie info from a given storage.
        /// The id of the movie info is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that deletes movie info from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessDelete(Request request)
        {
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);
            Console.WriteLine("Movie info Delete was invoked... " + "id: " + nameValueCollection["id"]);

            var id = int.Parse(nameValueCollection["id"]);

            return (storage => storage.Delete<MovieInfo>(id));
        }

    }
}
