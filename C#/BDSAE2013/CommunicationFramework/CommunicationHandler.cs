﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    public enum Protocols
    {
        HTTP
    }


    public class CommunicationHandler
    {
        private IProtocol _protocolInstance;
        private IProtocol ProtocolInstance
        {
            get
            {
                if( _protocolInstance == null )
                    _protocolInstance = getProtocol( Protocol );

                _protocolInstance.Address = "http://localhost:1337/";
                return _protocolInstance;
            }
            set
            {
                _protocolInstance = value;
            }
        }



        private IProtocol getProtocol(Protocols protocol)
        {
            try
            {

                IProtocol protocolInstance =
                    (IProtocol) Activator.CreateInstance(null, protocol.ToString() + "Protocol");

                if (protocol == null)
                    throw new Exception();

                return protocolInstance;

            }
            catch
            {
                return new HTTPProtocol("");
            }
            


        }



        public Protocols Protocol{ get; set; }



        public CommunicationHandler(Protocols protocol)
        {
            Protocol = protocol;
        }
        
        
        public void Send( string address, byte[] data, string method )
        {
            if( Protocol == null )
                throw new ProtocolException( "ERROR! Protocol not set" );

            ProtocolInstance.Address = address;
            ProtocolInstance.SendMessage(data, method);
        }

        public byte[] Receive( int timeout )
        {
            if( Protocol == null )
                throw new ProtocolException("ERROR! Protocol not set");

            return ProtocolInstance.GetResponse(timeout);
        }

        public Request GetRequest()
        {
            if (Protocol == null)
                throw new ProtocolException("ERROR! Protocol not set");

            return ProtocolInstance.getRequest();
        }

        public void RespondToRequest(Request request)
        {
            if (Protocol == null)
                throw new ProtocolException("ERROR! Protocol not set");

            ProtocolInstance.RespondToRequest(request);
        }
    }
}
