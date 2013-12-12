using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    /// <summary>
    ///     This is the implementation of the IProtocol interface specifying communication
    ///     over with HTTP
    ///     @inv !String.IsNullOrEmpty(_address)
    /// </summary>
    internal class HTTPProtocol : IProtocol
    {
        //Lookup table based on the request object and it's ListenerContext. This is specific for the HTTPProtocol
        private Dictionary<Request, HttpListenerContext> _lookupTable = new Dictionary<Request, HttpListenerContext>();

        private readonly Dictionary<Request, HttpListenerContext> _writeableLookupTable =
            new Dictionary<Request, HttpListenerContext>();

        private string _address;
        private HttpListener _listener;
        private WebRequest _request;

        /// <summary>
        ///     Default constructor, initializes the Address to the default address
        /// </summary>
        public HTTPProtocol() : this("http://localhost:1337/")
        {
        }

        /// <summary>
        ///     Construct a HTTPProtocol and sets the Address to the address parameter
        /// </summary>
        /// <param name="address">address of the protocol to connect to</param>
        public HTTPProtocol(string address)
        {
            Address = address;
        }

        private HttpListener Listener
        {
            get
            {
                if (_listener == null)
                {
                    _listener = new HttpListener();

                    //Add the address to the list of prefixes that the listener listens on
                    _listener.Prefixes.Add(Address);
                    //And the start the listening routine
                    _listener.Start();
                }

                return _listener;
            }
            set { _listener = value; }
        }

        public string Address
        {
            get { return _address; }
            set
            {
                //Check that the address is actually valid. This is specific for each protocol.
                //This regex makes sure that a address is either of the form "http://my.web.url/" or "localhost:1337"
                if (!Regex.Match(value, @"(http://)?([.]+?\.[a-zA-Z]{2,3}|localhost:[\d]+)(/)?").Success)
                    throw new ProtocolException("ERROR! Address not valid");

                _address = value;

                //CheckForInvariant !String.IsNullOrEmpty(_address)
                if (String.IsNullOrEmpty(_address))
                    throw new ProtocolException("ERROR! Invariant !String.IsNullOrEmpty(_address) not satisfied");
            }
        }

        /// <summary>
        ///     Get a response using the HTTPProtocol. After timeout amount of time has passed, casts a ProtocolException
        ///     @pre _request != null
        ///     @post response != null
        ///     @post webResponse.StatusDescription == "Ok"
        /// </summary>
        /// <param name="timeout">The amount of time to wait for the response before giving up</param>
        /// <returns>The data that was received</returns>
        public byte[] GetResponse(int timeout)
        {
            //CheckPreCondition _request != null
            if (_request == null)
                throw new ProtocolException("SendMessage must be used before GetResponse");

            WebResponse response = null;

            var tokenSource = new CancellationTokenSource();
            Exception exception = null;


            //Start a new thread to check for the response
            Task t = Task.Run(() =>
            {
                try
                {
                    response = _request.GetResponse();
                }
                catch (Exception e)
                {
                    exception = e;
                }
            }, tokenSource.Token
                );

            //Loop to check if we have passed the timeout
            while(response == null && timeout > 0)
            {
                if (exception != null)
                    throw exception;

                int interval = 100;
                Thread.Sleep(interval);
                timeout -= interval;
            }

            //CheckPostCondition response != null
            //If response is null at this time, it means we have waited enough and stop the thread from waiting
            //and then cast an exception
            if (response == null)
            {
                tokenSource.Cancel();
                throw new TimeoutException("Timeout of " + timeout + " surpassed without response");
            }

            var webResponse = (HttpWebResponse) response;
            //CheckPostCondition webResponse.StatusDescription == "Ok"
            if (webResponse.StatusDescription != "Ok")
                throw new ProtocolException(webResponse.StatusDescription);

            //Create a new MemoryStream and get the data from the responseStream and return it as a byte array
            using (var ms = new MemoryStream())
            {
                webResponse.GetResponseStream().CopyTo(ms);

                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Send a request using the HTTPProtocol
        /// </summary>
        /// <param name="data">Data to include in the request</param>
        /// <param name="method">Method to send the request as</param>
        public void SendMessage(byte[] data, string method)
        {
            //Create a new WebRequest on the specified address
            _request = WebRequest.Create(Address);
            _request.Method = method;

            if (method.ToLower() != "get")
            {
                //Set the length of the content
                _request.ContentLength = data.Length;
                //TODO: Is type relevant?
                _request.ContentType = "application/data";

                //And write the content into the requestStream
                Stream stream = _request.GetRequestStream();
                stream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        ///     Get a request using the HTTPProtocol
        /// </summary>
        /// <returns>A request object created from the request that was received using the http protocol</returns>
        public Request GetRequest()
        {
            //Wait until we actually receive a request, then process it
            HttpListenerContext context = Listener.GetContext();

            var request = new Request {Method = context.Request.HttpMethod + " " + context.Request.Url};

            //Add a mapping from the request object to the context that received it
            //so we know where to send a response to
            _writeableLookupTable.Add(request, context);
            Interlocked.Exchange(ref _lookupTable, _writeableLookupTable);

            request.Data =
                Encoding.GetEncoding("iso-8859-1").GetBytes(new StreamReader(context.Request.InputStream).ReadToEnd());

            //Listener.Close();

            return request;
        }

        /// <summary>
        ///     Send a response to where the request object came from using the data from it.
        ///     The request object must be the actual object returned from GetRequest.
        ///     @pre lookupTable.contains( request )
        /// </summary>
        /// <param name="request">The request object to respond with returned by the GetRequest function</param>
        public void RespondToRequest(Request request)
        {
            HttpListenerContext context;
            //CheckPreCondition lookupTable.contains( request )
            if (!_lookupTable.TryGetValue(request, out context))
                throw new ProtocolException(
                    "ERROR! No context found which received the supplied request. Did you create request object yourself?");


            context.Response.StatusDescription = request.ResponseStatusCode.ToString();
            context.Response.ContentLength64 = request.Data.Length;

            using (Stream stream = context.Response.OutputStream)
                stream.Write(request.Data, 0, request.Data.Length);
        }
    }
}