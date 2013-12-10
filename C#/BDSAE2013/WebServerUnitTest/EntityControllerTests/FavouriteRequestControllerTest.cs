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
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method when given the id 5
            storageMock.Setup(x => x.Get<FavouriteList>(5)).Returns(new FavouriteList() { Id = 5, Title = "Best Movies", UserAcc_Id = 10 });
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/FavouriteList/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Best Movies", values["title"]);
            Assert.AreEqual("10", values["userAccId"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Incoming argument must be formatted as an int")]
        public void Test_FavouriteRequestController_ProcessGet_Error_NonIntArgument()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/FavouriteList/Die_Hard" };

            //Invoke the ProcessRequest method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        public void Test_FavouriteRequestController_ProcessPost()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Initialize an object in which we save our callback
            FavouriteList testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<FavouriteList>(It.IsAny<FavouriteList>())).Callback<FavouriteList>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("title", "MyMovies", "userAccId", "5")) };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<FavouriteList>(It.IsAny<FavouriteList>()), Times.Once);
            
            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual("MyMovies", testObject.Title);
            Assert.AreEqual(5, testObject.UserAcc_Id);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Favourite List was successfully added", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to favourite controller post method did not contain either a title or a user account id")]
        public void Test_FavouriteRequestController_ProcessPost_DataError_NoUserAccId()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no userAccId in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("title", "MyMovies")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to favourite controller post method did not contain either a title or a user account id")]
        public void Test_FavouriteRequestController_ProcessPost_DataError_NoTitle()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no title in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("userAccId", "5")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Some data input was not in correct format")]
        public void Test_FavouriteRequestController_ProcessPost_DataError_UnparsableUserAccId()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that userAccId is not in int format)
            Request request = new Request() { Method = "POST https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("title", "MyMovies", "userAccId", "John")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate, which will result in the exception being thrown
            myDelegate.Invoke(storage);
        }

        [TestMethod]
        public void Test_FavouriteRequestController_ProcessPut()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Initialize an object in which we save our callback
            FavouriteList testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<FavouriteList>(10)).Returns(new FavouriteList { Id = 10, Title = "OtherMovies", UserAcc_Id = 2 });
            //Setup the mock so it saves the callback object in our test object variable;
            storageMock.Setup(x => x.Update<FavouriteList>(It.IsAny<FavouriteList>())).Callback<FavouriteList>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Put Method
            Request request = new Request() { Method = "PUT https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("id", "10", "title", "MyMovies", "userAccId", "5")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPut(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the update method has only been run once
            storageMock.Verify(x => x.Update<FavouriteList>(It.IsAny<FavouriteList>()), Times.Once);
            
            //Check that the object that was parsed to the Put method was correct relative to our input
            Assert.AreEqual(10, testObject.Id);
            Assert.AreEqual("MyMovies", testObject.Title);
            Assert.AreEqual(5, testObject.UserAcc_Id);
            
            //Get the values contained in the returned bytes
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Favourite List was successfully updated", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to favourite controller put method did not contain either a title or a user account id")]
        public void Test_FavouriteRequestController_ProcessPut_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<FavouriteList>(10)).Returns(new FavouriteList() { Id = 10, Title = "OtherMovies", UserAcc_Id = 2 });
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Update<FavouriteList>(It.IsAny<FavouriteList>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Put Method (Note that there is no id in the data)
            Request request = new Request() { Method = "PUT https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("title", "MyMovies", "userAccId", "5")) };

            //Invoke the method that will throw the exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Some data input was not in correct format")]
        public void Test_FavouriteRequestController_ProcessPut_DataError_UnparsableId()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<FavouriteList>(10)).Returns(new FavouriteList() { Id = 10, Title = "OtherMovies", UserAcc_Id = 2 });
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Update<FavouriteList>(It.IsAny<FavouriteList>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;   
                                 
            //Make the request to send to the Put Method (Note that the id cannot be parsed to int)
            Request request = new Request() { Method = "PUT https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("id", "John", "title", "MyMovies", "userAccId", "Hans")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPut(request);

            //Invoke the delegate, which will result in the exception being thrown
            myDelegate.Invoke(storage);
        }

        [TestMethod]
        public void Test_FavouriteRequestController_ProcessDelete()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Initialize an object in which we save our callback
            int testObject = -1;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable
            storageMock.Setup(x => x.Delete<FavouriteList>(It.IsAny<int>())).Callback<int>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method
            Request request = new Request() { Method = "DELETE https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("id", "10")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Run the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the delete method has only been run once
            storageMock.Verify(x => x.Delete<FavouriteList>(It.IsAny<int>()), Times.Once);

            //Check that the int that was parsed to the Delete method was correct relative to our input
            Assert.AreEqual(10, testObject);

            //Get the values from the returned json data
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Favourite List was successfully deleted", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to favourite controller delete method did not contain an id")]
        public void Test_FavouriteRequestController_ProcessDelete_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<FavouriteList>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("Name", "John")) };

            //Call the Process method which should throw an exception
            controller.ProcessDelete(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "The id parse to FavouriteRequestController delete method could not be parsed to int")]
        public void Test_FavouriteRequestController_ProcessDelete_DataError_UnparsableId()
        {
            //Initialize the request controller that is being tested
            FavouriteRequestController controller = new FavouriteRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<FavouriteList>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/FavouriteList", Data = Encoder.Encode(JSonParser.Parse("id", "John")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Call the delegate which should result in an exception
            myDelegate.Invoke(storage);
        }
    }
}
