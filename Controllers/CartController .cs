using Microsoft.AspNetCore.Mvc;

namespace DotNet_Header_Footer.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
