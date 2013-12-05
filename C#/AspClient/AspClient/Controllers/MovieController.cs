using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspClient.Models;

namespace AspClient.Controllers
{
    public class MovieController : Controller
    {
        //
        // GET: /Movie/

        public ActionResult Index()
        {
            return RedirectToAction( "Index", "Home" );
        }

        public ActionResult ViewInfo( MovieModel model )
        {
            return View( model );
        }
    }
}
