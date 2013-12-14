using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunicationFramework;
using FakeIMDB_DesktopClient.Model;
using Utils;

namespace FakeIMDB_DesktopClient.Services
{
    /// <summary>
    /// Implementation of a PersonExtendedInformationService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class PersonExtendedInformationService : IPersonExtendedInformationService
    {

        /// <summary>
        /// Method initializing fetching of extended data
        /// </summary>
        /// <param name="callback">Action with callback method to be used</param>
        /// <param name="searchItem">SearchItem which properties shall be used</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        public void GetData(Action<PersonSearchItem, Exception> callback, PersonSearchItem searchItem, ConnectionModel connectionModel)
        {
            try
            {
                callback(FetchInfo(searchItem, connectionModel), null);
            }
            catch (Exception e)
            {
                callback(null, e);
            }
        }

        /// <summary>
        /// Method initializing fetching of data in an Async manner
        /// </summary>
        /// <param name="callback">Action with callback method to be used</param>
        /// <param name="searchItem">SearchItem which properties shall be used</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        /// <param name="token">CancellationToken to be used</param>
        public async void GetDataAsync(Action<PersonSearchItem, Exception> callback, PersonSearchItem searchItem, ConnectionModel connectionModel, CancellationToken token)
        {
            await Task.Run(() =>
            {
                try
                {
                    callback(FetchInfo(searchItem, connectionModel), null);
                }
                catch (Exception e)
                {
                    callback(null, e);
                }
            }, token);
        }

        /// <summary>
        /// Method for fetching extended information
        /// </summary>
        /// <param name="item">SearchItem which properties shall be used</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        /// <returns>The same PersonSearchItem from parameters</returns>
        public PersonSearchItem FetchInfo(PersonSearchItem item, ConnectionModel connectionModel)
        {

            var ch = new CommunicationHandler(connectionModel.Protocol);

            //Build RESTful address
            string restAddress = connectionModel.Address + "PersonData/" + item.Id;

            // send message with no data
            ch.Send(restAddress, new byte[0], "GET");

            // decode response to string dictionary representation of the json
            Dictionary<string, string> json = JSonParser.GetValues(Encoder.Decode(ch.Receive(connectionModel.Timeout)));

            // set values from json dictionary
            item.Id = json["id"];
            item.Name = json["name"];
            item.Gender = json["gender"];

            // if dictionary does't contain birthday, set it to null
            item.Birthdate = json.ContainsKey("piBirthDate0Info") ? json["piBirthDate0Info"] : null;

            // set movies participated in from dictionary
            item.ParticipatesInList = new List<MovieSearchItem>();
            for (int i = 0; json.ContainsKey("m" + i + "Id"); i++)
            {
                item.ParticipatesInList.Add(new MovieSearchItem()
                {
                    Id = json["m" + i + "Id"],
                    Title = json["m" + i + "Title"],
                    Year = json["m" + i + "Year"]
                });
            }

            return item;
        }

        
    }
}
