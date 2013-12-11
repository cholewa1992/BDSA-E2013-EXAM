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
    public class MovieRequestControllerTest
    {
        [TestMethod]
        public void Test_MovieRequestController_ProcessGet_JSon_Attributes()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method when given the id 5
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 1, EpisodeNumber = 2, SeriesYear = "2000", EpisodeOf_Id = 3});
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Movie/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("1", values["seasonNumber"]);
            Assert.AreEqual("2", values["episodeNumber"]);
            Assert.AreEqual("2000", values["seriesYear"]);
            Assert.AreEqual("3", values["episodeOfId"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Incoming argument must be formatted as an int")]
        public void Test_MovieRequestController_ProcessGet_Error_NonIntArgument()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Movie/Die_Hard" };

            //Invoke the ProcessRequest method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_MovieRequestController_ProcessGet_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessGet method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request method must no be null")]
        public void Test_MovieRequestController_ProcessGet_Error_NullMethod()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = null };

            //Invoke the ProcessGet method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        public void Test_MovieRequestController_ProcessPost_MinimumInfo()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Initialize an object in which we save our callback
            Movies testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<Movies>(It.IsAny<Movies>())).Callback<Movies>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("title", "Die Hard 2", "year", "2000", "kind", "Movie")) };
            
            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<Movies>(It.IsAny<Movies>()), Times.Once);
            
            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual("Die Hard 2", testObject.Title);
            Assert.AreEqual(2000, testObject.Year);
            Assert.AreEqual("Movie", testObject.Kind);
            Assert.AreEqual(null, testObject.SeasonNumber);
            Assert.AreEqual(null, testObject.EpisodeNumber);
            Assert.AreEqual("", testObject.SeriesYear);
            Assert.AreEqual(null, testObject.EpisodeOf_Id);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Movie was successfully added", values["response"]);
        }

        [TestMethod]
        public void Test_MovieRequestController_ProcessPost_MaximumInfo()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Initialize an object in which we save our callback
            Movies testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<Movies>(It.IsAny<Movies>())).Callback<Movies>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("title", "Die Hard 2", "year", "2000", "kind", "TV Series", "seasonNumber", "1", "episodeNumber", "1", "seriesYear", "1998", "episodeOfId", "1")) };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<Movies>(It.IsAny<Movies>()), Times.Once);

            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual("Die Hard 2", testObject.Title);
            Assert.AreEqual(2000, testObject.Year);
            Assert.AreEqual("TV Series", testObject.Kind);
            Assert.AreEqual(1, testObject.SeasonNumber);
            Assert.AreEqual(1, testObject.EpisodeNumber);
            Assert.AreEqual("1998", testObject.SeriesYear);
            Assert.AreEqual(1, testObject.EpisodeOf_Id);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Movie was successfully added", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to MovieController post method did not contain either a title or a user account id")]
        public void Test_MovieRequestController_ProcessPost_DataError_NoTitle()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no Movie Id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("year", "2000")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to MovieController post method did not contain either a title or a user account id")]
        public void Test_MovieRequestController_ProcessPost_DataError_NoTypeId()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no type id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("title", "Die Hard 2")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_MovieRequestController_ProcessPost_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessPost method with null input. This invocation should throw an exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request Data must no be null")]
        public void Test_MovieRequestController_ProcessPost_Error_NullData()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Data = null };

            //Invoke the ProcessPost method with null input. This invocation should throw an exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        public void Test_MovieRequestController_ProcessPut()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Initialize an object in which we save our callback
            Movies testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard 2", Year = 2000, Kind = "TV Series", SeasonNumber = 1, EpisodeNumber = 1, SeriesYear = "1998", EpisodeOf_Id = 2});
            //Setup the mock so it saves the callback object in our test object variable;
            storageMock.Setup(x => x.Update<Movies>(It.IsAny<Movies>())).Callback<Movies>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Put Method
            Request request = new Request() { Method = "PUT https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("id", "5", "title", "Die Hard 3", "kind", "Movie", "seasonNumber", "0", "episodeNumber", "0", "seriesYear", "", "episodeOfId", "0")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPut(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the update method has only been run once
            storageMock.Verify(x => x.Update<Movies>(It.IsAny<Movies>()), Times.Once);
            
            //Check that the object that was parsed to the Put method was correct relative to our input
            Assert.AreEqual(5, testObject.Id);
            Assert.AreEqual("Die Hard 3", testObject.Title);
            Assert.AreEqual("Movie", testObject.Kind);
            Assert.AreEqual(0, testObject.SeasonNumber);
            Assert.AreEqual(0, testObject.EpisodeNumber);
            Assert.AreEqual("", testObject.SeriesYear);
            Assert.AreEqual(0, testObject.EpisodeOf_Id);

            //Get the values contained in the returned bytes
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Movie was successfully updated", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to MovieController put method did not contain either a title or a user account id")]
        public void Test_MovieRequestController_ProcessPut_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<Movies>(10)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998 });
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Update<Movies>(It.IsAny<Movies>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Put Method (Note that there is no id in the data)
            Request request = new Request() { Method = "PUT https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("title", "Die Hard 3", "year", "2001")) };

            //Invoke the method that will throw the exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_MovieRequestController_ProcessPut_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessPut method with null input. This invocation should throw an exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request Data must no be null")]
        public void Test_MovieRequestController_ProcessPut_Error_NullData()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Data = null };

            //Invoke the ProcessPut method with null input. This invocation should throw an exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        public void Test_MovieRequestController_ProcessDelete()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Initialize an object in which we save our callback
            int testObject = -1;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable
            storageMock.Setup(x => x.Delete<Movies>(It.IsAny<int>())).Callback<int>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method
            Request request = new Request() { Method = "DELETE https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("id", "10")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Run the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the delete method has only been run once
            storageMock.Verify(x => x.Delete<Movies>(It.IsAny<int>()), Times.Once);

            //Check that the int that was parsed to the Delete method was correct relative to our input
            Assert.AreEqual(10, testObject);

            //Get the values from the returned json data
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Movie was successfully deleted", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to favourite controller delete method did not contain an id")]
        public void Test_MovieRequestController_ProcessDelete_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<Movies>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("Name", "John")) };

            //Call the Process method which should throw an exception
            controller.ProcessDelete(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "The id parse to MovieRequestController delete method could not be parsed to int")]
        public void Test_MovieRequestController_ProcessDelete_DataError_UnparsableId()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<Movies>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("id", "John")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Call the delegate which should result in an exception
            myDelegate.Invoke(storage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_MovieRequestController_ProcessDelete_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessDelete method with null input. This invocation should throw an exception
            controller.ProcessDelete(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request Data must no be null")]
        public void Test_MovieRequestController_ProcessDelete_Error_NullData()
        {
            //Initialize the request controller that is being tested
            MovieRequestController controller = new MovieRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Data = null };

            //Invoke the ProcessDelete method with null input. This invocation should throw an exception
            controller.ProcessDelete(request);
        }
    }
}
