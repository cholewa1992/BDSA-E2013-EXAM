using System;
using System.Threading;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services
{
    /// <summary>
    /// Interface describing a PutMovieDataService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public interface IPutMovieDataService
    {
        void PutData(Action<string, Exception> callback, MovieSearchItem movieItem, ConnectionModel connectionModel);
        void PutDataAsync(Action<string, Exception> callback, MovieSearchItem movieItem, ConnectionModel connectionModel, CancellationToken token);
    }
}