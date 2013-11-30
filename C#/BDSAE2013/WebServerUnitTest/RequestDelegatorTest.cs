using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;
using Storage;
using EntityFrameworkStorage;

namespace WebServerUnitTest
{
    [TestClass]
    public class RequestDelegatorTest
    {
        [TestMethod]
        public void Test_RequestDelegator_Constructor_BaseConstructor()
        {
            //Initialize a request delegator using the base constructor
            RequestDelegator requestDelegator = new RequestDelegator();

            //If no exceptions are thrown, the test is passed
        }

        [TestMethod]
        public void Test_RequestDelegator_Constructor_InjectedStorage()
        {
            //Initialize a request delegator with injection
            RequestDelegator requestDelegator = new RequestDelegator(new StorageFacade(new EFConnectionFactory()));

            //If no exceptions are thrown, the test is passed
        }

        //TODO Make test to check functionality of ProcessRequest

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Parsed request is null")]
        public void Test_RequestDelegator_ProcessRequest_Error_NullInput()
        {
            RequestDelegator requestDelegator = new RequestDelegator();

            requestDelegator.ProcessRequest(null);
        }

        //TODO Make test to check functionality of DefineController
        /*
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Parsed method is null")]
        public void Test_RequestDelegator_DefineController_Error_NullInput()
        {
            //Initialize a request delegator with injection
            RequestDelegator requestDelegator = new RequestDelegator();

            requestDelegator.DefineController(null);
        }
        */


    }
}
