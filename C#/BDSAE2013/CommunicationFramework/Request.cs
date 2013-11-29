using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    public class Request
    {
        public byte[] Data{ get; set; }
        public string Method{ get; set; }
        //public string Address{ get; set; }
        public enum StatusCode
        {
            NotFount,
            Ok,
            BadRequest
        }

        public StatusCode ResponseStatusCode { get; set; }
    }
}
