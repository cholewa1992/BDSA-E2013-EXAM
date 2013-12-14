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
    /// Implementation of a MovieExtendedInformationService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class MovieExtendedInformationService : IMovieExtendedInformationService
    {

        /// <summary>
        /// Method initializing fetching of extended data
        /// </summary>
        /// <param name="callback">Action with callback method to be used</param>
        /// <param name="searchItem">SearchItem which properties shall be used</param>
        /// <param name="connectionModel">ConnectionModel to be used</param>
        public void GetData(Action<MovieSearchItem, Exception> callback, MovieSearchItem searchItem, ConnectionModel connectionModel)
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
        public async void GetDataAsync(Action<MovieSearchItem, Exception> callback, MovieSearchItem searchItem, ConnectionModel connectionModel, CancellationToken token)
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
        /// <returns>The same MovieSearchItem from parameters</returns>
        public MovieSearchItem FetchInfo(MovieSearchItem item, ConnectionModel connectionModel)
        {

            var ch = new CommunicationHandler(connectionModel.Protocol);

            // Build RESTful address
            string restAddress = connectionModel.Address + "MovieData/" + item.Id;

            // send message with no data
            ch.Send(restAddress, new byte[0], "GET");

            // decode response to string dictionary representation of the json
            Dictionary<string, string> json = JSonParser.GetValues(Encoder.Decode(ch.Receive(connectionModel.Timeout)));

            // set values from json dictionary
            item.Id = json["id"];
            item.Title = json["title"];
            item.Year = json["year"];
            item.Type = ItemType.Movie;

            // exstract persons
            item.ParticipantsList = ExstractParticipants(json);

            return item;
        }

        /// <summary>
        /// This method exstracts participants from the json
        /// </summary>
        /// <param name="json">json string</param>
        /// <returns>list of PersonSearchItems</returns>
        public List<PersonSearchItem> ExstractParticipants(Dictionary<string, string> json)
        {
            List<PersonSearchItem> list = new List<PersonSearchItem>();

            // build list of persons from the json
            for (int i = 0; json.ContainsKey("p" + i + "Id"); i++)
            {
                list.Add(new PersonSearchItem()
                {
                    Id = json["p" + i + "Id"],
                    Name = json["p" + i + "Name"],
                    CharacterName = json["p" + i + "CharacterName"],
                    Type = ItemType.Person,
                    Role = json["p" + i + "Role"]
                });
            }


            return list;

        }

        
    }
}
