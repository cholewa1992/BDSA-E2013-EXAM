using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspClient.Models;

namespace AspClient.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index( SearchModel model )
        {
            if( ModelState.IsValid )
                return RedirectToAction( "Search", "Search", model );

            return View( model );
        }

        /*public ActionResult Search( AspClient.Models.SearchModel model )
        {
            Console.WriteLine( model.SearchString );
            model.SearchString = "test2";

            return View( "~/Views/Home/Index.cshtml" );
        }*/

    }
}
