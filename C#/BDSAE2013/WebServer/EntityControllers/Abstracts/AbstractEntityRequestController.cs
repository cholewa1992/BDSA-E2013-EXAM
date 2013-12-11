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
    /// @invariant Keyword != null
    /// </summary>
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// Morten Rosenmeier (morr@itu.dk)
    /// </author>
    public abstract class AbstractEntityRequestController : AbstractRequestController
    {

        /// <summary>
        /// A general method for all request controllers that processes the part of the request that defines the type of the rest method (GET, PUT, POST, DELETE)
        /// The method determines which rest method to invoked and returns the product.
        /// @pre request != null
        /// @pre request.Method != null
        /// @pre request.Method.Split(' ').Length == 2
        /// </summary>
        /// <param name="request"> The original request received by the Web Server </param>
        /// <returns> A delegate that can be given a storage in order to perform a request. This can be GET, PUT, POST and DELETE requests from each of the entities </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessRequest(Request request)
        {
            if (request == null)
                throw new ArgumentNullException("Incoming request cannot be null");

            if (request.Method == null)
                throw new ArgumentNullException("Incoming request method cannot be null");

            if (request.Method.Split(' ').Length != 2)
                throw new UnsplittableStringParameterException("Incoming request method has bad syntax, must be [Method]' '[URL]");

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
            throw new InvalidRestMethodException("Input did not match any REST method");
        }

        public abstract Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request);
        public abstract Func<IStorageConnectionBridgeFacade, byte[]> ProcessPost(Request request);
        public abstract Func<IStorageConnectionBridgeFacade, byte[]> ProcessDelete(Request request);
        public abstract Func<IStorageConnectionBridgeFacade, byte[]> ProcessPut(Request request);
    }
}
