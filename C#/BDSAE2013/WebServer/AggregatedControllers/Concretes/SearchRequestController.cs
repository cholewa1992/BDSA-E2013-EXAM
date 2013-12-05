using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationFramework;
using Storage;
using EntityFrameworkStorage;
using System.Collections.Specialized;
using Utils;

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
            string[] searchInputArray = searchInput.Split('_');

#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("Search Get was invoked... " + "searchInput: " + searchInput);
#endif
            //Return the delegate 
            return (storage => 
            {
                HashSet<Movies> movieSet = new HashSet<Movies>();

                foreach (string searchString in searchInputArray)
                    movieSet.UnionWith(storage.Get<Movies>().Where(m => m.Title.Contains(searchString)));

                HashSet<People> peopleSet = new HashSet<People>();

                foreach (string searchString in searchInputArray)
                    peopleSet.UnionWith(storage.Get<People>().Where(p => p.Name.Contains(searchString)));

                //Initialize the list of attribute names/values
                List<string> jsonInput = new List<string>();

                //initialize an index to differentiate each different movie/person
                int index = 0;

                //Iterate through all movies and add them to the jsonInput
                foreach (Movies movie in movieSet)
                {
                    if ("m" + index + "Title" == "m613Title")
                        Console.WriteLine(movie.Title);

                    //For each movie we add the id of the movie
                    jsonInput.Add("m" + index + "Id");          //Add the attribute name
                    jsonInput.Add(""+movie.Id);                 //Add the attribute value

                    //For each movie we add the title of the movie
                    jsonInput.Add("m" + index + "Title");       //Add the attribute name
                    jsonInput.Add(""+movie.Title);              //Add the attribute value

                    //Increment the index
                    index++;
                }

                //Reet the index since we now work with person
                index = 0;

                foreach (People person in peopleSet)
                {
                    //For each person we add the id of the person
                    jsonInput.Add("p" + index + "Id");          //Add the attribute name
                    jsonInput.Add("" + person.Id);              //Add the attribute value

                    //For each person we add the title of the person
                    jsonInput.Add("p" + index + "Name");          //Add the attribute name
                    jsonInput.Add("" + person.Name);              //Add the attribute value

                    //Increment the index
                    index++;
                }

                //Convert the json input to actual json
                string json = JSonParser.Parse(jsonInput.ToArray());

                //return the json encoded as byte code
                return Encoder.Encode(json);
            }
            );
        }
    }
}
