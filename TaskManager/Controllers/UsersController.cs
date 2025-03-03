using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    [Authorize(Roles = "Admin")] // 只有 Admin 可以管理使用者
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //列出所有使用者
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new Dictionary<string, string>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id.ToString()] = roles.FirstOrDefault() ?? "未分配角色";
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        //修改使用者角色
        [HttpPost]
        public async Task<IActionResult> EditUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles); // 先移除舊角色
            await _userManager.AddToRoleAsync(user, newRole); // 再新增新角色

            return RedirectToAction(nameof(Index));
        }

        //刪除使用者
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // 防止刪除 Admin 自己
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return BadRequest("不能刪除 Admin 帳號！");
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        //Admin 重設使用者密碼（選擇性）
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            return BadRequest("密碼重設失敗！");
        }
    }
}
