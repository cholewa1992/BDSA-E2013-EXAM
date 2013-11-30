using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    //Interface for all the protocols to adhere to
    public interface IProtocol
    {
        string Address{ get; set; }

        byte[] GetResponse( int timeout );
        void SendMessage( byte[] data, string method );
        Request GetRequest();
        void RespondToRequest( Request request );
    }
}