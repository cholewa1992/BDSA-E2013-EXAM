using System.Web.Mvc;
using AspClient.Models;

namespace AspClient.Controllers
{
    /// <summary>
    /// Controller for handling exceptional usecases
    /// @Author Jacob Cholewa (jbec@itu.dk)
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Index file for ~/Error/
        /// Takes two parameters from GET and shows them on the page
        /// </summary>
        /// <param name="errorCode">The error code to display (eg. 404 not found)</param>
        /// <param name="errorMsg">The detailed error message (eg. a stacktrace) </param>
        /// <returns>An ActionResult contaning a ErrorModel view</returns>
        [HttpGet]
        public ActionResult Index(string errorCode = "", string errorMsg = "")
        {
            var errorModel = new ErrorModel {ErrorCode = errorCode, ErrorMsg = errorMsg};
            return View(errorModel);
        }
    }
}
