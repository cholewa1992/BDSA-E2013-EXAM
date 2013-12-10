using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServer;
using CommunicationFramework;
using Utils;

namespace WebServerTestRun
{
    class Program
    {
        static void Main(string[] args)
        {
            RequestDelegator delegator = new RequestDelegator();
            Request request = new Request();
            /*
            request.Method = "GET http://localhost:112/Movie/795";

            Request response = delegator.ProcessRequest(request);

            Console.WriteLine(response.ResponseStatusCode);

            var values = JSonParser.GetValues(Encoder.Decode(response.Data));

            Console.WriteLine(values["response"]);
            */
            /*
            Request request = new Request() { Method = "POST https://www.google.dk/Movie", Data = Encoder.Encode(JSonParser.Parse("title", "Die Hard 2", "year", "2000", "kind", "TV Series")) };
            Request response = delegator.ProcessRequest(request);

            Console.WriteLine(Encoder.Decode(request.Data));

            Console.WriteLine(JSonParser.GetValues(Encoder.Decode(request.Data))["response"]);

            Console.WriteLine(response.ResponseStatusCode);

            Console.ReadKey();
             */
            
            
            ////Test movie processing
            //request.Method = "GET http://localhost:112/Search/Titanic";
            //Request response = delegator.ProcessRequest(request);

            //string json = Encoder.Decode(response.Data);

            //Dictionary<string, string> values = JSonParser.GetValues(json);

            //int index = 0;

            //Console.WriteLine("Movies");
            //while (values.ContainsKey("m" + index + "Id"))
            //{
            //    Console.WriteLine(values["m" + index + "Id"] + ": " + values["m" + index + "Title"]); ;

            //    index++;
            //}

            //index = 0;

            //Console.WriteLine("People");

            //while (values.ContainsKey("p" + index + "Id"))
            //{
            //    Console.WriteLine(values["p" + index + "Id"] + ": " + values["p" + index + "Name"]); ;

            //    index++;
            //}


            request.Method = "GET http://localhost:112/MovieData/2332647";
            Request response = delegator.ProcessRequest(request);

            string json = Encoder.Decode(response.Data);
            Console.WriteLine(response.ResponseStatusCode);

            Dictionary<string, string> values = JSonParser.GetValues(json);

            Console.WriteLine(values["title"]);
            Console.WriteLine(values["year"]);
            Console.WriteLine(values["kind"]);
            Console.WriteLine(values["seasonNumber"]);
            Console.WriteLine(values["seriesYear"]);
            Console.WriteLine(values["episodeNumber"]);

            int index = 0;

            for (int i = 0; i < 20; i++)
            {
                while (values.ContainsKey("mi" + i + "," + index))
                {
                    Console.WriteLine(values["mi" + i + "," + index]) ;

                    index++;
                }

                index = 0;
            } 

            while (values.ContainsKey("p" + index + "Id"))
            {
                Console.WriteLine("Id: " + values["p" + index + "Id"] + " Name: " + values["p" + index + "Name"] + " Character: " + values["p" + index + "CharacterName"] + "Role: " + values["p" + index + "Role"] + "Note: " + values["p" + index + "Note"] + "NrOrder: " + values["p" + index + "NrOrder"]);

                index++;
            }
            
            
            /*
            request.Method = "GET http://localhost:112/PersonData/1630018";
            Request response = delegator.ProcessRequest(request);

            string json = Encoder.Decode(response.Data);
            Console.WriteLine(response.ResponseStatusCode);

            Dictionary<string, string> values = JSonParser.GetValues(json);

            Console.WriteLine(values["name"]);
            Console.WriteLine(values["gender"]);

            int index = 0;

            for (int i = 0; i < 20; i++)
            {
                while (values.ContainsKey("pi" + i + "," + index))
                {
                    Console.WriteLine(values["pi" + i + "," + index]);

                    index++;
                }

                index = 0;
            }

            while (values.ContainsKey("m" + index + "Id"))
            {
                Console.WriteLine("Id: " + values["m" + index + "Id"] + " Title: " + values["m" + index + "Title"] + " Kind: " + values["m" + index + "Kind"] + "Year: " + values["m" + index + "Year"] + "Character: " + values["m" + index + "CharacterName"] + "Role: " + values["m" + index + "Role"]);

                index++;
            }
            */
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
