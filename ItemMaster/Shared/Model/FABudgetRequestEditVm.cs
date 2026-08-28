using System.ComponentModel.DataAnnotations;

namespace ItemMaster.Shared.Model
{
	public class FABudgetRequestEditVm 
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
		public bool? RingishoProcess { get; set; }
		public string? AppropriatedBudgetNo { get; set; }
		public decimal? BudgetRemaining { get; set; }
		public decimal? BudgetSharing { get; set; }
		[Range(0, double.MaxValue)]
		public decimal? BudgetCanUsing { get; set; }
		public string? Remark { get; set; }
		public int? DepreciationPeriod { get; set; }
		public DateTime? DateCreate { get; set; }

		public bool Status { get; set; }

		//List
		public List<RateVm>? RateVms { get; set; }
		public decimal? _initialBudgetRemaining { get; set; }
		public List<FABudgetVm>? ListBudgetRemainingEdit { get; set; } = new List<FABudgetVm>();
		//
		//public decimal? TinhToan_FixedAssetsAccountedAmountEstimationUSD
		//{
		//	get => CurrentInvestmentAmountEstimationUSD;
		//	set
		//	{
		//		if (CurrentInvestmentAmountEstimationUSD != value)
		//		{
		//			CurrentInvestmentAmountEstimationUSD = value;
		//			UpdateFixedAssetsAccountedAmountBudget();
		//		}
		//	}
		//}
		//private void UpdateFixedAssetsAccountedAmountBudget()
		//{
		//	// Tính toán FixedAssetsAccountedAmountEstimationUSD
		//	FixedAssetsAccountedAmountEstimationUSD = (CurrentInvestmentAmountEstimationUSD ?? 0) + (PreviousInvestmentAmountEstimationUSD ?? 0);

		//	// Cập nhật BudgetRemaining
		//	if (CurrentInvestmentAmountUSD.HasValue)
		//	{
		//		BudgetRemaining = CurrentInvestmentAmountUSD - FixedAssetsAccountedAmountEstimationUSD;

		//		//if (BudgetRemaining > 0)
		//		//{
		//		//	BudgetRemaining = BudgetRemaining - (CurrentInvestmentAmountUSD - FixedAssetsAccountedAmountEstimationUSD);
		//		//}
		//		//else
		//		//{
		//		//	BudgetRemaining = (CurrentInvestmentAmountUSD - FixedAssetsAccountedAmountEstimationUSD);
		//		//}
		//	}
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

		public void UpdateGLAccount_AccountText(string _value, int? _glAccount)
		{
			AccountText = _value;
			GLACCOUNT = _glAccount;
		}
		public void UpdateSectionCode_Segment_SectionName(SectionVm sectionVm)
		{
			SectionCode = sectionVm.SectionCode;
			Segment = sectionVm.Segment;
			SectionName = sectionVm.SectionName;
		}


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
}
