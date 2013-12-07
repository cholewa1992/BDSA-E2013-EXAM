using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using System.Collections.Generic;
using Newtonsoft.Json;

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

            //Tests that the empty attribute name was omitted
            Assert.AreEqual("{\r\n  \"title1\": \"Die Hard\"\r\n}", json);
        }

        [TestMethod]
        public void Test_JSonParser_Parse_EmptyStringOmmitting_AttributeValue()
        {
            //Initialize the input string array.
            String[] inputString = new String[] { "title1", "Die Hard", "title2", "" };

            //Use the parse to get the ouput in order to test it.
            string json = JSonParser.Parse(inputString);

            //Tests that the empty attribute value was not omitted
            Assert.AreEqual("{\r\n  \"title1\": \"Die Hard\",\r\n  \"title2\": \"\"\r\n}", json);
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
        public void Test_JSonParser_CleanString_RemovedForeignCharacters()
        {
            //Initialize the input string.
            String inputString = "Dø Hårdt Mega Hårdt";

            //Clean the string
            String cleanedString = JSonParser.CleanString(inputString);

            //Assert that the foreign characters has been removed
            Assert.AreEqual("D Hrdt Mega Hrdt", cleanedString);
        }

        [TestMethod]
        public void Test_JSonParser_GetValues_One_Pair()
        {
            //Initialize the input json string.
            String inputJson = "{\r\n  \"title1\": \"Die Hard\",\r\n}";

            //Get the values from the string to test them
            Dictionary<string, string> values = JSonParser.GetValues(inputJson);

            //Assert that the dictionary contains the correct values
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual("Die Hard", values["title1"]);
        }

        [TestMethod]
        public void Test_JSonParser_GetValues_MoreThanOne_Pair()
        {
            //Initialize the input json string.
            String inputJson = "{\r\n  \"title1\": \"Die Hard\",\r\n  \"title2\": \"Die Another Day\"\r\n}";

            //Get the values from the string to test them
            Dictionary<string, string> values = JSonParser.GetValues(inputJson);

            //Assert that the dictionary contains the correct values
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("Die Hard", values["title1"]);
            Assert.AreEqual("Die Another Day", values["title2"]);
        }

        [TestMethod]
        public void Test_JSonParser_GetValues_EmptyString()
        {
            //Initialize the input json string.
            String inputJson = "";

            //Get the values from the string to test them
            Dictionary<string, string> values = JSonParser.GetValues(inputJson);

            //Assert that the dictionary does not contain any values
            Assert.AreEqual(0, values.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException),
        "Missing seperator. Excepted ':'")]
        public void Test_JSonParser_GetValues_Error_InvalidCharacters()
        {
            //Initialize the input json string. (Note that there is an '"' too much, which will make the jsonreader unable to read the file
            String inputJson = "{\r\n  \"title1\": \"Atenshon purÃ®zu supesharu: Ã\"sutoraria ShidonÃ® hen\"\r\n}";

            //Make the invocation that will throw an exception
            JSonParser.GetValues(inputJson);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException),
        "Missing seperator. Excepted ':'")]
        public void Test_JSonParser_GetValues_Error_Uneven_Pairs_NoSeperator()
        {
            //Initialize the input json string. (Note that there are uneven pairs)
            String inputJson = "{\r\n  \"title1\": \"Die Hard\",\r\n  \"title2\"\r\n}";

            //Make the invocation that will throw an exception
            JSonParser.GetValues(inputJson);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonReaderException),
        "Unexpected character, no value found")]
        public void Test_JSonParser_GetValues_Error_Uneven_Pairs_Seperator()
        {
            //Initialize the input json string.  (Note that there are uneven pairs, even though the seperator is given)
            String inputJson = "{\r\n  \"title1\": \"Die Hard\",\r\n  \"title2\":\r\n}";

            //Make the invocation that will throw an exception
            JSonParser.GetValues(inputJson);
        }
    }
}
