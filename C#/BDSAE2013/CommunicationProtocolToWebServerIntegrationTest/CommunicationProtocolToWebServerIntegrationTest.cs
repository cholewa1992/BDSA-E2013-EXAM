using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationFramework;
using System.Threading.Tasks;

namespace CommunicationProtocolToWebServerIntegrationTest
{
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
    [TestClass]
    public class CommunicationProtocolToWebServerIntegrationTest
    {
        [TestMethod]
        public void Test_CommunicationProtocolToWebServerIntegration_Send_GET()
        {
            var communicationHandler = new CommunicationHandler(Protocols.Http);

            string address = "http://localhost:1000/Movie/5/";

            Task.Run( () => 
            {
                var handler = new CommunicationHandler(Protocols.Http); 
                handler.Send(address, null, "GET");

                byte[] respondBytes = handler.Receive(100);

                CollectionAssert.AreEqual(new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 1 }, respondBytes);
            });

            var request = communicationHandler.GetRequest("http://localhost:1000/");
            Request oldRequest = new Request() { Method = request.Method, Data = request.Data };

            request.Data = new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 1 };

            communicationHandler.RespondToRequest(request);

            Assert.AreEqual("GET /Movie/5/", oldRequest.Method);
        }

        [TestMethod]
        public void Test_CommunicationProtocolToWebServerIntegration_Send_NotGET()
        {
            var communicationHandler = new CommunicationHandler(Protocols.Http);

            string address = "http://localhost:1001/Movie/5/";

            Task.Run(() =>
            {
                var handler = new CommunicationHandler(Protocols.Http); 
                handler.Send(address, new byte[]{0,1,1,1,0,1,1,0,0}, "POST");

                byte[] respondBytes = handler.Receive(100);

                CollectionAssert.AreEqual(new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 1 }, respondBytes);
            });

            var request = communicationHandler.GetRequest("http://localhost:1001/");
            Request oldRequest = new Request() { Method = request.Method, Data = request.Data };

            request.Data = new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 1 };

            communicationHandler.RespondToRequest(request);

            Assert.AreEqual("POST /Movie/5/", oldRequest.Method);
            CollectionAssert.AreEqual(new byte[] { 0, 1, 1, 1, 0, 1, 1, 0, 0 }, oldRequest.Data);
        }
    }
}