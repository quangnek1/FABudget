using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;
using ItemMaster.Server.Data;
using ItemMaster.Server.Data.Entities;
using ItemMaster.Shared.Model;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ItemMaster.Server.Services
{
	public class FABudgetServices : IFABudgetServices
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IUtilityService _utilityService;
		private readonly IPURCHASE_SYSTEM _IPURCHASE_SYSTEM;
		private readonly ILogger<FABudgetServices> _logger;
		private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
		public FABudgetServices(ApplicationDbContext context, IWebHostEnvironment environment, IUtilityService utilityService, IPURCHASE_SYSTEM iPURCHASE_SYSTEM, ILogger<FABudgetServices> logger)
		{
			_context = context;
			_hostingEnvironment = environment;
			_utilityService = utilityService;
			_IPURCHASE_SYSTEM = iPURCHASE_SYSTEM;
			_logger = logger;
			_logger.LogInformation("Start FABudgetServices.");
		}

		public async Task<ServiceResponse<FABudgetRequestEditVm>> GetDetailById(int id)
		{
			var query = await _context.FABudgets.Where(p => p.Id == id).SingleOrDefaultAsync();
			var queryRate = await _context.Rates.ToListAsync();
			var rateVm = queryRate.Select(o => new RateVm() { Current = o.Current, Currency = o.Currency, NextYear = o.NextYear, Status = o.Status }).ToList();

			var res = new FABudgetRequestEditVm();
			res.GLACCOUNT = query.GLACCOUNT;
			res.Id = query.Id;
			res.No = query.No;
			res.AccountText = query.AccountText;
			res.AssetName = query.AssetName;
			res.SectionCode = query.SectionCode;
			res.SectionName = query.SectionName;
			res.CurrentInvestmentAmountBudget = query.CurrentInvestmentAmountBudget; //Q
			res.CurrentInvestmentAmountUSD = query.CurrentInvestmentAmountUSD; // U
			res.DateCreate = query.DateCreate;
			res.DepreciationStartTimeBudget = query.DepreciationStartTimeBudget;
			res.DepreciationStartTimeEstimation = query.DepreciationStartTimeEstimation;
			res.FixedAssetsAccountedAmountBudget = query.FixedAssetsAccountedAmountBudget;
			res.EXRate = query.EXRate;// T
			res.FixedAssetsAccountedAmountUSD = query.FixedAssetsAccountedAmountUSD;
			res.Currency = query.Currency;
			res.PersonInCharge = query.PersonInCharge;
			res.PreviousInvestmentAmountBudget = query.PreviousInvestmentAmountBudget;
			res.PreviousInvestmentAmountUSD = query.PreviousInvestmentAmountUSD; // V
			res.PriorityRank = query.PriorityRank;
			res.PurchaseTimeBudget = query.PurchaseTimeBudget;
			res.PurchaseTimeEstimation = query.PurchaseTimeEstimation;
			res._initialBudgetRemaining = query.BudgetRemaining;
			res.Remark = query.Remark;
			res.CurrentInvestmentAmountEstimationUSD = query.CurrentInvestmentAmountEstimationUSD;
			res.PreviousInvestmentAmountEstimationUSD = query.PreviousInvestmentAmountEstimationUSD;
			res.FixedAssetsAccountedAmountEstimationUSD = query.FixedAssetsAccountedAmountEstimationUSD;
			res.RingishoNo = query.RingishoNo;
			res.AppropriatedBudgetNo = query.AppropriatedBudgetNo;
			res.BudgetRemaining = query.BudgetRemaining;
			res.BudgetSharing = await LoadBudgetSharing(query.No);
			res.BudgetCanUsing = !string.IsNullOrEmpty(query.AppropriatedBudgetNo) ? await LoadBudgetCanUsing(query.AppropriatedBudgetNo) : 0;
			res.Segment = query.Segment;
			res.RateVms = rateVm;
			res.Status = Convert.ToBoolean(query.Status);
			res.RingishoProcess = query.RingishoProcess;
			res.ListBudgetRemainingEdit = await LoadListBudgetRemainingEdit(query.No);

			var result = new ServiceResponse<FABudgetRequestEditVm>()
			{
				Data = res,
				IsSuccess = true,
				Message = "OK"
			};
			return result;
		}

		private async Task<List<FABudgetVm>?> LoadListBudgetRemainingEdit(int? no)
		{
			var query = await _context.FABudgets.Where(p => p.BudgetRemaining > 0 && p.CurrentInvestmentAmountUSD != null).ToListAsync();

			var queryUsing = await _context.BudgetSharings.Where(p => p.BudgetNo == no).ToListAsync();
			foreach (var item in queryUsing)
			{
				var existingBudget = query.Find(q => q.No == item.BudgetSharingNo);
				if (existingBudget != null)
				{
					existingBudget.BudgetRemaining += item.BudgetHasShare;
				}
				else
				{
					var newAdd = await _context.FABudgets.Where(p => p.No == item.BudgetSharingNo).SingleOrDefaultAsync();
					newAdd.BudgetRemaining = item.BudgetHasShare;
					query.Add(newAdd);
				}
			}
			var res = query.OrderBy(p=>p.No).Select((p, index) => new FABudgetVm()
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
				BudgetRemaining = p.BudgetRemaining,
				SectionCode = p.SectionCode,
				SectionName = p.SectionName,
				Segment = p.Segment,
				Rings = p.RingishoNo != null ? p.RingishoNo.Split(',', '、').ToList() : new List<string>(),
			}).ToList();

			return res;
			
		}

		private async Task<decimal?> LoadBudgetCanUsing(string? appropriatedBudgetNo)
		{
			string[] listBudgetSharingNo = appropriatedBudgetNo.Split(',');
			if (listBudgetSharingNo != null)
			{
				decimal? total = 0;
				foreach (var item in listBudgetSharingNo)
				{
					var query = await _context.FABudgets.Where(p => p.No == int.Parse(item)).ToListAsync();
					total += query?.SingleOrDefault()?.BudgetRemaining;
				}
				return total;
			}
			return 0;
		}
		private async Task<decimal?> LoadBudgetSharing(int? No)
		{
			var query = await _context.BudgetSharings.Where(p => p.BudgetSharingNo == No).ToListAsync();
			return query.Sum(p => p.BudgetHasShare);
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
		private async Task<decimal?> CalculateBudgetRemaining(decimal? _BudgetRemaining, int? No)
		{
			var query = await _context.BudgetSharings.Where(p => p.BudgetSharingNo == No).ToListAsync();
			if (query.Count() == 0)
			{
				return null;
			}
			var result = _BudgetRemaining - query.Sum(p => p.BudgetHasShare);
			return result;
		}
		public async Task<RateVm> GetCurrencynameAsync(string? currency)
		{
			var o = await _context.Rates.Where(p => p.Currency == currency).SingleOrDefaultAsync();
			var rateVm = new RateVm() { Current = o.Current, Currency = o.Currency, NextYear = o.NextYear, Status = o.Status };

			return rateVm;
		}
		public async Task<ServiceResponse<List<DataUploadVm>>> UploadDataAsync(IFormFile file)
		{
			string fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("yyMMddHHmmss") + Path.GetExtension(file.FileName);
			var path = $"{_hostingEnvironment.WebRootPath}\\Uploads\\{fileName}";
			await using FileStream fs = new(path, FileMode.Create);
			await file.CopyToAsync(fs);
			#region Khai bao license
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
			#endregion
			List<DataUploadVm> dataUploadRequestVm = new List<DataUploadVm>();
			using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
			{
				if (ValidateExcelTemplate(package) == false) return new ServiceResponse<List<DataUploadVm>>() { IsSuccess = false, Message = "NG", Data = new List<DataUploadVm>() };
				var ws = package.Workbook.Worksheets[0];

				int colCount = ws.Dimension.End.Column;  //get Column Count
				int rowCount = ws.Dimension.End.Row;     //get row count


				//	var str = Convert.ToString(ws.Cells[6, 8].Value);
				// Check tồn tại PO và Item đã upload trước đó hay chưa.
				var queryListDataUpload = await _context.PODataUploads.ToListAsync();
				bool status = false;
				for (int row = 2; row <= rowCount; row++)
				{
					if (!string.IsNullOrEmpty(Convert.ToString(ws.Cells[row, 1].Value)) || (string.IsNullOrEmpty(Convert.ToString(ws.Cells[row, 1].Value)) == true && !string.IsNullOrEmpty(Convert.ToString(ws.Cells[row, 10].Value))))
					{
						if (queryListDataUpload.Where(p => p.PO == Convert.ToString(ws.Cells[row, 1].Value) && p.Item == Convert.ToInt32(ws.Cells[row, 2].Value)).Count() > 0)
						{
							status = true;
						}

						dataUploadRequestVm.Add(new DataUploadVm()
						{
							PO = Convert.ToString(ws.Cells[row, 1].Value),
							Item = Convert.ToInt32(ws.Cells[row, 2].Value),
							Amount = Convert.ToDecimal(ws.Cells[row, 3].Value),
							Currency = Convert.ToString(ws.Cells[row, 4].Value),
							AmountInLocalCurrency = Convert.ToDecimal(ws.Cells[row, 5].Value),
							LocalCurrency = Convert.ToString(ws.Cells[row, 6].Value),
							Quantity = Convert.ToInt32(ws.Cells[row, 7].Value),
							Unit = Convert.ToString(ws.Cells[row, 8].Value),
							LaborCost = Convert.ToInt32(ws.Cells[row, 9].Value),
							Ring = Convert.ToString(ws.Cells[row, 10].Value),
							Status = status
						});
					}

				}
				var res = await _IPURCHASE_SYSTEM.GetAsyncPRByPO(dataUploadRequestVm);

				return new ServiceResponse<List<DataUploadVm>>() { IsSuccess = true, Message = "OK", Data = res };
			}
		}
		public bool ValidateExcelTemplate(ExcelPackage package)
		{
			string[] expectedColumnNames = new string[] { "Purchasing Document", "Item", "Amount in Doc. Curr.", "Document Currency", "Amount in Local Currency", "Local Currency", "Quantity", "Base Unit of Measure", "Labor cost", "Ring", "SAP code", "Remark" };
			ExcelWorkbook workbook = package.Workbook;
			if (workbook.Worksheets.Count == 0)
			{
				return false; // Không có sheet nào
			}

			ExcelWorksheet worksheet = workbook.Worksheets[0];
			if (worksheet.Name != "Data")
			{
				return false; // Tên sheet không đúng
			}

			if (worksheet.Dimension.Columns != 12)
			{
				return false; // Số lượng cột không đúng
			}

			// Kiểm tra tên cột
			for (int col = 1; col <= 12; col++)
			{
				if (worksheet.Cells[1, col].Text != expectedColumnNames[col - 1])
				{
					return false; // Tên cột không đúng
				}
			}

			return true; // Template đúng
		}
		public async Task<ServiceResponse<byte[]>> Export()
		{
			string sWebRootFolder = _hostingEnvironment.WebRootPath;
			string fileName = @"FA Budget.xlsx";
			FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, fileName));

			using (var package = CreateExport(file))
			{
				var result = new ServiceResponse<byte[]>();
				result.Data = package.GetAsByteArray();
				result.IsSuccess = true;
				result.Message = XlsxContentType;
				return result;
			}

		}
		private ExcelPackage CreateExport(FileInfo file)
		{
			var query2 = from f in _context.FABudgets
						 select f;

			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

			//var package = new ExcelPackage(file);
			//return package;
			var package = new ExcelPackage(file);
			ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
			var ws = package.Workbook.Worksheets["50期"];
			int count = query2.Count();
			int STT = 1;
			int row = 8;
			foreach (var item in query2)
			{
				ws.Cells["C" + row].Value = item.No;
				ws.Cells["D" + row].Value = item.GLACCOUNT;
				ws.Cells["E" + row].Value = item.AccountText;
				ws.Cells["F" + row].Value = item.PriorityRank;
				ws.Cells["G" + row].Value = item.AssetName;
				ws.Cells["H" + row].Value = item.PersonInCharge;
				ws.Cells["I" + row].Value = item.SectionCode;
				ws.Cells["J" + row].Value = item.Segment;
				ws.Cells["K" + row].Value = item.SectionName;
				ws.Cells["L" + row].Value = string.IsNullOrWhiteSpace(item.PurchaseTimeBudget.ToString()) ? "" : Convert.ToDateTime(item.PurchaseTimeBudget);
				ws.Cells["M" + row].Value = string.IsNullOrWhiteSpace(item.DepreciationStartTimeBudget.ToString()) ? "" : Convert.ToDateTime(item.DepreciationStartTimeBudget);
				ws.Cells["N" + row].Value = string.IsNullOrWhiteSpace(item.PurchaseTimeEstimation.ToString()) ? "" : Convert.ToDateTime(item.PurchaseTimeEstimation);
				ws.Cells["O" + row].Value = string.IsNullOrWhiteSpace(item.DepreciationStartTimeEstimation.ToString()) ? "" : Convert.ToDateTime(item.DepreciationStartTimeEstimation);
				ws.Cells["P" + row].Value = item.Currency;
				ws.Cells["Q" + row].Value = item.CurrentInvestmentAmountBudget;
				ws.Cells["R" + row].Value = item.PreviousInvestmentAmountBudget;
				ws.Cells["S" + row].Value = item.FixedAssetsAccountedAmountBudget;
				ws.Cells["T" + row].Value = item.EXRate;
				ws.Cells["U" + row].Value = item.CurrentInvestmentAmountUSD;
				ws.Cells["V" + row].Value = item.PreviousInvestmentAmountUSD;
				ws.Cells["W" + row].Value = item.FixedAssetsAccountedAmountUSD;
				ws.Cells["X" + row].Value = item.CurrentInvestmentAmountEstimationUSD;
				ws.Cells["Y" + row].Value = item.PreviousInvestmentAmountEstimationUSD;
				ws.Cells["Z" + row].Value = item.FixedAssetsAccountedAmountEstimationUSD;
				ws.Cells["AA" + row].Value = item.RingishoNo;
				ws.Cells["AB" + row].Value = item.AppropriatedBudgetNo;
				ws.Cells["AC" + row].Value = item.BudgetRemaining;
				ws.Cells["AD" + row].Value = item.Remark;
				row++;
				STT++;
			}
			return package;
		}

		public async Task<ServiceResponse<DataVm>> GetDataAsync()
		{
			DataVm? dataVm = new DataVm();
			var userList = await _utilityService.GetListUserAsync();

			var querySections = await _context.Sections.ToListAsync();
			var queryRates = await _context.Rates.ToListAsync();
			var queryAccounts = await _context.Accounts.ToListAsync();
			var queryGroupEmailSends = await _context.GroupEmailSends.ToListAsync();

			var queryHistory = await _context.FABudgetHistories.OrderByDescending(p => p.Id).ToListAsync();
			if (queryHistory.Count() > 0)
			{
				var historyVms = (from l in queryHistory
								  join u in userList on l.ChangedBy equals u.UserId
								  select new { l.Id, l.No, l.FABudgetId, u.Fullname, l.ChangeDate, l.OldValue, l.NewValue }).ToList();
				dataVm.FAbudgetHistoryListVms = historyVms.Select(p => new FAbudgetHistoryListVm() { Id = p.Id, No = p.No, FABudgetId = p.FABudgetId, ChangedBy = p.Fullname, ChangeDate = p.ChangeDate, OldValue = p.OldValue, NewValue = p.NewValue }).ToList();

			}

			dataVm.AccountVms = queryAccounts.Select(p => new AccountVm() { AccountText = p.AccountText, GLACCOUNT = Convert.ToInt32(p.GLACCOUNT), Status = p.Status }).ToList();
			dataVm.RateVms = queryRates.Select(p => new RateVm() { Currency = p.Currency, Current = p.Current, NextYear = p.NextYear, Status = p.Status }).ToList();
			dataVm.SectionVms = querySections.Select(p => new SectionVm() { SectionCode = p.SectionCode, SectionName = p.SectionName, Segment = p.Segment, Status = p.Status }).ToList();
			dataVm.PriorityVm = new List<int>() { 1, 2, 3, 4 };
			dataVm.GroupEmailSendsVms = queryGroupEmailSends.Select(p => new GroupEmailSendsVms() { Id = p.Id, Content = p.Content }).ToList();

			var result = new ServiceResponse<DataVm>()
			{
				Data = dataVm,
				IsSuccess = true,
				Message = "OK"
			};
			return result;
		}

		public async Task<ServiceResponse<HttpResponseMessage>> PutAsync(FABudgetRequestEditVm request)
		{
			using (var transaction = _context.Database.BeginTransaction())
			{
				try
				{
					var newBudget = new FABudget();
					newBudget.Id = request.Id;
					newBudget.GLACCOUNT = request.GLACCOUNT;
					newBudget.No = request.No;
					newBudget.AccountText = request.AccountText;
					newBudget.AppropriatedBudgetNo = request.AppropriatedBudgetNo;
					newBudget.AssetName = request.AssetName;
					newBudget.CurrentInvestmentAmountBudget = request.CurrentInvestmentAmountBudget; //Q
					newBudget.CurrentInvestmentAmountUSD = request.CurrentInvestmentAmountUSD; // U
					newBudget.DateCreate = request.DateCreate;
					newBudget.DepreciationStartTimeBudget = request.DepreciationStartTimeBudget;
					newBudget.DepreciationStartTimeEstimation = request.DepreciationStartTimeEstimation;
					newBudget.FixedAssetsAccountedAmountBudget = request.FixedAssetsAccountedAmountBudget;
					newBudget.FixedAssetsAccountedAmountUSD = request.FixedAssetsAccountedAmountUSD; // W
					newBudget.EXRate = request.EXRate;// T
					newBudget.FixedAssetsAccountedAmountUSD = request.FixedAssetsAccountedAmountUSD;
					newBudget.Currency = request.Currency;
					newBudget.PersonInCharge = request.PersonInCharge;
					newBudget.PreviousInvestmentAmountBudget = request.PreviousInvestmentAmountBudget;
					newBudget.PreviousInvestmentAmountUSD = request.PreviousInvestmentAmountUSD; // V
					newBudget.PriorityRank = request.PriorityRank;
					newBudget.PurchaseTimeBudget = request.PurchaseTimeBudget;
					newBudget.PurchaseTimeEstimation = request.PurchaseTimeEstimation;
					newBudget.Remark = request.Remark;
					newBudget.CurrentInvestmentAmountEstimationUSD = request.CurrentInvestmentAmountEstimationUSD;
					newBudget.PreviousInvestmentAmountEstimationUSD = request.PreviousInvestmentAmountEstimationUSD;
					newBudget.FixedAssetsAccountedAmountEstimationUSD = request.FixedAssetsAccountedAmountEstimationUSD;
					newBudget.RingishoNo = request.RingishoNo;
					newBudget.BudgetRemaining = request.BudgetRemaining;
					newBudget.SectionCode = request.SectionCode;
					newBudget.SectionName = request.SectionName;
					newBudget.Segment = request.Segment;
					newBudget.LastModified = DateTime.Now;
					newBudget.Status = request.Status;
					newBudget.RingishoProcess = request.RingishoProcess;

					//	var faBudget = await _context.FABudgets.SingleOrDefaultAsync(p => p.Id == request.Id);
					var faBudget = await _context.FABudgets.FindAsync(request.Id);
					#region Comment
					//faBudget.GLACCOUNT = request.GLACCOUNT;
					//faBudget.No = request.No;
					//faBudget.AccountText = request.AccountText;
					//faBudget.AppropriatedBudgetNo = request.AppropriatedBudgetNo;
					//faBudget.AssetName = request.AssetName;
					//faBudget.BudgetRemaining = request.BudgetRemaining;
					//faBudget.CurrentInvestmentAmountBudget = request.CurrentInvestmentAmountBudget; //Q
					//faBudget.CurrentInvestmentAmountUSD = request.CurrentInvestmentAmountUSD; // U
					//faBudget.DateCreate = request.DateCreate;
					//faBudget.DepreciationStartTimeBudget = request.DepreciationStartTimeBudget;
					//faBudget.DepreciationStartTimeEstimation = request.DepreciationStartTimeEstimation;
					//faBudget.FixedAssetsAccountedAmountBudget = request.FixedAssetsAccountedAmountBudget;
					//faBudget.FixedAssetsAccountedAmountUSD = request.FixedAssetsAccountedAmountUSD; // W
					//faBudget.EXRate = request.EXRate;// T
					//faBudget.FixedAssetsAccountedAmountUSD = request.FixedAssetsAccountedAmountUSD;
					//faBudget.Currency = request.Currency;
					//faBudget.PersonInCharge = request.PersonInCharge;
					//faBudget.PreviousInvestmentAmountBudget = request.PreviousInvestmentAmountBudget;
					//faBudget.PreviousInvestmentAmountUSD = request.PreviousInvestmentAmountUSD; // V
					//faBudget.PriorityRank = request.PriorityRank;
					//faBudget.PurchaseTimeBudget = request.PurchaseTimeBudget;
					//faBudget.PurchaseTimeEstimation = request.PurchaseTimeEstimation;
					//faBudget.Remark = request.Remark;
					//faBudget.CurrentInvestmentAmountEstimationUSD = request.CurrentInvestmentAmountEstimationUSD;
					//faBudget.PreviousInvestmentAmountEstimationUSD = request.PreviousInvestmentAmountEstimationUSD;
					//faBudget.FixedAssetsAccountedAmountEstimationUSD = request.FixedAssetsAccountedAmountEstimationUSD;
					//faBudget.RingishoNo = request.RingishoNo;
					//faBudget.SectionCode = request.SectionCode;
					//faBudget.SectionName = request.SectionName;
					//faBudget.Segment = request.Segment;
					//faBudget.LastModified = DateTime.Now;
					#endregion
					// Create an instance of FABudgetHistory to store change details
					var userLogin = await _utilityService.GetUserAsync();
					// Tính toán xem có dùng budget của thằng khác hay không.
					var totalhasShared = await _context.BudgetSharings.Where(p => p.BudgetSharingNo == request.No).Select(p => p.BudgetHasShare).SumAsync();
					var _remaning = newBudget.CurrentInvestmentAmountUSD - newBudget.CurrentInvestmentAmountEstimationUSD - totalhasShared;
					// Update lại Remaining 
					if (request.AppropriatedBudgetNo != null)
					{

						// Bước 1: Tìm đối tượng faBudget từ database dựa trên No
						var faBudgetFind = await _context.FABudgets.SingleOrDefaultAsync(b => b.No == request.No);
						if (faBudgetFind == null)
						{
							return new ServiceResponse<HttpResponseMessage>
							{
								Data = null,
								IsSuccess = false,
								Message = "Budget not found"
							};
						}

						// Bước 2: Khôi phục lại BudgetRemaining từ BudgetSharing
						var budgetSharings = await _context.BudgetSharings.Where(bs => bs.BudgetNo == faBudget.No).ToListAsync();
						foreach (var sharing in budgetSharings)
						{
							var faBudgetUpdate = await _context.FABudgets.SingleOrDefaultAsync(b => b.No == sharing.BudgetSharingNo);
							if (faBudgetUpdate != null)
							{
								faBudgetUpdate.BudgetRemaining += sharing.BudgetHasShare;  // Khôi phục lại số dư đã bị trừ
								_context.FABudgets.Update(faBudgetUpdate);
							}
						}

						// Bước 3: Xóa các bản ghi BudgetSharing trước đó
						_context.BudgetSharings.RemoveRange(budgetSharings);

						// Bước 4: Cập nhật lại giá trị mới cho faBudget
						newBudget.CurrentInvestmentAmountEstimationUSD = request.CurrentInvestmentAmountEstimationUSD;
						newBudget.LastModified = DateTime.Now;

						// Bước 5: Thực hiện lại quá trình phân bổ ngân sách
						string[] appropriatedBudgetNos = request.AppropriatedBudgetNo.Split(',');
						decimal? budgetNeeded = Math.Abs(Convert.ToDecimal(newBudget.CurrentInvestmentAmountEstimationUSD - (newBudget.CurrentInvestmentAmountUSD ?? 0)));
						if (_remaning < 0)
						{
							foreach (var budgetNo in appropriatedBudgetNos)
							{
								var faBudgetUpdate = await _context.FABudgets.SingleOrDefaultAsync(b => b.No == int.Parse(budgetNo));
								if (faBudgetUpdate == null) continue;

								var budgetRemaining = faBudgetUpdate.BudgetRemaining;
								// Tính toán BudgetHasShare trước khi thay đổi budgetNeeded
								var budgetHasShare = (budgetRemaining < budgetNeeded) ? budgetRemaining : budgetNeeded;

								if (budgetNeeded > budgetRemaining)
								{
									budgetNeeded -= budgetRemaining;
									faBudgetUpdate.BudgetRemaining = 0;
								}
								else
								{
									faBudgetUpdate.BudgetRemaining -= budgetNeeded;
									budgetNeeded = 0;
								}

								// Cập nhật BudgetSharing mới
								var budgetSharing = new BudgetSharing
								{
									BudgetNo = Convert.ToInt32(faBudget.No),
									BudgetSharingNo = Convert.ToInt32(faBudgetUpdate.No),
									BudgetHasShare = budgetHasShare
								};
								await _context.BudgetSharings.AddAsync(budgetSharing);
								_context.FABudgets.Update(faBudgetUpdate);


							}
						}
						if (_remaning == null && newBudget.CurrentInvestmentAmountUSD == null)
						{
							foreach (var budgetNo in appropriatedBudgetNos)
							{
								var faBudgetUpdate = await _context.FABudgets.SingleOrDefaultAsync(b => b.No == int.Parse(budgetNo));
								if (faBudgetUpdate == null) continue;

								var budgetRemaining = faBudgetUpdate.BudgetRemaining;
								// Tính toán BudgetHasShare trước khi thay đổi budgetNeeded
								var budgetHasShare = (budgetRemaining < budgetNeeded) ? budgetRemaining : budgetNeeded;

								if (budgetNeeded > budgetRemaining)
								{
									budgetNeeded -= budgetRemaining;
									faBudgetUpdate.BudgetRemaining = 0;
								}
								else
								{
									faBudgetUpdate.BudgetRemaining -= budgetNeeded;
									budgetNeeded = 0;
								}

								// Cập nhật BudgetSharing mới
								var budgetSharing = new BudgetSharing
								{
									BudgetNo = Convert.ToInt32(faBudget.No),
									BudgetSharingNo = Convert.ToInt32(faBudgetUpdate.No),
									BudgetHasShare = budgetHasShare
								};
								await _context.BudgetSharings.AddAsync(budgetSharing);
								_context.FABudgets.Update(faBudgetUpdate);


							}
						}
					}
					else
					{
						// Check trong database xem tồn tại chưa. Vì có thể data bị lỗi. cần khôi phục.
						// Bước 1: Tìm đối tượng faBudget từ database dựa trên No
						var faBudgetFind = await _context.FABudgets.SingleOrDefaultAsync(b => b.No == request.No);
						if (faBudgetFind != null)
						{
							// Bước 2: Khôi phục lại BudgetRemaining từ BudgetSharing
							var budgetSharings = await _context.BudgetSharings.Where(bs => bs.BudgetNo == faBudget.No).ToListAsync();
							if (budgetSharings.Count() > 0)
							{
								foreach (var sharing in budgetSharings)
								{
									var faBudgetUpdate = await _context.FABudgets.SingleOrDefaultAsync(b => b.No == sharing.BudgetSharingNo);
									if (faBudgetUpdate != null)
									{
										faBudgetUpdate.BudgetRemaining += sharing.BudgetHasShare;  // Khôi phục lại số dư đã bị trừ
										_context.FABudgets.Update(faBudgetUpdate);
									}
								}
							}

							// Bước 3: Xóa các bản ghi BudgetSharing trước đó
							_context.BudgetSharings.RemoveRange(budgetSharings);

							// Bước 4: Cập nhật lại giá trị mới cho faBudget
							newBudget.CurrentInvestmentAmountEstimationUSD = request.CurrentInvestmentAmountEstimationUSD;
							newBudget.LastModified = DateTime.Now;
						}

					}

					var budgetHistory = new FABudgetHistory
					{
						FABudgetId = faBudget.Id,
						No = faBudget.No,
						ChangeDate = DateTime.Now,
						ChangedBy = userLogin.Id,
						OldValue = ObjectSerializer.SerializeObject(faBudget), // Before update
						NewValue = ObjectSerializer.SerializeObject(newBudget) // After update
					};

					// Update the existing budget with new values
					_context.Attach(faBudget);
					_context.Entry(faBudget).CurrentValues.SetValues(newBudget);
					await _context.SaveChangesAsync();
					//	_context.FABudgets.Update(faBudget);

					// Save change history to FABudgetHistory table
					_context.FABudgetHistories.Add(budgetHistory);

					var res = await _context.SaveChangesAsync();
					var responseMessage = new HttpResponseMessage();

					if (res > 0)
					{
						responseMessage.StatusCode = HttpStatusCode.OK;
						transaction.Commit();
						_logger.LogInformation($"FABudget with ID {faBudget.Id} updated successfully");
					}
					else
					{
						responseMessage.StatusCode = HttpStatusCode.BadRequest;
						transaction.Rollback();
					}
					return new ServiceResponse<HttpResponseMessage>
					{
						Data = responseMessage,
						IsSuccess = true,
						Message = "Update successful"
					};
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					_logger.LogError(ex, "Error updating FABudget with ID {Id}", request.Id);
					return new ServiceResponse<HttpResponseMessage>
					{
						Data = null,
						IsSuccess = false,
						Message = $"Update failed: {ex.Message}"
					};
				}
			}

		}

		#region Comment Code
		//public async Task<ServiceResponse<HttpResponseMessage>> PostAsync(FABudgetRequestVm request)
		//{

		//	using (var transaction = _context.Database.BeginTransaction())
		//	{
		//		try
		//		{
		//			var faBudget = new FABudget();

		//			faBudget.GLACCOUNT = request.GLACCOUNT;
		//			faBudget.No = request.No;
		//			faBudget.AccountText = request.AccountText;
		//			faBudget.AppropriatedBudgetNo = request.AppropriatedBudgetNo;
		//			faBudget.AssetName = request.AssetName;
		//			faBudget.BudgetRemaining = request.BudgetRemaining;
		//			faBudget.CurrentInvestmentAmountBudget = request.CurrentInvestmentAmountBudget; //Q
		//			faBudget.CurrentInvestmentAmountUSD = request.CurrentInvestmentAmountUSD; // U
		//			faBudget.DateCreate = request.DateCreate;
		//			faBudget.DepreciationStartTimeBudget = request.DepreciationStartTimeBudget;
		//			faBudget.DepreciationStartTimeEstimation = request.DepreciationStartTimeEstimation;
		//			faBudget.FixedAssetsAccountedAmountBudget = request.FixedAssetsAccountedAmountBudget;
		//			faBudget.FixedAssetsAccountedAmountUSD = request.FixedAssetsAccountedAmountUSD; // W
		//			faBudget.EXRate = request.EXRate;// T
		//			faBudget.FixedAssetsAccountedAmountUSD = request.FixedAssetsAccountedAmountUSD;
		//			faBudget.Currency = request.Currency;
		//			faBudget.PersonInCharge = request.PersonInCharge;
		//			faBudget.PreviousInvestmentAmountBudget = request.PreviousInvestmentAmountBudget;
		//			faBudget.PreviousInvestmentAmountUSD = request.PreviousInvestmentAmountUSD; // V
		//			faBudget.PriorityRank = request.PriorityRank;
		//			faBudget.PurchaseTimeBudget = request.PurchaseTimeBudget;
		//			faBudget.PurchaseTimeEstimation = request.PurchaseTimeEstimation;
		//			faBudget.Remark = request.Remark;
		//			faBudget.CurrentInvestmentAmountEstimationUSD = request.CurrentInvestmentAmountEstimationUSD;
		//			faBudget.PreviousInvestmentAmountEstimationUSD = request.PreviousInvestmentAmountEstimationUSD;
		//			faBudget.FixedAssetsAccountedAmountEstimationUSD = request.FixedAssetsAccountedAmountEstimationUSD;
		//			faBudget.RingishoNo = request.RingishoNo;
		//			faBudget.SectionCode = request.SectionCode;
		//			faBudget.SectionName = request.SectionName;
		//			faBudget.Segment = request.Segment;
		//			faBudget.DateCreate = DateTime.Now;
		//			faBudget.LastModified = DateTime.Now;
		//			faBudget.Status = true;

		//			_context.FABudgets.Add(faBudget);

		//			// Update BudgetRemaining = 0 theo AppropriatedBudgetNo
		//			try
		//			{
		//				string[] _value = request.AppropriatedBudgetNo.Split(',');
		//				decimal? _budgetNeeded = faBudget.CurrentInvestmentAmountEstimationUSD;
		//				foreach (var item in _value)
		//				{
		//					BudgetSharing budgetSharing = new BudgetSharing();
		//					budgetSharing.BudgetNo = faBudget.No.ToString();
		//					budgetSharing.BudgetSharingNo = item.ToString();
		//					await _context.BudgetSharings.AddAsync(budgetSharing);

		//					var faBudgetUpdate = await _context.FABudgets.Where(b => b.No == int.Parse(item)).SingleOrDefaultAsync();
		//					var _BudgetRemaining = faBudgetUpdate.BudgetRemaining;

		//					if (_budgetNeeded > _BudgetRemaining)
		//					{
		//						_budgetNeeded = _budgetNeeded - _BudgetRemaining;
		//						faBudgetUpdate.BudgetRemaining = 0;
		//						_context.FABudgets.Update(faBudgetUpdate);
		//					}
		//					else
		//					{
		//						faBudgetUpdate.BudgetRemaining = _BudgetRemaining - _budgetNeeded;
		//						_context.FABudgets.Update(faBudgetUpdate);
		//					}


		//				}
		//			}
		//			catch (Exception) { }

		//			var res = await _context.SaveChangesAsync();
		//			var responseMessage = new HttpResponseMessage();
		//			if (res > 0)
		//			{
		//				responseMessage.StatusCode = HttpStatusCode.OK;
		//				transaction.Commit();
		//			}
		//			else
		//			{
		//				responseMessage.StatusCode = HttpStatusCode.BadRequest;
		//				transaction.Rollback();

		//			}
		//			var result = new ServiceResponse<HttpResponseMessage>()
		//			{
		//				Data = responseMessage,
		//				IsSuccess = true,
		//				Message = "OK"
		//			};
		//			return result;
		//		}
		//		catch (Exception)
		//		{
		//			transaction.Rollback();

		//		}
		//	}


		//}
		#endregion

		public async Task<ServiceResponse<HttpResponseMessage>> PostAsync(FABudgetRequestVm request)
		{
			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					var faBudget = new FABudget
					{
						GLACCOUNT = request.GLACCOUNT,
						No = request.No,
						AccountText = request.AccountText,
						AppropriatedBudgetNo = request.AppropriatedBudgetNo,
						AssetName = request.AssetName,
						CurrentInvestmentAmountBudget = request.CurrentInvestmentAmountBudget,
						CurrentInvestmentAmountUSD = request.CurrentInvestmentAmountUSD,
						DepreciationStartTimeBudget = request.DepreciationStartTimeBudget,
						DepreciationStartTimeEstimation = request.DepreciationStartTimeEstimation,
						FixedAssetsAccountedAmountBudget = request.FixedAssetsAccountedAmountBudget,
						FixedAssetsAccountedAmountUSD = request.FixedAssetsAccountedAmountUSD,
						EXRate = request.EXRate,
						Currency = request.Currency,
						PersonInCharge = request.PersonInCharge,
						PreviousInvestmentAmountBudget = request.PreviousInvestmentAmountBudget,
						PreviousInvestmentAmountUSD = request.PreviousInvestmentAmountUSD,
						PriorityRank = request.PriorityRank,
						PurchaseTimeBudget = request.PurchaseTimeBudget,
						PurchaseTimeEstimation = request.PurchaseTimeEstimation,
						Remark = request.Remark,
						CurrentInvestmentAmountEstimationUSD = request.CurrentInvestmentAmountEstimationUSD,
						PreviousInvestmentAmountEstimationUSD = request.PreviousInvestmentAmountEstimationUSD,
						FixedAssetsAccountedAmountEstimationUSD = request.FixedAssetsAccountedAmountEstimationUSD,
						RingishoNo = request.RingishoNo,
						BudgetRemaining = 0,
						SectionCode = request.SectionCode,
						SectionName = request.SectionName,
						Segment = request.Segment,
						DateCreate = DateTime.Now,
						LastModified = DateTime.Now,
						Status = true,
						RingishoProcess = false
					};

					_context.FABudgets.Add(faBudget);

					// Update BudgetRemaining = 0 theo AppropriatedBudgetNo
					if (!string.IsNullOrEmpty(request.AppropriatedBudgetNo))
					{
						string[] budgetNos = request.AppropriatedBudgetNo.Split(',');
						decimal? budgetNeeded = faBudget.CurrentInvestmentAmountEstimationUSD;

						foreach (var budgetNo in budgetNos)
						{
							var faBudgetUpdate = await _context.FABudgets.Where(b => b.No == Convert.ToInt32(budgetNo)).SingleOrDefaultAsync();

							if (faBudgetUpdate != null)
							{
								var budgetRemaining = faBudgetUpdate.BudgetRemaining ?? 0;

								// Tính toán BudgetHasShare trước khi thay đổi budgetNeeded
								var budgetHasShare = (budgetRemaining < budgetNeeded) ? budgetRemaining : budgetNeeded;

								if (budgetNeeded > budgetRemaining)
								{
									budgetNeeded -= budgetRemaining;
									faBudgetUpdate.BudgetRemaining = 0;
								}
								else
								{
									faBudgetUpdate.BudgetRemaining -= budgetNeeded;
									budgetNeeded = 0;
								}

								// Tạo một BudgetSharing mới cho mỗi lần lặp
								var budgetSharing = new BudgetSharing
								{
									BudgetNo = Convert.ToInt32(faBudget.No),  // No của budget hiện tại
									BudgetSharingNo = Convert.ToInt32(budgetNo),// No từ danh sách
									BudgetHasShare = budgetHasShare
								};

								_context.BudgetSharings.Add(budgetSharing);

								_context.FABudgets.Update(faBudgetUpdate);

								if (budgetNeeded == 0) break;
							}
						}
					}

					var result = await _context.SaveChangesAsync();

					var userLogin = await _utilityService.GetUserAsync();
					var budgetHistory = new FABudgetHistory
					{
						FABudgetId = faBudget.Id,
						No = faBudget.No,
						ChangeDate = DateTime.Now,
						ChangedBy = userLogin.Id,
						OldValue = ObjectSerializer.SerializeObject(new FABudget()), // Before update
						NewValue = ObjectSerializer.SerializeObject(faBudget) // After update
					};
					_context.FABudgetHistories.Add(budgetHistory);
					if (result > 0)
					{
						await transaction.CommitAsync();
						return new ServiceResponse<HttpResponseMessage>
						{
							Data = new HttpResponseMessage(HttpStatusCode.OK),
							IsSuccess = true,
							Message = "OK"
						};
					}

					await transaction.RollbackAsync();
					return new ServiceResponse<HttpResponseMessage>
					{
						Data = new HttpResponseMessage(HttpStatusCode.BadRequest),
						IsSuccess = false,
						Message = "Lưu dữ liệu thất bại"
					};
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					return new ServiceResponse<HttpResponseMessage>
					{
						Data = new HttpResponseMessage(HttpStatusCode.InternalServerError),
						IsSuccess = false,
						Message = $"Có lỗi xảy ra: {ex.Message}"
					};
				}
			}
		}

		public async Task<ServiceResponse<HttpResponseMessage>> SavePOUploadDataAsync(List<DataUploadVm> dataUploadVms)
		{
			var list = dataUploadVms;
			foreach (var item in list)
			{
				var po = new PODataUpload();
				po.PR = item.PR;
				po.PR_No = item.PR_No;
				po.PO = item.PO;
				po.Item = item.Item;
				po.Amount = item.Amount;
				po.Currency = item.Currency;
				po.AmountInLocalCurrency = item.AmountInLocalCurrency;
				po.LocalCurrency = item.LocalCurrency;
				po.Quantity = item.Quantity;
				po.Unit = item.Unit;
				po.LaborCost = item.LaborCost;
				po.Ring = item.Ring;
				po.SAP = item.SAP;

				await _context.PODataUploads.AddAsync(po);
			}
			await _context.SaveChangesAsync();
			return new ServiceResponse<HttpResponseMessage>
			{
				Data = new HttpResponseMessage(HttpStatusCode.OK),
				IsSuccess = true,
				Message = "OK"
			};
		}

		public async Task<ServiceResponse<byte[]>> ExportPO()
		{
			string sWebRootFolder = _hostingEnvironment.WebRootPath;
			string fileName = @"POExport.xlsx";
			FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, fileName));

			using (var package = CreateExportPO(file))
			{
				var result = new ServiceResponse<byte[]>();
				result.Data = package.GetAsByteArray();
				result.IsSuccess = true;
				result.Message = XlsxContentType;
				return result;
			}
		}
		private ExcelPackage CreateExportPO(FileInfo file)
		{
			var query = _context.PODataUploads.ToList();

			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

			//var package = new ExcelPackage(file);
			//return package;
			var package = new ExcelPackage(file);
			ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
			var ws = package.Workbook.Worksheets["Data"];
			int count = query.Count();
			int row = 2;

			var ringList = query.DistinctBy(x => x.Ring).Select(p => p.Ring).ToList();
			Dictionary<string, int> resultCheck = new Dictionary<string, int>();
			//	resultCheck.Add("ring", 0);
			foreach (var item in ringList)
			{
				decimal? totalBudget = 0;
				decimal? totalAmountUpload = 0;

				string pattern = @"AIH\d{2}-\d{3}";
				MatchCollection matches = Regex.Matches(item, pattern);
				string result = string.Join(",", matches);
				string[] listRingIn = result.Split(',');

				if (listRingIn.Count() > 1)
				{
					foreach (var i in listRingIn)
					{
						totalBudget += _context.FABudgets.Where(p => p.RingishoNo == item).Sum(o => o.CurrentInvestmentAmountEstimationUSD);
						totalAmountUpload += query.Where(p => p.Ring == item).Sum(o => o.AmountInLocalCurrency);

					}
				}
				else
				{
					totalBudget = _context.FABudgets.Where(p => p.RingishoNo == item).Sum(o => o.CurrentInvestmentAmountEstimationUSD);
					totalAmountUpload = query.Where(p => p.Ring == item).Sum(o => o.AmountInLocalCurrency);
				}

				resultCheck.Add(item, RingCheck(totalBudget, totalAmountUpload));
			}

			foreach (var item in query)
			{
				//	var checkPO = _IPURCHASE_SYSTEM.CheckPO(item);
				var check = resultCheck.Where(p => p.Key == item.Ring).FirstOrDefault();

				if (check.Value == 1)
				{
					using (var range = worksheet.Cells[row, 1, row, 10])
					{
						range.Style.Fill.PatternType = ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
					}
				}
				if (check.Value == 2)
				{
					using (var range = worksheet.Cells[row, 1, row, 10])
					{
						range.Style.Fill.PatternType = ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
					}
				}
				if (check.Value == 0)
				{
					using (var range = worksheet.Cells[row, 1, row, 10])
					{
						range.Style.Fill.PatternType = ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.Red);
					}
				}

				ws.Cells["A" + row].Value = item.PO;
				ws.Cells["B" + row].Value = item.Item;
				ws.Cells["C" + row].Value = item.Amount;
				ws.Cells["D" + row].Value = item.Currency;
				ws.Cells["E" + row].Value = item.AmountInLocalCurrency;
				ws.Cells["F" + row].Value = item.LocalCurrency;
				ws.Cells["G" + row].Value = item.Quantity;
				ws.Cells["H" + row].Value = item.Unit;
				ws.Cells["I" + row].Value = item.LaborCost;
				ws.Cells["J" + row].Value = item.Ring;
				ws.Cells["K" + row].Value = item.SAP;
				row++;
			}
			return package;
		}

		private int RingCheck(decimal? totalBudget, decimal? totalAmountUpload)
		{
			if (totalBudget == totalAmountUpload)
			{
				return 1;
			}
			else if (totalBudget > totalAmountUpload)
			{
				return 2;
			}
			else
			{
				return 0;
			}
		}

		public async Task<bool> UpdateDataMonthly()
		{
			var currentYear = DateTime.Now.Year;
			var currentMonth = DateTime.Now.Month;

			var query = await _context.FABudgets
				.Where(p =>
					p.PurchaseTimeEstimation.HasValue && // Kiểm tra không null
					(p.PurchaseTimeEstimation.Value.Year < currentYear ||
					(p.PurchaseTimeEstimation.Value.Year == currentYear && p.PurchaseTimeEstimation.Value.Month < currentMonth))
					&& string.IsNullOrEmpty(p.RingishoNo))
				.ToListAsync();

			if (query.Count > 0)
			{
				foreach (var item in query)
				{
					DateTime firstDayOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
					item.PurchaseTimeEstimation = firstDayOfCurrentMonth;
					_context.FABudgets.Update(item);
				}
				
			}
			var res = await _context.SaveChangesAsync();
			if (res > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
