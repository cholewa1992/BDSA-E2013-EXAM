using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                model.Year = dictionary.ContainsKey( "year" ) ? dictionary[ "year" ] : "";
                model.Kind = dictionary.ContainsKey( "kind" ) ? dictionary[ "kind" ] : "";
                model.SeasonNumber = dictionary.ContainsKey( "seasonNumber" ) ? dictionary[ "seasonNumber" ] : "";
                model.SeriesYear = dictionary.ContainsKey( "seriesYear" ) ? dictionary[ "seriesYear" ] : "";
                model.EpisodeNumber = dictionary.ContainsKey( "episodeNumber" ) ? dictionary[ "episodeNumber" ] : "";
                model.EpisodeOfId = dictionary.ContainsKey( "episodeOfId" ) ? dictionary[ "episodeOfId" ] : "";

                model.cast = new List<ActorModel>();
                for( int i = 0; dictionary.ContainsKey( "p" + i + "Id" ); i++ )
                {
                    model.cast.Add( new ActorModel() { Id = Int32.Parse( dictionary[ "p" + i + "Id" ] ), Name = dictionary[ "p" + i + "Name" ], Role = dictionary[ "p" + i + "Role" ], CharacterName = dictionary[ "p" + i + "CharacterName" ], SearchString = model.SearchString } );
                }
            }
            return View( model );
        }
    }
}
