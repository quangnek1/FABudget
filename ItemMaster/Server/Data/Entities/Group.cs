using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemMaster.Server.Data.Entities
{
    [Table("Group")]
    public class Group
    {
        [Key]
		[MaxLength(20)]
		[Column(TypeName = "nvarchar(20)")]
		public string Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string GroupName { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? Status { get; set; }
    }
}
