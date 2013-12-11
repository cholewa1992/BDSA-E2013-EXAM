using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspClient.Models;
using CommunicationFramework;

namespace AspClient.Controllers
{
    public class PersonController : Controller
    {
        //
        // GET: /Person/

        public ActionResult Index()
        {
            return RedirectToAction( "Index", "Home" );
        }

        public ActionResult ViewInfo( PersonModel model )
        {
            var handler = new CommunicationHandler( Protocols.HTTP );
            handler.Send( "http://localhost:1337/PersonData/" + model.Id, null, "GET" );

            byte[] receivedData = handler.Receive( 10000 );

            if( receivedData != null && receivedData.Count() != 0 )
            {
                var dictionary = Utils.JSonParser.GetValues( Utils.Encoder.Decode( receivedData ) );
                model.Name = dictionary.ContainsKey( "name" ) ? dictionary[ "name" ] : "";
                model.Gender = dictionary.ContainsKey( "gender" ) ? dictionary[ "gender" ] : "";
                model.BirthDate = dictionary.ContainsKey( "piBirthDate0Info" ) ? dictionary[ "piBirthDate0Info" ] : "";

                model.starring = new List<PersonMovieModel>();
                for( int i = 0; dictionary.ContainsKey( "m" + i + "Id" ); i++ )
                {
                    model.starring.Add( new PersonMovieModel()
                    {
                        Id = Int32.Parse( dictionary[ "m" + i + "Id" ] ), 
                        Title = dictionary.ContainsKey( "m" + i + "Title" ) ? dictionary[ "m" + i + "Title" ] : "", 
                        Kind = dictionary.ContainsKey( "m" + i + "Kind" ) ? dictionary[ "m" + i + "Kind" ] : "",
                        Year = dictionary.ContainsKey( "m" + i + "Year" ) ? dictionary[ "m" + i + "Year" ] : "",
                        CharacterName = dictionary.ContainsKey( "m" + i + "CharacterName" ) ? dictionary[ "m" + i + "CharacterName" ] : "",
                        Role = dictionary.ContainsKey( "m" + i + "Role" ) ? dictionary[ "m" + i + "Role" ] : "",
                        Note = dictionary.ContainsKey( "m" + i + "Note" ) ? dictionary[ "m" + i + "Note" ] : "",
                        NumberOrder = dictionary.ContainsKey( "m" + i + "NrOrder" ) && dictionary[ "m" + i + "NrOrder" ] != "" ? Int32.Parse( dictionary[ "m" + i + "NrOrder" ] ) : 0
                    } );
                }
            }

            return View( model );
        }
    }
}