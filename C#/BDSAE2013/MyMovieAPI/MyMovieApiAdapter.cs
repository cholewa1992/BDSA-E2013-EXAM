using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkStorage;
using Newtonsoft.Json;
using Storage;

namespace MyMovieAPI
{
    /// <author>
    /// Jacob Cholewa (jbec@itu.dk)
    /// </author>
    public class MyMovieApiAdapter : IMyMovieApiAdapter
    {
        /// <summary>
        /// Requests moviedata from MyMovieApi
        /// </summary>
        /// <param name="storage">Storage implementation</param>
        /// <param name="searchWord">Word to search for</param>
        /// <param name="limit">Limit of how many search results is wanted default = 3</param>
        /// <param name="timeout">How many millisecounds to wait for respons default = 5000ms</param>
        /// <returns>A list of movies fetched from MyMovieApi</returns>
        public List<Movies> MakeRequest(IStorageConnectionBridgeFacade storage, string searchWord, int limit = 3,
            int timeout = 5000)
        {
            try
            {
                return TransformToMovies(storage, MyMovieApiRequest.ParseJson(
                    MyMovieApiRequest.MakeRequest(searchWord, limit, timeout)));
            }
            catch (Exception e)
            {
                return new List<Movies>();
            }
        } 

        /// <summary>
        /// Transforming MyMovieAPIDTO objects to Movies objects and adds the to the given database
        /// </summary>
        /// <param name="storageConnectionBridgeFacade">Active storage to add transformed objects to</param>
        /// <param name="movies">The movies to transform</param>
        /// <returns>A list of movie objects</returns>
        public List<Movies> TransformToMovies(IStorageConnectionBridgeFacade storageConnectionBridgeFacade, MyMovieAPIDTO[] movies)
        {
            try
            {
                var newMovies = new List<Movies>();
                foreach (var movie in movies)
                {
                    var newM = new Movies
                    {
                        Kind = movie.type,
                        Title = movie.title,
                        Year = movie.year
                    };

                    if (storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Title == newM.Title && t.Year == newM.Year))
                    {
                        newMovies.Add(storageConnectionBridgeFacade.Get<Movies>().Single(t => t.Title == newM.Title && t.Year == newM.Year));
                        continue;
                    }

                    newMovies.Add(newM);
                    storageConnectionBridgeFacade.Add(newM);

                    foreach (var personName in movie.actors)
                    {
                        int id;
                        if (storageConnectionBridgeFacade.Get<People>().Any(t => t.Name == personName))
                        {
                            id = storageConnectionBridgeFacade.Get<People>().Where(t => t.Name == personName).Select(t => t.Id).First();
                        }
                        else
                        {
                            var newPerson = new People
                            {
                                Name = personName
                            };
                            storageConnectionBridgeFacade.Add(newPerson);
                            id = newPerson.Id;
                        }

                        if (id == 0) throw new InvalidOperationException("Id was 0");
                        storageConnectionBridgeFacade.Add(new Participate
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
