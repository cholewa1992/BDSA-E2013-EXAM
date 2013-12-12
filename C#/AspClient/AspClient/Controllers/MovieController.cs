using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Optimization;
using AspClient.Models;
using CommunicationFramework;

namespace AspClient.Controllers
{
    public class MovieController : Controller
    {
        //
        // GET: /Movie/

        public ActionResult Index()
        {
            return RedirectToAction( "Index", "Home" );
        }

        public ActionResult ViewInfo( MovieModel model )
        {
            var handler = new CommunicationHandler( Protocols.HTTP );
            handler.Send( "http://localhost:1337/MovieData/" + model.Id, null, "GET" );

            byte[] receivedData = handler.Receive( 10000 );
            if( receivedData != null && receivedData.Count() != 0 )
            {
                var dictionary = Utils.JSonParser.GetValues( Utils.Encoder.Decode( receivedData ) );
                model.Title = dictionary.ContainsKey( "title" ) ? dictionary[ "title" ] : "";
                dictionary.Remove("title");
                model.Year = dictionary.ContainsKey( "year" ) ? dictionary[ "year" ] : "";
                dictionary.Remove("year");
                model.Kind = dictionary.ContainsKey( "kind" ) ? dictionary[ "kind" ] : "";
                dictionary.Remove("kind");
                model.SeasonNumber = dictionary.ContainsKey( "seasonNumber" ) ? dictionary[ "seasonNumber" ] : "";
                dictionary.Remove("seasonNumber");
                model.SeriesYear = dictionary.ContainsKey( "seriesYear" ) ? dictionary[ "seriesYear" ] : "";
                dictionary.Remove("seriesYear");
                model.EpisodeNumber = dictionary.ContainsKey( "episodeNumber" ) ? dictionary[ "episodeNumber" ] : "";
                dictionary.Remove("episodeNumber");
                model.EpisodeOfId = dictionary.ContainsKey("episodeOfId") ? dictionary["episodeOfId"] : "";
                dictionary.Remove("episodeOfId");

                model.data = new Dictionary<string, IList<string>>();

                foreach (var kvp in dictionary)
                {
                    //Do regex
                    var match = Regex.Match(kvp.Key, @"(?![mi])[a-zA-Z]+(?=[0-9]+Info)");

                    if (!match.Success) continue;

                    string key = match.Value;




                    if (!model.data.ContainsKey(key))
                    {
                        model.data[key] = new List<string>();
                    }
                    model.data[key].Add(kvp.Value);
                }
                



                model.cast = new List<ActorModel>();


                for( int i = 0; dictionary.ContainsKey( "p" + i + "Id" ); i++ )
                {
                    model.cast.Add( new ActorModel()
                    {
                        Id = Int32.Parse( dictionary[ "p" + i + "Id" ] ), 
                        Name = dictionary[ "p" + i + "Name" ], 
                        Role = dictionary[ "p" + i + "Role" ], 
                        CharacterName = dictionary[ "p" + i + "CharacterName" ], 
                        SearchString = model.SearchString
                    } );
                }
            }
            return View( model );
        }
    }
}
