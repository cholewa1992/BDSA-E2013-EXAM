using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class Encoder
    {
        private static readonly string encoding = "iso-8859-1";

        public static byte[] Encode(string inputString)
        {
            return Encoding.GetEncoding(encoding).GetBytes(inputString);
        }

        public static string Decode(byte[] bytes)
        {
            return Encoding.GetEncoding(encoding).GetString(bytes);
        }

    }
}
