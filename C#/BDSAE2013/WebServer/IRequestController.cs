﻿using System;
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

        Func<IStorageConnectionBridge, object> ProcessRequest(Request request);

        Func<IStorageConnectionBridge, object> ProcessGet(Request request);
        Func<IStorageConnectionBridge, object> ProcessPost(Request request);
        Func<IStorageConnectionBridge, object> ProcessDelete(Request request);
        Func<IStorageConnectionBridge, object> ProcessPut(Request request);

        NameValueCollection ConvertByteToDataTable(byte[] bytes);
    }



}
