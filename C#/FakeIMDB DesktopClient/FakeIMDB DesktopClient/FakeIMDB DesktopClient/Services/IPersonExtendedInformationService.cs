using System;
using System.Collections.Generic;
using System.Threading;
using FakeIMDB_DesktopClient.Model;

namespace FakeIMDB_DesktopClient.Services
{
    /// <summary>
    /// Interface describing a PersonExtendedInformationService
    /// </summary>
    /// <author>
    /// Mathias Kindsholm Pedersen(mkin@itu.dk)
    /// </author>
    public interface IPersonExtendedInformationService
    {
        void GetData(Action<PersonSearchItem, Exception> callback, PersonSearchItem searchItem, ConnectionModel connectionModel);
        void GetDataAsync(Action<PersonSearchItem, Exception> callback, PersonSearchItem searchItem, ConnectionModel connectionModel,
            CancellationToken token);
    }
}