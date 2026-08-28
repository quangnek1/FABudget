using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ItemMaster.Server.Data.Entities
{

	[Table("Account")]
	public class Account
	{
		[Key]
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? GLACCOUNT { get; set; }
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? AccountText { get; set; }
		public bool? Status { get; set; }
	}
}
