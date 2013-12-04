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
    [TestClass]
    public class FavouriteRequestControllerTest
    {
        [TestMethod]
        public void Test_FavouriteRequestController_ProcessGet_JSon_Attributes()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ProcessRequest method from the abstract class
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage. Since this test is not concerned with the output of the database, we do not want an output from it.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();        //create a mock of a storage
            storageMock.Setup(x => x.Get<FavouriteList>(5)).Returns(new FavouriteList() { Id = 5, Title = "Best Movies", UserAccId = 10 });

            var storage = storageMock.Object;                                    //Make an intance of the storage class using the mock

            Request request = new Request() { Method = "GET https://www.google.dk/FavouriteList/5" };

            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            byte[] data = myDelegate.Invoke(storage);

            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Best Movies", values["title"]);
            Assert.AreEqual("10", values["userAccId"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Incoming argument must be formatted as an int")]
        public void Test_FavouriteRequestController_ProcessGet_Error_NonIntArgument()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ProcessRequest method from the abstract class
            FavouriteRequestController controller = new FavouriteRequestController();

            Request request = new Request() { Method = "GET https://www.google.dk/FavouriteList/Die_Hard" };

            //Invoke the ProcessRequest method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        public void Test_FavouriteRequestController_ProcessPost()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ProcessRequest method from the abstract class
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage. Since this test is not concerned with the output of the database, we do not want an output from it.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();        //create a mock of a storage
            storageMock.Setup(x => x.Add<FavouriteList>(It.IsAny<FavouriteList>()));
            var storage = storageMock.Object;                                    //Make an intance of the storage class using the mock

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("title", "MyMovies", "userAccId", "5")) };

            
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            byte[] data = myDelegate.Invoke(storage);

            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the add method has only been run once
            storageMock.Verify(x => x.Add<FavouriteList>(It.IsAny<FavouriteList>()), Times.Once);

            //Assert that the returned response is correct
            Assert.AreEqual("The Favourite List was successfully added", values["response"]);
        }

        /*
        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Incoming argument must be formatted as an int")]
        public void Test_FavouriteRequestController_ProcessGet_Error_NonIntArgument()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ProcessRequest method from the abstract class
            FavouriteRequestController controller = new FavouriteRequestController();

            Request request = new Request() { Method = "GET https://www.google.dk/FavouriteList/Die_Hard" };

            //Invoke the ProcessRequest method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }
         * */
    }
}
