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
using System.Linq;

namespace WebServerUnitTest
{
    [TestClass]
    public class SearchRequestControllerTest
    {
        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_Movies_1_Hit()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }};

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<Movies>()).Returns( movieList.AsQueryable() );
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Die" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_Movies_MoreThan1_Hit()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }, new Movies() { Id = 10, Title = "A Good Day To Die" }, new Movies() { Id = 6, Title = "Die Another Day" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<Movies>()).Returns(movieList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Die" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual(6, values.Count);
            Assert.AreEqual(true, values.ContainsValue("Die Hard"));            //name of Die Hard
            Assert.AreEqual(true, values.ContainsValue("5"));                   //id of Die Hard
            Assert.AreEqual(true, values.ContainsValue("A Good Day To Die"));   //name of A Good Day To Die
            Assert.AreEqual(true, values.ContainsValue("10"));                  //id of A Good Day To Die
            Assert.AreEqual(true, values.ContainsValue("Die Another Day"));     //name of Die Another Day
            Assert.AreEqual(true, values.ContainsValue("6"));                   //id of Die Another Day

        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_Movies_No_Hit()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }, new Movies() { Id = 10, Title = "A Good Day To Die" }, new Movies() { Id = 6, Title = "Die Another Day" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<Movies>()).Returns(movieList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Want" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual("There was no search hits", values["response"]);

        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_Movies_MoreThan1_Input_1_Hit()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }, new Movies() { Id = 10, Title = "A Good Day To Die" }, new Movies() { Id = 6, Title = "Die Another Day" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<Movies>()).Returns(movieList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Live_Hard" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(true, values.ContainsValue("Die Hard"));            //name of Die Hard
            Assert.AreEqual(true, values.ContainsValue("5"));                   //id of Die Hard
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_Movies_MoreThan1_Input_MoreThan1_Hit()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }, new Movies() { Id = 10, Title = "A Good Day To Die" }, new Movies() { Id = 6, Title = "Die Another Day" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<Movies>()).Returns(movieList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Good_Hard" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual(4, values.Count);
            Assert.AreEqual(true, values.ContainsValue("Die Hard"));            //name of Die Hard
            Assert.AreEqual(true, values.ContainsValue("5"));                   //id of Die Hard
            Assert.AreEqual(true, values.ContainsValue("A Good Day To Die"));   //name of A Good Day To Die
            Assert.AreEqual(true, values.ContainsValue("10"));                  //id of A Good Day To Die

        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_Movies_MoreThan1_Input_Same_Hit_Twice()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }, new Movies() { Id = 10, Title = "A Good Day To Die" }, new Movies() { Id = 6, Title = "Die Another Day" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<Movies>()).Returns(movieList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Good_To" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct, and that there are no duplicates
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(true, values.ContainsValue("A Good Day To Die"));   //name of A Good Day To Die
            Assert.AreEqual(true, values.ContainsValue("10"));                  //id of A Good Day To Die
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_People()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<People> peopleList = new List<People>() { new People() { Id = 5, Name = "Harrison Ford" }, new People() { Id = 5, Name = "Colin Ford" }, new People() { Id = 5, Name = "John Goodman" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<People>()).Returns(peopleList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Harrison" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct, and that there are no duplicates
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual(true, values.ContainsValue("Harrison Ford"));   //name of Harrison Ford
            Assert.AreEqual(true, values.ContainsValue("5"));               //id of Harrison Ford
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_People_2_Hits()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<People> peopleList = new List<People>() { new People() { Id = 5, Name = "Harrison Ford" }, new People() { Id = 10, Name = "Colin Ford" }, new People() { Id = 20, Name = "John Goodman" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<People>()).Returns(peopleList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Ford" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct, and that there are no duplicates
            Assert.AreEqual(4, values.Count);
            Assert.AreEqual(true, values.ContainsValue("Harrison Ford"));   //name of Harrison Ford
            Assert.AreEqual(true, values.ContainsValue("5"));               //id of Harrison Ford
            Assert.AreEqual(true, values.ContainsValue("Colin Ford"));      //name of Colin Ford
            Assert.AreEqual(true, values.ContainsValue("10"));               //id of Colin Ford
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_MoviesAndPeople()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }, new Movies() { Id = 10, Title = "A Good Day To Die" }, new Movies() { Id = 6, Title = "Die Another Day" } };
            IList<People> peopleList = new List<People>() { new People() { Id = 5, Name = "Harrison Ford" }, new People() { Id = 10, Name = "Colin Ford" }, new People() { Id = 20, Name = "John Goodman" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<Movies>()).Returns(movieList.AsQueryable());
            storageMock.Setup(x => x.Get<People>()).Returns(peopleList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Good" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct, and that there are no duplicates
            Assert.AreEqual(4, values.Count);
            Assert.AreEqual(true, values.ContainsValue("John Goodman"));             //name of John Goodman
            Assert.AreEqual(true, values.ContainsValue("20"));                      //id of John Goodman
            Assert.AreEqual(true, values.ContainsValue("A Good Day To Die"));       //name of A Good Day To Die
            Assert.AreEqual(true, values.ContainsValue("10"));                       //id of A Good Day To Die
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUrlParameterException),
        "Url ending did not contain an argument")]
        public void Test_SearchRequestController_ProcessGet_Error_InvalidUrl()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate. This invocation should throw the exception
            byte[] data = myDelegate.Invoke(storage);
        }
    }
}
