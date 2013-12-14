using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AspClient.Models;
using CommunicationFramework;
using Utils;

namespace AspClient.Controllers
{
    /// <summary>
    /// Controller for handling fetching of movie data
    /// @Author Jacob Cholewa (jbec@itu.dk)
    /// @Author Martin
    /// </summary>
    public class MovieController : Controller
    {
         
        /// <summary>
        /// ActionResult for ~/Movie/ redirecting clients back to frontpage (beacuse it is an invalid path)
        /// </summary>
        /// <returns>An actionResult redirecting back to frontpage</returns>
        public ActionResult Index()
        {
            return RedirectToAction( "Index", "Home" );
        }

        /// <summary>
        /// Action result for updating data through POST
        /// Invoked by ~/Movie/EditInfo/Id called with a post contaning the value of the title to change to
        /// </summary>
        /// <param name="id">The id of the movie you wish to change</param>
        /// <param name="value">The new title of the movie you want to chagne</param>
        /// <returns>An ActionResult containg a respons view</returns>
        [HttpPost]
        public ActionResult EditInfo(string id, string value)
        {
            var model = new ResponsModel();
            int intId;
            try
            {
                intId = int.Parse(id);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new
                {
                    ErrorCode = e.Message,
                    ErrorMsg = e.StackTrace
                });
            }

            if (intId < 0)
            {
                return RedirectToAction("Index", "Error", new
                {
                    ErrorCode = "Invalid id",
                    ErrorMsg = "The id was zero or below. Please change the value to a positive integer"
                });
            }

            if (value == null)
            {
                return RedirectToAction("Index", "Error", new
                {
                    ErrorCode = "Invalid input",
                    ErrorMsg = "The input value was null. Please change the value to a not null string"
                });
            }

            try
            {
                var handler = new CommunicationHandler(Protocols.Http);
                handler.Send(
                    "http://localhost:1337/Movie/",
                    Encoder.Encode("{\"id\": \""+ intId +"\",\"title\": \""+ value +"\"}"),
                    "PUT");
                byte[] receivedData = handler.Receive();
                model.Msg = Encoder.Decode(receivedData);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new
                {
                    ErrorCode = e.Message,
                    ErrorMsg = e.StackTrace
                });
            }


            return View( model );
        }

        /// <summary>
        /// ActionResult with a view of movieinfo ~/Movie/ViewInfo/Id
        /// If the Id parameter is wrong, the user is redirected to an error page
        /// </summary>
        /// <param name="id">The if of the movie to be shown</param>
        /// <returns>An ActionResult with the movie view</returns>
        [HttpGet]
        public ActionResult ViewInfo( string id )
        {
            int intId;
            try
            {
                intId = int.Parse(id);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new
                {
                    ErrorCode = e.Message,
                    ErrorMsg = e.StackTrace
                });
            }

            if (intId < 0)
            {
                return RedirectToAction("Index", "Error", new
                {
                    ErrorCode = "Invalid id",
                    ErrorMsg = "The id was zero or below. Please change the value to a positive integer"
                });
            }

            var model = new MovieModel();
            byte[] receivedData;
            try
            {
                var handler = new CommunicationHandler(Protocols.Http);
                handler.Send("http://localhost:1337/MovieData/" + intId, null, "GET");
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

            if( receivedData != null && receivedData.Count() != 0 )
            {
                var dictionary = JSonParser.GetValues( Encoder.Decode( receivedData ) );
                model.Id = intId;
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

                model.Plot = dictionary.ContainsKey("miPlot0Info") ? dictionary["miPlot0Info"] : "";
                dictionary.Remove("miPlot0Info");

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
                    model.cast.Add( new ActorModel
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
