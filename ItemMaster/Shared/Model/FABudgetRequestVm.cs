using System.ComponentModel.DataAnnotations;

namespace ItemMaster.Shared.Model
{
	public class FABudgetRequestVm
	{
		public int Id { get; set; }
		public int? No { get; set; }
		[Required(ErrorMessage = "Required")]
		public int? GLACCOUNT { get; set; }
		[Required(ErrorMessage = "Required")]
		public int? PriorityRank { get; set; }
		[Required(ErrorMessage = "Required")]
		public string? AccountText { get; set; }
		[Required(ErrorMessage = "Required")]
		public string? AssetName { get; set; }
		[Required(ErrorMessage = "Required")]
		public string? PersonInCharge { get; set; }
		[Required(ErrorMessage = "Required")]
		public string? SectionCode { get; set; }
		[Required(ErrorMessage = "Required")]
		public string? Segment { get; set; }
		[Required(ErrorMessage = "Required")]
		public string? SectionName { get; set; }
		public DateTime? PurchaseTimeBudget { get; set; }
		public DateTime? DepreciationStartTimeBudget { get; set; }
		[Required(ErrorMessage = "Required")]
		public DateTime? PurchaseTimeEstimation { get; set; }
		public DateTime? DepreciationStartTimeEstimation { get; set; }
		[Required(ErrorMessage = "Required")]
		public string? Currency { get; set; }
		public decimal? CurrentInvestmentAmountBudget { get; set; }
		public decimal? PreviousInvestmentAmountBudget { get; set; }
		public decimal? FixedAssetsAccountedAmountBudget
		{
			get; set;
		}
		[Required(ErrorMessage = "Required")]
		public decimal? EXRate { get; set; }
		public decimal? CurrentInvestmentAmountUSD { get; set; }
		public decimal? PreviousInvestmentAmountUSD { get; set; }
		public decimal? FixedAssetsAccountedAmountUSD { get; set; }
		public decimal? CurrentInvestmentAmountEstimationUSD { get; set; }
		public decimal? PreviousInvestmentAmountEstimationUSD { get; set; }
		public decimal? FixedAssetsAccountedAmountEstimationUSD { get; set; }
		public string? RingishoNo { get; set; }
		[Required(ErrorMessage = "Required")]
		public string? AppropriatedBudgetNo { get; set; }
		public decimal? BudgetRemaining { get; set; }
		public string? Remark { get; set; }
		public DateTime? DateCreate { get; set; }


		//
		public decimal? TinhToan_FixedAssetsAccountedAmountEstimationUSD
		{
			get
			{
				var abc = CurrentInvestmentAmountEstimationUSD;
				return CurrentInvestmentAmountEstimationUSD;
			}
			set
			{
				var abc = value;
				UpdateFixedAssetsAccountedAmountBudget(value);

			}
		}
		private void UpdateFixedAssetsAccountedAmountBudget(decimal? _value)
		{
			CurrentInvestmentAmountEstimationUSD = _value;
			FixedAssetsAccountedAmountEstimationUSD = (CurrentInvestmentAmountEstimationUSD ?? 0) + (PreviousInvestmentAmountEstimationUSD ?? 0);
	//		BudgetRemaining = (CurrentInvestmentAmountUSD ?? 0) - (CurrentInvestmentAmountEstimationUSD ?? 0);
		}

		public decimal? TinhToan2_FixedAssetsAccountedAmountEstimationUSD
		{
			get
			{
				var abc = PreviousInvestmentAmountEstimationUSD;
				return PreviousInvestmentAmountEstimationUSD;
			}
			set
			{
				var abc = value;
				Update2FixedAssetsAccountedAmountBudget(value);

			}
		}
		private void Update2FixedAssetsAccountedAmountBudget(decimal? _value)
		{
			PreviousInvestmentAmountEstimationUSD = _value;
			FixedAssetsAccountedAmountEstimationUSD = (CurrentInvestmentAmountEstimationUSD ?? 0) + (PreviousInvestmentAmountEstimationUSD ?? 0);
		}
		//
		//public string? TinhToan_AppropriatedBudgetNo
		//{
		//	get
		//	{
		//		var abc = AppropriatedBudgetNo;
		//		return AppropriatedBudgetNo;
		//	}
		//	set
		//	{
		//		var abc = value;
		//		UpdateAppropriatedBudgetNo(value);

		//	}
		//}
		//private void UpdateAppropriatedBudgetNo(string? _value)
		//{
		//	AppropriatedBudgetNo = _value;
		//}

		//
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
		public void UpdateCurrency_Rate(string _value, decimal? _rate)
		{
			Currency = _value;
			EXRate = _rate;
		}
		//public void UpdateBudgetRemaining(decimal? _value, string? _ppropriatedBudgetNo)
		//{
		//	BudgetRemaining = _value;
		//	AppropriatedBudgetNo = _ppropriatedBudgetNo;

  //      }

		// Format Date
		public string FormattedPurchaseTimeBudget
		{
			get => PurchaseTimeBudget.HasValue
				? PurchaseTimeBudget.Value.ToString("yyyy-MM")
				: string.Empty;
			set
			{
				if (DateTime.TryParse($"{value}-01", out var parsedDate))
				{
					PurchaseTimeBudget = parsedDate;
				}
				else
				{
					PurchaseTimeBudget = null;
				}
			}
		}
		public string FormattedDepreciationStartTimeBudget
		{
			get => DepreciationStartTimeBudget.HasValue
				? DepreciationStartTimeBudget.Value.ToString("yyyy-MM")
				: string.Empty;
			set
			{
				if (DateTime.TryParse($"{value}-01", out var parsedDate))
				{
					DepreciationStartTimeBudget = parsedDate;
				}
				else
				{
					DepreciationStartTimeBudget = null;
				}
			}
		}
		[Required(ErrorMessage = "Required")]
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
