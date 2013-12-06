using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// An encoder utility class used for encoding and decoding byte strings.
    /// </summary>
    public class Encoder
    {
        //The standard encoding of the encoder
        private static readonly string encoding = "iso-8859-1";

        /// <summary>
        /// Method used to encode a string into a byte array, using the preset encoding format of the encoder class
        /// </summary>
        /// <param name="inputString"> The string to encode </param>
        /// <returns> A byte array encoded using the preset encoding of the encoder class </returns>
        public static byte[] Encode(string inputString)
        {
            //Return the encoded byte array
            return Encoding.GetEncoding(encoding).GetBytes(inputString);
        }

        /// <summary>
        /// Method used to decode a byte array into a string, using the preset encoding of the encoder class
        /// </summary>
        /// <param name="bytes"> The byts to be decoded </param>
        /// <returns> A string decoded from the bytes using the preset encoding of the encoder class </returns>
        public static string Decode(byte[] bytes)
        {
            //Return the decoded string
            return Encoding.GetEncoding(encoding).GetString(bytes);
        }
    }
}
