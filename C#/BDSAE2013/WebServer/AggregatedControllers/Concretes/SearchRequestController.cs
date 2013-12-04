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
    public class SearchRequestController : AbstractAggregatedRequestController
    {
        /// <summary>
        /// The constructor defines the keyword associated with the controller on creation
        /// </summary>
        public SearchRequestController()
        {
            Keyword = "Search";

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
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Get the request value of the url
            string searchInput = GetUrlArgument(request.Method);
            
#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("Search Get was invoked... " + "searchInput: " + searchInput);
#endif
            //Return the delegate 
            return (storage => 
            {
                IEnumerable<Movies> movies = storage.Get<Movies>();
                List<Movies> movieList = movies.Where(m => m.Title.Contains(searchInput)).ToList();

                IEnumerable<People> people = storage.Get<People>();
                List<People> peopleList = people.Where(m => m.Name.Contains(searchInput)).ToList();

                List<string> jsonInput = new List<string>();

                int index = 0;

                foreach (Movies movie in movieList)
                {
                    jsonInput.Add("m" + index + "Id");
                    jsonInput.Add(""+movie.Id);

                    jsonInput.Add("m" + index + "Title");
                    jsonInput.Add(""+movie.Title);

                    index++;
                }

                index = 0;

                foreach (People person in peopleList)
                {
                    jsonInput.Add("p" + index + "Id");
                    jsonInput.Add("" + person.Id);

                    jsonInput.Add("p" + index + "Name");
                    jsonInput.Add("" + person.Name);

                    index++;
                }

                string json = JSonParser.Parse(jsonInput.ToArray());

                return Encoder.Encode(json);
            }
            );
        }
    }
}
