namespace ItemMaster.Shared.Model
{
	public class DataUploadVm
	{
		public string? PR { get; set; }
		public int? PR_No { get; set; }
		public string? PO { get; set; }
		public int? Item { get; set; }
		public decimal? Amount { get; set; }
		public string? Currency { get; set; }
		public decimal? AmountInLocalCurrency { get; set; }
		public string? LocalCurrency { get; set; }
		public int? Quantity { get; set; }
		public string? Unit { get; set; }
		public int? LaborCost { get; set; }
		public string? Ring { get; set; }
		public string? SAP { get; set; }
		public bool? Status { get; set; }
	}
	public class SaveUploadVm
	{
		public List<DataUploadVm> dataUploadVms { get; set; }
		public SaveUploadVm(List<DataUploadVm> dataUploadVms)
		{
			this.dataUploadVms = dataUploadVms;

		}

	}
}

