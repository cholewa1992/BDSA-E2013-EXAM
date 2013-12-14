using System;
using System.Threading;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services
{
    /// <summary>
    /// Interface describing a PutPersonDataService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public interface IPutPersonDataService
    {
        void PutData(Action<string, Exception> callback, PersonSearchItem personItem, ConnectionModel connectionModel);
        void PutDataAsync(Action<string, Exception> callback, PersonSearchItem personItem, ConnectionModel connectionModel, CancellationToken token);
    }
}