using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services.Stubs
{
    class PersonExtendedInformationServiceStub : IPersonExtendedInformationService
    {

        public void GetData(Action<PersonSearchItem, Exception> callback, PersonSearchItem searchItem, ConnectionModel connectionModel)
        {
            callback(FetchInfo(searchItem), null);
        }

        public async void GetDataAsync(Action<PersonSearchItem, Exception> callback, PersonSearchItem searchItem, ConnectionModel connectionModel, CancellationToken token)
        {
            await Task.Run(() =>
            {
                try
                {
                    callback(FetchInfo(searchItem), null);
                }
                catch (Exception e)
                {
                    callback(null, e);
                }
            }, token);
        }


        public PersonSearchItem FetchInfo(PersonSearchItem item)
        {
            Thread.Sleep(2000);

            item.Name = "Min-sik Choi";
            item.Birthdate = "April 27, 1962";
            item.Gender = "Male";
            item.Type = ItemType.Person;
            item.Id = "12345";
            item.ParticipatesInList = new List<MovieSearchItem>()
            {
                new MovieSearchItem() {Id = "123", Title = "OldBoy", Type = ItemType.Movie, Year = "2003"},
                new MovieSearchItem() {Id = "123", Title = "OldBoy", Type = ItemType.Movie, Year = "2003"},
                new MovieSearchItem() {Id = "123", Title = "OldBoy", Type = ItemType.Movie, Year = "2003"},
            };

            return item;

        }

    }
}
