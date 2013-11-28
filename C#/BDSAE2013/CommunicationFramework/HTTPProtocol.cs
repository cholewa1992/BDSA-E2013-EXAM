using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    class HTTPProtocol : IProtocol
    {
        private WebRequest _request;
        public string Address{ get; set; }

        public HTTPProtocol( string address )
        {
            
        }

        public byte[] GetResponse( int timeout )
        {
            WebResponse response = _request.GetResponse();
            HttpWebResponse webResponse = (HttpWebResponse) response;
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

            Request request = new Request() { OutputStream = context.Response.OutputStream, Method = context.Request.HttpMethod + " " + context.Request.RawUrl };

            request.Data = Encoding.GetEncoding( "iso-8859-1" ).GetBytes( new StreamReader( context.Request.InputStream ).ReadToEnd() );

            return request;
        }
    }
}
