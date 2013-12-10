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
    public class PersonDataRequestControllerTest
    {
        [TestMethod]
        public void Test_PersonDataRequestControllerTest_ProcessGet_UseAllInfo()
        {
            //Initialize the request controller that is being tested
            PersonDataRequestController controller = new PersonDataRequestController();

            //Make a list of person info for the person to search in
            IList<PersonInfo> personInfoList = new List<PersonInfo>() { 
                new PersonInfo() { Id = 20, Info = "Yipee Ki Yay Motherfucker", Person_Id = 10, Type_Id = 15 }
            };

            //Make a list of participants entities that the person is associated with
            //(Note that we add the lazy loaded person entity. This will normally be found in the database)
            IList<Participate> participateList = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star", Movies = new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 } }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<PersonInfo>()).Returns(personInfoList.AsQueryable());
            storageMock.Setup(x => x.Get<Participate>()).Returns(participateList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonData/10" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));
            
            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(13, values.Count);

            //Assert the person details
            Assert.AreEqual("10", values["id"]);
            Assert.AreEqual("Willis, Bruce", values["name"]);
            Assert.AreEqual("Male", values["gender"]);

            //Assert the person info details
            Assert.AreEqual("20", values["piQuotes0Id"]);
            Assert.AreEqual("Yipee Ki Yay Motherfucker", values["piQuotes0Info"]);

            //Assert the movie details
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
            Assert.AreEqual("Movie", values["m0Kind"]);
            Assert.AreEqual("1998", values["m0Year"]);
            Assert.AreEqual("Officer John McClane", values["m0CharacterName"]);
            Assert.AreEqual("Actor", values["m0Role"]);
            Assert.AreEqual("Star", values["m0Note"]);
            Assert.AreEqual("1", values["m0NrOrder"]);
        }

        [TestMethod]
        public void Test_PersonDataRequestControllerTest_ProcessGet_UseAllInfo_NoPersonInfo()
        {
            //Initialize the request controller that is being tested
            PersonDataRequestController controller = new PersonDataRequestController();

            //Make a list of participants entities that the person is associated with
            //(Note that we add the lazy loaded person entity. This will normally be found in the database)
            IList<Participate> participateList = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star", Movies = new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 } },
                new Participate() { Id = 2, Movie_Id = 6, Person_Id = 10, CharName = "Dr. Malcom Crowe", Role = "Actor", NrOrder = 2, Note = "Star", Movies = new Movies() { Id = 6, Title = "Sixth Sense", Year = 2001, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 } }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Participate>()).Returns(participateList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            storageMock.Setup(x => x.Get<People>(11)).Returns(new People() { Id = 11, Name = "Rickman, Alan", Gender = "Male" });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonData/10" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(19, values.Count);
            
            //Assert the person details
            Assert.AreEqual("10", values["id"]);
            Assert.AreEqual("Willis, Bruce", values["name"]);
            Assert.AreEqual("Male", values["gender"]);

            //Assert the movie details
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
            Assert.AreEqual("Movie", values["m0Kind"]);
            Assert.AreEqual("1998", values["m0Year"]);
            Assert.AreEqual("Officer John McClane", values["m0CharacterName"]);
            Assert.AreEqual("Actor", values["m0Role"]);
            Assert.AreEqual("Star", values["m0Note"]);
            Assert.AreEqual("1", values["m0NrOrder"]);
            Assert.AreEqual("6", values["m1Id"]);
            Assert.AreEqual("Sixth Sense", values["m1Title"]);
            Assert.AreEqual("Movie", values["m1Kind"]);
            Assert.AreEqual("2001", values["m1Year"]);
            Assert.AreEqual("Dr. Malcom Crowe", values["m1CharacterName"]);
            Assert.AreEqual("Actor", values["m1Role"]);
            Assert.AreEqual("Star", values["m1Note"]);
            Assert.AreEqual("2", values["m1NrOrder"]);
        }

        [TestMethod]
        public void Test_PersonDataRequestControllerTest_ProcessGet_UseAllInfo_NoMovies()
        {
            //Initialize the request controller that is being tested
            PersonDataRequestController controller = new PersonDataRequestController();

            //Make a list of person info for the person to search in
            IList<PersonInfo> personInfoList = new List<PersonInfo>() { 
                new PersonInfo() { Id = 20, Info = "Yipee Ki Yay Motherfucker", Person_Id = 10, Type_Id = 15 }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            storageMock.Setup(x => x.Get<PersonInfo>()).Returns(personInfoList.AsQueryable());
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonData/10" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(5, values.Count);

            //Assert the person details
            Assert.AreEqual("10", values["id"]);
            Assert.AreEqual("Willis, Bruce", values["name"]);
            Assert.AreEqual("Male", values["gender"]);

            //Assert the person info details
            Assert.AreEqual("20", values["piQuotes0Id"]);
            Assert.AreEqual("Yipee Ki Yay Motherfucker", values["piQuotes0Info"]);
        }

        [TestMethod]
        public void Test_PersonDataRequestControllerTest_ProcessGet_UseSomeInfo()
        {
            //Initialize the request controller that is being tested
            PersonDataRequestController controller = new PersonDataRequestController();

            //Make a list of person info for the person to search in
            IList<PersonInfo> personInfoList = new List<PersonInfo>() { 
                new PersonInfo() { Id = 20, Info = "Yipee Ki Yay Motherfucker", Person_Id = 10, Type_Id = 15 },
                new PersonInfo() { Id = 22, Info = "Surprise Motherfucker", Person_Id = 15, Type_Id = 15 }
            };

            //Make a list of participants entities that the person is associated with
            //(Note that we add the lazy loaded person entity. This will normally be found in the database)
            IList<Participate> participateList = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star", Movies = new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 } },
                new Participate() { Id = 2, Movie_Id = 6, Person_Id = 10, CharName = "Dr. Malcom Crowe", Role = "Actor", NrOrder = 2, Note = "Star", Movies = new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 }  },
                new Participate() { Id = 3, Movie_Id = 10, Person_Id = 12, CharName = "Aragorn", Role = "Actor", NrOrder = 2, Note = "Star", Movies = new Movies() { Id = 7, Title = "Lord of the Rings", Year = 2005, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 }  }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<PersonInfo>()).Returns(personInfoList.AsQueryable());
            storageMock.Setup(x => x.Get<Participate>()).Returns(participateList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            storageMock.Setup(x => x.Get<People>(12)).Returns(new People() { Id = 12, Name = "Mortensen, Viggo", Gender = "Male" });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonData/10" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(21, values.Count);

            //Assert the person details
            Assert.AreEqual("10", values["id"]);
            Assert.AreEqual("Willis, Bruce", values["name"]);
            Assert.AreEqual("Male", values["gender"]);

            //Assert the person info details
            Assert.AreEqual("20", values["piQuotes0Id"]);
            Assert.AreEqual("Yipee Ki Yay Motherfucker", values["piQuotes0Info"]);

            //Assert the actor details
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
            Assert.AreEqual("Movie", values["m0Kind"]);
            Assert.AreEqual("1998", values["m0Year"]);
            Assert.AreEqual("Officer John McClane", values["m0CharacterName"]);
            Assert.AreEqual("Actor", values["m0Role"]);
            Assert.AreEqual("Star", values["m0Note"]);
            Assert.AreEqual("1", values["m0NrOrder"]);
        }

        [TestMethod]
        public void Test_PersonDataRequestControllerTest_ProcessGet_SkipErronousParticipants()
        {
            //Initialize the request controller that is being tested
            PersonDataRequestController controller = new PersonDataRequestController();

            //Make a list of participants entities that the person is associated with
            //(Note that we add the lazy loaded person entity. This will normally be found in the database)
            IList<Participate> participateList = new List<Participate>() { 
                new Participate() { Id = 1, Movie_Id = 5, Person_Id = 10, CharName = "Officer John McClane", Role = "Actor", NrOrder = 1, Note = "Star", Movies = new Movies() { Id = 5, Title = "Die Hard", Year = 1998, Kind = "Movie", SeasonNumber = 0, EpisodeNumber = 0, SeriesYear = "", EpisodeOf_Id = 0 } },
                new Participate() { Id = 2, Movie_Id = 6, Person_Id = 10, CharName = "Dr. Malcom Crowe", Role = "Actor", NrOrder = 2, Note = "Star", Movies = null}
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<Participate>()).Returns(participateList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonData/10" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            //Assert the amount of information
            Assert.AreEqual(11, values.Count);
            
            //Assert the person details
            Assert.AreEqual("10", values["id"]);
            Assert.AreEqual("Willis, Bruce", values["name"]);
            Assert.AreEqual("Male", values["gender"]);

            //Assert the movie details
            Assert.AreEqual("5", values["m0Id"]);
            Assert.AreEqual("Die Hard", values["m0Title"]);
            Assert.AreEqual("Movie", values["m0Kind"]);
            Assert.AreEqual("1998", values["m0Year"]);
            Assert.AreEqual("Officer John McClane", values["m0CharacterName"]);
            Assert.AreEqual("Actor", values["m0Role"]);
            Assert.AreEqual("Star", values["m0Note"]);
            Assert.AreEqual("1", values["m0NrOrder"]);
        }

        [TestMethod]
        public void Test_PersonDataRequestControllerTest_ProcessGet_InfoSyntax_2SameTypes()
        {
            //Initialize the request controller that is being tested
            PersonDataRequestController controller = new PersonDataRequestController();

            //Make a list of person info for the person to search in
            IList<PersonInfo> personInfoList = new List<PersonInfo>() { 
                new PersonInfo() { Id = 20, Info = "Yipee Ki Yay Motherfucker", Person_Id = 10, Type_Id = 15 },
                new PersonInfo() { Id = 22, Info = "Surprise Motherfucker", Person_Id = 10, Type_Id = 15 }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<PersonInfo>()).Returns(personInfoList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonData/10" };

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
            Assert.AreEqual("Willis, Bruce", values["name"]);
            Assert.AreEqual("Male", values["gender"]);

            //Assert the person info details
            Assert.AreEqual("20", values["piQuotes0Id"]);
            Assert.AreEqual("Yipee Ki Yay Motherfucker", values["piQuotes0Info"]);
            Assert.AreEqual("22", values["piQuotes1Id"]);
            Assert.AreEqual("Surprise Motherfucker", values["piQuotes1Info"]);
        }

        [TestMethod]
        public void Test_PersonDataRequestControllerTest_ProcessGet_InfoSyntax_2DifferentTypes()
        {
            //Initialize the request controller that is being tested
            PersonDataRequestController controller = new PersonDataRequestController();

            //Make a list of person info for the person to search in
            IList<PersonInfo> personInfoList = new List<PersonInfo>() { 
                new PersonInfo() { Id = 20, Info = "Yipee Ki Yay Motherfucker", Person_Id = 10, Type_Id = 15 },
                new PersonInfo() { Id = 22, Info = "Very Bad", Person_Id = 10, Type_Id = 54 }
            };

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get methods
            storageMock.Setup(x => x.Get<PersonInfo>()).Returns(personInfoList.AsQueryable());
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 10, Name = "Willis, Bruce", Gender = "Male" });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonData/10" };

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
            Assert.AreEqual("Willis, Bruce", values["name"]);
            Assert.AreEqual("Male", values["gender"]);

            //Assert the person info details
            Assert.AreEqual("20", values["piQuotes0Id"]);
            Assert.AreEqual("Yipee Ki Yay Motherfucker", values["piQuotes0Info"]);
            Assert.AreEqual("22", values["piPictureFormat0Id"]);
            Assert.AreEqual("Very Bad", values["piPictureFormat0Info"]);
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