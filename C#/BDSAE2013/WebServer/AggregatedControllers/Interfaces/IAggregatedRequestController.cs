using System;
using CommunicationFramework;
using System.Collections.Specialized;
using Storage;

namespace WebServer
{
    /// <summary>
    /// The general interface that defines the structure of the request controller
    /// @invariant Keyword != null
    /// </summary>
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
    public interface IAggregatedRequestController : IRequestController
    {
        string Keyword { get; set; }

        Func<IStorageConnectionBridgeFacade, object> ProcessRequest(Request request);

        Func<IStorageConnectionBridgeFacade, object> ProcessGet(Request request);

        NameValueCollection ConvertByteToDataTable(byte[] bytes);
    }



}
