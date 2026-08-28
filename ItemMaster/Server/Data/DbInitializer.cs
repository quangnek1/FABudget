using ItemMaster.Server.Data.Entities;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;

namespace ItemMaster.Server.Data
{
	public class DbInitializer
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly string AdminRoleName = "Admin";
		private readonly string UserRoleName = "Member";
		private readonly IWebHostEnvironment _hostingEnvironment;
		public DbInitializer(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostingEnvironment)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_hostingEnvironment = hostingEnvironment;
		}
		public async Task Seed()
		{

			if (!_context.Rates.Any())
			{
				_context.Rates.AddRange(new List<Rate>()
				{
					new Rate {Currency = "円",Current = Convert.ToDecimal(149.39), NextYear  = Convert.ToDecimal(149.39), Status = true},
					new Rate {Currency = "THB", Current = Convert.ToDecimal(4.17), NextYear  = Convert.ToDecimal(4.1), Status = true},
					new Rate {Currency = "US$", Current = Convert.ToDecimal(1), NextYear  = Convert.ToDecimal(1), Status = true},
					new Rate {Currency = "RMB", Current = Convert.ToDecimal(19.5), NextYear  = Convert.ToDecimal(19.5), Status = true},
					new Rate {Currency = "EURO" ,Current = Convert.ToDecimal(160), NextYear  = Convert.ToDecimal(160), Status = true},
					new Rate {Currency = "VND",Current = Convert.ToDecimal(24095), NextYear  = Convert.ToDecimal(24095), Status = true }
				});
				await _context.SaveChangesAsync();
			}

			if (!_context.Sections.Any())
			{
				_context.Sections.AddRange(new List<Section>()
				{
					new Section {SectionCode = "V13", SectionName = "17-T-MATE", Segment = "D", Status = true},
					new Section {SectionCode = "V14", SectionName = "T-MATE Dilater", Segment = "D", Status = true},
					new Section {SectionCode = "V16", SectionName = "T-MATE PTFE", Segment = "D", Status = true},
					new Section {SectionCode = "V19", SectionName = "03-CENTERLESS", Segment = "D", Status = true},
					new Section {SectionCode = "V20", SectionName = "15-Centerless MD", Segment = "D", Status = true},
					new Section {SectionCode = "V21", SectionName = "05-PTFE", Segment = "D", Status = true},
					new Section {SectionCode = "V22", SectionName = "ENDOSCOPE (CORE)", Segment = "D", Status = true},
					new Section {SectionCode = "V23", SectionName = "23-SHAFT COMMON", Segment = "D", Status = true},
					new Section {SectionCode = "V24", SectionName = "19-MC SHAFT", Segment = "D", Status = true},
					new Section {SectionCode = "V25", SectionName = "20-PLGW SHAFT", Segment = "D", Status = true},
					new Section {SectionCode = "V26", SectionName = "21-CAG SHAFT", Segment = "D", Status = true},
					new Section {SectionCode = "V27", SectionName = "22-GC SHAFT", Segment = "D", Status = true},
					new Section {SectionCode = "V28", SectionName = "27-BC SHAFT", Segment = "D", Status = true},
					new Section {SectionCode = "V29", SectionName = "04-COILING", Segment = "D", Status = true},
					new Section {SectionCode = "V30", SectionName = "16-Coil MD", Segment = "D", Status = true},
					new Section {SectionCode = "V31", SectionName = "13-MEDICAL DEVICE", Segment = "M", Status = true},
					new Section {SectionCode = "V32", SectionName = "06-PTCA", Segment = "M", Status = true},
					new Section {SectionCode = "V34", SectionName = "26-BC ASSY", Segment = "M", Status = true},
					new Section {SectionCode = "V35", SectionName = "02-CAG", Segment = "M", Status = true},
					new Section {SectionCode = "V36", SectionName = "24-GC ASSY", Segment = "M", Status = true},
					new Section {SectionCode = "V37", SectionName = "CORSAIR", Segment = "M", Status = true},
					new Section {SectionCode = "V38", SectionName = "25-MC ASSY", Segment = "M", Status = true},
					new Section {SectionCode = "V39", SectionName = "18-PLGW", Segment = "M", Status = true},
					new Section {SectionCode = "V41", SectionName = "Silverway Assy", Segment = "M", Status = true},
					new Section {SectionCode = "V42", SectionName = "ENDOSCOPE (Assy)", Segment = "M", Status = true},
					new Section {SectionCode = "V43", SectionName = "14-Sterilization", Segment = "M", Status = true},
					new Section {SectionCode = "V44", SectionName = "07-QA", Segment = "M", Status = true},
					new Section {SectionCode = "V47", SectionName = "PRO ENG", Segment = "M", Status = true},
					new Section {SectionCode = "V61", SectionName = "10-OTHER", Segment = "H", Status = true},
					new Section {SectionCode = "V78", SectionName = "Admin-ALL", Segment = "H", Status = true}
				});

				await _context.SaveChangesAsync();
			}

			if (!_context.Accounts.Any())
			{
				_context.Accounts.AddRange(new List<Account>()
					{
						new Account {AccountText = "建物", GLACCOUNT = "21101", Status = true},
						//new Account {AccountText = "建物付属設備", GLACCOUNT = "21201", Status = true},
						//new Account {AccountText = "構築物", GLACCOUNT = "21301", Status = true},
						new Account {AccountText = "機械装置", GLACCOUNT = "21401", Status = true},
						new Account {AccountText = "車両運搬具", GLACCOUNT = "21501", Status = true},
						new Account {AccountText = "工具器具備品", GLACCOUNT = "21601", Status = true},
						new Account {AccountText = "土地", GLACCOUNT = "22101", Status = true},
						new Account {AccountText = "建設仮勘定", GLACCOUNT = "22201", Status = true},
						new Account {AccountText = "ソフトウェア", GLACCOUNT = "23501", Status = true},
						//new Account {AccountText = "ﾘｰｽ資産（有形）", GLACCOUNT = "30100", Status = true},
						//new Account {AccountText = "ﾘｰｽ資産（無形）",GLACCOUNT= "0" ,Status = true},
						new Account {AccountText = "長期前払費用", GLACCOUNT = "24709", Status = true}
					});

				await _context.SaveChangesAsync();
			}

			//if (!_context.GroupEmailSends.Any())
			//{
			//	_context.GroupEmailSends.AddRange(new List<GroupEmailSend>()
			//		{
			//			new GroupEmailSend {Id = "06F97CF5-4F34-4FCD-B391-5B21BAADB359", CountSend = 0, Status = true},
			//		});

			//	await _context.SaveChangesAsync();
			//}

			if (!_context.FABudgets.Any())
			{

				await ReadData();
			}
		}

		public async Task ReadData()
		{
			string sWebRootFolder = _hostingEnvironment.WebRootPath;
			string fileName = @"FY50-TemplateData.xlsx";
			FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, fileName));
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

			using (var package = new ExcelPackage(file))
			{
				var ws = package.Workbook.Worksheets[0];
				int colCount = ws.Dimension.End.Column;  //get Column Count
				int rowCount = ws.Dimension.End.Row;     //get row count
				List<FABudget> List = new List<FABudget>();
				for (int row = 2; row <= rowCount; row++)
				{
					FABudget fABudget = new FABudget();


					fABudget.No = Convert.ToInt32(ws.Cells[row, 1].Value);
					fABudget.GLACCOUNT = Convert.ToInt32(ws.Cells[row, 2].Value);// Chỉ mục cho GLACCOUNT
					fABudget.AccountText = ws.Cells[row, 3].Value?.ToString(); // Chỉ mục cho AccountText
					fABudget.PriorityRank = Convert.ToInt32(ws.Cells[row, 4].Value); // Chỉ mục cho PriorityRank
					fABudget.AssetName = ws.Cells[row, 5].Value?.ToString(); // Chỉ mục cho AssetName
					fABudget.PersonInCharge = ws.Cells[row, 6].Value?.ToString();// Chỉ mục cho PersonInCharge
					fABudget.SectionCode = ws.Cells[row, 7].Value?.ToString();// Chỉ mục cho SectionCode
					fABudget.Segment = ws.Cells[row, 8].Value?.ToString(); // Chỉ mục cho Segment
					fABudget.SectionName = ws.Cells[row, 9].Value?.ToString(); // Chỉ mục cho SectionName
					fABudget.PurchaseTimeBudget = ws.Cells[row, 10].Value != null ? Convert.ToDateTime(ws.Cells[row, 10].Value.ToString()) : null; // Chỉ mục cho PurchaseTimeBudget
					fABudget.DepreciationStartTimeBudget = ws.Cells[row, 11].Value != null ? Convert.ToDateTime(ws.Cells[row, 11].Value.ToString()) : null; // Chỉ mục cho DepreciationStartTimeBudget
					fABudget.PurchaseTimeEstimation = ws.Cells[row, 12].Value != null ? Convert.ToDateTime(ws.Cells[row, 12].Value.ToString()) : null; // Chỉ mục cho PurchaseTimeEstimation
					fABudget.DepreciationStartTimeEstimation = ws.Cells[row, 13].Value != null ? Convert.ToDateTime(ws.Cells[row, 13].Value.ToString()) : null; // Chỉ mục cho DepreciationStartTimeEstimation
					fABudget.Currency = ws.Cells[row, 14].Value?.ToString(); // Chỉ mục cho Currency
					fABudget.CurrentInvestmentAmountBudget = ws.Cells[row, 15].Value != null ? Convert.ToDecimal(ws.Cells[row, 15].Value) : (decimal?)null; // Chỉ mục cho CurrentInvestmentAmountBudget
					fABudget.PreviousInvestmentAmountBudget = ws.Cells[row, 16].Value != null ? Convert.ToDecimal(ws.Cells[row, 16].Value) : (decimal?)null; // Chỉ mục cho PreviousInvestmentAmountBudget
					fABudget.FixedAssetsAccountedAmountBudget = ws.Cells[row, 17].Value != null ? Convert.ToDecimal(ws.Cells[row, 17].Value) : (decimal?)null; // Chỉ mục cho FixedAssetsAccountedAmountBudget
					fABudget.EXRate = ws.Cells[row, 18].Value != null ? Convert.ToDecimal(ws.Cells[row, 18].Value) : (decimal?)null; // Chỉ mục cho EXRate
					fABudget.CurrentInvestmentAmountUSD = ws.Cells[row, 19].Value != null ? Convert.ToDecimal(ws.Cells[row, 19].Value) : (decimal?)null; // Chỉ mục cho CurrentInvestmentAmountUSD
					fABudget.PreviousInvestmentAmountUSD = ws.Cells[row, 20].Value != null ? Convert.ToDecimal(ws.Cells[row, 20].Value) : (decimal?)null; // Chỉ mục cho PreviousInvestmentAmountUSD
					fABudget.FixedAssetsAccountedAmountUSD = ws.Cells[row, 21].Value != null ? Convert.ToDecimal(ws.Cells[row, 21].Value) : (decimal?)null; // Chỉ mục cho FixedAssetsAccountedAmountUSD
					fABudget.CurrentInvestmentAmountEstimationUSD = ws.Cells[row, 22].Value != null ? Convert.ToDecimal(ws.Cells[row, 22].Value) : (decimal?)null; // Chỉ mục cho CurrentInvestmentAmountEstimationUSD
					fABudget.PreviousInvestmentAmountEstimationUSD = ws.Cells[row, 23].Value != null ? Convert.ToDecimal(ws.Cells[row, 23].Value) : (decimal?)null; // Chỉ mục cho PreviousInvestmentAmountEstimationUSD
					fABudget.FixedAssetsAccountedAmountEstimationUSD = ws.Cells[row, 24].Value != null ? Convert.ToDecimal(ws.Cells[row, 24].Value) : (decimal?)null; // Chỉ mục cho FixedAssetsAccountedAmountEstimationUSD
					fABudget.RingishoNo = ws.Cells[row, 25].Value?.ToString(); // Chỉ mục cho RingishoNo
					fABudget.AppropriatedBudgetNo = ws.Cells[row, 26].Value?.ToString(); // Chỉ mục cho AppropriatedBudgetNo
					fABudget.BudgetRemaining = ws.Cells[row, 27].Value != null ? Convert.ToDecimal(ws.Cells[row, 27].Value) : (decimal?)null; // Chỉ mục cho BudgetRemaining
					fABudget.Remark = ws.Cells[row, 28].Value?.ToString(); // Chỉ mục cho Remark
					fABudget.DateCreate = DateTime.Now; // Chỉ mục cho DateCreate
					fABudget.LastModified = DateTime.Now; // Chỉ mục cho LastModified
					fABudget.Status = true;// Chỉ mục cho Status
					fABudget.RingishoProcess = !string.IsNullOrEmpty(fABudget.RingishoNo) ? false : true;

					List.Add(fABudget);

					//try
					//{
					//    List.Add(new FABudget()
					//    {
					//        No = Convert.ToInt32(ws.Cells[row, 1].Value),
					//        GLACCOUNT = Convert.ToInt32(ws.Cells[row, 2].Value), // Chỉ mục cho GLACCOUNT
					//        AccountText = ws.Cells[row, 3].Value?.ToString(), // Chỉ mục cho AccountText
					//        PriorityRank = Convert.ToInt32(ws.Cells[row, 4].Value), // Chỉ mục cho PriorityRank
					//        AssetName = ws.Cells[row, 5].Value?.ToString(), // Chỉ mục cho AssetName
					//        PersonInCharge = ws.Cells[row, 6].Value?.ToString(), // Chỉ mục cho PersonInCharge
					//        SectionCode = ws.Cells[row, 7].Value?.ToString(), // Chỉ mục cho SectionCode
					//        Segment = ws.Cells[row, 8].Value?.ToString(), // Chỉ mục cho Segment
					//        SectionName = ws.Cells[row, 9].Value?.ToString(), // Chỉ mục cho SectionName
					//        PurchaseTimeBudget = ws.Cells[row, 10].Value != null ? DateTime.Parse(ws.Cells[row, 10].Value.ToString()) : (DateTime?)null, // Chỉ mục cho PurchaseTimeBudget
					//        DepreciationStartTimeBudget = ws.Cells[row, 11].Value != null ? DateTime.Parse(ws.Cells[row, 11].Value.ToString()) : (DateTime?)null, // Chỉ mục cho DepreciationStartTimeBudget
					//        PurchaseTimeEstimation = ws.Cells[row, 12].Value != null ? DateTime.Parse(ws.Cells[row, 12].Value.ToString()) : (DateTime?)null, // Chỉ mục cho PurchaseTimeEstimation
					//        DepreciationStartTimeEstimation = ws.Cells[row, 13].Value != null ? DateTime.Parse(ws.Cells[row, 13].Value.ToString()) : (DateTime?)null, // Chỉ mục cho DepreciationStartTimeEstimation
					//        Currency = ws.Cells[row, 14].Value?.ToString(), // Chỉ mục cho Currency
					//        CurrentInvestmentAmountBudget = ws.Cells[row, 15].Value != null ? Convert.ToDecimal(ws.Cells[row, 15].Value) : (decimal?)null, // Chỉ mục cho CurrentInvestmentAmountBudget
					//        PreviousInvestmentAmountBudget = ws.Cells[row, 16].Value != null ? Convert.ToDecimal(ws.Cells[row, 16].Value) : (decimal?)null, // Chỉ mục cho PreviousInvestmentAmountBudget
					//        FixedAssetsAccountedAmountBudget = ws.Cells[row, 17].Value != null ? Convert.ToDecimal(ws.Cells[row, 17].Value) : (decimal?)null, // Chỉ mục cho FixedAssetsAccountedAmountBudget
					//        EXRate = ws.Cells[row, 18].Value != null ? Convert.ToDecimal(ws.Cells[row, 18].Value) : (decimal?)null, // Chỉ mục cho EXRate
					//        CurrentInvestmentAmountUSD = ws.Cells[row, 19].Value != null ? Convert.ToDecimal(ws.Cells[row, 19].Value) : (decimal?)null, // Chỉ mục cho CurrentInvestmentAmountUSD
					//        PreviousInvestmentAmountUSD = ws.Cells[row, 20].Value != null ? Convert.ToDecimal(ws.Cells[row, 20].Value) : (decimal?)null, // Chỉ mục cho PreviousInvestmentAmountUSD
					//        FixedAssetsAccountedAmountUSD = ws.Cells[row, 21].Value != null ? Convert.ToDecimal(ws.Cells[row, 21].Value) : (decimal?)null, // Chỉ mục cho FixedAssetsAccountedAmountUSD
					//        CurrentInvestmentAmountEstimationUSD = ws.Cells[row, 22].Value != null ? Convert.ToDecimal(ws.Cells[row, 22].Value) : (decimal?)null, // Chỉ mục cho CurrentInvestmentAmountEstimationUSD
					//        PreviousInvestmentAmountEstimationUSD = ws.Cells[row, 23].Value != null ? Convert.ToDecimal(ws.Cells[row, 23].Value) : (decimal?)null, // Chỉ mục cho PreviousInvestmentAmountEstimationUSD
					//        FixedAssetsAccountedAmountEstimationUSD = ws.Cells[row, 24].Value != null ? Convert.ToDecimal(ws.Cells[row, 24].Value) : (decimal?)null, // Chỉ mục cho FixedAssetsAccountedAmountEstimationUSD
					//        RingishoNo = ws.Cells[row, 25].Value?.ToString(), // Chỉ mục cho RingishoNo
					//        AppropriatedBudgetNo = ws.Cells[row, 26].Value != null ? Convert.ToDecimal(ws.Cells[row, 26].Value) : (decimal?)null, // Chỉ mục cho AppropriatedBudgetNo
					//        BudgetRemaining = ws.Cells[row, 27].Value != null ? Convert.ToDecimal(ws.Cells[row, 27].Value) : (decimal?)null, // Chỉ mục cho BudgetRemaining
					//        Remark = ws.Cells[row, 28].Value?.ToString(), // Chỉ mục cho Remark
					//        DateCreate = DateTime.Now, // Chỉ mục cho DateCreate
					//        LastModified = DateTime.Now, // Chỉ mục cho LastModified
					//        Status = true // Chỉ mục cho Status

					//    });
					//}
					//catch (Exception ex)
					//{

					//    Console.WriteLine($"Loi tai hang {row}: {ex.Message}");
					//    Console.WriteLine($"Gia tra gây loi: {ws.Cells[row, 1].Value}");
					//    throw; // Ném lại ngoại lệ để không bỏ qua lỗi
					//}


					#region Comment Code



					//try
					//{
					//	int? no = null;
					//	if (ws.Cells[row, 1].Value != null)
					//	{
					//		try
					//		{
					//			no = Convert.ToInt32(ws.Cells[row, 1].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 1 (No): Giá trị gây lỗi: {ws.Cells[row, 1].Value}");
					//			throw;
					//		}
					//	}

					//	int? glAccount = null;
					//	if (ws.Cells[row, 2].Value != null)
					//	{
					//		try
					//		{
					//			glAccount = Convert.ToInt32(ws.Cells[row, 2].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 2 (GLACCOUNT): Giá trị gây lỗi: {ws.Cells[row, 2].Value}");
					//			throw;
					//		}
					//	}

					//	string? accountText = null;
					//	if (ws.Cells[row, 3].Value != null)
					//	{
					//		accountText = ws.Cells[row, 3].Value.ToString();
					//	}

					//	int? priorityRank = null;
					//	if (ws.Cells[row, 4].Value != null)
					//	{
					//		try
					//		{
					//			priorityRank = Convert.ToInt32(ws.Cells[row, 4].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 4 (PriorityRank): Giá trị gây lỗi: {ws.Cells[row, 4].Value}");
					//			throw;
					//		}
					//	}

					//	string? assetName = null;
					//	if (ws.Cells[row, 5].Value != null)
					//	{
					//		assetName = ws.Cells[row, 5].Value.ToString();
					//	}

					//	string? personInCharge = null;
					//	if (ws.Cells[row, 6].Value != null)
					//	{
					//		personInCharge = ws.Cells[row, 6].Value.ToString();
					//	}

					//	string? sectionCode = null;
					//	if (ws.Cells[row, 7].Value != null)
					//	{
					//		sectionCode = ws.Cells[row, 7].Value.ToString();
					//	}

					//	string? segment = null;
					//	if (ws.Cells[row, 8].Value != null)
					//	{
					//		segment = ws.Cells[row, 8].Value.ToString();
					//	}

					//	string? sectionName = null;
					//	if (ws.Cells[row, 9].Value != null)
					//	{
					//		sectionName = ws.Cells[row, 9].Value.ToString();
					//	}

					//	DateTime? purchaseTimeBudget = null;
					//	if (ws.Cells[row, 10].Value != null)
					//	{
					//		try
					//		{
					//			purchaseTimeBudget = DateTime.Parse(ws.Cells[row, 10].Value.ToString());
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 10 (PurchaseTimeBudget): Giá trị gây lỗi: {ws.Cells[row, 10].Value}");
					//			throw;
					//		}
					//	}

					//	DateTime? depreciationStartTimeBudget = null;
					//	if (ws.Cells[row, 11].Value != null)
					//	{
					//		try
					//		{
					//			depreciationStartTimeBudget = DateTime.Parse(ws.Cells[row, 11].Value.ToString());
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 11 (DepreciationStartTimeBudget): Giá trị gây lỗi: {ws.Cells[row, 11].Value}");
					//			throw;
					//		}
					//	}

					//	DateTime? purchaseTimeEstimation = null;
					//	if (ws.Cells[row, 12].Value != null)
					//	{
					//		try
					//		{
					//			purchaseTimeEstimation = DateTime.Parse(ws.Cells[row, 12].Value.ToString());
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 12 (PurchaseTimeEstimation): Giá trị gây lỗi: {ws.Cells[row, 12].Value}");
					//			throw;
					//		}
					//	}

					//	DateTime? depreciationStartTimeEstimation = null;
					//	if (ws.Cells[row, 13].Value != null)
					//	{
					//		try
					//		{
					//			depreciationStartTimeEstimation = DateTime.Parse(ws.Cells[row, 13].Value.ToString());
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 13 (DepreciationStartTimeEstimation): Giá trị gây lỗi: {ws.Cells[row, 13].Value}");
					//			throw;
					//		}
					//	}

					//	string? currency = null;
					//	if (ws.Cells[row, 14].Value != null)
					//	{
					//		currency = ws.Cells[row, 14].Value.ToString();
					//	}

					//	decimal? currentInvestmentAmountBudget = null;
					//	if (ws.Cells[row, 15].Value != null)
					//	{
					//		try
					//		{
					//			currentInvestmentAmountBudget = Convert.ToDecimal(ws.Cells[row, 15].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 15 (CurrentInvestmentAmountBudget): Giá trị gây lỗi: {ws.Cells[row, 15].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? previousInvestmentAmountBudget = null;
					//	if (ws.Cells[row, 16].Value != null)
					//	{
					//		try
					//		{
					//			previousInvestmentAmountBudget = Convert.ToDecimal(ws.Cells[row, 16].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 16 (PreviousInvestmentAmountBudget): Giá trị gây lỗi: {ws.Cells[row, 16].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? fixedAssetsAccountedAmountBudget = null;
					//	if (ws.Cells[row, 17].Value != null)
					//	{
					//		try
					//		{
					//			fixedAssetsAccountedAmountBudget = Convert.ToDecimal(ws.Cells[row, 17].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 17 (FixedAssetsAccountedAmountBudget): Giá trị gây lỗi: {ws.Cells[row, 17].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? exRate = null;
					//	if (ws.Cells[row, 18].Value != null)
					//	{
					//		try
					//		{
					//			exRate = Convert.ToDecimal(ws.Cells[row, 18].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 18 (EXRate): Giá trị gây lỗi: {ws.Cells[row, 18].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? currentInvestmentAmountUSD = null;
					//	if (ws.Cells[row, 19].Value != null)
					//	{
					//		try
					//		{
					//			currentInvestmentAmountUSD = Convert.ToDecimal(ws.Cells[row, 19].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 19 (CurrentInvestmentAmountUSD): Giá trị gây lỗi: {ws.Cells[row, 19].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? previousInvestmentAmountUSD = null;
					//	if (ws.Cells[row, 20].Value != null)
					//	{
					//		try
					//		{
					//			previousInvestmentAmountUSD = Convert.ToDecimal(ws.Cells[row, 20].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 20 (PreviousInvestmentAmountUSD): Giá trị gây lỗi: {ws.Cells[row, 20].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? fixedAssetsAccountedAmountUSD = null;
					//	if (ws.Cells[row, 21].Value != null)
					//	{
					//		try
					//		{
					//			fixedAssetsAccountedAmountUSD = Convert.ToDecimal(ws.Cells[row, 21].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 21 (FixedAssetsAccountedAmountUSD): Giá trị gây lỗi: {ws.Cells[row, 21].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? currentInvestmentAmountEstimationUSD = null;
					//	if (ws.Cells[row, 22].Value != null)
					//	{
					//		try
					//		{
					//			currentInvestmentAmountEstimationUSD = Convert.ToDecimal(ws.Cells[row, 22].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 22 (CurrentInvestmentAmountEstimationUSD): Giá trị gây lỗi: {ws.Cells[row, 22].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? previousInvestmentAmountEstimationUSD = null;
					//	if (ws.Cells[row, 23].Value != null)
					//	{
					//		try
					//		{
					//			previousInvestmentAmountEstimationUSD = Convert.ToDecimal(ws.Cells[row, 23].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 23 (PreviousInvestmentAmountEstimationUSD): Giá trị gây lỗi: {ws.Cells[row, 23].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? fixedAssetsAccountedAmountEstimationUSD = null;
					//	if (ws.Cells[row, 24].Value != null)
					//	{
					//		try
					//		{
					//			fixedAssetsAccountedAmountEstimationUSD = Convert.ToDecimal(ws.Cells[row, 24].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 24 (FixedAssetsAccountedAmountEstimationUSD): Giá trị gây lỗi: {ws.Cells[row, 24].Value}");
					//			throw;
					//		}
					//	}

					//	string? ringishoNo = null;
					//	if (ws.Cells[row, 24].Value != null)
					//	{
					//		try
					//		{
					//			ringishoNo = Convert.ToString(ws.Cells[row, 25].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 25 (ringishoNo): Giá trị gây lỗi: {ws.Cells[row, 25].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? appropriatedBudgetNo = null;
					//	if (ws.Cells[row, 26].Value != null)
					//	{
					//		try
					//		{
					//			appropriatedBudgetNo = Convert.ToDecimal(ws.Cells[row, 26].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 26 (appropriatedBudgetNo): Giá trị gây lỗi: {ws.Cells[row, 26].Value}");
					//			throw;
					//		}
					//	}

					//	decimal? budgetRemaining = null;
					//	if (ws.Cells[row, 27].Value != null)
					//	{
					//		try
					//		{
					//			budgetRemaining = Convert.ToDecimal(ws.Cells[row, 27].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 27 (BudgetRemaining): Giá trị gây lỗi: {ws.Cells[row, 27].Value}");
					//			throw;
					//		}
					//	}

					//	string? remark = null;
					//	if (ws.Cells[row, 28].Value != null)
					//	{
					//		try
					//		{
					//			remark = Convert.ToString(ws.Cells[row, 28].Value);
					//		}
					//		catch (FormatException)
					//		{
					//			Console.WriteLine($"Lỗi tại hàng {row}, cột 28 (ringishoNo): Giá trị gây lỗi: {ws.Cells[row, 28].Value}");
					//			throw;
					//		}
					//	}

					// Sau khi lấy được tất cả các giá trị, thêm vào danh sách
					//budgets.Add(new FABudget()
					//{
					//	No = no,
					//	GLACCOUNT = glAccount,
					//	AccountText = accountText,
					//	PriorityRank = priorityRank,
					//	AssetName = assetName,
					//	PersonInCharge = personInCharge,
					//	SectionCode = sectionCode,
					//	Segment = segment,
					//	SectionName = sectionName,
					//	PurchaseTimeBudget = purchaseTimeBudget,
					//	DepreciationStartTimeBudget = depreciationStartTimeBudget,
					//	PurchaseTimeEstimation = purchaseTimeEstimation,
					//	DepreciationStartTimeEstimation = depreciationStartTimeEstimation,
					//	Currency = currency,
					//	CurrentInvestmentAmountBudget = currentInvestmentAmountBudget,
					//	PreviousInvestmentAmountBudget = previousInvestmentAmountBudget,
					//	FixedAssetsAccountedAmountBudget = fixedAssetsAccountedAmountBudget,
					//	EXRate = exRate,
					//	CurrentInvestmentAmountUSD = currentInvestmentAmountUSD,
					//	PreviousInvestmentAmountUSD = previousInvestmentAmountUSD,
					//	FixedAssetsAccountedAmountUSD = fixedAssetsAccountedAmountUSD,
					//	CurrentInvestmentAmountEstimationUSD = currentInvestmentAmountEstimationUSD,
					//	PreviousInvestmentAmountEstimationUSD = previousInvestmentAmountEstimationUSD,
					//	FixedAssetsAccountedAmountEstimationUSD = fixedAssetsAccountedAmountEstimationUSD,
					//	// Các thuộc tính khác nếu cần...
					//});
					//}
					//catch (Exception ex)
					//{
					//	Console.WriteLine($"Lỗi không mong đợi tại hàng {row}: {ex.Message}");
					//	throw;
					//}
					#endregion

				}
				_context.FABudgets.AddRange(List);
				var res = await _context.SaveChangesAsync();
			}

		}
	}
}
