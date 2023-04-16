using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.Models;
using LeBook.Models.ViewModel;
using LeBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LeBookWeb.Areas.Admin.Controllers
{
    [Authorize("AdminOnly")]
    [Area("Dashboard")]
    public class ManagerRoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public INotyfService _notifyService { get; }

        public ManagerRoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, INotyfService notyfService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _notifyService = notyfService;
        }

        public async Task<IActionResult> Index()
        {

            var listRole = await _roleManager.Roles.ToListAsync();

            return View(listRole);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            _notifyService.Success("Thêm vai trò thành công");
            return RedirectToAction("Roles");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = new IdentityRole();
            if (id != null) role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }

            _notifyService.Success("Xoá vai trò thành công");
            return RedirectToAction("Roles");
        }

        [HttpGet]
        public async Task<IActionResult> UserRoles(string Id)
        {
            var viewModel = new List<UserRolesViewModel>();
            var user = await _userManager.FindByIdAsync(Id);

            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }

            var model = new UserWithRoles()
            {
                UserId = Id,
                UserRoles = viewModel
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> UpdateUserRole(string Id, UserWithRoles model)
        {
            var user = await _userManager.FindByIdAsync(Id);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));

            _notifyService.Success("Sửa vai trò thành công");
            return RedirectToAction("Index", "ManagerUser");
        }

    }
}
