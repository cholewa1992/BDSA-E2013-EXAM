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
    class PutPersonDataService : IPutPersonDataService
    {


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


        private string PutAllData(PersonSearchItem personItem, ConnectionModel connectionModel)
        {
            string json = JSonParser.Parse(
                "id", "" + personItem.Id,
                "title", "" + personItem.Name,
                "gender", "" + personItem.Gender
                );

            string restAddress = connectionModel + "Person";

            //Return the json as encoded bytes
            byte[] data = Encoder.Encode(json);

            var chandler = new CommunicationHandler(Protocols.Http);

            chandler.Send(restAddress, data, "PUT");


            string response = Encoder.Decode(chandler.Receive(10000));

            return response;
        }
    }
}
