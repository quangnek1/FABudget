using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using ItemMaster.Server.Data;
using ItemMaster.Server.Data.Entities;
using ItemMaster.Shared.Model;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static MudBlazor.CategoryTypes;

namespace ItemMaster.Server.Services
{
	public class PURCHASE_SYSTEM : IPURCHASE_SYSTEM
	{
		private readonly PurchaseContext _purchaseContext;
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _hostingEnvironment;

		public PURCHASE_SYSTEM(PurchaseContext purchaseContext, ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
		{
			_purchaseContext = purchaseContext;
			_context = context;
			_hostingEnvironment = hostingEnvironment;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pODataUpload"></param>
		/// <returns>0 là Fail; 1 là OK, 2 là đang thực hiện</returns>
		//public async Task<int> CheckPO(PODataUpload pODataUpload)
		//{
		//	var query = @" select p.PR_HDR_NO as PRNo, p.PRNO_SAP_DTL as PRNumber, p.SHORT_TEXT as Name, p.QTY as Qty, p.UNIT as Unit,
		//			  p.NET_PRICE as NetPrice, p.CUR as Cur, p.AMOUNT as Amount, p.DELIVERY_DATE as DeliveryDate from [PUR_REQ_SAP_DTL] as p where p.PR_HDR_NO = @PR and PRNO_SAP_DTL = @PR_No";
		//	using (var connection = _purchaseContext.CreateConnection())
		//	{
		//		var list = await connection.QueryAsync<PRDetailVm>(query, new { pODataUpload.PR, pODataUpload.PR_No });
		//		if (list.Count() > 0)
		//		{
		//			foreach (var item in list)
		//			{
		//				if (pODataUpload.Amount == item.Amount)
		//				{
		//					return 1;
		//				}
		//				else if (pODataUpload.Amount < item.Amount && pODataUpload.Quantity < item.Qty)
		//				{
		//					var queryAmountVm = _context.PODataUploads.Where(p => p.PO == pODataUpload.PO && p.PR_No == pODataUpload.PR_No).ToList();
		//					var queryAmount = queryAmountVm.Select(p => p.Amount).Sum();
		//					if (queryAmount == item.Amount)
		//					{
		//						return 1;
		//					}
		//					else
		//					{
		//						return 2;
		//					}
		//				}
		//				else
		//				{
		//					return 0;
		//				}
		//			}
		//		}
		//		else
		//		{
		//			if (!string.IsNullOrEmpty(pODataUpload.Ring))
		//			{
		//				var checkRing = @"select p.PUR_NO as PRNo from [PUR_REQ_SAP_HDR] as p where RING_NO =  @Ring";
		//				var listcheckRing = await connection.QueryAsync<PrVm>(checkRing, new { pODataUpload.Ring });
		//				var listcheck = listcheckRing.Select(p => p.PRNo);

		//				var querysumPR = " select sum(p.AMOUNT) as total from PUR_REQ_SAP_DTL as p where PR_HDR_NO in @listcheck";
		//				var sumPR = await connection.QueryAsync<decimal>(querysumPR, new { listcheck });

		//				var checkFA = _context.FABudgets.Where(p => p.RingishoNo == pODataUpload.Ring).ToList();
		//				decimal? total = checkFA.Sum(p => p.CurrentInvestmentAmountEstimationUSD);

		//				//	where PR_NO in (SELECT[PR_HDR_NO]FROM[IT_PURCHASE_SYSTEM].[dbo].[PUR_REQ_SAP_HDR] where RING_NO = 'AIH24-022')
		//				//   select * from PUR_REQ_SAP_DTL as p where PR_HDR_NO in (SELECT [PR_HDR_NO]FROM [IT_PURCHASE_SYSTEM].[dbo].[PUR_REQ_SAP_HDR] where RING_NO = 'AIH24-022')
		//			}
		//		}

		//		return 0;
		//	}

		//}


		public async Task<int> CheckPO(PODataUpload pODataUpload)
		{
			var query = @" select p.PR_HDR_NO as PRNo, p.PRNO_SAP_DTL as PRNumber, p.SHORT_TEXT as Name, p.QTY as Qty, p.UNIT as Unit,
					  p.NET_PRICE as NetPrice, p.CUR as Cur, p.AMOUNT as Amount, p.DELIVERY_DATE as DeliveryDate from [PUR_REQ_SAP_DTL] as p where p.PR_HDR_NO = @PR and PRNO_SAP_DTL = @PR_No";
			using (var connection = _purchaseContext.CreateConnection())
			{
				var list = await connection.QueryAsync<PRDetailVm>(query, new { pODataUpload.PR, pODataUpload.PR_No });
				if (list.Count() > 0)
				{
					foreach (var item in list)
					{
						if (pODataUpload.Amount == item.Amount)
						{
							return 1;
						}
						else if (pODataUpload.Amount < item.Amount && pODataUpload.Quantity < item.Qty)
						{
							var queryAmountVm = _context.PODataUploads.Where(p => p.PO == pODataUpload.PO && p.PR_No == pODataUpload.PR_No).ToList();
							var queryAmount = queryAmountVm.Select(p => p.Amount).Sum();
							if (queryAmount == item.Amount)
							{
								return 1;
							}
							else
							{
								return 2;
							}
						}
						else
						{
							return 0;
						}
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(pODataUpload.Ring))
					{
						var checkRing = @"select p.PUR_NO as PRNo from [PUR_REQ_SAP_HDR] as p where RING_NO =  @Ring";
						var listcheckRing = await connection.QueryAsync<PrVm>(checkRing, new { pODataUpload.Ring });
						var listcheck = listcheckRing.Select(p => p.PRNo);

						var querysumPR = " select sum(p.AMOUNT) as total from PUR_REQ_SAP_DTL as p where PR_HDR_NO in @listcheck";
						var sumPR = await connection.QueryAsync<decimal>(querysumPR, new { listcheck });

						var checkFA = _context.FABudgets.Where(p => p.RingishoNo == pODataUpload.Ring).ToList();
						decimal? total = checkFA.Sum(p => p.CurrentInvestmentAmountEstimationUSD);

						//	where PR_NO in (SELECT[PR_HDR_NO]FROM[IT_PURCHASE_SYSTEM].[dbo].[PUR_REQ_SAP_HDR] where RING_NO = 'AIH24-022')
						//   select * from PUR_REQ_SAP_DTL as p where PR_HDR_NO in (SELECT [PR_HDR_NO]FROM [IT_PURCHASE_SYSTEM].[dbo].[PUR_REQ_SAP_HDR] where RING_NO = 'AIH24-022')
					}
				}

				return 0;
			}

		}

		public async Task<ServiceResponse<FABudgetObjectVm>> GetFAAsync()
		{
			var fABudgetObjectVm = new FABudgetObjectVm();

			var query = await _context.FABudgets.ToListAsync();
			var myOrderVms = query.Select((p, index) => new FABudgetVm()
			{
				No = p.No,
				AccountText = p.AccountText,
				AppropriatedBudgetNo = p.AppropriatedBudgetNo,
				AssetName = p.AssetName,
				CurrentInvestmentAmountBudget = p.CurrentInvestmentAmountBudget,
				CurrentInvestmentAmountUSD = p.CurrentInvestmentAmountUSD,
				Currency = p.Currency,
				DateCreate = p.DateCreate,
				DepreciationStartTimeBudget = p.DepreciationStartTimeBudget,
				DepreciationStartTimeEstimation = p.DepreciationStartTimeEstimation,
				EXRate = p.EXRate,
				FixedAssetsAccountedAmountBudget = p.FixedAssetsAccountedAmountBudget,
				FixedAssetsAccountedAmountUSD = p.FixedAssetsAccountedAmountUSD,
				GLACCOUNT = p.GLACCOUNT,
				Id = p.Id,
				PersonInCharge = p.PersonInCharge,
				PreviousInvestmentAmountBudget = p.PreviousInvestmentAmountBudget,
				PreviousInvestmentAmountUSD = p.PreviousInvestmentAmountUSD,
				PriorityRank = p.PriorityRank,
				PurchaseTimeBudget = p.PurchaseTimeBudget,
				PurchaseTimeEstimation = p.PurchaseTimeEstimation,
				CurrentInvestmentAmountEstimationUSD = p.CurrentInvestmentAmountEstimationUSD,
				PreviousInvestmentAmountEstimationUSD = p.PreviousInvestmentAmountEstimationUSD,
				FixedAssetsAccountedAmountEstimationUSD = p.FixedAssetsAccountedAmountEstimationUSD,
				Remark = p.Remark,
				RingishoNo = p.RingishoNo,
				//	BudgetRemaining = CalculateBudgetRemaining(p.BudgetRemaining, p.No).Result == null ? p.BudgetRemaining : CalculateBudgetRemaining(p.BudgetRemaining, p.No).Result,
				BudgetRemaining = p.BudgetRemaining,
				SectionCode = p.SectionCode,
				SectionName = p.SectionName,
				Segment = p.Segment,
				Rings = p.RingishoNo != null ? p.RingishoNo.Split(',', '、').ToList() : new List<string>(),
			}).ToList();

			fABudgetObjectVm.FABudgetVms = myOrderVms;
			fABudgetObjectVm.FABudgetSumaryVm = new FABudgetVm()
			{
				CurrentInvestmentAmountBudget = query.Sum(p => p.CurrentInvestmentAmountBudget),
				PreviousInvestmentAmountBudget = query.Sum(p => p.PreviousInvestmentAmountBudget),
				FixedAssetsAccountedAmountBudget = query.Sum(p => p.FixedAssetsAccountedAmountBudget),

				CurrentInvestmentAmountUSD = query.Sum(p => p.CurrentInvestmentAmountUSD),
				PreviousInvestmentAmountUSD = query.Sum(p => p.PreviousInvestmentAmountUSD),
				FixedAssetsAccountedAmountUSD = query.Sum(p => p.FixedAssetsAccountedAmountUSD),

				CurrentInvestmentAmountEstimationUSD = query.Sum(p => p.CurrentInvestmentAmountEstimationUSD),
				PreviousInvestmentAmountEstimationUSD = query.Sum(p => p.PreviousInvestmentAmountEstimationUSD),
				FixedAssetsAccountedAmountEstimationUSD = query.Sum(p => p.FixedAssetsAccountedAmountEstimationUSD),

			};

			fABudgetObjectVm.ListBudgetRemaining = myOrderVms.Where(p => p.BudgetRemaining > 0 && p.CurrentInvestmentAmountUSD != null).ToList();

			fABudgetObjectVm.FABudgetAddVm = new FABudgetVm()
			{
				No = query.Select(p => p.No).Max() + 1,

			};


			var result = new ServiceResponse<FABudgetObjectVm>()
			{
				Data = fABudgetObjectVm,
				IsSuccess = true,
				Message = "OK"
			};
			return result;
		}
		public async Task DownloadAllData()
		{
			var data = await GetFAAsync();
			var dataloc = data.Data.FABudgetVms;

			string sWebRootFolder = _hostingEnvironment.WebRootPath;
			string fileName = @"POExport.xlsx";

			FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, fileName));

			var package = CreateExportPO(file, dataloc);


		}
		private async Task<ExcelPackage> CreateExportPO(FileInfo file, List<FABudgetVm>? FABudgetVms)
		{
			var query = FABudgetVms;

			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

			//var package = new ExcelPackage(file);
			//return package;
			var package = new ExcelPackage(file);
			ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
			var ws = package.Workbook.Worksheets["Data"];
			int count = query.Count();
			int row = 2;

			string folder = Path.Combine(Directory.GetCurrentDirectory(), "exports");

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			string filePath = Path.Combine(folder, "POExport1.xlsx");
			try
			{
				foreach (var item in query)
				{
					ws.Cells["A" + row].Value = item.No;
					ws.Cells["B" + row].Value = item.PersonInCharge;
					ws.Cells["C" + row].Value = item.AccountText;
					ws.Cells["D" + row].Value = item.Currency;
					ws.Cells["E" + row].Value = item.AssetName;
					ws.Cells["F" + row].Value = item.GLACCOUNT;
					ws.Cells["G" + row].Value = item.BudgetRemaining;
					ws.Cells["H" + row].Value = item.AppropriatedBudgetNo;
					ws.Cells["I" + row].Value = item.DepreciationStartTimeEstimation;
					ws.Cells["J" + row].Value = item.RingishoNo;

					foreach (var utem in item.Rings)
					{
						try
						{
							var listPr = await GetAsyncByRingishoNoDownload(utem.ToString());
							if (listPr.Count() == 0)
								break;
							foreach (var p in listPr)
							{
								ws.Cells["K" + row].Value = p.PONo;
								ws.Cells["L" + row].Value = p.PRNo;
								//var listPrDetail = await GetPRDetailByPRNoDownload(p.PRNo);
								//foreach (var o in listPrDetail)
								//{
								//	ws.Cells["M" + row].Value = o.PRNo;
								//	ws.Cells["N" + row].Value = o.PRNumber;
								//	ws.Cells["O" + row].Value = o.NetPrice;
								//	row++;
								//}
								row++;
							}
						}
						catch (Exception ex)
						{

						}

					}
					row++;


				}
			}
			catch (Exception)
			{

			}

			package.SaveAs(new FileInfo(filePath));
			return package;
		}
		public async Task<List<PrVm>> GetAsyncByRingishoNoDownload(string? ringNo)
		{
			try
			{
				var query = @"select ROW_NUMBER() OVER (ORDER BY pu.PR_HDR_NO) AS RowNum, pu.PR_HDR_NO as PRNo, pu.CREATE_BY as Section, pu.ISSUE_DATE as IssueDate, pu.ISSUE_BY as IssueBy, pu.RECEIVE_DATE as ReceiveDate,
				  pu.RING_NO as RingishoNo, pu.REMARK as Remark, pu.BUDGET_NO as BudgetNo,
				  p.PR_NO as PR_No, p.PO_NO as PONo, p.INVOICE_NO as InvoiceNo, p.INVOICE_DATE as InvoiceDate, dtl.total as totalAmount
				  from[PUR_REQ_SAP_HDR] as pu
				left  join
				  (SELECT distinct[PO_NO], PR_NO, INVOICE_NO, INVOICE_DATE
				  FROM[IT_PURCHASE_SYSTEM].[dbo].[PUR_PO_SAP]) as p on pu.PR_HDR_NO = p.PR_NO
				left join (select PR_HDR_NO ,SUM(AMOUNT) as total from PUR_REQ_SAP_DTL group by PR_HDR_NO )  as dtl on pu.PR_HDR_NO = dtl.PR_HDR_NO 
					where RING_NO  = @ringNo";

				using (var connection = _purchaseContext.CreateConnection())
				{
					var list = await connection.QueryAsync<PrVm>(query, new { ringNo });
					if (list.Count() == 0 || list == null)
					{
						return new List<PrVm>();
					}

					var listPrNo = list.ToList();

					var res = listPrNo.Select(p =>
					{
						var poItem = listPrNo.Where(x => x.PRNo == p.PRNo).Sum(p => p.totalAmount);
						return new PrVm()
						{
							PRNo = p.PRNo,
							BudgetNo = p.BudgetNo,
							InvoiceDate = p.InvoiceDate,
							InvoiceNo = p.InvoiceNo,
							IssueBy = p.IssueBy,
							IssueDate = p.IssueDate,
							PONo = p.PONo,
							ReceiveDate = p.ReceiveDate,
							Remark = p.Remark,
							RingishoNo = p.RingishoNo,
							RowNum = p.RowNum,
							Section = p.Section,
							Status = (Convert.ToInt32(p.totalAmount) == poItem) ? 1 : 0,
						};
					}).ToList();
					return res;
				}
			}
			catch (Exception)
			{
				return new List<PrVm>();
			}
		}
		public async Task<List<PRDetailVm>> GetPRDetailByPRNoDownload(string? prNo)
		{
			var query = @"select ROW_NUMBER() OVER (ORDER BY p.PR_HDR_NO) AS RowNum, p.PR_HDR_NO as PRNo, p.PRNO_SAP_DTL as PRNumber, p.SHORT_TEXT as Name, p.QTY as Qty, p.UNIT as Unit,
					  p.NET_PRICE as NetPrice, p.CUR as Cur, p.AMOUNT as Amount, p.DELIVERY_DATE as DeliveryDate,
					  p.PUCHASE_GROUP as PurchaseGroup,su.NAME_BRIEF as Supplier
					  from [PUR_REQ_SAP_DTL] as p
					  join [PURCHASE_SUPPLIER] as su on p.VENDOR = su.SUPPLIER_ID
					  where p.PR_HDR_NO = @prNo";
			using (var connection = _purchaseContext.CreateConnection())
			{
				var list = await connection.QueryAsync<PRDetailVm>(query, new { prNo });

				if (list.Count() == 0 || list == null)
				{
					return new List<PRDetailVm>();
				}

				var listData = list.ToList();
				var res = list.Select(p =>
				{
					return new PRDetailVm()
					{
						PRNo = p.PRNo,
						Amount = p.Amount,
						Cur = p.Cur,
						DeliveryDate = p.DeliveryDate,
						Name = p.Name,
						NetPrice = p.NetPrice,
						PurchaseGroup = p.PurchaseGroup,
						Qty = p.Qty,
						RowNum = p.RowNum,
						Supplier = p.Supplier,
						Unit = p.Unit,
					};
				}).ToList();


				return res;
			}
		}
		public async Task<List<PrVm>> GetAsyncByBudgetNo(int? budgetNo)
		{
			var query = "select * from [PUR_REQ_SAP_HDR] as i join PUR_PO_SAP as l on i.PR_HDR_NO = l.PR_NO join [PUR_REQ_SAP_DTL] as d on i.PR_HDR_NO = d.PR_HDR_NO where BUDGET_NO = @budgetNo";
			using (var connection = _purchaseContext.CreateConnection())
			{
				var list = await connection.QueryAsync<PrVm>(query, new { budgetNo });
				return list.ToList();
			}
		}

