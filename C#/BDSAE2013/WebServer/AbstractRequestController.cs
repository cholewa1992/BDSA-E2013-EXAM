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
    /// <summary>
    /// Abstract class that implements parts of the RequestController class hierachy
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
        public Func<IStorageConnectionBridge, object> ProcessRequest(Request request)
        {
            //QUESTION: Null was checked in preceeding call hierachy. Should we check for null again for the sake of potential decoupling?

            //Split the request method string by the 'space' character, and get the first part of the resulting array.
            //This is the part of the method string that contains the rest request type
            string input = request.Method.Split(' ')[0];

            //Check which rest request type the request is and parse it to the proper method
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

            //If the request does not match any rest methods it is an invalid input and thus the program throws an error
            throw new ArgumentException("Input did not match any controllers");
        }

        /// <summary>
        /// Converts a byte code into a table of values.
        /// </summary>
        /// <param name="bytes"> The bytes containing the values </param>
        /// <returns> A table with the values contained in the byte code </returns>
        public NameValueCollection ConvertByteToDataTable(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentException("bytes cannot be null");

            //Decode the byte code with the proper encoding.
            string decodedString = Encoding.GetEncoding("iso-8859-1").GetString(bytes);
            
            //Then we parse the resulting string through the HttpUtility module which converts it into a table for easy access of the data contained in the byte code.
            //The string contained in the byte code should have the format 'valueName'='value'&'valueName'='value'&...
            return HttpUtility.ParseQueryString(decodedString); 
        }

        public abstract Func<IStorageConnectionBridge, object> ProcessGet(Request request);
        public abstract Func<IStorageConnectionBridge, object> ProcessPost(Request request);
        public abstract Func<IStorageConnectionBridge, object> ProcessDelete(Request request);
        public abstract Func<IStorageConnectionBridge, object> ProcessPut(Request request);
    }
}
