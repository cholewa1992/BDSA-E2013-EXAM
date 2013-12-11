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
    public class MovieDataRequestControllerTest
    {
        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_UseAllInfo()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();


            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0,
            MovieInfo = new List<MovieInfo>() { 
                new MovieInfo() { Id = 20, Info = "Color", Movie_Id = 5, Type_Id = 2 }
            },
            Participate = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star", People = new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male"}}
            }
            });
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
            Assert.AreEqual(17, values.Count);

            //Assert the movie details
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);

            //Assert the movie info details
            Assert.AreEqual("20", values["miColorInfo0Id"]);
            Assert.AreEqual("Color", values["miColorInfo0Info"]);

            //Assert the actor details
            Assert.AreEqual("10", values["p0Id"]);
            Assert.AreEqual("Willis, Bruce", values["p0Name"]);
            Assert.AreEqual("Officer John McClane", values["p0CharacterName"]);
            Assert.AreEqual("Actor", values["p0Role"]);
            Assert.AreEqual("Star", values["p0Note"]);
            Assert.AreEqual("Male", values["p0Gender"]);
            Assert.AreEqual("1", values["p0NrOrder"]);
        }

        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_UseAllInfo_NoMovieInfo()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0,
            Participate = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star", People = new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" } },
                new Participate() { Id = 2, Movie_Id = 5, Person_Id = 11, CharName = "Hans Gruber", Role = "Actor", NrOrder = 2, Note = "Star", People = new People() { Id = 11, Name = "Rickman, Alan", Gender = "Male" } }
            }
            });
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
            Assert.AreEqual("10", values["p0Id"]);
            Assert.AreEqual("Willis, Bruce", values["p0Name"]);
            Assert.AreEqual("Officer John McClane", values["p0CharacterName"]);
            Assert.AreEqual("Actor", values["p0Role"]);
            Assert.AreEqual("Star", values["p0Note"]);
            Assert.AreEqual("Male", values["p0Gender"]);
            Assert.AreEqual("1", values["p0NrOrder"]);
            Assert.AreEqual("11", values["p1Id"]);
            Assert.AreEqual("Rickman, Alan", values["p1Name"]);
            Assert.AreEqual("Hans Gruber", values["p1CharacterName"]);
            Assert.AreEqual("Actor", values["p1Role"]);
            Assert.AreEqual("Star", values["p1Note"]);
            Assert.AreEqual("Male", values["p1Gender"]);
            Assert.AreEqual("2", values["p1NrOrder"]);
        }

        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_UseAllInfo_NoActors()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0,
            MovieInfo = new List<MovieInfo>() { 
                new MovieInfo() { Id = 20, Info = "Color", Movie_Id = 5, Type_Id = 2 }
            }
            });
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
            Assert.AreEqual(10, values.Count);
            
            //Assert the movie details
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);
            
            //Assert the movie info details
            Assert.AreEqual("20", values["miColorInfo0Id"]);
            Assert.AreEqual("Color", values["miColorInfo0Info"]);
        }
        

        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_SkipErronousParticipants()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0,
            Participate = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star", People = new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" } },
                new Participate() { Id = 2, Movie_Id = 5, Person_Id = 11, CharName = "Hans Gruber", Role = "Actor", NrOrder = 2, Note = "Star", People = null},
            }
            });
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
            Assert.AreEqual("10", values["p0Id"]);
            Assert.AreEqual("Willis, Bruce", values["p0Name"]);
            Assert.AreEqual("Officer John McClane", values["p0CharacterName"]);
            Assert.AreEqual("Actor", values["p0Role"]);
            Assert.AreEqual("Star", values["p0Note"]);
            Assert.AreEqual("Male", values["p0Gender"]);
            Assert.AreEqual("1", values["p0NrOrder"]);
        }

        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_InfoSyntax_2SameTypes()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0,
            MovieInfo = new List<MovieInfo>() { 
                new MovieInfo() { Id = 20, Info = "Color", Movie_Id = 5, Type_Id = 2 },
                new MovieInfo() { Id = 22, Info = "Black And White", Movie_Id = 5, Type_Id = 2 }
            }
            });
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
            Assert.AreEqual(12, values.Count);

            //Assert the movie details
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);

            //Assert the movie info details
            Assert.AreEqual("20", values["miColorInfo0Id"]);
            Assert.AreEqual("Color", values["miColorInfo0Info"]);
            Assert.AreEqual("22", values["miColorInfo1Id"]);
            Assert.AreEqual("Black And White", values["miColorInfo1Info"]);
        }

        [TestMethod]
        public void Test_MovieDataRequestControllerTest_ProcessGet_InfoSyntax_2DifferentTypes()
        {
            //Initialize the request controller that is being tested
            MovieDataRequestController controller = new MovieDataRequestController();

            
            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 ,
            MovieInfo = new List<MovieInfo>() { 
                new MovieInfo() { Id = 20, Info = "Color", Movie_Id = 5, Type_Id = 2 },
                new MovieInfo() { Id = 22, Info = "English", Movie_Id = 5, Type_Id = 4 }
            }
            });
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
            Assert.AreEqual(12, values.Count);

            //Assert the movie details
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);

            //Assert the movie info details
            Assert.AreEqual("20", values["miColorInfo0Id"]);
            Assert.AreEqual("Color", values["miColorInfo0Info"]);
            Assert.AreEqual("22", values["miLanguage0Id"]);
            Assert.AreEqual("English", values["miLanguage0Info"]);
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