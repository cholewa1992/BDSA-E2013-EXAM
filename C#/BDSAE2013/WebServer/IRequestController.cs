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
    public interface IRequestController
    {
        string Keyword { get; set; }

        Func<IStorageConnectionBridgeFacade, object> ProcessRequest(Request request);

        Func<IStorageConnectionBridgeFacade, object> ProcessGet(Request request);
        Func<IStorageConnectionBridgeFacade, object> ProcessPost(Request request);
        Func<IStorageConnectionBridgeFacade, object> ProcessDelete(Request request);
        Func<IStorageConnectionBridgeFacade, object> ProcessPut(Request request);

        NameValueCollection ConvertByteToDataTable(byte[] bytes);
    }



}
