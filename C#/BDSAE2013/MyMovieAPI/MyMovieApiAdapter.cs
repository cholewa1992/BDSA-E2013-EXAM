﻿using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkStorage;
using Newtonsoft.Json;
using Storage;

namespace MyMovieAPI
{
    public class MyMovieApiAdapter
    {
        /// <summary>
        /// Requests moviedata from MyMovieApi
        /// </summary>
        /// <param name="storage">Storage implementation</param>
        /// <param name="searchWord">Word to search for</param>
        /// <param name="limit">Limit of how many search results is wanted default = 3</param>
        /// <param name="timeout">How many millisecounds to wait for respons default = 5000ms</param>
        /// <returns>A list of movies fetched from MyMovieApi</returns>
        public static List<Movies> MakeRequest(IStorageConnectionBridgeFacade storage, string searchWord, int limit = 3,
            int timeout = 5000)
        {
            try
            {
                return TransformToMovies(storage, MyMovieApiRequest.ParseJson(
                    MyMovieApiRequest.MakeRequest(searchWord, limit, timeout)));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Could not get movies from MyMovieApi",e);
            }
        } 

        /// <summary>
        /// Transforming MyMovieAPIDTO objects to Movies objects and adds the to the given database
        /// </summary>
        /// <param name="sb">Active storage to add transformed objects to</param>
        /// <param name="movies">The movies to transform</param>
        /// <returns>A list of movie objects</returns>
        internal static List<Movies> TransformToMovies(IStorageConnectionBridgeFacade sb, MyMovieAPIDTO[] movies)
        {
            try
            {
                var newMovies = new List<Movies>();
                foreach (var m in movies)
                {
                    var newM = new Movies
                    {
                        Kind = m.type,
                        Title = m.title,
                        Year = m.year
                    };
                    sb.Add(newM);
                    newMovies.Add(newM);

                    foreach (var p in m.actors)
                    {
                        int id;
                        if (sb.Get<People>().Any(t => t.Name == p))
                        {
                            id = sb.Get<People>().Where(t => t.Name == p).Select(t => t.Id).First();
                        }
                        else
                        {
                            var newP = new People
                            {
                                Name = p
                            };
                            sb.Add(newP);
                            id = newP.Id;
                        }

                        if (id == 0) throw new InvalidOperationException("Id was 0");
                        sb.Add(new Participate
                        {
                            Role = "actor",
                            Movie_Id = newM.Id,
                            Person_Id = id
                        });
                    }
                }
                return newMovies;
            }
            catch(Exception e)
            {
                throw new JsonException("Could not parse the respons from mymovie api", e);
            }
        }
    }
}
