using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    class HTTPProtocol : IProtocol
    {
        private WebRequest _request;
        public string Address{ get; set; }
        private Dictionary<Request, HttpListenerContext> lookupTable; 

        public HTTPProtocol( string address )
        {
            Address = address;
        }

        public byte[] GetResponse( int timeout )
        {
            WebResponse response = null;

            if (_request == null)
                throw new ProtocolException("SendMessage must be used before GetResponse");
            

            Task t = Task.Run(() => response = _request.GetResponse());


            while (response == null && timeout > 0)
            {
                int interval = 0;
                Thread.Sleep(interval);
                timeout = timeout - interval;
            }

            if (response == null)
            {
                t.Dispose();
                throw new TimeoutException("Timeout of " + timeout + " surpassed without response");
            }
                
            
            
            HttpWebResponse webResponse = (HttpWebResponse) response;

            if (webResponse.StatusDescription != "Ok")
                throw new ProtocolException(webResponse.StatusDescription);


            using (MemoryStream ms = new MemoryStream())
            {
                webResponse.GetResponseStream().CopyTo(ms);

                return ms.ToArray();
            }


        }

        public void SendMessage( byte[] data, string method )
        {
            _request = WebRequest.Create( Address );
            _request.Method = method;

            _request.ContentLength = data.Length;
            //TODO: Is type relevant?
            _request.ContentType = "";

            Stream stream = _request.GetRequestStream();
            stream.Write( data, 0, data.Length );
        }



        public Request getRequest()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add( Address );
            HttpListenerContext context = listener.GetContext();

            Request request = new Request() { Method = context.Request.HttpMethod + " " + context.Request.RawUrl };

            lookupTable.Add(request, context);

            request.Data = Encoding.GetEncoding( "iso-8859-1" ).GetBytes( new StreamReader( context.Request.InputStream ).ReadToEnd() );

            return request;
        }


        public void RespondToRequest(Request request)
        {
            HttpListenerContext context;
            if (lookupTable.TryGetValue(request, out context))
            {

                context.Response.StatusDescription = request.ResponseStatusCode.ToString();
                context.Response.ContentLength64 = request.Data.Length;

                using (var stream = context.Response.OutputStream)
                {
                    
                    stream.Write(request.Data, 0, request.Data.Length);

                }

            }
            

        }
    }
}
