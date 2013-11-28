using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    public interface IProtocol
    {
        string Address{ get; set; }

        byte[] GetResponse( int timeout );
        void SendMessage( byte[] data, string method );
        Request getRequest();
        void RespondToRequest(Request request);
    }
}
