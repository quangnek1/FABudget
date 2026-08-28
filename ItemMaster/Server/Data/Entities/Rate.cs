using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ItemMaster.Server.Data.Entities
{
	[Table("Rate")]
	public class Rate
	{
		[Key]
		[MaxLength(20)]
		[Column(TypeName = "nvarchar(20)")]
		public string? Currency { get; set; }
		[Precision(18, 2)]
		public decimal? Current { get; set; }
		[Precision(18, 2)]
		public decimal? NextYear { get; set; }
		public bool? Status { get; set; }
	}
}
