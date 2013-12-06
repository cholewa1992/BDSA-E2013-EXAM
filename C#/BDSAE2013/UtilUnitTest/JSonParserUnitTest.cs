using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UtilUnitTest
{
    [TestClass]
    public class JSonParserUnitTest
    {
        [TestMethod]
        public void Test_JSonParser_Parse()
        {
            //Initialize the input string array.
            String[] inputString = new String[] { "title1", "Die Hard", "title2", "Die Another Day" };

            //Use the parse to get the ouput in order to test it.
            string json = JSonParser.Parse(inputString);

            Assert.AreEqual("{\r\n  \"title1\": \"Die Hard\",\r\n  \"title2\": \"Die Another Day\"\r\n}", json);
        }

        [TestMethod]
        public void Test_JSonParser_Parse_EmptyStringOmmitting_AttributeName()
        {
            //Initialize the input string array.
            String[] inputString = new String[] { "title1", "Die Hard", "", "Die Another Day" };

            //Use the parse to get the ouput in order to test it.
            string json = JSonParser.Parse(inputString);

            Assert.AreEqual("{\r\n  \"title1\": \"Die Hard\"\r\n}", json);
        }

        [TestMethod]
        public void Test_JSonParser_Parse_EmptyStringOmmitting_AttributeValue()
        {
            //Initialize the input string array.
            String[] inputString = new String[] { "title1", "Die Hard", "titl2", "" };

            //Use the parse to get the ouput in order to test it.
            string json = JSonParser.Parse(inputString);

            Assert.AreEqual("{\r\n  \"title1\": \"Die Hard\"\r\n}", json);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
         "Array does not contain any valid entries")]
        public void Test_JSonParser_Parse_Error_NoValidEntries()
        {
            //Initialize the input string array.
            String[] inputString = new String[]{"", ""};

            //Make the invocation that will throw the exception
            JSonParser.Parse(inputString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
        "Array does not contain any strings")]
        public void Test_JSonParser_Parse_Error_EmptyArray()
        {
            //Initialize the input string array.
            String[] inputString = new String[0];

            //Make the invocation that will throw the exception
            JSonParser.Parse(inputString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
        "Uneven number of strings")]
        public void Test_JSonParser_Parse_Error_UnevenStringInput()
        {
            //Initialize the input string array. Note that it has an uneven number of entries
            String[] inputString = new String[] { "id", "5", "title", "Die Hard", "Mega Hard"};

            //Make the invocation that will throw the exception
            JSonParser.Parse(inputString);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),
        "Input string array cannot be null")]
        public void Test_JSonParser_Parse_Error_Null()
        {
            //Initialize the input string array. Note that it has an uneven number of entries
            String[] inputString = null;

            //Make the invocation that will throw the exception
            JSonParser.Parse(inputString);
        }

        [TestMethod]
        public void Test_JSonParser_CleanString_NoChange()
        {
            //Initialize the input string.
            String inputString = "Die Hard";

            //Clean the string
            String cleanedString = JSonParser.CleanString(inputString);

            //Assert that there are no difference
            Assert.AreEqual("Die Hard", cleanedString);
        }

        [TestMethod]
        public void Test_JSonParser_CleanString_RemovedColons()
        {
            //Initialize the input string.
            String inputString = "Die Hard : Mega Hard";

            //Clean the string
            String cleanedString = JSonParser.CleanString(inputString);

            //Assert that the colon has been removed
            Assert.AreEqual("Die Hard  Mega Hard", cleanedString);
        }

        [TestMethod]
        public void Test_JSonParser_CleanString_RemovedForeignCharacters()
        {
            //Initialize the input string.
            String inputString = "Dø Hårdt Mega Hårdt";

            //Clean the string
            String cleanedString = JSonParser.CleanString(inputString);

            //Assert that the foreign characters has been removed
            Assert.AreEqual("D Hrdt Mega Hrdt", cleanedString);
        }
    }
}
