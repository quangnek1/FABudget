using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ItemMaster.Server.Data.Entities
{
	[Table("PODataUpload")]
	public class PODataUpload
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Column(TypeName = "nvarchar(15)")]
		public string? PR { get; set; }
		public int? PR_No { get; set; }
		[Column(TypeName = "nvarchar(15)")]
		public string? PO { get; set; }
		public int? Item { get; set; }
		[Precision(18, 2)]
		public decimal? Amount { get; set; }
		[MaxLength(10)]
		[Column(TypeName = "nvarchar(10)")]
		public string? Currency { get; set; }
		[Precision(18, 2)]
		public decimal? AmountInLocalCurrency { get; set; }
		[MaxLength(10)]
		[Column(TypeName = "nvarchar(10)")]
		public string? LocalCurrency { get; set; }
		public int? Quantity { get; set; }
		[MaxLength(10)]
		[Column(TypeName = "nvarchar(10)")]
		public string? Unit { get; set; }
		public int? LaborCost { get; set; }
		[MaxLength(25)]
		[Column(TypeName = "nvarchar(25)")]
		public string? Ring { get; set; }
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? SAP { get; set; }
	}
}
