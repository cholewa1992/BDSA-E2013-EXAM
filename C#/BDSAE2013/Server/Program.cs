using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server
{
    public class UrlPaser
    {
        public static void Main(string[] args)
        {
            new UrlPaser().ParseUrl(new Uri("http://dr.dk/Search"));
            new UrlPaser().ParseUrl(new Uri("http://dr.dk/Search/PulpFiction"));
            new UrlPaser().ParseUrl(new Uri("http://dr.dk/Search/id/5"));
            new UrlPaser().ParseUrl(new Uri("http://dr.dk/Search/Samuel"));
            Console.ReadKey();
        }

        public void ParseUrl(Uri uri)
        {
            var match = Regex.Matches(uri.AbsolutePath, @"/([a-zA-z0-9]+)");
            if (match.Count == 0)
            {
                Console.WriteLine("No controller avalible");
                return;
            }
            if (match[0].Groups[1].Value.ToLower() == "search")
            {
                if (match.Count < 1)
                {
                    Console.WriteLine("Search");
                }
                else if (match.Count == 2)
                {
                    Console.WriteLine("Search for movies or actors containing: " + match[1].Groups[1].Value);
                }
                else if (match.Count == 3)
                {
                    Console.WriteLine("Search for movies or actors's " + match[1].Groups[1].Value + " field contaning " +
                                      match[2].Groups[1].Value);
                }
                else
                {
                    Console.WriteLine("Search needs to have parameters");
                }
            }
        }
    }
}
