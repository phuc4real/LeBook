using AspNetCoreHero.ToastNotification.Abstractions;
using LeBook.DataAccess.Repository.IRepository;
using LeBook.Models;
using LeBook.Models.ViewModel;
using LeBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace LeBookWeb.Areas.Admin.Controllers
{
    [Authorize("canView")]
    [Area("Dashboard")]
    public class ManagerUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public INotyfService _notifyService { get; }

        public ManagerUserController(UserManager<ApplicationUser> userManager, INotyfService notyfService)
        {
            _notifyService = notyfService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userWithRolesList = new List<UserWithRoles>();
            IEnumerable<ApplicationUser> users = new List<ApplicationUser>();
            var currentUser = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(currentUser);

            if (roles.Contains(Roles.Admin))
            {
                users = await _userManager.Users.ToListAsync();
            }
            else
            {
                users = await _userManager.GetUsersInRoleAsync(Roles.Customer);
            }

            foreach (var user in users)
            {
                UserWithRoles userWithRoles = new()
                {
                    User = user,
                    Roles = await _userManager.GetRolesAsync(user)
                };
                userWithRolesList.Add(userWithRoles);
            }

            return View(userWithRolesList);
        }
    }
}
