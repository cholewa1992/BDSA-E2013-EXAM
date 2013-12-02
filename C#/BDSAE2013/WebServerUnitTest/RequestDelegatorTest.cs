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
            RequestDelegator requestDelegator = new RequestDelegator(new StorageBridgeFacade(new EFConnectionFactory()));

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
        public void Test_RequestDelegator_ProcessRequest_Output_BadRequest_FromRequestController()
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


        //DefineController has almost been fully tested through process request - should it be private ???
        //TODO Make test to check functionality of DefineController
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Parsed method is null")]
        public void Test_RequestDelegator_DefineController_Error_NullInput()
        {
            //Initialize a request delegator with injection
            RequestDelegator requestDelegator = new RequestDelegator();

            //Make a request with a null method.
            Request req = new Request() { Method = null, Data = Encoding.GetEncoding("iso-8859-1").GetBytes("id=6&name=John") };

            //The invocation should throw the ArgumentNullException
            requestDelegator.DefineController(req.Method);
        }

        [TestMethod]
        [ExpectedException(typeof(UnsplittableStringParameterException),
        "Parsed method is null")]
        public void Test_RequestDelegator_DefineController_Error_WrongMethodSyntax()
        {
            //Initialize a request delegator with injection
            RequestDelegator requestDelegator = new RequestDelegator();

            //Make a request with a wrong method. Syntax should be [Method]' '[Url].
            Request req = new Request() { Method = "GET", Data = Encoding.GetEncoding("iso-8859-1").GetBytes("id=6&name=John") };

            //The invocation should throw the UnsplittableStringParameterException
            requestDelegator.DefineController(req.Method);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidServiceRequestException),
        "Parsed method is null")]
        public void Test_RequestDelegator_DefineController_Error_NoMatch()
        {
            RequestDelegator requestDelegator = new RequestDelegator();

            //Make a request with a wrong method. (url does not contain a valid keyword)
            Request req = new Request() { Method = "UPDATE https://www.google.dk/MovieDatabase", Data = Encoding.GetEncoding("iso-8859-1").GetBytes("id=6&name=John") };
            
            //The invocation should throw the InvalidServiceRequestException
            requestDelegator.DefineController(req.Method);
        }
    }
}
