using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;
using Storage;
using EntityFrameworkStorage;
using CommunicationFramework;
using System.Text;
using Moq;


namespace WebServerUnitTest
{
    [TestClass]
    public class RequestDelegatorTest
    {
        [TestMethod]
        public void Test_RequestDelegator_Constructor_BaseConstructor()
        {
            //Initialize a request delegator using the base constructor
            RequestDelegator requestDelegator = new RequestDelegator();

            //If no exceptions are thrown, the test is passed
        }

        [TestMethod]
        public void Test_RequestDelegator_Constructor_InjectedStorage()
        {
            //Initialize a request delegator with injection
            RequestDelegator requestDelegator = new RequestDelegator(new StorageFacade(new EFConnectionFactory()));

            //If no exceptions are thrown, the test is passed
        }

        [TestMethod]
        public void Test_RequestDelegator_ProcessRequest_Output_Ok()
        {
            //Make a mock of the storage. Since this test is not concerned with the output of the database, we do not want an output from it.
            var storageMock = new Mock<IStorageConnectionBridge>();        //create a mock of a storage
            var storage = storageMock.Object;                              //Make an intance of the storage class using the mock

            RequestDelegator requestDelegator = new RequestDelegator(storage);

            //Make a request with a wrong method. (No right-hand url input)
            Request req = new Request() { Method = "GET https://www.google.dk/Movie", Data = Encoding.GetEncoding("iso-8859-1").GetBytes("id=6&name=John") };

            Request requestResult = requestDelegator.ProcessRequest(req);

            Assert.AreEqual(Request.StatusCode.Ok, requestResult.ResponseStatusCode);
        }

        [TestMethod]
        public void Test_RequestDelegator_ProcessRequest_Output_BadRequest_FromDefineController()
        {
            RequestDelegator requestDelegator = new RequestDelegator();

            //Make a request with a wrong method. (No right-hand url input)
            Request req = new Request() { Method = "GET https://www.google.dk/Movies", Data = Encoding.GetEncoding("iso-8859-1").GetBytes("id=6&name=John") };

            Request requestResult = requestDelegator.ProcessRequest(req);

            Assert.AreEqual(Request.StatusCode.BadRequest, requestResult.ResponseStatusCode);
        }

        [TestMethod]
        public void Test_RequestDelegator_ProcessRequest_Output_BadRequest_FromProcessRequest()
        {
            RequestDelegator requestDelegator = new RequestDelegator();

            //Make a request with a wrong method. (No right-hand url input)
            Request req = new Request() { Method = "UPDATE https://www.google.dk/Movie", Data = Encoding.GetEncoding("iso-8859-1").GetBytes("id=6&name=John") };

            Request requestResult = requestDelegator.ProcessRequest(req);

            Assert.AreEqual(Request.StatusCode.BadRequest, requestResult.ResponseStatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Parsed request is null")]
        public void Test_RequestDelegator_ProcessRequest_Error_NullInput()
        {
            RequestDelegator requestDelegator = new RequestDelegator();

            requestDelegator.ProcessRequest(null);
        }

        //TODO Make test to check functionality of DefineController
        
        /*
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Parsed method is null")]
        public void Test_RequestDelegator_DefineController_Error_NullInput()
        {
            //Initialize a request delegator with injection
            RequestDelegator requestDelegator = new RequestDelegator();

            requestDelegator.DefineController(null);
        }
        */


    }
}
