using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.ViewModels;
using TaskManager.Models;
using TaskManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TaskManagerApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 登入頁面
        [HttpGet]
        public IActionResult Login() => View();

        // 登入處理
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByNameAsync(model.UserName);
                //if (user == null || !user.EmailConfirmed)
                //{
                //    ModelState.AddModelError(string.Empty, "請確認您的 Email 並重新登入。");
                //    return View(model);
                //}

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Tasks");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    // 可以列印 result 顯示更多錯誤訊息
                    ModelState.AddModelError(string.Empty, $"Sign-in failed: {result.ToString()}");
                }
            }
            return View(model);
        }

        // 註冊頁面
        [HttpGet]
        public IActionResult Register() => View();

        // 註冊處理
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 直接將 EmailConfirmed 設為 true
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);

                    if (!await _userManager.IsInRoleAsync(user, "User"))
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Tasks");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // 登出
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Tasks");
        }
    }
}
