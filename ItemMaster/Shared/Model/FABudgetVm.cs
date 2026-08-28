namespace ItemMaster.Shared.Model
{
	public class FABudgetVm
	{
		public int Id { get; set; }
		public int? No { get; set; }
		public int? GLACCOUNT { get; set; }
		public int? PriorityRank { get; set; }
		public string? AccountText { get; set; }
		public string? AssetName { get; set; }
		public string? PersonInCharge { get; set; }
		public string? SectionCode { get; set; }
		public string? Segment { get; set; }
		public string? SectionName { get; set; }
		public DateTime? PurchaseTimeBudget { get; set; }
		public DateTime? DepreciationStartTimeBudget { get; set; }
		public DateTime? PurchaseTimeEstimation { get; set; }
		public DateTime? DepreciationStartTimeEstimation { get; set; }
		public string? Currency { get; set; }
		public decimal? CurrentInvestmentAmountBudget { get; set; }
		public decimal? PreviousInvestmentAmountBudget { get; set; }
		public decimal? FixedAssetsAccountedAmountBudget
		{
			get; set;
		}
		public decimal? EXRate { get; set; }
		public decimal? CurrentInvestmentAmountUSD { get; set; }
		public decimal? PreviousInvestmentAmountUSD { get; set; }
		public decimal? FixedAssetsAccountedAmountUSD { get; set; }
		public decimal? CurrentInvestmentAmountEstimationUSD { get; set; }
		public decimal? PreviousInvestmentAmountEstimationUSD { get; set; }
		public decimal? FixedAssetsAccountedAmountEstimationUSD { get; set; }
		public string? RingishoNo { get; set; }
		public string? AppropriatedBudgetNo { get; set; }
		public decimal? BudgetRemaining { get; set; }
		public string? Remark { get; set; }
		public int? DepreciationPeriod { get; set; }
		public DateTime? DateCreate { get; set; }

		public List<string> Rings { get; set; } = new();



		//List
		public List<RateVm>? RateVms { get; set; }

		//
		//public decimal? TinhToan_FixedAssetsAccountedAmountEstimationUSD
		//{
		//	get
		//	{
		//		var abc = CurrentInvestmentAmountEstimationUSD;
		//		return CurrentInvestmentAmountEstimationUSD;
		//	}
		//	set
		//	{
		//		var abc = value;
		//		UpdateFixedAssetsAccountedAmountBudget(value);

		//	}
		//}
		//private void UpdateFixedAssetsAccountedAmountBudget(decimal? _value)
		//{
		//	CurrentInvestmentAmountEstimationUSD = _value;
		//	FixedAssetsAccountedAmountEstimationUSD = (CurrentInvestmentAmountEstimationUSD ?? 0) + (PreviousInvestmentAmountEstimationUSD ?? 0);
		//	BudgetRemaining = (CurrentInvestmentAmountUSD ?? 0) - (CurrentInvestmentAmountEstimationUSD ?? 0);
		//}

		//public decimal? TinhToan2_FixedAssetsAccountedAmountEstimationUSD
		//{
		//	get
		//	{
		//		var abc = PreviousInvestmentAmountEstimationUSD;
		//		return PreviousInvestmentAmountEstimationUSD;
		//	}
		//	set
		//	{
		//		var abc = value;
		//		Update2FixedAssetsAccountedAmountBudget(value);

		//	}
		//}
		//private void Update2FixedAssetsAccountedAmountBudget(decimal? _value)
		//{
		//	PreviousInvestmentAmountEstimationUSD = _value;
		//	FixedAssetsAccountedAmountEstimationUSD = (CurrentInvestmentAmountEstimationUSD ?? 0) + (PreviousInvestmentAmountEstimationUSD ?? 0);
		//}

		////Update Account Text
		////public string? Update_AccountText
		////{
		////	get
		////	{
		////		var abc = AccountText;
		////		return AccountText;
		////	}
		////	set
		////	{
		////		var abc = value;
		////		UpdateGLAccount_AccountText(value);

		////	}
		////}
		//public void UpdateGLAccount_AccountText(string _value, int? _glAccount)
		//{
		//	AccountText = _value;
		//	GLACCOUNT = _glAccount;
		//}
		//public void UpdateSectionCode_Segment_SectionName(SectionVm sectionVm)
		//{
		//	SectionCode = sectionVm.SectionCode;
		//	Segment = sectionVm.Segment;
		//	SectionName = sectionVm.SectionName;
		//}


		// Format Date
		public string FormattedPurchaseTimeEstimation
		{
			get => PurchaseTimeEstimation.HasValue
				? PurchaseTimeEstimation.Value.ToString("yyyy-MM")
				: string.Empty;
			set
			{
				if (DateTime.TryParse($"{value}-01", out var parsedDate))
				{
					PurchaseTimeEstimation = parsedDate;
				}
				else
				{
					PurchaseTimeEstimation = null;
				}
			}
		}
		public string FormattedDepreciationStartTimeEstimation
		{
			get => DepreciationStartTimeEstimation.HasValue
				? DepreciationStartTimeEstimation.Value.ToString("yyyy-MM")
				: string.Empty;
			set
			{
				if (DateTime.TryParse($"{value}-01", out var parsedDate))
				{
					DepreciationStartTimeEstimation = parsedDate;
				}
				else
				{
					DepreciationStartTimeEstimation = null;
				}
			}
		}
	}
	public class RingList
	{
		public List<string> RingListss { get; set; }
		public List<PrVm> PrVms { get; set; }
	}
}
