using Microsoft.AspNetCore.Mvc;

namespace DotNet_Header_Footer.Controllers
{
    public class HeaderController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
