﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AspClient.Models;
using CommunicationFramework;

namespace AspClient.Controllers
{
    /// <summary>
    /// @Author Jacob Cholewa (jbec@itu.dk)
    /// @Author Martin
    /// </summary>
    public class PersonController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction( "Index", "Home" );
        }

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


            var model = new PersonModel();
            byte[] receivedData;
            try
            {
                var handler = new CommunicationHandler(Protocols.Http);
                handler.Send("http://localhost:1337/PersonData/" + intId, null, "GET");
                receivedData = handler.Receive();
            }
            catch(Exception e)
            {
                return RedirectToAction("Index", "Error", new
                {
                    ErrorCode = e.Message,
                    ErrorMsg = e.StackTrace
                });
            }

            if( receivedData != null && receivedData.Count() != 0 )
            {
                var dictionary = Utils.JSonParser.GetValues( Utils.Encoder.Decode( receivedData ) );
                model.Name = dictionary.ContainsKey( "name" ) ? dictionary[ "name" ] : "";
                model.Gender = dictionary.ContainsKey( "gender" ) ? dictionary[ "gender" ] : "";
                model.BirthDate = dictionary.ContainsKey( "piBirthDate0Info" ) ? dictionary[ "piBirthDate0Info" ] : "";


                model.Bio = dictionary.ContainsKey("piMiniBiography0Info") ? dictionary["piMiniBiography0Info"] : "";
                dictionary.Remove("piMiniBiography0Info");

                model.Data = new Dictionary<string, IList<string>>();

                foreach (var kvp in dictionary)
                {
                    //Do regex
                    var match = Regex.Match(kvp.Key, @"(?![pi])[a-zA-Z]+(?=[0-9]+Info)");

                    if (!match.Success) continue;

                    string key = match.Value;

                    if (!model.Data.ContainsKey(key))
                    {
                        model.Data[key] = new List<string>();
                    }
                    model.Data[key].Add(kvp.Value);
                }

                model.starring = new List<PersonMovieModel>();
                for( int i = 0; dictionary.ContainsKey( "m" + i + "Id" ); i++ )
                {
                    model.starring.Add( new PersonMovieModel
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