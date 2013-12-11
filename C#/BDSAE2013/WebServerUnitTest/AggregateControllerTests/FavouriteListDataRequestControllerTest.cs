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
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
    [TestClass]
    public class FavouriteListDataControllerTest
    {
        [TestMethod]
        public void Test_FavouriteListDataRequestControllerTest_ProcessGet_UseAllInfo()
        {
            //Initialize the request controller that is being tested
            FavouriteListDataRequestController controller = new FavouriteListDataRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<FavouriteList>(10)).Returns(new FavouriteList() { Id = 10, Title = "Best Movies of 2013", UserAcc_Id = 20,
            FavouritedMovie = new List<FavouritedMovie>() { 
                new FavouritedMovie() { Id = 1, FavList_Id = 10, Movie_Id = 5, Movies = new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 }  }
            }
            });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/FavouriteListData/10" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));
            
            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(7, values.Count);

            //Assert the person details
            Assert.AreEqual("10", values["id"]);
            Assert.AreEqual("Best Movies of 2013", values["title"]);
            Assert.AreEqual("20", values["userAccountId"]);

            //Assert the movie details
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
            Assert.AreEqual("Movie", values["m0Kind"]);
            Assert.AreEqual("1998", values["m0Year"]);
        }

        [TestMethod]
        public void Test_FavouriteListDataRequestControllerTest_ProcessGet_SkipErronousParticipants()
        {
            //Initialize the request controller that is being tested
            FavouriteListDataRequestController controller = new FavouriteListDataRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<FavouriteList>(10)).Returns(
                new FavouriteList()
                {
                    Id = 10,
                    Title = "Best Movies of 2013",
                    UserAcc_Id = 20,
                    FavouritedMovie = new List<FavouritedMovie>{
                new FavouritedMovie() { Id = 1, FavList_Id = 10, Movie_Id = 5, Movies = new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 } },
                new FavouritedMovie() { Id = 2, FavList_Id = 10, Movie_Id = 10, Movies = null  }}
                });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/FavouriteListData/10" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));
            
            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(7, values.Count);
            
            //Assert the person details
            Assert.AreEqual("10", values["id"]);
            Assert.AreEqual("Best Movies of 2013", values["title"]);
            Assert.AreEqual("20", values["userAccountId"]);

            //Assert the movie details
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
            Assert.AreEqual("Movie", values["m0Kind"]);
            Assert.AreEqual("1998", values["m0Year"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUrlParameterException),
        "Url ending did not contain an argument")]
        public void Test_PersonDataRequestController_ProcessGet_Error_InvalidUrl()
        {
            //Initialize the request controller that is being tested
            PersonDataRequestController controller = new PersonDataRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonData/" };
            
            //Make the invocation that will throw the exception
            controller.ProcessGet(request);
        }
    }
}