using System.Web.Mvc;
namespace AspClient.Controllers
{
    /// <summary>
    /// Default controller. This controller is the main controller and is invoved when http://host:port/ is invoked
    /// @Author Jacob Cholewa (jbec@itu.dk)
    /// @Author Martin
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Index page for http://hostname:port/
        /// </summary>
        /// <returns>Returns an action result with the default index page</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
