namespace ItemMaster.Shared.Model
{
	public class FABudgetObjectVm
	{
		public FABudgetVm? FABudgetSumaryVm { get; set; }
		public FABudgetVm? FABudgetAddVm { get; set; }
		public List<FABudgetVm>? FABudgetVms { get; set; }
		public List<FABudgetVm>? ListBudgetRemaining { get; set; }
	}
}
