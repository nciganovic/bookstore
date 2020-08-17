using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public bool IsSelected { get; set; }
        public string Username { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
