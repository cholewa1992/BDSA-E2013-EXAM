using System;
using System.Collections.Generic;
using System.Threading;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services
{
    public interface IMovieExtendedInformationService
    {
        void GetData(Action<MovieSearchItem, Exception> callback, MovieSearchItem searchItem, ConnectionModel connectionModel);

        void GetDataAsync(Action<MovieSearchItem, Exception> callback, MovieSearchItem searchItem, ConnectionModel connectionModel,
            CancellationToken token);
    }
}