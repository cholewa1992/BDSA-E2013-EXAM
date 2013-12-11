using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using WebServer;
using CommunicationFramework;
using System.Collections.Generic;

namespace WebServerUnitTest
{

    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
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
        public void Test_RequestController_ProcessRequest_Error_BadMethodSyntax_1_Keyword()
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
        [ExpectedException(typeof(UnsplittableStringParameterException),
        "Method syntax is wrong,  must be [Method]' '[URL]")]
        public void Test_RequestController_ProcessRequest_Error_BadMethodSyntax_3_Keywords()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ProcessRequest method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Make a request with a wrong method. (No right-hand url input)
            Request req = new Request() { Method = "GET https://www.google.dk/Movie/5 POST", Data = new byte[0] };

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
        public void Test_RequestController_GetRequestValues_1_Input()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Encode some bytes used to test the convert method
            byte[] bytes = Encoder.Encode(JSonParser.Parse("id", "5"));

            //Get the result of the method
            Dictionary<string, string> values = controller.GetRequestValues(bytes);

            //And test the results
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual("5", values["id"]);
        }

        [TestMethod]
        public void Test_RequestController_GetRequestValues_MoreThan1_Input()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Encode some bytes used to test the convert method
            byte[] bytes = Encoder.Encode(JSonParser.Parse("id", "5", "title", "Die Hard"));

            //Get the result of the method
            Dictionary<string, string> values = controller.GetRequestValues(bytes);

            //And test the results
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("5", values["id"]);
            Assert.AreEqual("Die Hard", values["title"]);
        }

        [TestMethod]
        public void Test_RequestController_GetRequestValues_0_Input()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Encode some bytes used to test the convert method
            byte[] bytes = new byte[0];

            //Get the result of the method
            Dictionary<string, string> values = controller.GetRequestValues(bytes);

            //And test the results
            Assert.AreEqual(0, values.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException),
        "Arguments did not contain proper json")]
        public void Test_RequestController_GetRequestValues_Error_InvalidJson()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Encode some bytes used to test the convert method
            byte[] bytes = Encoder.Encode("sdfgiudgo8e");

            //Do the invocation that should throw an exception
            controller.GetRequestValues(bytes);
        }

        [TestMethod]
        public void Test_RequestController_GetUrlArgument()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Encode some bytes used to test the convert method
            string method = "GET https://www.google.dk/Movie/5";

            //Get the value from the URL
            string value = controller.GetUrlArgument(method);

            //Check that the value is correct
            Assert.AreEqual("5", value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidUrlParameterException),
        "Url did not contain a value after last '/'")]
        public void Test_RequestController_GetUrlArgument_Empty()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Encode some bytes used to test the convert method
            string method = "GET https://www.google.dk/Movie/";

            //Do the invocation that should throw an exception
            controller.GetUrlArgument(method);
        }

        [TestMethod]
        [ExpectedException(typeof(UnsplittableStringParameterException),
        "Url could not be split properly")]
        public void Test_RequestController_GetUrlArgument_Unsplittable_1_Keyword()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Encode some bytes used to test the convert method
            string method = "GET";

            //Do the invocation that should throw an exception
            controller.GetUrlArgument(method);
        }


        [TestMethod]
        [ExpectedException(typeof(UnsplittableStringParameterException),
        "Url could not be split properly")]
        public void Test_RequestController_GetUrlArgument_Unsplittable_3_Keyword()
        {
            //Initialize an arbitrary RequestController (they all implement the same version of ConvertByteToDataTable method from the abstract class
            IRequestController controller = new MovieRequestController();

            //Encode some bytes used to test the convert method
            string method = "GET https://www.google.dk/Movie/5 UPDATE";

            //Get the value from the URL
            string value = controller.GetUrlArgument(method);

            //Do the invocation that should throw an exception
            controller.GetUrlArgument(method);
        }
    }
}
