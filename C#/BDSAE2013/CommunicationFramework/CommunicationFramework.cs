﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationFramework
{
    public class CommunicationFramework
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
        
        public Protocols Protocol{ get; set; }
        public enum Protocols
        {
            HTTP
        }
        
        public void Send( string address, byte[] data, string method )
        {
            if( Protocol == null )
                throw new Exception( "ERROR! Protocol not set" );

            _protocolInstance.Address = address;
            _protocolInstance.SendMessage( data, method );
        }

        public byte[] Receive( int timeout )
        {
            if( Protocol == null )
                throw new Exception( "ERROR! Protocol not set" );

            return _protocolInstance.GetResponse( timeout );
        }

        public Request GetRequest()
        {
            return _protocolInstance.getRequest();
        }
    }
}