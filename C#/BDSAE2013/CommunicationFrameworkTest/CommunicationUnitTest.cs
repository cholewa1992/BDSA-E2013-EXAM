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
        public void TestHTTProtocolWithNullData()
        {
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                new CommunicationHandler( Protocols.HTTP ).Send( "http://localhost:1337/", null, "test" );
            }, "ERROR! Data cannot be null" );
        }

        [ TestMethod ]
        public void TestHTTPProtocolFunctionalityWithoutData()
        {
            var Server = new CommunicationHandler( Protocols.HTTP );

            Task.Run( delegate
            {
                Thread.Sleep( 500 );

                var Client = new CommunicationHandler( Protocols.HTTP );
                Client.Send( "http://localhost:1337/", new byte[ 0 ], "Test" );
            } );

            String[] method = Server.GetRequest().Method.Split( ' ' );
            Assert.AreEqual( "Test", method[ 0 ] );
        }

        [ TestMethod ]
        public void TestHTTPProtocolWithData()
        {
            var Server = new CommunicationHandler( Protocols.HTTP );

            Task.Run( () =>
            {
                Thread.Sleep( 500 );

                var Client = new CommunicationHandler( Protocols.HTTP );
                Client.Send( "http://localhost:1337/", Encoding.GetEncoding( "iso-8859-1" ).GetBytes( "test".ToCharArray() ), "Test" );
            } );

            Assert.AreEqual( "test", Encoding.GetEncoding( "iso-8859-1" ).GetString( Server.GetRequest().Data ) );
        }

        [ TestMethod ]
        public void TestHttpProtocolInvalidAddress()
        {
            var Client = new CommunicationHandler( Protocols.HTTP );
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                Client.Send( "abc", Encoding.GetEncoding( "iso-8859-1" ).GetBytes( "test1234".ToCharArray() ), "Test" );
            }, "ERROR! Address not valid" );
        }

        [ TestMethod ]
        public void TestHttpProtocolInvalidAddressDomain()
        {
            var Client = new CommunicationHandler( Protocols.HTTP );
            ExceptionAssert.Throws<ProtocolException>( () =>
            {
                Client.Send( "http://localhost.123", Encoding.GetEncoding( "iso-8859-1" ).GetBytes( "test1234".ToCharArray() ), "Test" );
            }, "ERROR! Address not valid" );
        }
    }
}