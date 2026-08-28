using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ItemMaster.Server.Data.Entities
{
	[Table("BudgetSharing")]
	public class BudgetSharing
	{
		public int BudgetNo { get; set; }
		public int BudgetSharingNo { get; set; }
		[Precision(18, 2)]
		public decimal? BudgetHasShare { get; set; }
	}
}
