using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspClient.Models;
using CommunicationFramework;
using Newtonsoft.Json;

namespace AspClient.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index()
        {
            return RedirectToAction( "Index", "Home" );
        }

        public ActionResult Search( SearchModel model )
        {
            var handler = new CommunicationHandler( Protocols.HTTP );
            handler.Send( "http://localhost:1337/Search/" + model.SearchString, null, "GET" );

            byte[] receivedData = handler.Receive( 10000 );

            SearchResults newModel = new SearchResults();

            newModel.Results = new Dictionary<int, MovieResult>();
            newModel.SearchString = model.SearchString;

            var jsonAttributes = Utils.JSonParser.GetValues( Utils.Encoder.Decode( receivedData ) );
            for( int i = 0; jsonAttributes.ContainsKey( "m" + i + "Id" ); i++ )
            {
                //newModel.Results.Add( jsonAttributes[ "m" + i + "Title" ], "http://localhost:8485/Movie/ViewInfo?Id=" + jsonAttributes[ "m" + i + "Id" ] + "&SearchString=" + model.SearchString );
                int movieId = Int32.Parse( jsonAttributes[ "m" + i + "Id" ] );
                newModel.Results.Add( movieId, new MovieResult() { Id = movieId, Title = jsonAttributes[ "m" + i + "Title" ], Url = "http://localhost:8485/Movie/ViewInfo?Id=" + movieId + "&SearchString=" + model.SearchString } );
            }
            
            for( int i = 0; jsonAttributes.ContainsKey( "p" + i + "Id" ); i++ )
            {
                int personId = Int32.Parse( jsonAttributes[ "p" + i + "Id" ] );
                newModel.Results.Add( personId, new MovieResult() { Id = personId, Title = jsonAttributes[ "p" + i + "Name" ], Url = "http://localhost:8485/Person/ViewInfo?Id=" + personId + "&SearchString=" + model.SearchString } );
            }
            return View( newModel );
        }
    }
}
