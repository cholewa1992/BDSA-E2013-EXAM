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
    class PersonExtendedInformationService : IPersonExtendedInformationService
    {


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


        public void GetData(Action<PersonSearchItem, Exception> callback, ISearchItem searchItem)
        {
            
        }

        public PersonSearchItem FetchInfo(PersonSearchItem item, ConnectionModel connectionModel)
        {

            var ch = new CommunicationHandler(connectionModel.Protocol);

            string restAddress = connectionModel.Address + "PersonData/" + item.Id;

            ch.Send(restAddress, new byte[0], "GET");


            var json = JSonParser.GetValues(Encoder.Decode(ch.Receive(10000)));

            item.Id = json["id"];
            item.Name = json["name"];
            item.Gender = json["gender"];

            item.Birthdate = json.ContainsKey("piBirthDate0Info") ? json["piBirthDate0Info"] : null;


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
