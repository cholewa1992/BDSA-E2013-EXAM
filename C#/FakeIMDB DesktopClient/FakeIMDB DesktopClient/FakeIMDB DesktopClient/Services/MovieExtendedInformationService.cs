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
    class MovieExtendedInformationService : IMovieExtendedInformationService
    {

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

        public MovieSearchItem FetchInfo(MovieSearchItem item, ConnectionModel connectionModel)
        {

            var ch = new CommunicationHandler(connectionModel.Protocol);

            string restAddress = connectionModel.Address + "MovieData/" + item.Id;

            ch.Send(restAddress, new byte[0], "GET");

            var json = JSonParser.GetValues(Encoder.Decode(ch.Receive(10000)));

            // add unique values
            item.Id = json["id"];
            item.Title = json["title"];
            item.Year = json["year"];
            item.Type = ItemType.Movie;

            item.ParticipantsList = ExstractParticipants(json);

            return item;
        }


        public List<PersonSearchItem> ExstractParticipants(Dictionary<string, string> json)
        {
            List<PersonSearchItem> list = new List<PersonSearchItem>();

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
