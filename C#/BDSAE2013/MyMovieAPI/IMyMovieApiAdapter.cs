using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkStorage;
using Storage;

namespace MyMovieAPI
{
    /// <author>
    /// Jonas Kastberg Hinrichsen (jkas@itu.dk)
    /// Michael Frikke Madsen (mifr@itu.dk)
    /// </author>
    public interface IMyMovieApiAdapter
    {
        List<Movies> MakeRequest(IStorageConnectionBridgeFacade storage, string searchWord, int limit = 3,
            int timeout = 5000);
    }
}
