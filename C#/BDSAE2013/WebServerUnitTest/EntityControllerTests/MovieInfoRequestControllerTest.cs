using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;
using Storage;
using EntityFrameworkStorage;
using System.Collections.Specialized;
using CommunicationFramework;
using System.Collections.Generic;
using Moq;
using Utils;

namespace WebServerUnitTest
{
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
    [TestClass]
    public class MovieInfoRequestControllerTest
    {
        [TestMethod]
        public void Test_MovieInfoRequestController_ProcessGet_JSon_Attributes()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method when given the id 5
            storageMock.Setup(x => x.Get<MovieInfo>(5)).Returns(new MovieInfo() { Id = 5, Movie_Id = 10, Type_Id = 2, Info = "52", Note = "Old"});
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/MovieInfo/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("10", values["movieId"]);
            Assert.AreEqual("2", values["typeId"]);
            Assert.AreEqual("52", values["info"]);
            Assert.AreEqual("Old", values["note"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Incoming argument must be formatted as an int")]
        public void Test_MovieInfoRequestController_ProcessGet_Error_NonIntArgument()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/MovieInfo/Die_Hard" };

            //Invoke the ProcessGet method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_MovieInfoRequestController_ProcessGet_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessGet method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request method must no be null")]
        public void Test_MovieInfoRequestController_ProcessGet_Error_NullMethod()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = null };

            //Invoke the ProcessGet method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        public void Test_MovieInfoRequestController_ProcessPost_MinimumInfo()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Initialize an object in which we save our callback
            MovieInfo testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<MovieInfo>(It.IsAny<MovieInfo>())).Callback<MovieInfo>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("movieId", "1", "typeId", "2", "info", "Bald")) };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<MovieInfo>(It.IsAny<MovieInfo>()), Times.Once);
            
            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual(1, testObject.Movie_Id);
            Assert.AreEqual(2, testObject.Type_Id);
            Assert.AreEqual("Bald", testObject.Info);
            Assert.AreEqual("", testObject.Note);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The MovieInfo was successfully added", values["response"]);
        }

        [TestMethod]
        public void Test_MovieInfoRequestController_ProcessPost_MaximumInfo()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Initialize an object in which we save our callback
            MovieInfo testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<MovieInfo>(It.IsAny<MovieInfo>())).Callback<MovieInfo>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("movieId", "1", "typeId", "2", "info", "Bald", "note", "Do not tease")) };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<MovieInfo>(It.IsAny<MovieInfo>()), Times.Once);

            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual(1, testObject.Movie_Id);
            Assert.AreEqual(2, testObject.Type_Id);
            Assert.AreEqual("Bald", testObject.Info);
            Assert.AreEqual("Do not tease", testObject.Note);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The MovieInfo was successfully added", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to MovieInfoController post method did not contain either a title or a user account id")]
        public void Test_MovieInfoRequestController_ProcessPost_DataError_NoMovieId()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no Movie Id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("typeId", "2", "info", "Bald")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to MovieInfoController post method did not contain either a title or a user account id")]
        public void Test_MovieInfoRequestController_ProcessPost_DataError_NoTypeId()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no type id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("movieId", "1", "info", "Bald")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to MovieInfoController post method did not contain either a title or a user account id")]
        public void Test_MovieInfoRequestController_ProcessPost_DataError_NoInfo()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no info in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("movieId", "1", "typeId", "2")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Some data input was not in correct format")]
        public void Test_MovieInfoRequestController_ProcessPost_DataError_UnparsableMovieId()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that movieId is not in int format)
            Request request = new Request() { Method = "POST https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("movieId", "Die Hard", "typeId", "2", "info", "Bald")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate, which will result in the exception being thrown
            myDelegate.Invoke(storage);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Some data input was not in correct format")]
        public void Test_MovieInfoRequestController_ProcessPost_DataError_UnparsableTypeId()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that typeId is not in int format)
            Request request = new Request() { Method = "POST https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("movieId", "1", "typeId", "Hair", "info", "Bald")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate, which will result in the exception being thrown
            myDelegate.Invoke(storage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_MovieInfoRequestController_ProcessPost_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessPost method with null input. This invocation should throw an exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request data must no be null")]
        public void Test_MovieInfoRequestController_ProcessPost_Error_NullData()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Data = null };

            //Invoke the ProcessPost method with null input. This invocation should throw an exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        public void Test_MovieInfoRequestController_ProcessPut()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Initialize an object in which we save our callback
            MovieInfo testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<MovieInfo>(5)).Returns(new MovieInfo() { Id = 5, Movie_Id = 10, Type_Id = 2, Info = "52", Note = "Old" });
            //Setup the mock so it saves the callback object in our test object variable;
            storageMock.Setup(x => x.Update<MovieInfo>(It.IsAny<MovieInfo>())).Callback<MovieInfo>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Put Method
            Request request = new Request() { Method = "PUT https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("id", "5", "movieId", "1", "typeId", "2", "info", "Bald", "note", "Do not tease")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPut(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the update method has only been run once
            storageMock.Verify(x => x.Update<MovieInfo>(It.IsAny<MovieInfo>()), Times.Once);
            
            //Check that the object that was parsed to the Put method was correct relative to our input
            Assert.AreEqual(5, testObject.Id);
            Assert.AreEqual(1, testObject.Movie_Id);
            Assert.AreEqual(2, testObject.Type_Id);
            Assert.AreEqual("Bald", testObject.Info);
            Assert.AreEqual("Do not tease", testObject.Note);
            
            //Get the values contained in the returned bytes
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The MovieInfo was successfully updated", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to MovieInfoController put method did not contain either a title or a user account id")]
        public void Test_MovieInfoRequestController_ProcessPut_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<MovieInfo>(10)).Returns(new MovieInfo() { Id = 10, Movie_Id = 1, Type_Id = 2, Info = "Bald"});
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Update<MovieInfo>(It.IsAny<MovieInfo>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Put Method (Note that there is no id in the data)
            Request request = new Request() { Method = "PUT https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("movieId", "1", "typeId", "2", "info", "Bald", "note", "Do not tease")) };

            //Invoke the method that will throw the exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Some data input was not in correct format")]
        public void Test_MovieInfoRequestController_ProcessPut_DataError_UnparsableId()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<MovieInfo>(10)).Returns(new MovieInfo() { Id = 10, Movie_Id = 1, Type_Id = 2, Info = "Bald" });
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Update<MovieInfo>(It.IsAny<MovieInfo>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;   
                                 
            //Make the request to send to the Put Method (Note that the id cannot be parsed to int)
            Request request = new Request() { Method = "PUT https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("id", "John", "movieId", "1", "typeId", "2", "info", "Bald", "note", "Do not tease", "movieInfoId", "10")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPut(request);

            //Invoke the delegate, which will result in the exception being thrown
            myDelegate.Invoke(storage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_MovieInfoRequestController_ProcessPut_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessPut method with null input. This invocation should throw an exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request data must no be null")]
        public void Test_MovieInfoRequestController_ProcessPut_Error_NullData()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Data = null };

            //Invoke the ProcessPut method with null input. This invocation should throw an exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        public void Test_MovieInfoRequestController_ProcessDelete()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Initialize an object in which we save our callback
            int testObject = -1;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable
            storageMock.Setup(x => x.Delete<MovieInfo>(It.IsAny<int>())).Callback<int>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method
            Request request = new Request() { Method = "DELETE https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("id", "10")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Run the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the delete method has only been run once
            storageMock.Verify(x => x.Delete<MovieInfo>(It.IsAny<int>()), Times.Once);

            //Check that the int that was parsed to the Delete method was correct relative to our input
            Assert.AreEqual(10, testObject);

            //Get the values from the returned json data
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The MovieInfo was successfully deleted", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to favourite controller delete method did not contain an id")]
        public void Test_MovieInfoRequestController_ProcessDelete_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<MovieInfo>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("Name", "John")) };

            //Call the Process method which should throw an exception
            controller.ProcessDelete(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "The id parse to MovieInfoRequestController delete method could not be parsed to int")]
        public void Test_MovieInfoRequestController_ProcessDelete_DataError_UnparsableId()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<MovieInfo>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/MovieInfo", Data = Encoder.Encode(JSonParser.Parse("id", "John")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Call the delegate which should result in an exception
            myDelegate.Invoke(storage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_MovieInfoRequestController_ProcessDelete_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessPut method with null input. This invocation should throw an exception
            controller.ProcessDelete(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request data must no be null")]
        public void Test_MovieInfoRequestController_ProcessDelete_Error_NullData()
        {
            //Initialize the request controller that is being tested
            MovieInfoRequestController controller = new MovieInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Data = null };

            //Invoke the ProcessPut method with null input. This invocation should throw an exception
            controller.ProcessDelete(request);
        }
    }
}
