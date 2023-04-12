using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class UserWithRoles
    {
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        [DisplayName("Vai trò")]
        public IList<string>? Roles { get; set; }
        public IList<UserRolesViewModel>? UserRoles { get; set; }
    }
    public class UserRolesViewModel
    {
        public string? RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
