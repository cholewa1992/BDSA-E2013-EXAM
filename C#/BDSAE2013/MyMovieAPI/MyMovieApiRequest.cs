using System;
using CommunicationFramework;
using Newtonsoft.Json;

namespace MyMovieAPI
{
    /// <author>
    /// Jacob Cholewa (jbec@itu.dk)
    /// </author>
    public class MyMovieApiRequest
    {
        /// <summary>
        /// Search the MyMovieAPI for movies. Returns the movies in json format
        /// </summary>
        /// <param name="searchWord"> The search word to match </param>
        /// <param name="limit"> The amount of movies to find </param>
        /// <param name="timeout"> The timeout in miliseconds </param>
        /// <returns> The found movies in json format </returns>
        public static string MakeRequest(string searchWord, int limit = 3, int timeout = 5000)
        {
            try
            {
                var comHandler = new CommunicationHandler(Protocols.Http);
                var restAddress = "http://mymovieapi.com/?title=" + searchWord + "&limit=" + limit;

                comHandler.Send(restAddress, new byte[0], "GET");
                var json = Utils.Encoder.Decode(comHandler.Receive(timeout));

                return json;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error connecting to MyMovieApi",e);
            }
        }

        /// <summary>
        /// Desirializing JSON to MyMovieAPIDTOs
        /// </summary>
        /// <param name="json">The json string</param>
        /// <returns>An array of MyMovieAPIDTOs derived from the JSON</returns>
        public static MyMovieAPIDTO[] ParseJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<MyMovieAPIDTO[]>(json);
            }
            catch (Exception e)
            {
                throw new JsonException("Could not deserialize the json respons", e);
            }
        }
    }
}
