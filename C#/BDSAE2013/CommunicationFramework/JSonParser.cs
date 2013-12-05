using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class JSonParser
    {
        public static string Parse(params string[] parameters)
        {
            if (parameters.Length % 2 != 0)
                throw new ArgumentException("Uneven number of parameters");

            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);

            using (JsonWriter writer = new JsonTextWriter(stringWriter))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                for (int i = 0; i < parameters.Length; i += 2)
                {
                    writer.WritePropertyName(parameters[i]);
                    writer.WriteValue(parameters[i+1]);
                }
                
                writer.WriteEndObject();
            }

            return stringBuilder.ToString();
        }

        public static Dictionary<string, string> GetValues(string json)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string entry = "";

            JsonTextReader reader = new JsonTextReader(new StringReader(json));

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (entry != "")
                    {
                        dictionary.Add(entry, reader.Value.ToString());
                        entry = "";
                    }
                    else
                        entry = reader.Value.ToString();
                }
            }

            return dictionary;
        }

    }
}
