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
    public class PersonRequestControllerTest
    {
        [TestMethod]
        public void Test_PersonRequestController_ProcessGet_JSon_Attributes()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method when given the id 5
            storageMock.Setup(x => x.Get<People>(5)).Returns(new People() { Id = 5, Name = "Bruce Willis", Gender = "Male"});
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/People/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Bruce Willis", values["name"]);
            Assert.AreEqual("Male", values["gender"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Incoming argument must be formatted as an int")]
        public void Test_PersonRequestController_ProcessGet_Error_NonIntArgument()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/People/Die_Hard" };

            //Invoke the ProcessRequest method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_PersonRequestController_ProcessGet_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessGet method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request method must no be null")]
        public void Test_PersonRequestController_ProcessGet_Error_NullMethod()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = null };

            //Invoke the ProcessGet method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        public void Test_PersonRequestController_ProcessPost()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Initialize an object in which we save our callback
            People testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<People>(It.IsAny<People>())).Callback<People>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/People", Data = Encoder.Encode(JSonParser.Parse("name", "Bruce Willis", "gender", "Male")) };
            
            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<People>(It.IsAny<People>()), Times.Once);
            
            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual("Bruce Willis", testObject.Name);
            Assert.AreEqual("Male", testObject.Gender);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Person was successfully added", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to PeopleController post method did not contain enough information")]
        public void Test_PersonRequestController_ProcessPost_DataError_NoName()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no People Id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/People", Data = Encoder.Encode(JSonParser.Parse("gender", "Male")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to PeopleController post method did not contain enough information")]
        public void Test_PersonRequestController_ProcessPost_DataError_NoGender()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no type id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/People", Data = Encoder.Encode(JSonParser.Parse("name", "Bruce Willis")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_PersonRequestController_ProcessPost_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessPost method with null input. This invocation should throw an exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request data must no be null")]
        public void Test_PersonRequestController_ProcessPost_Error_NullData()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Data = null };

            //Invoke the ProcessPost method with null input. This invocation should throw an exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        public void Test_PersonRequestController_ProcessPut()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Initialize an object in which we save our callback
            People testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<People>(5)).Returns(new People() { Id = 5, Name = "Bruce Willis", Gender = "Male"});
            //Setup the mock so it saves the callback object in our test object variable;
            storageMock.Setup(x => x.Update<People>(It.IsAny<People>())).Callback<People>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Put Method
            Request request = new Request() { Method = "PUT https://www.google.dk/People", Data = Encoder.Encode(JSonParser.Parse("id", "5", "name", "Angelina Jolie", "gender", "Female")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPut(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the update method has only been run once
            storageMock.Verify(x => x.Update<People>(It.IsAny<People>()), Times.Once);
            
            //Check that the object that was parsed to the Put method was correct relative to our input
            Assert.AreEqual(5, testObject.Id);
            Assert.AreEqual("Angelina Jolie", testObject.Name);
            Assert.AreEqual("Female", testObject.Gender);

            //Get the values contained in the returned bytes
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Person was successfully updated", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to PeopleController put method did not contain an id")]
        public void Test_PersonRequestController_ProcessPut_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<People>(10)).Returns(new People() { Id = 5, Name = "Bruce Willis", Gender = "Male" });
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Update<People>(It.IsAny<People>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Put Method (Note that there is no id in the data)
            Request request = new Request() { Method = "PUT https://www.google.dk/People", Data = Encoder.Encode(JSonParser.Parse( "name", "Angelina Jolie", "gender", "Female")) };

            //Invoke the method that will throw the exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_PersonRequestController_ProcessPut_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessPut method with null input. This invocation should throw an exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request data must no be null")]
        public void Test_PersonRequestController_ProcessPut_Error_NullData()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Data = null };

            //Invoke the ProcessPut method with null input. This invocation should throw an exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        public void Test_PersonRequestController_ProcessDelete()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Initialize an object in which we save our callback
            int testObject = -1;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable
            storageMock.Setup(x => x.Delete<People>(It.IsAny<int>())).Callback<int>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method
            Request request = new Request() { Method = "DELETE https://www.google.dk/People", Data = Encoder.Encode(JSonParser.Parse("id", "10")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Run the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the delete method has only been run once
            storageMock.Verify(x => x.Delete<People>(It.IsAny<int>()), Times.Once);

            //Check that the int that was parsed to the Delete method was correct relative to our input
            Assert.AreEqual(10, testObject);

            //Get the values from the returned json data
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The Person was successfully deleted", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to favourite controller delete method did not contain an id")]
        public void Test_PersonRequestController_ProcessDelete_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<People>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/People", Data = Encoder.Encode(JSonParser.Parse("Name", "John")) };

            //Call the Process method which should throw an exception
            controller.ProcessDelete(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "The id parse to PersonRequestController delete method could not be parsed to int")]
        public void Test_PersonRequestController_ProcessDelete_DataError_UnparsableId()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<People>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/People", Data = Encoder.Encode(JSonParser.Parse("id", "John")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Call the delegate which should result in an exception
            myDelegate.Invoke(storage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request must no be null")]
        public void Test_PersonRequestController_ProcessDelete_Error_NullRequest()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Set up the request that is being parsed to the process method
            Request request = null;

            //Invoke the ProcessDelete method with null input. This invocation should throw an exception
            controller.ProcessDelete(request);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Incoming request data must no be null")]
        public void Test_PersonRequestController_ProcessDelete_Error_NullData()
        {
            //Initialize the request controller that is being tested
            PersonRequestController controller = new PersonRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Data = null };

            //Invoke the ProcessDelete method with null input. This invocation should throw an exception
            controller.ProcessDelete(request);
        }
    }
}
