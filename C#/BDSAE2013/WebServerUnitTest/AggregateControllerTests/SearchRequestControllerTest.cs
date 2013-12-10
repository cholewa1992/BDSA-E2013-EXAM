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
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Live%20Hard" };

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
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Good%20Hard" };

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
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Good%20To" };

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
        public void Test_SearchRequestController_ProcessGet_Trimming()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }, new Movies() { Id = 10, Title = "A Good Day To Die" }, new Movies() { Id = 6, Title = "Die Another Day" }, new Movies() { Id = 11, Title = "Die Hard : Mega Hard" }, new Movies() { Id = 14, Title = "A Game to Die for" }, new Movies() { Id = 23, Title = "DieDieDie" } };
            IList<People> peopleList = new List<People>() { new People() { Id = 5, Name = "Vin Diesel" }, new People() { Id = 10, Name = "Dien Anderson" }, new People() { Id = 20, Name = "Die Antwoord" }, new People() { Id = 11, Name = "Mierto Dies Luos" }, new People() { Id = 23, Name = "Dies Louise" }, new People() { Id = 30, Name = "Cameron Dies" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<Movies>()).Returns(movieList.AsQueryable());
            storageMock.Setup(x => x.Get<People>()).Returns(peopleList.AsQueryable());
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

            //Test, that even though the search criteria matches all entries, the returned amount of hits has still been trimmed to 10
            Assert.AreEqual(20, values.Count);
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_Ordering()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }, new Movies() { Id = 10, Title = "A Good Day To Die" }, new Movies() { Id = 6, Title = "Die Another Day" }, new Movies() { Id = 11, Title = "Die Hard : Mega Hard" }, new Movies() { Id = 14, Title = "A Game to Die for" }, new Movies() { Id = 23, Title = "DieDieDie" } };
            IList<People> peopleList = new List<People>() { new People() { Id = 5, Name = "Vin Diesel" }, new People() { Id = 10, Name = "Dien Anderson" }, new People() { Id = 20, Name = "Die Antwoord" }, new People() { Id = 11, Name = "Mierto Dies Luos" }, new People() { Id = 23, Name = "Dies Louise" }, new People() { Id = 30, Name = "Cameron Dies" } };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            storageMock.Setup(x => x.Get<Movies>()).Returns(movieList.AsQueryable());
            storageMock.Setup(x => x.Get<People>()).Returns(peopleList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/Search/Die%20Hard%20:%20Mega%20Hard" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Test that the first result in the list will be Die Hard : Mega Hard, the next will be Die Hard and after that it is the movie that matches "Die"
            Assert.AreEqual("Die Hard : Mega Hard", values["m0Title"]);
            Assert.AreEqual("Die Hard", values["m1Title"]);

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

        [TestMethod]
        public void Test_SearchRequestController_GetSearchKeywords()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            //Set up an input string that we wish to compute
            string[] searchInput = new string[]{"Hello", "My", "Friend"};

            //Run it through the computation method
            List<string> searchInputList = controller.GetSearchKeywords(searchInput);

            //Test that we have all the correct results, in the correct order
            Assert.AreEqual(6, searchInputList.Count);
            Assert.AreEqual("Hello My Friend", searchInputList[0]);
            Assert.AreEqual("Hello My", searchInputList[1]);
            Assert.AreEqual("My Friend", searchInputList[2]);
            Assert.AreEqual("Hello", searchInputList[3]);
            Assert.AreEqual("My", searchInputList[4]);
            Assert.AreEqual("Friend", searchInputList[5]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Input must not be null")]
        public void Test_SearchRequestController_GetSearchKeywords_Error_Null()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            //Set up an input string array to null
            string[] searchInput = null;

            //Invoke the method that will throw the exception
            controller.GetSearchKeywords(searchInput);
        }
    }
}
