using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AspClient.Models;
using CommunicationFramework;

namespace AspClient.Controllers
{
    /// <summary>
    /// @Author Jacob Cholewa (jbec@itu.dk)
    /// @Author Martin
    /// </summary>
    public class SearchController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction( "Index", "Home" );
        }

        [HttpGet]
        public ActionResult Search(string searchString)
        {
            byte[] receivedData;

            try
            {
                var handler = new CommunicationHandler(Protocols.Http);
                handler.Send("http://localhost:1337/Search/" + searchString, null, "GET");
                receivedData = handler.Receive();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new
                {
                    ErrorCode = e.Message,
                    ErrorMsg = e.StackTrace
                });
            }

            var newModel = new SearchResults
            {
                MovieResults = new Dictionary<int, MovieResult>(),
                PersonResults = new Dictionary<int, PersonResult>()
            };

            var jsonAttributes = Utils.JSonParser.GetValues( Utils.Encoder.Decode( receivedData ) );
            for( int i = 0; jsonAttributes.ContainsKey( "m" + i + "Id" ); i++ )
            {
                int movieId = Int32.Parse( jsonAttributes[ "m" + i + "Id" ] );
                newModel.MovieResults.Add( movieId, new MovieResult
                {
                    Id = movieId, 
                    Title = jsonAttributes[ "m" + i + "Title" ], 
                    Plot = jsonAttributes["m" + i + "Plot"], 
                } );
            }
            
            for( int i = 0; jsonAttributes.ContainsKey( "p" + i + "Id" ); i++ )
            {
                int personId = Int32.Parse( jsonAttributes[ "p" + i + "Id" ] );
                newModel.PersonResults.Add( personId, new PersonResult
                {
                    Id = personId, 
                    Name = jsonAttributes[ "p" + i + "Name" ], 
                    Biography = jsonAttributes["p" + i + "Biography"]
                } );
            }
            return View( newModel );
        }
    }
}
