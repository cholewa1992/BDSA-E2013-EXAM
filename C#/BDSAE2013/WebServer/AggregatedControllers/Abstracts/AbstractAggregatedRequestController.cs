﻿using System;
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
    /// </author>
    public abstract class AbstractAggregatedRequestController : AbstractRequestController
    {
        /// <summary>
        /// A general method for all aggregate request controllers that processes the part of the request that defines the type of the rest method (allows GET)
        /// The method determines whether or not the incoming request matches a get request
        /// @pre request != null
        /// @pre request.Method != null
        /// @pre request.Method.Split(' ').Length == 2
        /// </summary>
        /// <param name="request"> The original request received by the Web Server </param>
        /// <returns> A delegate that can be given a storage in order to perform a request. This can be a GET requests </returns>
        public override Func<IStorageConnectionBridgeFacade, byte[]> ProcessRequest(Request request)
        {
            //Perform pre condition checks and throw relevant exceptions
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
            switch (input.ToUpper()) 
            { 
                case "GET":
                    return ProcessGet(request);
            }

            //If the request does not match any rest methods it is an invalid input and thus the program throws an error
            throw new InvalidRestMethodException("Input did not match any REST method. Aggregated controllers only allows GET requests");
        }

        public abstract Func<IStorageConnectionBridgeFacade, byte[]> ProcessGet(Request request);
    }
}
