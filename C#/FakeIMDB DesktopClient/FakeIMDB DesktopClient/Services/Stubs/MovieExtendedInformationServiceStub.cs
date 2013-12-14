using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services.Stubs
{
    /// <summary>
    /// Implementation stub of a MovieExtendedInformationService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class MovieExtendedInformationServiceStub : IMovieExtendedInformationService
    {


        public void GetData(Action<MovieSearchItem, Exception> callback, MovieSearchItem searchItem, ConnectionModel connectionModel)
        {

            callback(FetchInfo(searchItem), null);

        }


        public async void GetDataAsync(Action<MovieSearchItem, Exception> callback, MovieSearchItem searchItem, ConnectionModel connectionModel, CancellationToken token)
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


        private MovieSearchItem FetchInfo(MovieSearchItem item)
        {
            item.Title = "Titanic";
            item.Type = ItemType.Movie;
            item.Year = "19923";
            item.ParticipantsList = new List<PersonSearchItem>()
            {
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"},
                new PersonSearchItem(){ Id = "381455", Name = "Leonardo DiCaprio", CharacterName = "HisCharacterName"}
            };

            return item;
        }

    }
}
