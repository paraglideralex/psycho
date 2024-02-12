using Microsoft.AspNetCore.Mvc;

namespace Psycho;

public class HomeController : Controller
{
    // GET: Home
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Index(string name)
    {
        ViewBag.Message = string.Format("Hello {0}.\\nCurrent Date and Time: {1}", name, DateTime.Now.ToString());
        return View();
    }
}
