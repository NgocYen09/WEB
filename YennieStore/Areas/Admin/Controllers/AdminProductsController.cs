using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using YennieStore.Extension;
using YennieStore.Models;

namespace YennieStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminProductsController : Controller
    {
        private readonly dbMarketsContext _context;

        public AdminProductsController(dbMarketsContext context)
        {
            _context = context;
        }
        #region danh sách sản phảm
        public IActionResult Index(int page = 1, int CatID = 0)
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                TempData["ShowSuccessMessage"] = true;
            }
            var pageNumber = page;
            var pageSize = 20;

            List<Product> lsProducts = new List<Product>();
            if (CatID != 0)
            {
                lsProducts = _context.Products.AsNoTracking().Where(x => x.CatId == CatID)
                    .Include(x => x.Cat).Include(x => x.Brand).OrderBy(x => x.ProductId).ToList();
            }
            else
            {
                lsProducts = _context.Products
                    .AsNoTracking()
                    .Include(x => x.Cat)
                    .Include(x => x.Brand)
                    .OrderBy(x => x.ProductId).ToList();
            }
            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);
            ViewBag.CurrentCateID = CatID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            ViewData["MauSac"] = new SelectList(_context.Colors, "IdColor", "NameColor");
            ViewData["ThuongHieu"] = new SelectList(_context.Brands, "IdBrand", "NameBrand");
            return View(models);
        }
        public IActionResult Filtter(int CatID = 0)
        {
            var url = $"/Admin/AdminProducts?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = " success", redirecUrl = url });

        }
        public IActionResult FiltterBrand(int IdBrand = 0)
        {
            var url = $"/Admin/AdminProducts?IdBrand={IdBrand}";
            if (IdBrand == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = " success", redirecUrl = url });
        }
        #endregion
        #region chi tiết sản phẩm
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products
                .Include(p => p.Cat)
                .Include(p => p.Color)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            var color = await _context.Colors
                .FirstOrDefaultAsync(c => c.IdColor == product.IdColor);
            //kiểm tra nếu không tìm thấy màu sắc
            if (color == null)
            {
                return NotFound();
            }
            //gán giá trị NameColor vào thuộc tính của product
            ViewBag.NameColor = color.NameColor;
            return View(product);
        }

        #endregion
        #region thêm sp
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName");
            ViewData["MauSac"] = new SelectList(_context.Colors, "IdColor", "NameColor");
            ViewData["ThuongHieu"] = new SelectList(_context.Brands, "IdBrand", "NameBrand");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,DiscountPercentage,Thumb,Imei,IdBrand,DateCreated,DateModified,BestSellers,HomeFlag,Active,IdColor,Tags,Title,Alias,UnitsInStock")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                product.ProductName = Utilities.ToTitleCase(product.ProductName);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(product.ProductName) + extension;
                    product.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                }
                if (string.IsNullOrEmpty(product.Thumb))
                    product.Thumb = "default.jpg";
                product.Alias = Utilities.SEOUrl(product.ProductName);
                product.DateModified = DateTime.Now;
                product.DateCreated = DateTime.Now;

                // Calculate the discount amount based on the discount percentage
                double discountAmount = (double)(product.Price * (100 - product.DiscountPercentage) / 100);
                product.Discount = (int?)Math.Round(discountAmount, 2); // Làm tròn đến 2 chữ số thập phân

                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            ViewData["MauSac"] = new SelectList(_context.Colors, "IdColor", "NameColor", product.IdColor);
            ViewData["ThuongHieu"] = new SelectList(_context.Brands, "IdBrand", "NameBrand", product.IdBrand);
            return View(product);
        }
        #endregion
        #region sửa sp
        public async Task<IActionResult> Edit(int id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            ViewData["MauSac"] = new SelectList(_context.Colors, "IdColor", "NameColor", product.IdColor);
            ViewData["ThuongHieu"] = new SelectList(_context.Brands, "IdBrand", "NameBrand", product.IdBrand);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit ( int id,[Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,DiscountPercentage,Thumb,Imei,IdBrand,DateCreated,DateModified,BestSellers,HomeFlag,Active,IdColor,Tags,Title,Alias,UnitsInStock")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if( id != product.ProductId)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    product.ProductName = Utilities.ToTitleCase(product.ProductName);
                    if(fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(product.ProductName) + extension;
                        product.Thumb = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(product.Thumb)) product.Thumb = "default.jpg";
                    product.Alias = Utilities.SEOUrl(product.ProductName);
                    product.DateModified = DateTime.Now;
                    // tính % giảm
                    double discountAmount = (double)(product.Price * (100 - product.DiscountPercentage) / 100);
                    product.Discount = (int?)Math.Round(discountAmount, 2); //Làm tròn đến 2 chữ số thập phân
                    _context.Update(product);
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công.";
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException) {
                    if (!ProductExists((int)product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CatId", "CatName", product.CatId);
            ViewData["MauSac"] = new SelectList(_context.Colors, "IdColor", "NameColor", product.IdColor);
            ViewData["ThuongHieu"] = new SelectList(_context.Brands, "IdBrand", "NameBrand", product.IdBrand);
            return View(product);

        }
        #endregion
        #region xóa sp
        public async Task<IActionResult>Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.Include(p => p.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeleteConfrimed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }
        #endregion
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