		public async Task<ServiceResponse<List<PrVm>>> GetAsyncByRingishoNo(string? ringNo)
		{
			var query = @"select ROW_NUMBER() OVER (ORDER BY pu.PR_HDR_NO) AS RowNum, pu.PR_HDR_NO as PRNo, pu.CREATE_BY as Section, pu.ISSUE_DATE as IssueDate, pu.ISSUE_BY as IssueBy, pu.RECEIVE_DATE as ReceiveDate,
				  pu.RING_NO as RingishoNo, pu.REMARK as Remark, pu.BUDGET_NO as BudgetNo,
				  p.PR_NO as PR_No, p.PO_NO as PONo, p.INVOICE_NO as InvoiceNo, p.INVOICE_DATE as InvoiceDate, dtl.total as totalAmount
				  from[PUR_REQ_SAP_HDR] as pu
				left  join
				  (SELECT distinct[PO_NO], PR_NO, INVOICE_NO, INVOICE_DATE
				  FROM[IT_PURCHASE_SYSTEM].[dbo].[PUR_PO_SAP]) as p on pu.PR_HDR_NO = p.PR_NO
				left join (select PR_HDR_NO ,SUM(AMOUNT) as total from PUR_REQ_SAP_DTL group by PR_HDR_NO )  as dtl on pu.PR_HDR_NO = dtl.PR_HDR_NO 
					where RING_NO  = @ringNo";


			using (var connection = _purchaseContext.CreateConnection())
			{
				var list = await connection.QueryAsync<PrVm>(query, new { ringNo });

				var listPO = await _context.PODataUploads.Where(p => list.Select(p => p.PRNo).Any(pr => pr == p.PR)).ToListAsync();
				var res = list.Select(p =>
				{
					var poItem = listPO.Where(x => x.PR == p.PRNo).Sum(p => p.Amount);
					return new PrVm()
					{
						PRNo = p.PRNo,
						BudgetNo = p.BudgetNo,
						InvoiceDate = p.InvoiceDate,
						InvoiceNo = p.InvoiceNo,
						IssueBy = p.IssueBy,
						IssueDate = p.IssueDate,
						PONo = p.PONo,
						ReceiveDate = p.ReceiveDate,
						Remark = p.Remark,
						RingishoNo = p.RingishoNo,
						RowNum = p.RowNum,
						Section = p.Section,
						Status = (Convert.ToInt32(p.totalAmount) == poItem) ? 1 : 0,
					};
				}).ToList();



				var result = new ServiceResponse<List<PrVm>>()
				{
					Data = res,
					IsSuccess = true,
					Message = "OK"
				};
				return result;
			}
		}

