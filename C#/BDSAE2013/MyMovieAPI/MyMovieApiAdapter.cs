using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkStorage;
using Newtonsoft.Json;
using Storage;

namespace MyMovieAPI
{
    public class MyMovieApiAdapter
    {
        public static List<Movies> MakeRequest(IStorageConnectionBridgeFacade sb, string searchWord, int limit = 3,
            int timeout = 5000)
        {
            return TransformToMovies(sb, MyMovieApiRequest.ParseJson(
                MyMovieApiRequest.MakeRequest(searchWord, limit, timeout)));
        } 

        //Implemented to make code testable
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
