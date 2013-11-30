using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunicationFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommunicationFrameworkUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var Server = new CommunicationHandler(Protocols.HTTP);


            Task.Run(delegate
            {
                Thread.Sleep(500);

                var Client = new CommunicationHandler(Protocols.HTTP);

                
                Client.Send("http://localhost:1337", null, "Test");

            });

            

            String[] method = Server.GetRequest().Method.Split(' ');

            
            Assert.AreEqual("Test", method[0]);
        }
    }
}
