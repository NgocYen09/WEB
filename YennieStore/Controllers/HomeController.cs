using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YennieStore.Models;
using YennieStore.ModelViews;
using System.Diagnostics;
using YennieStore.Models;

namespace YennieStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbMarketsContext _context;

        public HomeController(ILogger<HomeController> logger, dbMarketsContext context)
        {
            _logger = logger;
            _context = context;
        }
        #region trang ch?
        public IActionResult Index()
        {
            
            return View();
        }
        #endregion

        #region trang liên hệ

        


        public IActionResult Contact()
        {
            return View();
        }
        #endregion

        [Route("gioi-thieu.html", Name = "About")]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool IsCustomerLoggedIn(HttpContext context)
        {
            return context.Session.GetString("CustomerId") != null;
        }
    }
}
