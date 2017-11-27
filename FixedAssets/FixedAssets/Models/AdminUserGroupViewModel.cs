using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixedAssets.Models
{
    //public class AdminUserGroupViewModel
    //{
    //    public string UserName { get; set; }
    //    public string Email { get; set; }
    //    public ICollection<Role> Roles { get; set; }
    //}

    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool ActiveForUser { get; set; }
    }

    public class RoleCollection
    {
        public IList<Role> Roles { get; set; }
    }
}