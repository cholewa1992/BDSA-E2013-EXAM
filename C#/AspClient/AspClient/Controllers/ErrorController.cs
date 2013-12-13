using System.Web.Mvc;
using AspClient.Models;

namespace AspClient.Controllers
{
    /// <summary>
    /// @Author Jacob Cholewa (jbec@itu.dk)
    /// </summary>
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult Index(string errorCode = "", string errorMsg = "")
        {
            var errorModel = new ErrorModel {ErrorCode = errorCode, ErrorMsg = errorMsg};
            return View(errorModel);
        }
    }
}
