using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommunicationFramework;
using FakeIMDB_DesktopClient.Model;
using Utils;

namespace FakeIMDB_DesktopClient.Services
{
    /// <summary>
    /// Implementation of a PutMovieDataService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class PutMovieDataService : IPutMovieDataService
    {

        /// <summary>
        /// Method initialicing Put'ing of data
        /// </summary>
        /// <param name="callback">Action with callback method to be used</param>
        /// <param name="movieItem">MovieItem which properties shall be used</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        public void PutData(Action<string, Exception> callback, MovieSearchItem movieItem, ConnectionModel connectionModel)
        {
            try
            {
                callback(PutAllData(movieItem, connectionModel), null);
            }
            catch (Exception e)
            {
                callback(null, e);
            }
        }

        /// <summary>
        /// Method initializing Put'ing of data in an Async manner
        /// </summary>
        /// <param name="callback">Action with callback method to be used</param>
        /// <param name="movieItem">MovieItem which properties shall be used</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        /// <param name="token">CancellationToken to be used</param>
        public async void PutDataAsync(Action<string, Exception> callback, MovieSearchItem movieItem, ConnectionModel connectionModel, CancellationToken token)
        {
            await Task.Run(() =>
            {
                try
                {
                    callback(PutAllData(movieItem, connectionModel), null);
                }
                catch (Exception e)
                {
                    callback(null, e);
                }
            }, token);
        }


        /// <summary>
        /// Method for Put'ing movie data
        /// </summary>
        /// <param name="movieItem">The movieItem which properties shall be put</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        /// <returns>String response from the storage Put'ing to</returns>
        private string PutAllData(MovieSearchItem movieItem, ConnectionModel connectionModel)
        {
            // Build json representation of MovieItem properties
            string json = JSonParser.Parse(
                "id", "" + movieItem.Id,
                "title", "" + movieItem.Title,
                "year", "" + movieItem.Year
                );

            //Return the json as encoded bytes
            byte[] data = Encoder.Encode(json);

            var chandler = new CommunicationHandler(connectionModel.Protocol);

            // Build Json address
            string restAddress = connectionModel.Address + "Movie";

            // Send message
            chandler.Send(restAddress, data, "PUT");

            // Decode response
            Dictionary<string, string> jsonDictionary = JSonParser.GetValues(Encoder.Decode(chandler.Receive(10000)));

            return jsonDictionary["response"];
        }

        
    }
}
