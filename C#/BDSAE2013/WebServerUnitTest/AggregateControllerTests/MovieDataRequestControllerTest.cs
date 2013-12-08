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
    public class MovieDataRequestControllerTest
    {
        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_UseAllInfo()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a list of movie info for the movie to search in
            IList<MovieInfo> movieInfoList = new List<MovieInfo>() { 
                new MovieInfo() { Id = 20, Info = "1998", Movie_Id = 5, Type_Id = 2 }
            };
            //Make a list of participants in the movie for the movie to search in
            IList<Participate> participateList = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star" }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 } );
            storageMock.Setup(x => x.Get<MovieInfo>()).Returns(movieInfoList.AsQueryable());
            storageMock.Setup(x => x.Get<Participate>()).Returns(participateList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/MovieData/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(16, values.Count);

            //Assert the movie details
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);

            //Assert the actor details
            Assert.AreEqual("10", values["a0Id"]);
            Assert.AreEqual("Willis, Bruce", values["a0Name"]);
            Assert.AreEqual("Officer John McClane", values["a0CharacterName"]);
            Assert.AreEqual("Actor", values["a0Role"]);
            Assert.AreEqual("Star", values["a0Note"]);
            Assert.AreEqual("Male", values["a0Gender"]);
            Assert.AreEqual("1", values["a0NrOrder"]);

            //NEEDS UPDATING WITH MOVIE INFO
        }

        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_UseAllInfo_NoMovieInfo()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a list of participants in the movie for the movie to search in
            IList<Participate> participateList = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star" },
                new Participate() { Id = 2, Movie_Id = 5, Person_Id = 11, CharName = "Hans Gruber", Role = "Actor", NrOrder = 2, Note = "Star" }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 });
            storageMock.Setup(x => x.Get<Participate>()).Returns(participateList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            storageMock.Setup(x => x.Get<People>(11)).Returns(new People() { Id = 11, Name = "Rickman, Alan", Gender = "Male" });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/MovieData/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(22, values.Count);
            
            //Assert the movie details
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);

            //Assert the actor details
            Assert.AreEqual("10", values["a0Id"]);
            Assert.AreEqual("Willis, Bruce", values["a0Name"]);
            Assert.AreEqual("Officer John McClane", values["a0CharacterName"]);
            Assert.AreEqual("Actor", values["a0Role"]);
            Assert.AreEqual("Star", values["a0Note"]);
            Assert.AreEqual("Male", values["a0Gender"]);
            Assert.AreEqual("1", values["a0NrOrder"]);
            Assert.AreEqual("11", values["a1Id"]);
            Assert.AreEqual("Rickman, Alan", values["a1Name"]);
            Assert.AreEqual("Hans Gruber", values["a1CharacterName"]);
            Assert.AreEqual("Actor", values["a1Role"]);
            Assert.AreEqual("Star", values["a1Note"]);
            Assert.AreEqual("Male", values["a1Gender"]);
            Assert.AreEqual("2", values["a1NrOrder"]);
        }

        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_UseAllInfo_NoActors()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a list of movie info for the movie to search in
            IList<MovieInfo> movieInfoList = new List<MovieInfo>() { 
                new MovieInfo() { Id = 20, Info = "1998", Movie_Id = 5, Type_Id = 2 }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 });
            storageMock.Setup(x => x.Get<MovieInfo>()).Returns(movieInfoList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/MovieData/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(9, values.Count);
            
            //Assert the movie details
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);

            //NEEDS UPDATING WITH MOVIE INFO
        }

        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_UseSomeInfo()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a list of movie info for the movie to search in
            IList<MovieInfo> movieInfoList = new List<MovieInfo>() { 
                new MovieInfo() { Id = 20, Info = "1998", Movie_Id = 5, Type_Id = 2 },
                new MovieInfo() { Id = 22, Info = "2003", Movie_Id = 10, Type_Id = 2 }
            };

            //Make a list of participants in the movie for the movie to search in
            IList<Participate> participateList = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star" },
                new Participate() { Id = 2, Movie_Id = 5, Person_Id = 11, CharName = "Hans Gruber", Role = "Actor", NrOrder = 2, Note = "Star" },
                new Participate() { Id = 3, Movie_Id = 10, Person_Id = 12, CharName = "Aragorn", Role = "Actor", NrOrder = 2, Note = "Star" }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 });
            storageMock.Setup(x => x.Get<MovieInfo>()).Returns(movieInfoList.AsQueryable());
            storageMock.Setup(x => x.Get<Participate>()).Returns(participateList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            storageMock.Setup(x => x.Get<People>(11)).Returns(new People() { Id = 11, Name = "Rickman, Alan", Gender = "Male" });
            storageMock.Setup(x => x.Get<People>(12)).Returns(new People() { Id = 12, Name = "Mortensen, Viggo", Gender = "Male" });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/MovieData/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(23, values.Count);

            //Assert the movie details
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);

            //Assert the actor details
            Assert.AreEqual("10", values["a0Id"]);
            Assert.AreEqual("Willis, Bruce", values["a0Name"]);
            Assert.AreEqual("Officer John McClane", values["a0CharacterName"]);
            Assert.AreEqual("Actor", values["a0Role"]);
            Assert.AreEqual("Star", values["a0Note"]);
            Assert.AreEqual("Male", values["a0Gender"]);
            Assert.AreEqual("1", values["a0NrOrder"]);
            Assert.AreEqual("11", values["a1Id"]);
            Assert.AreEqual("Rickman, Alan", values["a1Name"]);
            Assert.AreEqual("Hans Gruber", values["a1CharacterName"]);
            Assert.AreEqual("Actor", values["a1Role"]);
            Assert.AreEqual("Star", values["a1Note"]);
            Assert.AreEqual("Male", values["a1Gender"]);
            Assert.AreEqual("2", values["a1NrOrder"]);

            //NEEDS UPDATING WITH MOVIE INFO
        }

        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_SkipErronousParticipants()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a list of participants in the movie for the movie to search in
            IList<Participate> participateList = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star" },
                new Participate() { Id = 2, Movie_Id = 5, Person_Id = 11, CharName = "Hans Gruber", Role = "Actor", NrOrder = 2, Note = "Star" }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 });
            storageMock.Setup(x => x.Get<Participate>()).Returns(participateList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            storageMock.Setup(x => x.Get<People>(11)).Throws(new InvalidOperationException("No person with that id exists")); //Simulate the exception being thrown when no entity with the specified id is found
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/MovieData/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct (and that the person from participation entity with id 2 was not included in the data)
            //Assert the amount of information
            Assert.AreEqual(15, values.Count);

            //Assert the movie details
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);

            //Assert the actor details
            Assert.AreEqual("10", values["a0Id"]);
            Assert.AreEqual("Willis, Bruce", values["a0Name"]);
            Assert.AreEqual("Officer John McClane", values["a0CharacterName"]);
            Assert.AreEqual("Actor", values["a0Role"]);
            Assert.AreEqual("Star", values["a0Note"]);
            Assert.AreEqual("Male", values["a0Gender"]);
            Assert.AreEqual("1", values["a0NrOrder"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUrlParameterException),
        "Url ending did not contain an argument")]
        public void Test_MovieDataRequestController_ProcessGet_Error_InvalidUrl()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/MovieData/" };
            
            //Make the invocation that will throw the exception
            controller.ProcessGet(request);
        }
    }
}