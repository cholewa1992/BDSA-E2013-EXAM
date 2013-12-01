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
    public class MovieRequestController : AbstractRequestController
    {
        /// <summary>
        /// The constructor defines the keyword associated with the controller on creation
        /// </summary>
        public MovieRequestController()
        {
            Keyword = "Movie";

            //Check the invariant
            if (Keyword == null)
                throw new KeywordNullException("Keyword must never be null");
        }

        /// <summary>
        /// This method returns a delegate that can be used to get a movie from a given storage.
        /// The id of the movie is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that gets a movie from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessGet(Request request)
        {
            //Get the values of the given request.
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);
            
            #if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("Movie Get was invoked... " + "id: " + nameValueCollection["id"]);
            #endif 

            //Return the delegate 
            return (storage => storage.Get<Movies>(int.Parse(nameValueCollection["id"])));
        }

        /// <summary>
        /// This method returns a delegate that can be used to post a movie to a given storage.
        /// The parameters of the movie is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that posts a movie to a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessPost(Request request)
        {
            //Get the values from the request
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);
            
            //Post the values to the console (should be deleted before release)
            Console.WriteLine("Movie Post was invoked..." 
                + " title: " + nameValueCollection["title"]
                + " kind: " + nameValueCollection["kind"]
                + " year: " + nameValueCollection["year"]
                + " seasonNumber: " + nameValueCollection["seasonNumber"]
                + " episodeNumber: " + nameValueCollection["episodeNumber"]
                + " seriesYear: " + nameValueCollection["seriesYear"]
                + " episodeOfId: " + nameValueCollection["episodeOfId"]
                );

            //(Should we create an object to parse or parse parameters?)
            Movies movie = new Movies()
            {
                Title = nameValueCollection["title"], 
                Kind = nameValueCollection["kind"],
                Year = int.Parse(nameValueCollection["year"]),
                SeasonNumber = int.Parse(nameValueCollection["seasonNumber"]),
                EpisodeNumber = int.Parse(nameValueCollection["episodeNumber"]),
                SeriesYear = nameValueCollection["seriesYear"],
                EpisodeOf_Id = int.Parse(nameValueCollection["episodeOfId"])
            };

            return (storage => storage.Add<Movies>(movie));
        }

        /// <summary>
        /// This method returns a delegate that can be used to update a movie in a given storage.
        /// The id of the movie to update, as well as the parameters to be updated is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that updates a movie in a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessPut(Request request)
        {
            //Get the values of the request
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);

            //Print the values to the console (should be deleted before release)
            Console.WriteLine("Movie Put was invoked..."
                + " id: " + nameValueCollection["id"] 
                + " title: " + nameValueCollection["title"]
                + " kind: " + nameValueCollection["kind"]
                + " year: " + nameValueCollection["year"]
                + " seasonNumber: " + nameValueCollection["seasonNumber"]
                + " episodeNumber: " + nameValueCollection["episodeNumber"]
                + " seriesYear: " + nameValueCollection["seriesYear"]
                + " episodeOfId: " + nameValueCollection["episodeOfId"]
                );

            //Should we create an object to parse or parse parameters?
            Movies movie = new Movies()
            {
                Id = int.Parse(nameValueCollection["id"]),
                Title = nameValueCollection["title"],
                Kind = nameValueCollection["kind"],
                Year = int.Parse(nameValueCollection["year"]),
                SeasonNumber = int.Parse(nameValueCollection["seasonNumber"]),
                EpisodeNumber = int.Parse(nameValueCollection["episodeNumber"]),
                SeriesYear = nameValueCollection["seriesYear"],
                EpisodeOf_Id = int.Parse(nameValueCollection["episodeOfId"])
            };

            return (storage => storage.Update<Movies>(movie));
        }

        /// <summary>
        /// This method returns a delegate that can be used to delete a movie from a given storage.
        /// The id of the movie is determined by the parsed request
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that deletes a movie from a given storage, based on the contents of the request </returns>
        public override Func<IStorageConnectionBridge, object> ProcessDelete(Request request)
        {
            //Get the values of the request
            NameValueCollection nameValueCollection = ConvertByteToDataTable(request.Data);
            
            //Print the values to the console (should be deleted before release)
            Console.WriteLine("Movie Delete was invoked... " + "id: " + nameValueCollection["id"]);

            var id = int.Parse(nameValueCollection["id"]);

            //Return the delegate 
            return (storage => storage.Delete<Movies>(id));
        }

    }
}
