using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieAPI
{
    class RedefinedMyMovieApiBridge : AbstractMyMovieAPIBridge
    {
        List<MyMovieAPIDTO> Search(string searchInput)
        {
            throw new NotImplementedException();
        }
    }
}
