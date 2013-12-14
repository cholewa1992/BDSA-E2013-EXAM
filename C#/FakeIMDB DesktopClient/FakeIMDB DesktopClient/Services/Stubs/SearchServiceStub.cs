using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services.Stubs
{
    /// <summary>
    /// Implementation stub of a SearchService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public class SearchServiceStub : ISearchService
    {
        public void Search(Action<List<ISearchItem>, Exception> callback, string searchTerm, ConnectionModel connectionModel)
        {
            var item = new List<ISearchItem>
            {
                new MovieSearchItem
                {
                    Id = "1",
                    Title = "Title1",
                    Type = ItemType.Movie,
                    ImageSource = "pack://application:,,,/FakeIMDB DesktopClient;component/Icons/movie-icon.png"
                },
                new PersonSearchItem
                {
                    Id = "3",
                    Name = "Name1",
                    Type = ItemType.Person,
                    ImageSource = "pack://application:,,,/FakeIMDB DesktopClient;component/Icons/User-blue-icon.png"
                }
            };


            callback(item, null);
        }

        public async void SearchAsync(Action<List<ISearchItem>, Exception> callback, string searchTerm, ConnectionModel connectionModel,
            CancellationToken token)
        {
            
            await Task.Run(() =>
            {
                Thread.Sleep(2000);
                var item = new List<ISearchItem>
                {
                    new MovieSearchItem
                    {
                        Id = "1",
                        Title = "Title1",
                        Type = ItemType.Movie,
                        ImageSource = "pack://application:,,,/FakeIMDB DesktopClient;component/Icons/movie-icon.png"
                    },
                    new PersonSearchItem
                    {
                        Id = "3",
                        Name = "Name1",
                        Type = ItemType.Person,
                        ImageSource = "pack://application:,,,/FakeIMDB DesktopClient;component/Icons/User-blue-icon.png"
                    }
                };
                callback(item, null);
            });
        }
    }
}
