using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using System.Collections.Specialized;
using Storage;

namespace WebServer
{
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
