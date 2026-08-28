using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace ItemMaster.Server.Data.Entities
{
	[Table("FABudget")]
	public class FABudget
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int? No { get; set; }
		public int? GLACCOUNT { get; set; }
		[MaxLength(200)]
		[Column(TypeName = "nvarchar(200)")]
		public string? AccountText { get; set; }
		public int? PriorityRank { get; set; }
		[MaxLength(500)]
		[Column(TypeName = "nvarchar(500)")]
		public string? AssetName { get; set; }
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? PersonInCharge { get; set; }
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? SectionCode { get; set; }
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? Segment { get; set; }
		[MaxLength(50)]
		[Column(TypeName = "nvarchar(50)")]
		public string? SectionName { get; set; }
		public DateTime? PurchaseTimeBudget { get; set; }
		public DateTime? DepreciationStartTimeBudget { get; set; }
		public DateTime? PurchaseTimeEstimation { get; set; }
		public DateTime? DepreciationStartTimeEstimation { get; set; }
		[MaxLength(20)]
		[Column(TypeName = "nvarchar(20)")]
		public string? Currency { get; set; }
		[Precision(18, 2)]
		public decimal? CurrentInvestmentAmountBudget { get; set; }
		[Precision(18, 2)]
		public decimal? PreviousInvestmentAmountBudget { get; set; }
		[Precision(18, 2)]
		public decimal? FixedAssetsAccountedAmountBudget { get; set; }
		[Precision(18, 2)]
		public decimal? EXRate { get; set; }
		[Precision(18, 2)]
		public decimal? CurrentInvestmentAmountUSD { get; set; }
		[Precision(18, 2)]
		public decimal? PreviousInvestmentAmountUSD { get; set; }
		[Precision(18, 2)]
		public decimal? FixedAssetsAccountedAmountUSD { get; set; }
		[Precision(18, 2)]
		public decimal? CurrentInvestmentAmountEstimationUSD { get; set; }
		[Precision(18, 2)]
		public decimal? PreviousInvestmentAmountEstimationUSD { get; set; }
		[Precision(18, 2)]
		public decimal? FixedAssetsAccountedAmountEstimationUSD { get; set; }
		[MaxLength(20)]
		[Column(TypeName = "nvarchar(20)")]
		public string? RingishoNo { get; set; }
		public bool? RingishoProcess { get; set; }
		[MaxLength(100)]
		[Column(TypeName = "nvarchar(100)")]
		public string? AppropriatedBudgetNo { get; set; }
		[Precision(18, 2)]
		public decimal? BudgetRemaining { get; set; }
		public string? Remark { get; set; }
		public DateTime? DateCreate { get; set; }
		public DateTime? LastModified { get; set; }
		public bool? Status { get; set; }

	}
}
