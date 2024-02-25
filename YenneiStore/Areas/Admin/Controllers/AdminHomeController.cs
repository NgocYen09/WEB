using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YenneiStore.Models;

namespace YenneiStore.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminHomeController : Controller
    {

        public IActionResult Index()
        {
           return View();
        }
    }
}
