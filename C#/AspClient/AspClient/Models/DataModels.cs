using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspClient.Models
{
    public interface DataModel
    {
        string SearchString{ get; set; }
        int Id{ get; set; }
    }

    public class MovieModel : DataModel
    {
        public string SearchString{ get; set;  }

        public int Id{ get; set; }
        public string Title{ get; set; }
    }
}