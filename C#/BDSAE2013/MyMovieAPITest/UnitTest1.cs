using System.Collections.Generic;
using System.Linq;
using EntityFrameworkStorage;
using InMemoryStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMovieAPI;
using Newtonsoft.Json;
using Storage;

namespace MyMovieAPITest
{
    [TestClass]
    public class MyMovieAPITest
    {
        private readonly List<MyMovieAPIDTO> _testDto = new List<MyMovieAPIDTO>
            {
                new MyMovieAPIDTO
                {
                    year = 2013,
                    title = "Die Hard XI",
                    type = "M",
                    actors = new[] {"Willis, Bruce", "Cholewa, Jacob"}
                }
            };


        [TestMethod]
        public void ParseJsonTest()
        {
            //Creating some sample JSON for the parser
            var json = JsonConvert.SerializeObject(_testDto);

            //Deserializing the JSON
            var movies = MyMovieApiRequest.ParseJson(json);

            //Using In-Memory storage to test the movie api
            using(var s = new StorageConnectionBridgeFacade(new InMemoryStorageConnectionFactory()))
            {
                //Transforming the MyMovieAPIDTO objects to Movies objects
                MyMovieApiAdapter.TransformToMovies(s, movies);

                //Asserting that the data was transformed corretly and that it was added to the database
                Assert.IsTrue(s.Get<Movies>().Any(t =>
                    t.Title == "Die Hard XI" &&
                    t.Year == 2013 &&
                    t.Kind == "M"
                    ));

                //Checking that Bruce Willis was added to the database
                Assert.IsTrue(s.Get<People>().Any(t => t.Name == "Willis, Bruce"));
                //Checking that Jacob Cholewa (me, because i'm awesome enough to be in die hard) was added to the database
                Assert.IsTrue(s.Get<People>().Any(t => t.Name == "Cholewa, Jacob"));
                //Checking that the two actors was associated to the movie
                Assert.IsTrue(s.Get<Participate>().Count() == 2);
            }
        }

        [TestMethod]
        public void AdapterTest()
        {
            //JSON serializing the object
            //Exsample of how the JSON from MyMovieApi looks like
            var json = JsonConvert.SerializeObject(_testDto);

            //Deserializing the JSON
            var movies = MyMovieApiRequest.ParseJson(json);

            //Asserting that the Deserialized data is correct
            Assert.AreEqual("Die Hard XI", movies[0].title);
            Assert.AreEqual(2013, movies[0].year);
            Assert.AreEqual("M", movies[0].type);
            Assert.AreEqual("Willis, Bruce", movies[0].actors[0]);
            Assert.AreEqual("Cholewa, Jacob", movies[0].actors[1]);
        }
    }
}
