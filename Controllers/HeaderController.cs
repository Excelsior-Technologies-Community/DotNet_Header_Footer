using Microsoft.AspNetCore.Mvc;

namespace DotNet_Header_Footer.Controllers
{
    public class HeaderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
