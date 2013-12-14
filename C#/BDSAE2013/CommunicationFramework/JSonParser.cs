using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Utils
{
    /// <summary>
    /// A utiliy class to parse and read a custom json object.
    /// This utility class can only handle json string containing single objects with a list of attributes
    /// </summary>
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// </author>
    public class JSonParser
    {
        /// <summary>
        /// Parse an even amount of strings to set up a json formatted string.
        /// The json is a single object containing a pair of attributes/values for each pair of strings given
        /// </summary>
        /// <param name="parameters"> An array of strings. The array must be even. Each odd string is used as an attribute name and each even is used as its predececcors value </param>
        /// <returns> A json formatted string mapping a single object with attributes with values based on the input string array </returns>
        public static string Parse(params string[] parameters)
        {
            //Check that the incoming parameters are not null
            if (parameters == null)
                throw new ArgumentNullException("Input string array cannot be null");

            //Check that the incoming array actually has values
            if (parameters.Length == 0)
                throw new ArgumentException("JSonParser: Input string array must contain strings");

            //Check if the incoming string array is even. The strings must be ordered in pairs for the method to work
            if (parameters.Length % 2 != 0)
                throw new ArgumentException("Uneven number of parameters");

            //Initialize a string builder and a string writer to create the json string
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);

            //Make a boolean that contains information about whether or not the parser has successfully parsed any entries
            bool hasAdded = false;

            //Use the JsonTextWRiter to write the json string, using the stringwriter
            using (JsonWriter writer = new JsonTextWriter(stringWriter))
            {
                //Set the formatting to be indented
                writer.Formatting = Formatting.Indented;

                //Write the start object of the json string
                writer.WriteStartObject();

                //Iterate through the string array, 2 entries at a time
                for (int i = 0; i < parameters.Length; i += 2)
                {
                    //Skip the entry if the attribute name is an empty string
                    if (parameters[i] == null || parameters[i] == "" || parameters[i + 1] == null)
                        continue;

                    //Write the attribute name
                    writer.WritePropertyName(parameters[i]);
                    //Write the attribut value. Note that the value is reformatted, removing any illegal characters (concering json)
                    writer.WriteValue(CleanString(parameters[i+1]));

                    //Notify the program that there was atleast one successfull entry
                    hasAdded = true;
                }
                
                //Write the end object of the json string
                writer.WriteEndObject();
            }

            //Check if the parser did not add any entries. If it did not, we throw an exception, since the input should contain valid information
            if (!hasAdded)
                throw new ArgumentException("Input string array did not contain any valid entries");

            //Return the completed json string
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Cleans the string from any character not being either in the english alphabet or being a number
        /// Some extra characters are also allowed [' ', ',', '@', '.', '-', '/']
        /// </summary>
        /// <param name="input"> The string to be cleaned </param>
        /// <returns> The cleaned string </returns>
        public static string CleanString(string input)
        {
            return Regex.Replace(input, @"[^A-Za-z0-9()\[\]\s\,\@\.\-\/\:]", "");
        }

        /// <summary>
        /// Get the values from a given json formatted string.
        /// The method will only recognize a single json object contained unique attributes with values
        /// </summary>
        /// <param name="json"> A json formatted string containing only one object with attributes and values </param>
        /// <returns> A dictionary with the attributes of the parsed json string as keys and their values as values </returns>
        public static Dictionary<string, string> GetValues(string json)
        {
            //Initialize the dictionary used for storing our values
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            //The string to contain the key of each key/value pair
            string key = "";

            //Initialize the reader to read the json string
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            
            //Iterate through the entire json using the reader
            while (reader.Read())
            {
                //We only wish to concern ourselves with the JsonToken if it has a value
                if (reader.Value != null)
                {
                    if (key != "")
                    {
                        //If the key has been givenin the key/value pair we use the value acquired from the reader to make an entry in the dictionary
                        dictionary.Add(key, reader.Value.ToString());

                        //Reset the key string 
                        key = "";
                    }
                    else
                    {
                        //If the key has not been given yet in the key/value pair we save the key in the entry variable
                        key = reader.Value.ToString();
                    }
                }
            }

            //When we have read through the entire file we return the dictionary
            return dictionary;
        }
    }
}
