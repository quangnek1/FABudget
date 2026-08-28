namespace ItemMaster.Shared.Model
{
	public class DataVm
	{
		public List<SectionVm>? SectionVms { get; set; }
		public List<AccountVm>? AccountVms { get; set; }
		public List<RateVm>? RateVms { get; set; }
		public List<int>? PriorityVm { get; set; }
		public List<FAbudgetHistoryListVm>? FAbudgetHistoryListVms { get; set; }
		public List<GroupEmailSendsVms>? GroupEmailSendsVms { get; set; }
	}
	
	
}
