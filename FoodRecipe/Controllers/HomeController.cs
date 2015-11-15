using System.Web.Mvc;

namespace FoodRecipe.Controllers
{
    public class HomeController
        : Controller
    {
        public ActionResult Browse()
        {
            return View();
        }

        public ActionResult About()
            => View();
    }
}