using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationFramework;
using FakeIMDB_DesktopClient.Model;
using Utils;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {

            var ch = new CommunicationHandler(Protocols.HTTP);
            ch.Send("http://localhost:1337/MovieData/2332647", new byte[0], "GET");


            var result = Encoder.Decode(ch.Receive(10000));

            Console.WriteLine("result received");

            var json = JSonParser.GetValues(result);

            List<PersonSearchItem> itemList = new List<PersonSearchItem>();

            int id = 0;
            while (json.ContainsKey("a" + id + "Id"))
            {
                
                itemList.Add(new PersonSearchItem(){ Id =  json["a"+id+"Id"], Name = json["a"+id+"Id"]});

            }

            foreach (var item in itemList)
            {
                Console.WriteLine(item.Id + " - " + item.Name);
            }
            


            Console.ReadKey();
        }
    }
}
