using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ItemMaster.Server.Data.Entities
{
	public class GroupEmailSend
	{
		[Key]
		public string Id { get; set; }
		[Column(TypeName = "nvarchar(500)")]
		public string Content { get; set; }
		public int? CountSend { get; set; }
		public DateTime? LastModified { get; set; }
		public bool? Status { get; set; }
	}
}
