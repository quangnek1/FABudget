namespace ItemMaster.Shared.Model
{
	public class PRDetailVm
	{
		public int? RowNum { get; set; }
		public string? PRNo { get; set; }
		public int? PRNumber { get; set; }
		public string? Name { get; set; }
		public decimal? Qty { get; set; }
		public string? Unit { get; set; }
		public decimal? NetPrice { get; set; }
		public string? Cur { get; set; }
		public decimal? Amount { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public string? PurchaseGroup { get; set; }
		public int? Item { get; set; }
		public decimal? DocCur { get; set; }
		public string? Curr { get; set; }
		public int? QtyPO { get; set; }
		public string? Supplier { get; set; }
		public int? Status { get; set; }
	}
	public class POVm
	{
		public int? RowNum { get; set; }
		public string? PO_NO { get; set; }
		public string? PO_DTL_NO { get; set; }
		public string? PR_NO { get; set; }
		public string? PR_DTL_NO { get; set; }
		public string? INVOICE_NO { get; set; }
		public string? INVOICE_DATE { get; set; }
		public string? PAYMENT_FLG { get; set; }
		public string? DELIVERY_SEQ { get; set; }
		public string? DELIVERY_QTY { get; set; }
	}

	public class PrVm
	{
		public int? RowNum { get; set; }
		public string? PRNo { get; set; }
		public string? PONo { get; set; }
		public string? InvoiceNo { get; set; }
		public DateTime? InvoiceDate { get; set; }
		public string? Section { get; set; }
		public DateTime? IssueDate { get; set; }
		public string? IssueBy { get; set; }
		public DateTime? ReceiveDate { get; set; }
		public string? RingishoNo { get; set; }
		public string? Remark { get; set; }
		public string? BudgetNo { get; set; }
		public decimal? totalAmount { get; set; }
		public int? Status { get; set; }
		public List<PRDetailVm>? pRDetailVm { get; set; }
	}
}
