using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServer;
using Storage;
using EntityFrameworkStorage;
using System.Text;
using System.Collections.Specialized;
using CommunicationFramework;

namespace WebServerUnitTest
{
    [TestClass]
    public class RequestControllerTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Parsed request is null")]
        public void Test_RequestController_ProcessRequest_Error_NullInput()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ProcessRequest method from the abstract class
            IRequestController controller = new MovieRequestController();
            
            //Invoke the ProcessRequest method with null input. This invocation should throw an exception
            controller.ProcessRequest(null);
        }

        [TestMethod]
        [ExpectedException(typeof(UnsplittableStringParameterException),
        "Method syntax is wrong,  must be [Method]' '[URL]")]
        public void Test_RequestController_ProcessRequest_Error_BadMethod()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ProcessRequest method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Make a request with a wrong method. (No right-hand url input)
            Request req = new Request() { Method = "GET", Data = new byte[0]};

            //Invoke the ProcessRequest method with an input with a wrongly formatted Method. 
            //This invocation should throw an exception
            controller.ProcessRequest(req);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRestMethodException),
        "Parsed method has no proper rest method")]
        public void Test_RequestController_ProcessRequest_Error_NoHit()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ProcessRequest method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Make a request with a wrong method. (No matching rest method)
            Request req = new Request() { Method = "UPDATE https://www.google.dk/", Data = new byte[0] };

            //Invoke the ProcessRequest method with null input. This invocation should throw an exception
            controller.ProcessRequest(req);


        }

        [TestMethod]
        public void Test_RequestController_ConvertByteToDataTable()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Encode some bytes used to test the convert method
            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes("id=6&name=John");

            //Get the result of the method
            NameValueCollection table = controller.ConvertByteToDataTable(bytes);

            //And test the results
            Assert.AreEqual("6", table["id"]);
            Assert.AreEqual("John", table["name"]);
        }

        [TestMethod]
        public void Test_RequestController_ConvertByteToDataTable_EmptyInput()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Create the input variable and make it null
            byte[] bytes = new byte[0];

            //Invoke the method that should cause the exception
            NameValueCollection table = controller.ConvertByteToDataTable(bytes);

            Assert.AreEqual(0, table.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Parsed bytes are null")]
        public void Test_RequestController_ConvertByteToDataTable_Error_NullInput()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Create the input variable and make it null
            byte[] bytes = null;

            //Invoke the method that should cause the exception
            NameValueCollection table = controller.ConvertByteToDataTable(bytes);
        }
    }
}
