using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KokaarQrCoder.Utility.Options
{
    public class SuperAministratorOptions
    {
        public const string ConfigSectionName = "SuperAministratorOptions";

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
