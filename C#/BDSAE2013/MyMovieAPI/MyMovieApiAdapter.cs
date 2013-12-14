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
                //Initialize a new list of movies to return
                var newMovies = new List<Movies>();

                //Iterate through all movies from the MyMovieAPIDTO list
                foreach (var movie in movies)
                {
                    //Initialize the new movie converted from MyMovieAPI to the program main database structure
                    var newMovie = new Movies
                    {
                        Kind = movie.type,
                        Title = movie.title,
                        Year = movie.year
                    };

                    //If the movie already exists in the database we add it to the list of results, without adding it to the database
                    if (storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Title == newMovie.Title && t.Year == newMovie.Year))
                    {
                        newMovies.Add(storageConnectionBridgeFacade.Get<Movies>().Single(t => t.Title == newMovie.Title && t.Year == newMovie.Year));
                        continue;
                    }

                    newMovies.Add(newMovie);
                    storageConnectionBridgeFacade.Add(newMovie);

                    //Iterate through all persons in the movie
                    foreach (var personName in movie.actors)
                    {
                        //try to find the person in our own database
                        People person = storageConnectionBridgeFacade.Get<People>().FirstOrDefault(t => t.Name == personName);

                        int id = 0;

                        //If the person exists we simply use our own id
                        if (person != null)
                        {
                            id = person.Id;
                        }
                        else
                        {
                            //If he does not we initialize a new one and add it to the database, and use the new id
                            var newPerson = new People
                            {
                                Name = personName
                            };
                            storageConnectionBridgeFacade.Add(newPerson);
                            id = newPerson.Id;
                        }

                        if (id == 0) 
                            throw new InvalidOperationException("Id was 0");
                        
                        //Add a parcitipate link between the actor and the movie
                        storageConnectionBridgeFacade.Add(new Participate
                        {
                            Role = "actor",
                            Movie_Id = newMovie.Id,
                            Person_Id = id
                        });
                    }
                }
                return newMovies;
            }
            catch(Exception e)
            {
                throw new JsonException("Could not parse the response from mymovie api", e);
            }
        }
    }
}
