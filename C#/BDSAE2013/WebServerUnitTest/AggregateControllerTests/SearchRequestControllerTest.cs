﻿using System;
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
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
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
            Assert.AreEqual(true, values.ContainsValue("John Goodman"));             //name of John Goodman
            Assert.AreEqual(true, values.ContainsValue("20"));                      //id of John Goodman
            Assert.AreEqual(true, values.ContainsValue("A Good Day To Die"));       //name of A Good Day To Die
            Assert.AreEqual(true, values.ContainsValue("10"));                       //id of A Good Day To Die
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_LowerCaseMatching()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { new Movies() { Id = 5, Title = "Die Hard" }, new Movies() { Id = 10, Title = "The diel is right" } };
            IList<People> peopleList = new List<People>() { new People() { Id = 6, Name = "Vin Diesel" }, new People() { Id = 12, Name = "Nicole Kidieman" } };

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

            //Check that the values returned by the delegate are correct, and that there are no duplicates
            Assert.AreEqual(true, values.ContainsValue("Die Hard"));                 //name of Die Hard
            Assert.AreEqual(true, values.ContainsValue("5"));                       //id of Die Hard
            Assert.AreEqual(true, values.ContainsValue("The diel is right"));       //name of The diel is right
            Assert.AreEqual(true, values.ContainsValue("10"));                       //id of The diel is right
            Assert.AreEqual(true, values.ContainsValue("Vin Diesel"));              //name of Vin Diesel
            Assert.AreEqual(true, values.ContainsValue("6"));                       //id of Vin Diesel
            Assert.AreEqual(true, values.ContainsValue("Nicole Kidieman"));       //name of Nicole Kidieman
            Assert.AreEqual(true, values.ContainsValue("12"));                       //id of Nicole Kidieman
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_PlotSearch_0_Plots()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { 
                new Movies() { Id = 5, Title = "Die Hard"}};
            IList<People> peopleList = new List<People>();

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

            //Test that when there are no plots available we return an empty string
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
            Assert.AreEqual("", values["m0Plot"]);
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_PlotSearch_1_Plot()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { 
                new Movies() { Id = 5, Title = "Die Hard", MovieInfo = new List<MovieInfo>(){new MovieInfo(){ Info = "This movie is awesome", Type_Id = 98 }}}};
            IList<People> peopleList = new List<People>();

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

            //Test that we only get the first plot
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
            Assert.AreEqual("This movie is awesome", values["m0Plot"]);
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_PlotSearch_MoreThan1_Plot()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { 
                new Movies() { Id = 5, Title = "Die Hard", MovieInfo = new List<MovieInfo>(){new MovieInfo(){ Info = "This movie is awesome", Type_Id = 98 }, new MovieInfo(){ Info = "Bruce Willis is the man", Type_Id = 98 }}}};
            IList<People> peopleList = new List<People>();

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

            //Test that we only get the first plot
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
            Assert.AreEqual("This movie is awesome", values["m0Plot"]);
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_BiographySearch_0_Biographies()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() {};
            IList<People> peopleList = new List<People>() { new People() { Id = 6, Name = "Vin Diesel" } };

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

            //Test that when there are no biographies available we return an empty string
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual("6", values["p0Id"]);
            Assert.AreEqual("Vin Diesel", values["p0Name"]);
            Assert.AreEqual("", values["p0Biography"]);
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_BiographySearch_1_Biography()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { };
            IList<People> peopleList = new List<People>() { new People() { Id = 6, Name = "Vin Diesel", PersonInfo = new List<PersonInfo>() { new PersonInfo() { Info = "He is bald", Type_Id = 19 } } } };

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


            //Test that we only get the first biography
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual("6", values["p0Id"]);
            Assert.AreEqual("Vin Diesel", values["p0Name"]);
            Assert.AreEqual("He is bald", values["p0Biography"]);
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_BiographySearch_MoreThan1_Biography()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { };
            IList<People> peopleList = new List<People>() { new People() { Id = 6, Name = "Vin Diesel", PersonInfo = new List<PersonInfo>() { new PersonInfo() { Info = "He is bald", Type_Id = 19 }, new PersonInfo(){ Info = "He is strong", Type_Id = 19} } } };

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

            //Test that we only get the first biography
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual("6", values["p0Id"]);
            Assert.AreEqual("Vin Diesel", values["p0Name"]);
            Assert.AreEqual("He is bald", values["p0Biography"]);
        }

        [TestMethod]
        public void Test_SearchRequestController_ProcessGet_Trimming()
        {
            //Initialize the request controller that is being tested
            SearchRequestController controller = new SearchRequestController();

            IList<Movies> movieList = new List<Movies>() { 
                new Movies() { Id = 5, Title = "Die Hard" }, 
                new Movies() { Id = 10, Title = "A Good Day To Die" }, 
                new Movies() { Id = 6, Title = "Die Another Day" }, 
                new Movies() { Id = 11, Title = "Die Hard : Mega Hard" }, 
                new Movies() { Id = 14, Title = "A Game to Die for" }, 
                new Movies() { Id = 23, Title = "DieDieDie" }, 
                new Movies() { Id = 24, Title = "I Want to die" }, 
                new Movies() { Id = 25, Title = "A test to die for" }, 
                new Movies() { Id = 26, Title = "Live or Die" }, 
                new Movies() { Id = 27, Title = "I am die" }, 
                new Movies() { Id = 28, Title = "Die 2: Return of die" }, 
                new Movies() { Id = 29, Title = "Die 3: Die reloaded" } };
            IList<People> peopleList = new List<People>() { 
                new People() { Id = 5, Name = "Vin Diesel" }, 
                new People() { Id = 10, Name = "Dien Anderson" }, 
                new People() { Id = 20, Name = "Die Antwoord" }, 
                new People() { Id = 11, Name = "Mierto Dies Luos" }, 
                new People() { Id = 23, Name = "Dies Louise" }, 
                new People() { Id = 30, Name = "Luiz Guzdies" }, 
                new People() { Id = 31, Name = "Celine Dieon" }, 
                new People() { Id = 32, Name = "Matt Dieson" }, 
                new People() { Id = 33, Name = "Sindo Dies" }, 
                new People() { Id = 34, Name = "Die Hard" }, 
                new People() { Id = 35, Name = "John Dies" }, 
                new People() { Id = 36, Name = "Samuel Dieson" }, 
                new People() { Id = 37, Name = "Leonardo DieCaprio" }, 
                new People() { Id = 38, Name = "Nicolie Kidieman" } };

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

            //Test, that even though the search criteria matches all entries, the returned amount of hits has still been trimmed to 40
            Assert.AreEqual(60, values.Count);
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
    }
}
