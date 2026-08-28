namespace ItemMaster.Shared.Model
{
	public class DataUploadRequestVm
	{
        public string PR { get; set; }		
        public string PO { get; set; }		
        public int Item { get; set; }		
        public decimal Amount { get; set; }		
        public string Currency { get; set; }		
        public decimal AmountInLocalCurrency { get; set; }		
        public string LocalCurrency { get; set; }		
        public int Quantity { get; set; }		
        public string Unit { get; set; }		
        public bool Status { get; set; }		
    }

}
