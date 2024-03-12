using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YennieStore.Areas.Admin.Models;
using YennieStore.Models;

namespace YennieStore.Areas.Admin.Controllers
{
    
    [Authorize(Roles ="Admin")]
    [Area("Admin")]
    [Route("admin.html", Name = "AdminIndex")]

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
