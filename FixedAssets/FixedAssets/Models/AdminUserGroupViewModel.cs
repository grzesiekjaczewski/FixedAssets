using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FixedAssets.Models
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool ActiveForUser { get; set; }
    }

    //public class RoleCollection
    //{
    //    public IList<Role> Roles { get; set; }
    //}
}