using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using Storage;

namespace WebServer
{
    public abstract class AbstractRequestController : IRequestController
    {
        public string Keyword { get; set; }

        public Func<IStorageConnectionBridge, object> ProcessRequest(Request request)
        {
            string input = request.Method.Split(' ')[0];

            switch (input) 
            { 
                case "GET":
                    return ProcessGet(request);

                case "POST":
                    return ProcessPost(request);
                    
                case "DELETE":
                    return ProcessDelete(request);
                    
                case "PUT":
                    return ProcessPut(request);
            }

            throw new ArgumentException("Input did not match any controllers");
        }

        public NameValueCollection ConvertByteToDataTable(byte[] bytes)
        {
            return HttpUtility.ParseQueryString(Encoding.GetEncoding("iso-8859-1").GetString(bytes)); 
        }

        public abstract Func<IStorageConnectionBridge, object> ProcessGet(Request request);
        public abstract Func<IStorageConnectionBridge, object> ProcessPost(Request request);
        public abstract Func<IStorageConnectionBridge, object> ProcessDelete(Request request);
        public abstract Func<IStorageConnectionBridge, object> ProcessPut(Request request);


    }
}
