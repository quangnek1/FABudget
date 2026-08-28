using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemMaster.Shared.Model
{
    public class UserVm
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public int? ParrenId { get; set; }
        public string? Code { get; set; }
        public string? Fullname { get; set; }
    }
}
