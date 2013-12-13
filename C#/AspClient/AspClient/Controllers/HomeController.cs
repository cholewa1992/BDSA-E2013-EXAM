using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspClient.Models;

namespace AspClient.Controllers
{
    /// <summary>
    /// @Author Jacob Cholewa (jbec@itu.dk)
    /// @Author Martin
    /// </summary>
    public class HomeController : Controller
    {
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
    }
}
