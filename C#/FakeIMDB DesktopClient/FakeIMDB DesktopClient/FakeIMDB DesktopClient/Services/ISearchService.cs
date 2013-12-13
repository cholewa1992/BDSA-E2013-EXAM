using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services
{
    public interface ISearchService
    {
        void Search(Action<List<ISearchItem>, Exception> callback, string searchTerm, ConnectionModel connectionModel);
        void SearchAsync(Action<List<ISearchItem>, Exception> callback, string searchTerm, ConnectionModel connectionModel,CancellationToken token);
    }
}
