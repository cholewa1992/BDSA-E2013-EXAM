using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using WebServer;
using Storage;
using EntityFrameworkStorage;
using CommunicationFramework;
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
            RequestDelegator requestDelegator = new RequestDelegator(new StorageConnectionBridgeFacade(new EFConnectionFactory()));

            //If no exceptions are thrown, the test is passed
        }

        [TestMethod]
        public void Test_RequestDelegator_ProcessRequest_Output_Ok()
        {
            //Make a mock of the storage. Since this test is not concerned with the output of the database, we do not want an output from it.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();        //create a mock of a storage
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Title = "Die Hard"});

            var storage = storageMock.Object;                                    //Make an intance of the storage class using the mock

            RequestDelegator requestDelegator = new RequestDelegator(storage);

            //Make a request with a correct method.
            Request req = new Request() { Method = "GET http://localhost:112/Movie/5"};

            Request requestResult = requestDelegator.ProcessRequest(req);

            Assert.AreEqual(Request.StatusCode.Ok, requestResult.ResponseStatusCode);
        }

        [TestMethod]
        public void Test_RequestDelegator_ProcessRequest_Output_NotFound()
        {
            //Make a mock of the storage. Since this test is not concerned with the output of the database, we do not want an output from it.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();        //create a mock of a storage
            
            //Set up the storage to throw an exception (simulating a result that was not found)
            storageMock.Setup(x => x.Get<Movies>(5)).Throws(new InvalidOperationException()) ;

            var storage = storageMock.Object;                                    //Make an intance of the storage class using the mock

            RequestDelegator requestDelegator = new RequestDelegator(storage);

            //Make a request with a correct method.
            Request req = new Request() { Method = "GET http://localhost:112/Movie/5" };

            //We make a request to the request delegator.
            Request requestResult = requestDelegator.ProcessRequest(req);

            //Check that the return code is correct (NotFound)
            Assert.AreEqual(Request.StatusCode.NotFound, requestResult.ResponseStatusCode);
        }

        [TestMethod]
        public void Test_RequestDelegator_ProcessRequest_Output_InternalError()
        {
            //Make a mock of the storage. Since this test is not concerned with the output of the database, we do not want an output from it.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();        //create a mock of a storage

            //Set up the storage to throw an exception (simulating that there was an internal error)
            storageMock.Setup(x => x.Get<Movies>(5)).Throws(new Exception());

            var storage = storageMock.Object;                                    //Make an intance of the storage class using the mock

            RequestDelegator requestDelegator = new RequestDelegator(storage);

            //Make a request with a correct method.
            Request req = new Request() { Method = "GET http://localhost:112/Movie/5" };

            //We make a request to the request delegator.
            Request requestResult = requestDelegator.ProcessRequest(req);

            //Check that the return code is correct (NotFound)
            Assert.AreEqual(Request.StatusCode.InternalError, requestResult.ResponseStatusCode);
        }

        [TestMethod]
        public void Test_RequestDelegator_ProcessRequest_Output_BadRequest_FromDefineController()
        {
            RequestDelegator requestDelegator = new RequestDelegator();

            //Make a request with a wrong method. (No right-hand url input)
            Request req = new Request() { Method = "GET https://www.google.dk/Movies/5" };

            Request requestResult = requestDelegator.ProcessRequest(req);

            Assert.AreEqual(Request.StatusCode.BadRequest, requestResult.ResponseStatusCode);
        }

        [TestMethod]
        public void Test_RequestDelegator_ProcessRequest_Output_BadRequest_FromRequestController()
        {
            RequestDelegator requestDelegator = new RequestDelegator();

            //Make a request with a wrong method. (No right-hand url input)
            Request req = new Request() { Method = "UPDATE https://www.google.dk/Movie" };

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
            Request req = new Request() { Method = null, Data = Encoder.Encode(JSonParser.Parse("id", "6")) };

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
            Request req = new Request() { Method = "GET", Data = Encoder.Encode(JSonParser.Parse("id", "6")) };

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
            Request req = new Request() { Method = "UPDATE https://www.google.dk/MovieDatabase", Data = Encoder.Encode(JSonParser.Parse("id", "6")) };
            
            //The invocation should throw the InvalidServiceRequestException
            requestDelegator.DefineController(req.Method);
        }
    }
}
