using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class BooksController : Controller
    {
        // GET: BooksController
        public ActionResult Index()
        {
            return View();
        }

    }
}
