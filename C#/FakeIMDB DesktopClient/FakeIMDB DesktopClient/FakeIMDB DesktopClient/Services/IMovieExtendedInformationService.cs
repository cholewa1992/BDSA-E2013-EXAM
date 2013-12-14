using System;
using System.Collections.Generic;
using System.Threading;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services
{
    /// <summary>
    /// Interface describing a MovieExtendedInformationService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public interface IMovieExtendedInformationService
    {
        void GetData(Action<MovieSearchItem, Exception> callback, MovieSearchItem searchItem, ConnectionModel connectionModel);
        void GetDataAsync(Action<MovieSearchItem, Exception> callback, MovieSearchItem searchItem, ConnectionModel connectionModel,
            CancellationToken token);
    }
}