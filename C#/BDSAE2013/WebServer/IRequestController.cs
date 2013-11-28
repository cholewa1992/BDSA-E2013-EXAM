using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationFramework;

namespace WebServer
{
    public interface IRequestController
    {
        string Keyword { get; set; }

        void ProcessRequest(Request request);

    }



}