		public async Task<List<DataUploadVm>> GetAsyncPRByPO(List<DataUploadVm> uploadVms)
		{
			var poVms = uploadVms.Select(p => p.PO).ToList();
			// Tách thành 2 list
			var listNotNullPO = uploadVms.Where(p => !string.IsNullOrEmpty(p.PO)).ToList();
			// list null Ring
			var listNotNullRing = uploadVms.Where(p => !string.IsNullOrEmpty(p.Ring)).ToList();

			var listNullPO = uploadVms.Where(p => string.IsNullOrEmpty(p.PO) && !string.IsNullOrEmpty(p.Ring)).ToList();
			var listRing = listNullPO.Select(p => p.Ring).ToList();

			var query = @"select * from [PUR_PO_SAP] as pu where pu.PO_NO in @poVms";

			using (var connection = _purchaseContext.CreateConnection())
			{
				try
				{
					var list = await connection.QueryAsync<POVm>(query, new { poVms });

					List<DataUploadVm> listResult = new List<DataUploadVm>();

					foreach (var p in uploadVms)
					{

						DataUploadVm dataUploadVm = p;
						if (!string.IsNullOrEmpty(p.Ring) && string.IsNullOrEmpty(p.PO))
						{
							string pattern = @"AIH\d{2}-\d{3}";

							MatchCollection matches = Regex.Matches(dataUploadVm.Ring, pattern);
							string result = string.Join(",", matches);
							dataUploadVm.Ring = result;

							string[] listRingIn = dataUploadVm.Ring.Split(',');
							// get BudgetNo
							if (listRingIn.Count() > 1)
							{
								foreach (var item in listRingIn)
								{
									var getBudget = _context.FABudgets.Where(x => x.RingishoNo == dataUploadVm.Ring).ToList();
									if (getBudget.Count() > 0)
									{
										dataUploadVm.SAP = String.Join(",", getBudget.Select(x => x.No).DistinctBy(p => p));

									}
								}
							}
							else
							{
								var getBudget = _context.FABudgets.Where(x => x.RingishoNo == dataUploadVm.Ring).ToList();
								if (getBudget.Count() > 0)
								{
									dataUploadVm.SAP = String.Join(",", getBudget.Select(x => x.No));

								}
							}


							listResult.Add(dataUploadVm);
						}
						else
						{
							var getPr = list.Where(x => x.PO_NO == p.PO && x.PO_DTL_NO == p.Item.ToString()).ToList();
							if (getPr.Count() >= 1)
							{
								dataUploadVm.PR = getPr.FirstOrDefault().PR_NO;
								dataUploadVm.PR_No = int.Parse(getPr.FirstOrDefault().PR_DTL_NO);

								if (string.IsNullOrEmpty(dataUploadVm.Ring))
								{
									// Get ringNo
									var queryRing = @"select RING_NO as RingishoNo from [PUR_REQ_SAP_HDR] where PR_HDR_NO = @PR";
									var listqueryRing = await connection.QueryAsync<PrVm>(queryRing, new { dataUploadVm.PR });
									dataUploadVm.Ring = listqueryRing.FirstOrDefault().RingishoNo;

								}

								// get BudgetNo
								var getBudget = _context.FABudgets.Where(x => x.RingishoNo == dataUploadVm.Ring).ToList();
								if (getBudget.Count() > 0)
								{
									dataUploadVm.SAP = String.Join(",", getBudget.Select(x => x.No));

								}
							}
							else
							{
								Console.Write(p.PO + "\n");
							}
							listResult.Add(dataUploadVm);
						}
					}




					return listResult;

				}
				catch (Exception ex)
				{
					return null;

				}

				//var resultJoin = (from l in uploadVms
				//			 join i in list on l.PO equals i.PO_NO
				//			 select new { l, i }).ToList();


				//var result = uploadVms.Select(p => new DataUploadVm() 
				//{
				//	PO = p.l.PO,
				//	Item = p.l.Item,
				//	Amount = p.l.Amount,
				//	Currency = p.l.Currency,
				//	AmountInLocalCurrency = p.l.AmountInLocalCurrency,
				//	LocalCurrency = p.l.LocalCurrency,
				//	Quantity = p.l.Quantity,
				//	Unit = p.l.Unit,
				//	Status = p.l.Status,
				//	PR = p.i.PR_NO
				//}).ToList();


			}
		}

