using System;
using System.Collections.Generic;
using System.Linq;
using CommunicationFramework;
using Storage;
using EntityFrameworkStorage;
using Utils;
using MyMovieAPI;

namespace WebServer
{
    /// <summary>
    /// A aggregated request controller that handle the rest methods GET.
    /// The controller receives the request and based on the type of method being invoked, the class will return a delegate
    /// which can be used by the RequestDelegator to contact the database.
    /// @invariant Keyword != null
    /// </summary>
    public class SearchRequestController : AbstractAggregatedRequestController
    {
        //The search standard search limit, used in case no other is defined in the request
        private const int SearchLimit = 10;

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
        /// This method returns a delegate that can be used to search for movies that matches certain criteria
        /// The search criteria is determined by the parsed request
        /// @pre request != null
        /// @pre request.Method != null
        /// </summary>
        /// <param name="request"> The original request received by the web server. </param>
        /// <returns> A delegate that searches for movies in the database based on the request </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request)
        {
            //Pre condition check that the incoming request is not null
            if (request == null)
                throw new ArgumentNullException("Incoming request must not be null");

            //Pre condition check that the incoming requests method is not null
            if (request.Method == null)
                throw new ArgumentNullException("Incoming request method must not be null");

            //Get the request value of the url
            string searchInput = GetUrlArgument(request.Method).Replace("%20", " ");
            string[] splitSearchInput = searchInput.Split(' ');

            //Get the list of keywords based on the split strings
            List<string> searchInputList = GetSearchKeywords(splitSearchInput);

#if DEBUG
            //Print the incoming data to the console (Should be deleted before release)
            Console.WriteLine("Search Get was invoked... " + "searchInput: " + searchInput);
#endif

            //Return the delegate 
            return (storage => 
            {
                //Initialize the set of movies
                HashSet<Movies> movieSet = new HashSet<Movies>();

                //Initialize a variable defining how many search hits are left to fill the search limit
                int hitsLeftToLimit = SearchLimit;

                //Iterate through each search input
                for (int i = 0; i < searchInputList.Count; i++ )
                {
                    //If the amount of movies which has been found exceeds the amount we want, we stop searching
                    if (movieSet.Count >= SearchLimit)
                        break;

                    string searchString = searchInputList[i];

                    //Add the first amount of movies that matches the search credentials
                    //The amount is the amount of search hits left to reach the search limit
                    //If we try to take more than the amount, the method only takes the amount of hits
                    movieSet.UnionWith(storage.Get<Movies>().Where(m => m.Title.ToLower().Contains(searchString.ToLower())).Take(hitsLeftToLimit));

                    //Update the search hits left to hit the limit
                    hitsLeftToLimit = SearchLimit - movieSet.Count;
                    
                    //If this is the first iteration (with the complete search input), and if we haven't reached the search limit yet
                    //We search in the MyMovieAPI database
                    if (i == 0 && hitsLeftToLimit > 0)
                    {
                        //Union the results of the MyMovieAPI search with the current list
                        movieSet.UnionWith(MyMovieApiAdapter.MakeRequest(storage, searchString, hitsLeftToLimit));

                        //Update the search hits left to hit the limit
                        hitsLeftToLimit = SearchLimit - movieSet.Count;
                    }
                }

                //Initialize the set of movies
                HashSet<People> peopleSet = new HashSet<People>();

                //Reset the counting variable
                hitsLeftToLimit = SearchLimit;

                //Iterate through each search input
                foreach (string searchString in searchInputList)
                {
                    //If the amount of people which has been found exceeds the amount we want, we stop searching
                    if (peopleSet.Count >= SearchLimit)
                        break;

                    //Add the first amount of persons that matches the search credentials
                    //The amount is the amount of search hits left to reach the search limit
                    //If we try to take more than the amount, the method only takes the amount of hits
                    peopleSet.UnionWith(storage.Get<People>().Where(p => p.Name.ToLower().Contains(searchString.ToLower())).Take(hitsLeftToLimit));

                    //Update the search hits left to hit the limit
                    hitsLeftToLimit = SearchLimit - peopleSet.Count;
                }

                //Initialize the list of attribute names/values
                List<string> jsonInput = new List<string>();

                //initialize an index to differentiate each different movie/person
                int index = 0;

                //Iterate through the first movies, until we have enough based on our search limit, and add them to the jsonInput
                foreach (Movies movie in movieSet)
                {
                    //For each movie we add the id of the movie
                    jsonInput.Add("m" + index + "Id");          //Add the attribute name
                    jsonInput.Add(""+movie.Id);                 //Add the attribute value

                    //For each movie we add the title of the movie
                    jsonInput.Add("m" + index + "Title");       //Add the attribute name
                    jsonInput.Add(""+movie.Title);              //Add the attribute value

                    //Increment the index
                    index++;
                }

                //Reset the index since we now work with person
                index = 0;

                //Iterate through the first people, until we have enough based on our search limit, and add them to the jsonInput
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

                //Initialize the json string
                string json = "";

                if (jsonInput.Count > 0)
                {
                    //If there was any search hits we convert the json input to actual json
                    json = JSonParser.Parse(jsonInput.ToArray());
                }
                else
                {
                    //Otherwise we input a response message
                    json = JSonParser.Parse(new string[]{"response", "There was no search hits"});
                }

                //return the json encoded as byte code
                return Encoder.Encode(json);
            }
            );
        }

        /// <summary>
        /// Computes a list of the concatenation of each adjacent search input, of all lengths from n to 1
        /// </summary>
        /// <param name="stringArray"> An array of keywords </param>
        /// <returns> A list of all relevant keyword combinations </returns>
        public List<string> GetSearchKeywords(string[] stringArray)
        {
            if (stringArray == null)
                throw new ArgumentNullException("String array parsed to GetSearchKeywords method must not be null");

            //Initialize the search input list
            List<string> searchInputList = new List<string>();

            //i: The amount of words to be concattenated to form the sentence to be added to the list
            //j: The start position of the word to be added
            //k: The current position of the word to be concattenated to the complete string to be added
            for(int i = stringArray.Length; i > 0; i--)
            {
                for (int j = 0; j < stringArray.Length; j++)
                {
                    //Check if there are enough array entries to add a sentence concattenated of i words
                    //counting from start position j
                    //Otherwise we escape the current iteration
                    if (j + i > stringArray.Length)
                        break;

                    //Initialize the word to be added
                    string concattedWord = "";

                    //Iterate through each word until a sentence with i words in it has been completed
                    for (int k = j; k < j + i; k++)
                    {
                        //Check if the word has just begun, otherwise we add a space between each word
                        if (concattedWord != "")
                            concattedWord += " ";

                        //Add the current word to the complete sentence
                        concattedWord += stringArray[k];
                    }

                    //Add the completed sentence to the list of search inputs
                    searchInputList.Add(concattedWord);
                }
            }

            return searchInputList;
        }
    }
}
