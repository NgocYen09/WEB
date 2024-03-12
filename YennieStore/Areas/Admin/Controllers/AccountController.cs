using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YennieStore.Areas.Admin.Models;
using YennieStore.Extension;
using YennieStore.Models;

namespace YennieStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly dbMarketsContext _context;
        public AccountController(dbMarketsContext context)
        {
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        #region đăng nhập admin
        [AllowAnonymous]
        [Route("login.html", Name = "Login")]
        public IActionResult AdminLogin(string returnUrl = null)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (accountId != null)
            {
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login.html", Name = "Login")]
        public async Task<IActionResult> AdminLogin(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                Account? kh = _context.Accounts
                    .Include(p => p.Role)
                    .FirstOrDefault(p => p.Email.ToLower() == model.UserName.ToLower().Trim());

                if (kh == null || !kh.Active)
                {
                    ViewBag.Error = "Thông tin đăng nhập chưa chính xác hoặc bị khóa";
                    return View(model);
                }
                if (kh.Password?.Trim() != HashMD5.ToMD5(model.Password.Trim()))
                {
                    ViewBag.Error = "Sai mật khẩu";
                    return View(model);
                }
                kh.LastLogin = DateTime.Now;
                _context.Update(kh);
                await _context.SaveChangesAsync();

                // Phiên và quyền truy cập
                HttpContext.Session.SetString("AccountId", kh.AccountId.ToString());

                var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, kh.FullName),
                new Claim(ClaimTypes.Email, kh.Email),
                new Claim("AccountId", kh.AccountId.ToString()),
                new Claim("RoleId", kh.RoleId.ToString()),
                new Claim(ClaimTypes.Role, kh.Role.RoleName)
            };

                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                await HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.Error = "Đã xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại.";
                return View(model);
            }
        }
        #endregion

        #region đăng xuất admin
        [Route("logout.html", Name = "Logout")]
        public IActionResult AdminLogout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("AdminLogin", "Account", new { Area = "Admin" });
            }
        }
        #endregion
    }
}