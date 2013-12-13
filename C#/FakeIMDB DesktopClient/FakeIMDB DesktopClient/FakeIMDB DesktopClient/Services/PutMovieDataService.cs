using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommunicationFramework;
using FakeIMDB_DesktopClient.Model;
using Utils;

namespace FakeIMDB_DesktopClient.Services
{
    internal class PutMovieDataService : IPutMovieDataService
    {

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


        private string PutAllData(MovieSearchItem movieItem, ConnectionModel connectionModel)
        {
            string json = JSonParser.Parse(
                "id", "" + movieItem.Id,
                "title", "" + movieItem.Title,
                "year", "" + movieItem.Year
                );

            //Return the json as encoded bytes
            byte[] data = Encoder.Encode(json);

            var chandler = new CommunicationHandler(connectionModel.Protocol);

            string restAddress = connectionModel.Address + "Movie";

            chandler.Send(restAddress, data, "PUT");


            Dictionary<string, string> jsonDictionary = JSonParser.GetValues(Encoder.Decode(chandler.Receive(10000)));

            return jsonDictionary["response"];
        }

        
    }
}
