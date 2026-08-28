using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ItemMaster.Server.Data.Entities
{
	[Table("Section")]
	public class Section
	{
		[Key]
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? SectionCode { get; set; }
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? Segment { get; set; }
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? SectionName { get; set; }
		public bool? Status { get; set; }
	}
}
 
