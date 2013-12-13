using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunicationFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunicationFrameworkUnitTest
{
    public class ExceptionAssert
    {
        public static void Throws< T >( Action action, string expectedMessage ) where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch( T exc )
            {
                Assert.AreEqual( expectedMessage, exc.Message );
                return;
            }
            Assert.Fail( "Exception of type {0} should be thrown.", typeof( T ) );
        }
    }

    [ TestClass ]
    public class CommunicationUnitTest
    {
        //Test ideas: Check to make sure timeout actually works,
        //Check whether or not the actual send -> receive -> do stuff -> send -> receive actually works,

        [ TestMethod ]
        public void CommunicationHandler_Constructor_ProtocolIsNotNull()
        {
            var handler = new CommunicationHandler( Protocols.Http );

            Assert.AreNotEqual( null, handler.Protocol );
        }

        [ TestMethod ]
        public void CommunicationHandler_Constructor_ProtocolIsHTTP()
        {
            var handler = new CommunicationHandler( Protocols.Http );

            Assert.AreEqual( Protocols.Http, handler.Protocol );
        }

        [ TestMethod ]
        public void CommunicationHandler_Protocol_InvalidInteger()
        {
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                var handler = new CommunicationHandler( (Protocols) 10 );
            }, "ERROR! Supplied protocol does not exist" );
        }

        [ TestMethod ]
        public void CommunicationHandler_Send_NullArguments()
        {
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                new CommunicationHandler( Protocols.Http ).Send( null, null, null );
            }, "ERROR! Address cannot be null or empty" );
        }

        [ TestMethod ]
        public void CommunicationHandler_Send_NullDataNullMethod()
        {
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                new CommunicationHandler( Protocols.Http ).Send( "http://localhost/", null, null );
            }, "ERROR! Method cannot be null or empty" );
        }

        [ TestMethod ]
        public void CommunicationHandler_Send_NullMethod()
        {
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                new CommunicationHandler( Protocols.Http ).Send( "http://localhost/", new byte[10], null );
            }, "ERROR! Method cannot be null or empty" );
        }

        [ TestMethod ]
        public void CommunicationHandler_Send_NullData()
        {
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                new CommunicationHandler( Protocols.Http ).Send( "http://localhost/", null, "POST" );
            }, "ERROR! Data cannot be null for any methods other than GET" );
        }

        [ TestMethod ] 
        public void HTTPProtocol_Send_AddressInvalid_abc()
        {
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                new CommunicationHandler( Protocols.Http ).Send( "abc", null, "GET" );
            }, "ERROR! Address not valid" );
        }

        [ TestMethod ]
        public void HTTPProtocol_Send_AddressInvalid_localhostWithDot()
        {
            var Client = new CommunicationHandler( Protocols.Http );
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                Client.Send( "http://localhost.123/", null, "GET" );
            }, "ERROR! Address not valid" );
        }

        [ TestMethod ]
        public void HTTPProtocol_Send_AddressInvalid_localhostWithoutH()
        {
            var Client = new CommunicationHandler( Protocols.Http );
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                Client.Send( "http://localost:123/", null, "GET" );
            }, "ERROR! Address not valid" );
        }

        [ TestMethod ]
        public void HTTPProtocol_Send_AddressInvalid_InvalidIP()
        {
            var Client = new CommunicationHandler( Protocols.Http );
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                Client.Send( "192.168.10.2000:1337", null, "GET" );
            }, "ERROR! Address not valid" );
        }

        [ TestMethod ]
        public void HTTPProtocol_Send_AddressInvalid_InvalidIPMissingDot()
        {
            var Client = new CommunicationHandler( Protocols.Http );
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                Client.Send( "http://192.168.102000:1337/", null, "GET" );
            }, "ERROR! Address not valid" );
        }

        [ TestMethod ]
        public void HTTPProtocol_Send_AddressInvalid_UrlDomainLongerThan3()
        {
            var Client = new CommunicationHandler( Protocols.Http );
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                Client.Send( "http://www.google.xxxx/", null, "GET" );
            }, "ERROR! Address not valid" );
        }

        [ TestMethod ]
        public void HTTPProtocol_Receive_TimeoutTest()
        {
            var handler = new CommunicationHandler( Protocols.Http );
            handler.Send( "http://localhost:1000/", null, "GET" );
            ExceptionAssert.Throws<TimeoutException>( () =>
            {
                handler.Receive( 100 );
            }, "Timeout of 100 surpassed without response" );
        }

        [ TestMethod ]
        public void TestHTTPProtocolFunctionalityWithoutData()
        {
            var Server = new CommunicationHandler( Protocols.Http );

            int portToUse = 1;

            Task.Run( delegate
            {
                Thread.Sleep( 500 );

                var Client = new CommunicationHandler( Protocols.Http );
                Client.Send( "http://localhost:" + portToUse + "/", new byte[ 0 ], "Test" );
                Client.Receive();
            } );

            String[] method = Server.GetRequest( "http://localhost:" + portToUse + "/" ).Method.Split( ' ' );
            Assert.AreEqual( "Test", method[ 0 ] );
        }

        [ TestMethod ]
        public void TestHTTPProtocolWithData()
        {
            var Server = new CommunicationHandler( Protocols.Http );

            int portToUse = 2;

            Task.Run( () =>
            {
                Thread.Sleep( 500 );

                var Client = new CommunicationHandler( Protocols.Http );
                Client.Send( "http://localhost:" + portToUse + "/", Encoding.GetEncoding( "iso-8859-1" ).GetBytes( "test".ToCharArray() ), "Test" );
                Client.Receive();
            } );

            Assert.AreEqual( "test", Encoding.GetEncoding( "iso-8859-1" ).GetString( Server.GetRequest( "http://localhost:" + portToUse + "/" ).Data ) );
        }

       
    }
}