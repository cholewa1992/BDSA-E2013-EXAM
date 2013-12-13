using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage;
using WebServer;
using EntityFrameworkStorage;
using InMemoryStorage;
using System.Linq;
using CommunicationFramework;
using Utils;

namespace WebServerToStorageIntegrationTest
{
    /// <summary>
    /// Tests the integration between the web server and the storage.
    /// Only one sub controller is tested, since all other individual logic is tested during unit testing.
    /// The tested controller is the movie controller
    /// </summary>
    [TestClass]
    public class WebServerToStorageIntegrationTest
    {
        [TestInitialize]
        public void Init()
        {
            InMemoryStorageSet<Movies>.Clear();
        }

        [TestMethod]
        public void Test_WebServerToStorageIntegrationTest_AddEntity()
        {
            StorageConnectionBridgeFacade storageConnectionBridgeFacade = new StorageConnectionBridgeFacade(new InMemoryStorageConnectionFactory());

            using (RequestDelegator requestDelegator = new RequestDelegator(storageConnectionBridgeFacade))
            {
                Request request = new Request() { 
                    Method = "POST https://www.google.dk/Movie", 
                    Data = Encoder.Encode(JSonParser.Parse("title", "Die Hard 2", "year", "2000", "kind", "TV Series")) };
               
                Assert.IsFalse(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Title == "Die Hard 2"));
                
                request = requestDelegator.ProcessRequest(request);

                Assert.AreEqual(Request.StatusCode.Ok, request.ResponseStatusCode);
                Assert.IsTrue(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Title == "Die Hard 2"));
            }
        }

        [TestMethod]
        public void Test_WebServerToStorageIntegrationTest_UpdateExistingEntity()
        {
            Movies movieToAdd = new Movies() { Title = "Die Hard", Year = 1998, Kind = "TV Series" };
            
            StorageConnectionBridgeFacade storageConnectionBridgeFacade = new StorageConnectionBridgeFacade(new InMemoryStorageConnectionFactory());
            storageConnectionBridgeFacade.Add<Movies>(movieToAdd);
            
            using (RequestDelegator requestDelegator = new RequestDelegator(storageConnectionBridgeFacade))
            {
                Request request = new Request()
                {
                    Method = "PUT https://www.google.dk/Movie",
                    Data = Encoder.Encode(JSonParser.Parse("id", "" + movieToAdd.Id, "title", "Die Hard 2", "year", "2000", "kind", "Movie"))
                };

                Assert.IsTrue(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Id == movieToAdd.Id && t.Title == "Die Hard"));

                request = requestDelegator.ProcessRequest(request);

                Assert.AreEqual(Request.StatusCode.Ok, request.ResponseStatusCode);
                Assert.IsFalse(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Id == movieToAdd.Id && t.Title == "Die Hard"));
                Assert.IsTrue(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Id == movieToAdd.Id && t.Title == "Die Hard 2"));
            }
        }

        [TestMethod]
        public void Test_WebServerToStorageIntegrationTest_UpdateNonExistingEntity()
        {
            Movies movieToAdd = new Movies() { Title = "Die Hard", Year = 1998, Kind = "TV Series" };

            StorageConnectionBridgeFacade storageConnectionBridgeFacade = new StorageConnectionBridgeFacade(new InMemoryStorageConnectionFactory());
            
            using (RequestDelegator requestDelegator = new RequestDelegator(storageConnectionBridgeFacade))
            {
                Request request = new Request()
                {
                    Method = "PUT https://www.google.dk/Movie",
                    Data = Encoder.Encode(JSonParser.Parse("id", "1337", "title", "Die Hard 2", "year", "2000", "kind", "Movie"))
                };

                Assert.IsFalse(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Id == movieToAdd.Id && t.Title == "Die Hard"));

                request = requestDelegator.ProcessRequest(request);

                Assert.AreEqual(Request.StatusCode.NotFound, request.ResponseStatusCode);

                Assert.IsFalse(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Id == movieToAdd.Id && t.Title == "Die Hard 2"));
            }
        }

        [TestMethod]
        public void Test_WebServerToStorageIntegrationTest_DeleteExistingEntity()
        {
            Movies movieToAdd = new Movies() { Title = "Die Hard", Year = 1998, Kind = "TV Series" };

            StorageConnectionBridgeFacade storageConnectionBridgeFacade = new StorageConnectionBridgeFacade(new InMemoryStorageConnectionFactory());
            storageConnectionBridgeFacade.Add<Movies>(movieToAdd);
            
            using (RequestDelegator requestDelegator = new RequestDelegator(storageConnectionBridgeFacade))
            {
                Request request = new Request()
                {
                    Method = "DELETE https://www.google.dk/Movie",
                    Data = Encoder.Encode(JSonParser.Parse("id", ""+movieToAdd.Id))
                };

                Assert.IsTrue(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Id == movieToAdd.Id && t.Title == "Die Hard"));

                request = requestDelegator.ProcessRequest(request);

                Assert.AreEqual(Request.StatusCode.Ok, request.ResponseStatusCode);

                Assert.IsFalse(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Id == movieToAdd.Id && t.Title == "Die Hard"));
            }
        }

        [TestMethod]
        public void Test_WebServerToStorageIntegrationTest_DeleteNonExistingEntity()
        {
            Movies movieToAdd = new Movies() { Title = "Die Hard", Year = 1998, Kind = "TV Series" };

            StorageConnectionBridgeFacade storageConnectionBridgeFacade = new StorageConnectionBridgeFacade(new InMemoryStorageConnectionFactory());

            using (RequestDelegator requestDelegator = new RequestDelegator(storageConnectionBridgeFacade))
            {
                Request request = new Request()
                {
                    Method = "DELETE https://www.google.dk/Movie",
                    Data = Encoder.Encode(JSonParser.Parse("id", "1337"))
                };

                Assert.IsFalse(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Id == movieToAdd.Id && t.Title == "Die Hard"));

                request = requestDelegator.ProcessRequest(request);

                Assert.AreEqual(Request.StatusCode.NotFound, request.ResponseStatusCode);

                Assert.IsFalse(storageConnectionBridgeFacade.Get<Movies>().Any(t => t.Id == movieToAdd.Id && t.Title == "Die Hard"));
            }
        }
    }
}
