﻿using System;
using CommunicationFramework;
using System.Collections.Specialized;
using Storage;
using System.Collections.Generic;

namespace WebServer
{
    /// <summary>
    /// The general interface that defines the structure of the request controller
    /// @invariant Keyword != null
    /// </summary>
    public interface IRequestController
    {
        string Keyword { get; set; }

        Func<IStorageConnectionBridgeFacade, byte[]> ProcessRequest(Request request);

        Dictionary<string, string> GetRequestValues(byte[] bytes);
        int GetUrlArgument(string method);
    }



}
