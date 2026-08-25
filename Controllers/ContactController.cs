using Microsoft.AspNetCore.Mvc;

namespace DotNet_Header_Footer.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
