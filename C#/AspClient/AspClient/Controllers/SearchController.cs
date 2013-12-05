using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspClient.Models;
using CommunicationFramework;

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

            byte[] receivedData = handler.Receive( 2000 );
            string convertedData = receivedData == null ? null : Utils.Encoder.Decode( receivedData );


            SearchResults newModel = new SearchResults();

            newModel.Results = new Dictionary<string, string>();
            newModel.Results.Add( "" + convertedData, "http://localhost:8485/Movie/Movie?Title=Die Hard" );
            newModel.SearchString = model.SearchString;

            return View( newModel );
        }
    }
}
