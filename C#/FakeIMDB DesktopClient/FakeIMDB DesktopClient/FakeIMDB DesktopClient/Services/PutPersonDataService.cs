using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunicationFramework;
using FakeIMDB_DesktopClient.Model;
using Utils;

namespace FakeIMDB_DesktopClient.Services
{
    /// <summary>
    /// Implementation of a PutPersonDataService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    class PutPersonDataService : IPutPersonDataService
    {

        /// <summary>
        /// Method initializing Put'ing of data
        /// </summary>
        /// <param name="callback">Action with callback method to be used</param>
        /// <param name="personItem">PersonItem which properties shall be used</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        public void PutData(Action<string, Exception> callback, PersonSearchItem personItem, ConnectionModel connectionModel)
        {
            try
            {
                callback(PutAllData(personItem, connectionModel), null);
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
        /// <param name="personItem">PersonItem which properties shall be used</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        /// <param name="token">CancellationToken to be used</param>
        public async void PutDataAsync(Action<string, Exception> callback, PersonSearchItem personItem, ConnectionModel connectionModel, CancellationToken token)
        {
            await Task.Run(() =>
            {
                try
                {
                    callback(PutAllData(personItem, connectionModel), null);
                }
                catch (Exception e)
                {
                    callback(null, e);
                }
            }, token);
        }


        /// <summary>
        /// Method for put'ing person data
        /// </summary>
        /// <param name="personItem">PersonItem containing the information to be sent</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        /// <returns>String response from the storage Put'ing to</returns>
        private string PutAllData(PersonSearchItem personItem, ConnectionModel connectionModel)
        {

            // Build Json data from PersonItem
            string json = JSonParser.Parse(
                "id", "" + personItem.Id,
                "title", "" + personItem.Name,
                "gender", "" + personItem.Gender
                );

            // Build RESTful address to be used
            string restAddress = connectionModel + "Person";

            // Encode json to byte array
            byte[] data = Encoder.Encode(json);

            var chandler = new CommunicationHandler(Protocols.Http);

            // Send message
            chandler.Send(restAddress, data, "PUT");

            // Decode response
            Dictionary<string, string> jsonDictionary = JSonParser.GetValues(Encoder.Decode(chandler.Receive(10000)));

            return jsonDictionary["response"];
        }
    }
}
