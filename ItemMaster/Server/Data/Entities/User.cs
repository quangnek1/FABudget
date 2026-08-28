using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemMaster.Server.Data.Entities
{
    public class User : IdentityUser
    {
        public int DivisionId { get; set; }
        public int SectionId { get; set; }
        public int ParentId { get; set; } = 1;
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string FullName { get; set; }
        public string Code { get; set; }
    }
}
