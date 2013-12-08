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
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star" },
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 11, CharName = "Hans Gruber", Role = "Actor", NrOrder = 2, Note = "Star" }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Movies>(5)).Returns(new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 } );
            storageMock.Setup(x => x.Get<MovieInfo>()).Returns(movieInfoList.AsQueryable());
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
            //Assert the movie details
            Assert.AreEqual("Die Hard", values["title"]);
            Assert.AreEqual("1998", values["year"]);
            Assert.AreEqual("Movie", values["kind"]);
            Assert.AreEqual("0", values["seasonNumber"]);
            Assert.AreEqual("0", values["episodeNumber"]);
            Assert.AreEqual("", values["seriesYear"]);
            Assert.AreEqual("0", values["episodeOfId"]);

            //Assert the actor details
            Assert.AreEqual("Willis, Bruce", values["a0Name"]);
            Assert.AreEqual("Officer John McClane", values["a0CharacterName"]);
            Assert.AreEqual("Actor", values["a0Role"]);
            Assert.AreEqual("Star", values["a0Note"]);
            Assert.AreEqual("Rickman, Alan", values["a1Name"]);
            Assert.AreEqual("Hans Gruber", values["a1CharacterName"]);
            Assert.AreEqual("Actor", values["a1Role"]);
            Assert.AreEqual("Star", values["a1Note"]);

            //NEEDS UPDATING WITH MOVIE INFO
        }
    }
}
