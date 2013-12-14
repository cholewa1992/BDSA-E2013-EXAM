using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AspClient.Models;
using CommunicationFramework;

namespace AspClient.Controllers
{
    /// <summary>
    /// Controller for handling search requests
    /// @Author Jacob Cholewa (jbec@itu.dk)
    /// @Author Martin
    /// </summary>
    public class SearchController : Controller
    {
        /// <summary>
        /// Action result for Search index (~/Search/)
        /// </summary>
        /// <returns>Returns an search view with no results</returns>
        public ActionResult Index()
        {
            return Search(null);
        }

        /// <summary>
        /// Action result for getting and showing search results from Fake IMDb
        /// </summary>
        /// <param name="searchString">The string to search the database for</param>
        /// <returns>An ActionResult containg the results from the search</returns>
        [HttpGet]
        public ActionResult Search(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return View();
            }
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
