using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using YennieStore.Models;

namespace YennieStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminReviewsController : Controller
    {
        private readonly dbMarketsContext _context;

        public AdminReviewsController(dbMarketsContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index( int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsReviews = _context.Reviews
                .Include(f => f.Customer)
                .Include(f => f.Product)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .Select(f => new Review
                {
                    CustomerName = f.Customer.FullName,
                    ReviewId = f.ReviewId,
                    Email = f.Email,
                    Content = f.Content,
                    Rate = f.Rate,
                    CreatedDate = f.CreatedDate,
                    Status = f.Status,
                    ProductName = f.Product.ProductName
                });
            PagedList<Review> models = new PagedList<Review>(lsReviews, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }


    }
}