		public async Task<ServiceResponse<List<PRDetailVm>>> GetPRDetailByPRNo(string? prNo)
		{
			var query = @"select ROW_NUMBER() OVER (ORDER BY p.PR_HDR_NO) AS RowNum, p.PR_HDR_NO as PRNo, p.PRNO_SAP_DTL as PRNumber, p.SHORT_TEXT as Name, p.QTY as Qty, p.UNIT as Unit,
					  p.NET_PRICE as NetPrice, p.CUR as Cur, p.AMOUNT as Amount, p.DELIVERY_DATE as DeliveryDate,
					  p.PUCHASE_GROUP as PurchaseGroup,su.NAME_BRIEF as Supplier
					  from [PUR_REQ_SAP_DTL] as p
					  join [PURCHASE_SUPPLIER] as su on p.VENDOR = su.SUPPLIER_ID
					  where p.PR_HDR_NO = @prNo";
			using (var connection = _purchaseContext.CreateConnection())
			{
				var list = await connection.QueryAsync<PRDetailVm>(query, new { prNo });



				var listPO = await _context.PODataUploads.Where(p => p.PR == prNo).ToListAsync();


				var res = list.Select(p =>
				{
					var poItem = listPO.SingleOrDefault(x => x.Item == (p.PRNumber * 10));
					return new PRDetailVm()
					{
						PRNo = p.PRNo,
						Amount = p.Amount,
						Cur = p.Cur,
						DeliveryDate = p.DeliveryDate,
						Name = p.Name,
						NetPrice = p.NetPrice,
						PurchaseGroup = p.PurchaseGroup,
						Qty = p.Qty,
						RowNum = p.RowNum,
						Supplier = p.Supplier,
						Unit = p.Unit,
						Item = poItem?.Item,
						DocCur = poItem?.AmountInLocalCurrency,
						Curr = poItem?.LocalCurrency.ToString(),
						QtyPO = poItem?.Quantity,
						Status = (Convert.ToInt32(p.Qty) == poItem?.Quantity) ? 1 : 0,
					};
				}).ToList();

				var result = new ServiceResponse<List<PRDetailVm>>()
				{
					Data = res,
					IsSuccess = true,
					Message = "OK"
				};

				return result;
			}
		}
	}
}
