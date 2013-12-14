using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    /// <summary>
    /// The object to pass around in the system that contains all the information that should be relevant
    /// </summary>
    /// <author>
    /// Mathias Pedersen (mkin@itu.dk)
    /// Martin Juul Petersen (mjup@itu.dk)
    /// </author>
    public class Request
    {
        public byte[] Data{ get; set; }
        public string Method{ get; set; }
        //Enum of the status codes that can be returned by the request
        public enum StatusCode
        {
            BadRequest,
            NotFound,
            InternalError,
            Ok
        }
        public StatusCode ResponseStatusCode{ get; set; }
    }
}