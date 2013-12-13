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
    /// @Author Jacob Cholewa (jbec@itu.dk)
    /// @Author Martin
    /// </summary>
    public class MovieController : Controller
    {
        //
        // GET: /Movie/

        public ActionResult Index()
        {
            return RedirectToAction( "Index", "Home" );
        }

        [HttpGet]
        public ActionResult EditInfo(string id, string value)
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

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new
                {
                    ErrorCode = e.Message,
                    ErrorMsg = e.StackTrace
                });
            }

            return RedirectToAction("ViewInfo", "Movie", new
            {
                Id = intId
            });

            //return ViewInfo(id);
        }

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
                var dictionary = Utils.JSonParser.GetValues( Encoder.Decode( receivedData ) );
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
