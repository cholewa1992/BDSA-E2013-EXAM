using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using System.Threading.Tasks;
using CommunicationFramework;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using Storage;
using Newtonsoft.Json;

namespace WebServer
{
    /// <summary>
    /// Abstract class that implements parts of the RequestController class hierachy
    /// @invariant Keyword != null
    /// </summary>
    public abstract class AbstractRequestController : IRequestController
    {
        public string Keyword { get; set; }

        /// <summary>
        /// A general method for all request controllers that processes the part of the request that defines the type of the rest method (GET, PUT, POST, DELETE)
        /// The method determines which rest method to invoked and returns the product.
        /// </summary>
        /// <param name="request"> The original request received by the Web Server </param>
        /// <returns> A delegate that can be given a storage in order to perform a request. This can be GET, PUT, POST and DELETE requests from each of the entities </returns>
        public abstract Func<IStorageConnectionBridgeFacade, byte[]> ProcessRequest(Request request);

        /// <summary>
        /// Converts a byte code into a dictionary of values.
        /// </summary>
        /// <param name="bytes"> The bytes containing the values </param>
        /// <returns> A dictionary with the values contained in the byte code </returns>
        public Dictionary<string, string> GetRequestValues(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes cannot be null");

            //Decode the byte code with the proper encoding.
            string json = Encoder.Decode(bytes);
            
            //Then we parse the resulting string through the JSonParser class to get the values contained in the byte code.
            try
            {
                return JSonParser.GetValues(json);
            }
            catch (JsonReaderException e)
            {
                throw new InvalidDataException("Data did not contain proper JSon");
            }
        }

        public string GetUrlArgument(string method)
        {
            if (method == null)
                throw new ArgumentNullException("Method cannot be null");

            if (method.Split(' ').Length != 2)
                throw new UnsplittableStringParameterException("Incoming request method has bad syntax, must be [Method]' '[URL]");

            //Get the request value of the url
            string url = method.Split(' ')[1];
            string returnString = url.Split('/').Last();

            if (returnString == "")
                throw new InvalidUrlParameterException("URL ending was an empty string");

            return returnString;
        }
    }
}
