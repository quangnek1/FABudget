using ItemMaster.Server.Data.Entities;
using ItemMaster.Shared.Model;

namespace ItemMaster.Server.Extensions
{
	public class EmailSendVm
	{
		public UserVm? User { get; set; }
		public List<FABudget>? FABudgets { get; set; }
	}
}
