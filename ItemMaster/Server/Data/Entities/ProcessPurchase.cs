using System.ComponentModel.DataAnnotations.Schema;

namespace ItemMaster.Server.Data.Entities
{
	[Table("ProcessPurchase")]
	public class ProcessPurchase
	{
		public int Id { get; set; }
		public int? FABudgetId { get; set; }
		public DateTime? Date { get; set; }
	}
}
