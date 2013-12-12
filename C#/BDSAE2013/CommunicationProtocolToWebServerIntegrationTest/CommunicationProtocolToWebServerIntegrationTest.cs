using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationFramework;
using WebServer;
using System.Threading.Tasks;

namespace CommunicationProtocolToWebServerIntegrationTest
{
    [TestClass]
    public class CommunicationProtocolToWebServerIntegrationTest
    {
        [TestMethod]
        public void Test_CommunicationProtocolToWebServerIntegration_Send_GET()
        {
            var communicationHandler = new CommunicationHandler(Protocols.HTTP);

            string address = "http://localhost:1000/Movie/5/";

            Task.Run( () => 
            {
                var handler = new CommunicationHandler(Protocols.HTTP); 
                handler.Send(address, null, "GET");

                byte[] respondBytes = communicationHandler.Receive(100);

                CollectionAssert.AreEqual(new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 1 }, respondBytes);
            });

            var request = communicationHandler.GetRequest(address);
            Request oldRequest = new Request() { Method = request.Method, Data = request.Data };

            request.Data = new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 1 };

            communicationHandler.RespondToRequest(request);

            Assert.AreEqual("GET http://localhost:1000/Movie/5/", oldRequest.Method);
        }

        [TestMethod]
        public void Test_CommunicationProtocolToWebServerIntegration_Send_NotGET()
        {
            var communicationHandler = new CommunicationHandler(Protocols.HTTP);

            string address = "http://localhost:1001/Movie/5/";

            Task.Run(() =>
            {
                var handler = new CommunicationHandler(Protocols.HTTP); 
                handler.Send(address, new byte[]{0,1,1,1,0,1,1,0,0}, "POST");

                byte[] respondBytes = communicationHandler.Receive(100);

                CollectionAssert.AreEqual(new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 1 }, respondBytes);
            });

            var request = communicationHandler.GetRequest(address);
            Request oldRequest = new Request() { Method = request.Method, Data = request.Data };

            request.Data = new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 1 };

            communicationHandler.RespondToRequest(request);

            Assert.AreEqual("POST http://localhost:1001/Movie/5/", oldRequest.Method);
            CollectionAssert.AreEqual(new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 0 }, oldRequest.Data);
        }
    }
}