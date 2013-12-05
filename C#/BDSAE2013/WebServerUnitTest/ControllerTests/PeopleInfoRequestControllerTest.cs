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
    public class PeopleInfoRequestControllerTest
    {
        [TestMethod]
        public void Test_PeopleInfoRequestController_ProcessGet_JSon_Attributes()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Map the returned values of the storage Get method when given the id 5
            storageMock.Setup(x => x.Get<PersonInfo>(5)).Returns(new PersonInfo() { Id = 5, Person_Id = 1, Type_Id = 2, Info = "Bald", Note = "Do not tease"});
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonInfo/5" };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessGet(request);

            //Use the delegate to acquire the data from the storage
            byte[] data = myDelegate.Invoke(storage);

            //Convert the received json bytes to a value dictionary
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Check that the values returned by the delegate are correct
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("1", values["personId"]);
            Assert.AreEqual("2", values["typeId"]);
            Assert.AreEqual("Bald", values["info"]);
            Assert.AreEqual("Do not tease", values["note"]);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "Incoming argument must be formatted as an int")]
        public void Test_PeopleInfoRequestController_ProcessGet_Error_NonIntArgument()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Set up the request that is being parsed to the process method
            Request request = new Request() { Method = "GET https://www.google.dk/PersonInfo/Die_Hard" };

            //Invoke the ProcessRequest method with null input. This invocation should throw an exception
            controller.ProcessGet(request);
        }

        [TestMethod]
        public void Test_PeopleInfoRequestController_ProcessPost_MinimumInfo()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Initialize an object in which we save our callback
            PersonInfo testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<PersonInfo>(It.IsAny<PersonInfo>())).Callback<PersonInfo>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("personId", "1", "typeId", "2", "info", "62 years")) };
            
            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<PersonInfo>(It.IsAny<PersonInfo>()), Times.Once);
            
            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual(1, testObject.Person_Id);
            Assert.AreEqual(2, testObject.Type_Id);
            Assert.AreEqual("62 years", testObject.Info);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The PersonInfo was successfully added", values["response"]);
        }

        [TestMethod]
        public void Test_PeopleInfoRequestController_ProcessPost_MaximumInfo()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Initialize an object in which we save our callback
            PersonInfo testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable.
            storageMock.Setup(x => x.Add<PersonInfo>(It.IsAny<PersonInfo>())).Callback<PersonInfo>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method
            Request request = new Request() { Method = "POST https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("personId", "1", "typeId", "2", "info", "62 years", "note", "Old")) };

            //Call the process method to get the delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPost(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the add method has only been run once (Through the delegate)
            storageMock.Verify(x => x.Add<PersonInfo>(It.IsAny<PersonInfo>()), Times.Once);

            //Check that the object that was parsed to the add method was correct relative to our input
            Assert.AreEqual(0, testObject.Id);
            Assert.AreEqual(1, testObject.Person_Id);
            Assert.AreEqual(2, testObject.Type_Id);
            Assert.AreEqual("62 years", testObject.Info);
            Assert.AreEqual("Old", testObject.Note);

            //Get the values contained in the json returned from the delegate
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The PersonInfo was successfully added", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to PersonInfoController post method did not contain enough information")]
        public void Test_PeopleInfoRequestController_ProcessPost_DataError_NoPersonId()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no PersonInfo Id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("typeId", "2", "info", "62 years")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to PersonInfoController post method did not contain enough information")]
        public void Test_PeopleInfoRequestController_ProcessPost_DataError_NoTypeId()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no type id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("personId", "1", "info", "62 years")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to PersonInfoController post method did not contain enough information")]
        public void Test_PeopleInfoRequestController_ProcessPost_DataError_NoInfo()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Post Method (Note that there is no type id in the data)
            Request request = new Request() { Method = "POST https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("personId", "1", "typeId", "2")) };

            //Make the method call that will throw the desired exception
            controller.ProcessPost(request);
        }

        [TestMethod]
        public void Test_PeopleInfoRequestController_ProcessPut()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Initialize an object in which we save our callback
            PersonInfo testObject = null;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<PersonInfo>(5)).Returns(new PersonInfo() { Id = 5, Person_Id = 1, Type_Id = 2, Info = "Bald", Note = "Do not tease"});
            //Setup the mock so it saves the callback object in our test object variable;
            storageMock.Setup(x => x.Update<PersonInfo>(It.IsAny<PersonInfo>())).Callback<PersonInfo>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Put Method
            Request request = new Request() { Method = "PUT https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("id", "5", "personId", "2", "typeId", "3", "info", "62 years", "note", "Old")) };

            //Process The request to get the desired delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessPut(request);

            //Invoke the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the update method has only been run once
            storageMock.Verify(x => x.Update<PersonInfo>(It.IsAny<PersonInfo>()), Times.Once);
            
            //Check that the object that was parsed to the Put method was correct relative to our input
            Assert.AreEqual(5, testObject.Id);
            Assert.AreEqual(2, testObject.Person_Id);
            Assert.AreEqual(3, testObject.Type_Id);
            Assert.AreEqual("62 years", testObject.Info);
            Assert.AreEqual("Old", testObject.Note);

            //Get the values contained in the returned bytes
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The PersonInfo was successfully updated", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to PersonInfoController put method did not contain either a title or a user account id")]
        public void Test_PeopleInfoRequestController_ProcessPut_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the get method of the storage so it returns something when using the put request
            storageMock.Setup(x => x.Get<PersonInfo>(10)).Returns(new PersonInfo() { Id = 5, Person_Id = 1, Type_Id = 2, Info = "Bald", Note = "Do not tease" });
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Update<PersonInfo>(It.IsAny<PersonInfo>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Put Method (Note that there is no id in the data)
            Request request = new Request() { Method = "PUT https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("personId", "2", "typeId", "3", "info", "62 years", "note", "Old")) };

            //Invoke the method that will throw the exception
            controller.ProcessPut(request);
        }

        [TestMethod]
        public void Test_PeopleInfoRequestController_ProcessDelete()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Initialize an object in which we save our callback
            int testObject = -1;

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it saves the callback object in our test object variable
            storageMock.Setup(x => x.Delete<PersonInfo>(It.IsAny<int>())).Callback<int>((obj => testObject = obj));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method
            Request request = new Request() { Method = "DELETE https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("id", "10")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Run the delegate to make it testable
            byte[] data = myDelegate.Invoke(storage);

            //Check that the delete method has only been run once
            storageMock.Verify(x => x.Delete<PersonInfo>(It.IsAny<int>()), Times.Once);

            //Check that the int that was parsed to the Delete method was correct relative to our input
            Assert.AreEqual(10, testObject);

            //Get the values from the returned json data
            Dictionary<string, string> values = JSonParser.GetValues(Encoder.Decode(data));

            //Assert that the returned response is correct
            Assert.AreEqual("The PersonInfo was successfully deleted", values["response"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "The data parsed to favourite controller delete method did not contain an id")]
        public void Test_PeopleInfoRequestController_ProcessDelete_DataError_NoId()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<PersonInfo>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;
            
            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("Name", "John")) };

            //Call the Process method which should throw an exception
            controller.ProcessDelete(request);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException),
        "The id parse to PeopleInfoRequestController delete method could not be parsed to int")]
        public void Test_PeopleInfoRequestController_ProcessDelete_DataError_UnparsableId()
        {
            //Initialize the request controller that is being tested
            PeopleInfoRequestController controller = new PeopleInfoRequestController();

            //Make a mock of the storage.
            var storageMock = new Mock<IStorageConnectionBridgeFacade>();
            //Setup the mock so it does not actually use the functionality of the storage class
            storageMock.Setup(x => x.Delete<PersonInfo>(It.IsAny<int>()));
            //Make an intance of the storage class using the mock
            var storage = storageMock.Object;

            //Make the request to send to the Delete Method (Note that the data contains no id)
            Request request = new Request() { Method = "DELETE https://www.google.dk/PersonInfo", Data = Encoder.Encode(JSonParser.Parse("id", "John")) };

            //Call the Process method to get the designated delegate
            Func<IStorageConnectionBridgeFacade, byte[]> myDelegate = controller.ProcessDelete(request);

            //Call the delegate which should result in an exception
            myDelegate.Invoke(storage);
        }
    }
}
