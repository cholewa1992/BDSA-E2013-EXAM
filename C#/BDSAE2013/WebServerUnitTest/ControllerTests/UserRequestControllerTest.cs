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
    public class UserRequestControllerTest
    {
        [TestMethod]
        public void Test_UserRequestController_ProcessGet_JSon_Attributes()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method when given the id 5
            storageMock.Setup(x => x.Get<UserAcc>(5)).Returns(new UserAcc() { Id = 5, Firstname = "John", Lastname = "Anderson", Username = "darkknight42", Password = "donttellanyone123", Email = "princeOfDarkness333@hotmail.com"});
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/User/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("John", values["firstname"]);
            Assert.AreEqual("Anderson", values["lastname"]);
            Assert.AreEqual("darkknight42", values["username"]);
            Assert.AreEqual("donttellanyone123", values["password"]);
            Assert.AreEqual("princeOfDarkness333@hotmail.com", values["email"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Incoming argument must be formatted as an int")]
        public void Test_UserRequestController_ProcessGet_Error_NonIntArgument()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/User/Die_Hard" };

            //Invoke the ProcessRequest method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        public void Test_UserRequestController_ProcessPost_MinimumInfo()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Initialize an object in which we save our callback
            UserAcc testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<UserAcc>(It.IsAny<UserAcc>())).Callback<UserAcc>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/User", Data = Encoder.Encode(JSonParser.Parse("username", "darkknight42", "password", "donttellanyone123")) };
            
            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<UserAcc>(It.IsAny<UserAcc>()), Times.Once);
            
            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual("darkknight42", testObject.Username);
            Assert.AreEqual("donttellanyone123", testObject.Password);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The User was successfully added", values["response"]);
        }

        [TestMethod]
        public void Test_UserRequestController_ProcessPost_MaximumInfo()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Initialize an object in which we save our callback
            UserAcc testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<UserAcc>(It.IsAny<UserAcc>())).Callback<UserAcc>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/User", Data = Encoder.Encode(JSonParser.Parse("username", "darkknight42", "password", "donttellanyone123", "firstname", "John", "lastname", "Anderson", "email", "princeOfDarkness333@hotmail.com")) };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<UserAcc>(It.IsAny<UserAcc>()), Times.Once);

            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual("darkknight42", testObject.Username);
            Assert.AreEqual("donttellanyone123", testObject.Password);
            Assert.AreEqual("John", testObject.Firstname);
            Assert.AreEqual("Anderson", testObject.Lastname);
            Assert.AreEqual("princeOfDarkness333@hotmail.com", testObject.Email);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The User was successfully added", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to UserAccController post method did not contain enough information")]
        public void Test_UserRequestController_ProcessPost_DataError_NoUsername()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no UserAcc Id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/User", Data = Encoder.Encode(JSonParser.Parse("password", "donttellanyone123")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to UserAccController post method did not contain enough information")]
        public void Test_UserRequestController_ProcessPost_DataError_NoPassword()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no type id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/User", Data = Encoder.Encode(JSonParser.Parse("username", "darkknight42")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        public void Test_UserRequestController_ProcessPut()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Initialize an object in which we save our callback
            UserAcc testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<UserAcc>(5)).Returns(new UserAcc() { Id = 5, Firstname = "Christie", Lastname = "Samuelson", Username = "totalfan13", Password = "shhmypassword", Email = "imdbUser23@hotmail.com" });
            //Setup the mock so it saves the callback object in our test object variable;
            storageMock.Setup(x => x.Update<UserAcc>(It.IsAny<UserAcc>())).Callback<UserAcc>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Put Method
            Request request = new Request() { Method = "PUT https://www.google.dk/User", Data = Encoder.Encode(JSonParser.Parse("id", "5", "username", "darkknight42", "password", "donttellanyone123", "firstname", "John", "lastname", "Anderson", "email", "princeOfDarkness333@hotmail.com")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPut(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the update method has only been run once
            storageMock.Verify(x => x.Update<UserAcc>(It.IsAny<UserAcc>()), Times.Once);
            
            //Check that the object that was parsed to the Put method was correct relative to our input
            Assert.AreEqual(5, testObject.Id);
            Assert.AreEqual("darkknight42", testObject.Username);
            Assert.AreEqual("donttellanyone123", testObject.Password);
            Assert.AreEqual("John", testObject.Firstname);
            Assert.AreEqual("Anderson", testObject.Lastname);
            Assert.AreEqual("princeOfDarkness333@hotmail.com", testObject.Email);

            //Get the values contained in the returned bytes
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The User was successfully updated", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to UserAccController put method did not contain an id")]
        public void Test_UserRequestController_ProcessPut_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<UserAcc>(10)).Returns(new UserAcc() { Id = 5, Firstname = "Christie", Lastname = "Samuelson", Username = "totalfan13", Password = "shhmypassword", Email = "imdbUser23@hotmail.com" });
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Update<UserAcc>(It.IsAny<UserAcc>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Put Method (Note that there is no id in the data)
            Request request = new Request() { Method = "PUT https://www.google.dk/User", Data = Encoder.Encode(JSonParser.Parse("username", "darkknight42", "password", "donttellanyone123", "firstname", "John", "lastname", "Anderson", "email", "princeOfDarkness333@hotmail.com")) };

            //Invoke the method that will throw the exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        public void Test_UserRequestController_ProcessDelete()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Initialize an object in which we save our callback
            int testObject = -1;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable
            storageMock.Setup(x => x.Delete<UserAcc>(It.IsAny<int>())).Callback<int>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method
            Request request = new Request() { Method = "DELETE https://www.google.dk/User", Data = Encoder.Encode(JSonParser.Parse("id", "10")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Run the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the delete method has only been run once
            storageMock.Verify(x => x.Delete<UserAcc>(It.IsAny<int>()), Times.Once);

            //Check that the int that was parsed to the Delete method was correct relative to our input
            Assert.AreEqual(10, testObject);

            //Get the values from the returned json data
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The User was successfully deleted", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to favourite controller delete method did not contain an id")]
        public void Test_UserRequestController_ProcessDelete_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<UserAcc>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/User", Data = Encoder.Encode(JSonParser.Parse("Name", "John")) };

            //Call the Process method which should throw an exception
            controller.ProcessDelete(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "The id parse to UserRequestController delete method could not be parsed to int")]
        public void Test_UserRequestController_ProcessDelete_DataError_UnparsableId()
        {
            //Initialize the request controller that is being tested
            UserRequestController controller = new UserRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<UserAcc>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/User", Data = Encoder.Encode(JSonParser.Parse("id", "John")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Call the delegate which should result in an exception
            myDelegate.Invoke(storage);
        }
    }
}
