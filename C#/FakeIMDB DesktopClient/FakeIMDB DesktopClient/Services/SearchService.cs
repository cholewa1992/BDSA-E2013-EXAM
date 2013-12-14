using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunicationFramework;
using FakeIMDB_DesktopClient.Model;
using Utils;

namespace FakeIMDB_DesktopClient.Services
{

    /// <summary>
    /// Class containing an implementation of a SearchService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class SearchService : ISearchService
    {

        /// <summary>
        /// Method initializing a search
        /// </summary>
        /// <param name="callback">Action with callback method to be used</param>
        /// <param name="searchTerm">String to be searched for</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        public void Search(Action<List<ISearchItem>, Exception> callback, string searchTerm, ConnectionModel connectionModel)
        {
            try
            {
                callback(FetchData(searchTerm, connectionModel), null);
            }
            catch (Exception e)
            {
                callback(null, e);
            }
        }

        /// <summary>
        /// Method initializing a search in an Async manner
        /// </summary>
        /// <param name="callback">Action with callback method to be used</param>
        /// <param name="searchTerm">String to be searched for</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        /// <param name="token">CancellationToken to be used</param>
        public async void SearchAsync(Action<List<ISearchItem>, Exception> callback, string searchTerm, ConnectionModel connectionModel,
            CancellationToken token)
        {
            
            await Task.Run(() =>
            {
                try
                {
                    callback(FetchData(searchTerm, connectionModel), null);
                }
                catch (Exception e)
                {
                    callback(null, e);
                }
            }, token);
        }


        /// <summary>
        /// Method searching and returning a list of results
        /// </summary>
        /// <param name="term">String to be searched for</param>
        /// <param name="connectionModel">Connection model to be used</param>
        /// <returns>a list of ISearchItems</returns>
        public List<ISearchItem> FetchData(String term, ConnectionModel connectionModel)
        {

            var ch = new CommunicationHandler(connectionModel.Protocol);

            // Build RESTful string
            string restAddress = connectionModel.Address + "Search/" + term;

            // Send message with no Data
            ch.Send(restAddress, new byte[0], "GET");

            // Decode result to string
            string json = Encoder.Decode(ch.Receive(connectionModel.Timeout));

            // Build dictionary representation of the json result
            Dictionary<string, string> resultDictionary = JSonParser.GetValues(json);


            var resultList = new List<ISearchItem>();


            // Add movies to results
            for (int i = 0; resultDictionary.ContainsKey("m" + i + "Id"); i++)
            {
                resultList.Add(new MovieSearchItem
                {
                    Id = resultDictionary["m" + i + "Id"],
                    Title = resultDictionary["m" + i + "Title"],
                    Type = ItemType.Movie,
                    ImageSource = "pack://application:,,,/FakeIMDB DesktopClient;component/Icons/movie-icon.png",
                    ShortDescription = resultDictionary["m" + i + "Plot"]
                });
            }

            // Add persons to results
            for (int i = 0; resultDictionary.ContainsKey("p" + i + "Id"); i++)
            {
                resultList.Add(new PersonSearchItem
                {
                    Id = resultDictionary["p" + i + "Id"],
                    Name = resultDictionary["p" + i + "Name"],
                    Type = ItemType.Person,
                    ImageSource = "pack://application:,,,/FakeIMDB DesktopClient;component/Icons/User-blue-icon.png",
                    ShortDescription = resultDictionary["p" + i + "Biography"]
                    
                });
            }

            return resultList;
        }
    }
}
