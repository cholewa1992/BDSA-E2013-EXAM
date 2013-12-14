using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services
{
    /// <summary>
    /// Interface describing a SearchService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public interface ISearchService
    {
        void Search(Action<List<ISearchItem>, Exception> callback, string searchTerm, ConnectionModel connectionModel);
        void SearchAsync(Action<List<ISearchItem>, Exception> callback, string searchTerm, ConnectionModel connectionModel,CancellationToken token);
    }
}
