using System.Text.Json;
using ItemMaster.Server.Services;
using ItemMaster.Shared.Extensions;
using ItemMaster.Shared.Model;

namespace ItemMaster.Server.Extensions
{
	public class WeeklyEmailService : BackgroundService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly ILogger<WeeklyEmailService> _logger;
		private readonly TimeSpan _scheduledTime = new TimeSpan(8, 0, 0); // 8:00 AM thứ 2

		public WeeklyEmailService(IServiceProvider serviceProvider, ILogger<WeeklyEmailService> logger, IServiceScopeFactory serviceScopeFactory)
		{
			_serviceProvider = serviceProvider;
			_logger = logger;
			_serviceScopeFactory = serviceScopeFactory;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				// Lịch chạy tiếp theo
				var nextDailyRun = CalculateNextRunTime(_scheduledTime);
				var nextMonthlyRun = CalculateNextMonthFirstDay(_scheduledTime);


				while (!stoppingToken.IsCancellationRequested)
				{
					var now = DateTime.Now;

					// Tính toán thời gian trễ (delay) ngắn nhất giữa ngày và tháng
					var delayDaily = nextDailyRun - now;
					var delayMonthly = nextMonthlyRun - now;
					var delay = delayDaily < delayMonthly ? delayDaily : delayMonthly;

					_logger.LogInformation($"Next task scheduled for Weekly: {nextDailyRun}, Monthly: {nextMonthlyRun}");


					//if (delay > TimeSpan.Zero)
					//{
					//	_logger.LogInformation("Waiting {Delay} until next run. Weekly={Weekly}, Monthly={Monthly}",delay, nextWeeklyRun, nextMonthlyRun);

					//	await Task.Delay(delay, stoppingToken); // chờ
					//}
					//else
					//{
					//	// nếu delay <= 0 => đã quá thời điểm, không chờ, xử lý ngay
					//	_logger.LogInformation("Due tasks detected (delay <= 0). Processing immediately.");
					//}


					// Chờ đến thời gian gần nhất
					await Task.Delay(delay, stoppingToken);


					// Nếu đã đến thời gian chạy hàng tuần	
					if (now >= nextDailyRun)
					{
						await SendWeeklyEmail(stoppingToken); // Gửi email hàng tuần
						nextDailyRun = CalculateNextRunTime(_scheduledTime); // Cập nhật thời gian chạy tuần kế tiếp
					}


					// Nếu đã đến thời gian chạy hàng tháng
					if (now >= nextMonthlyRun)
					{
						await UpdateMonthlyData(stoppingToken); // Cập nhật dữ liệu hàng tháng
						nextMonthlyRun = CalculateNextMonthFirstDay(_scheduledTime); // Cập nhật thời gian chạy tháng kế tiếp
					}

					//	var nextRun = CalculateNextRunTime(_scheduledTime);
					//	var delay = nextRun - now;

					//	_logger.LogInformation($"Next email scheduled for {nextRun}");
					//	await Task.Delay(delay, stoppingToken);
					//	//		await Task.Delay(50000000, stoppingToken);

					// Gửi email
					//	await SendWeeklyEmail(stoppingToken);
				}

			}
			catch (Exception)
			{


			}

		}
		private DateTime CalculateNextRunTime(TimeSpan scheduledTime)
		{
			var now = DateTime.Now;
			var nextRun = now.Date + scheduledTime;

			// Nếu thời điểm gửi đã qua trong ngày, đặt lại cho ngày hôm sau
			if (nextRun <= now)
			{
				nextRun = nextRun.AddDays(1);
			}

			return nextRun;
		}
		private DateTime CalculateNextMonday(TimeSpan scheduledTime)
		{
			var now = DateTime.Now;
			var daysUntilMonday = ((int)DayOfWeek.Monday - (int)now.DayOfWeek + 7) % 7;
			var nextMonday = now.AddDays(daysUntilMonday).Date + scheduledTime;

			return nextMonday;
		}
		private DateTime CalculateNextMonthFirstDay(TimeSpan scheduledTime)
		{
			var now = DateTime.Now;
			var nextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1); // Mùng 1 tháng tiếp theo
			var nextRun = nextMonth + scheduledTime;

			return nextRun;
		}
		private async Task UpdateMonthlyData(CancellationToken stoppingToken)
		{
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var dataService = scope.ServiceProvider.GetRequiredService<IFABudgetServices>();

				_logger.LogInformation("Starting monthly data update...");
				await dataService.UpdateDataMonthly();
				_logger.LogInformation("Monthly data update completed.");
			}
		}



		private async Task SendWeeklyEmail(CancellationToken stoppingToken)
		{

			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var emailService = scope.ServiceProvider.GetRequiredService<IEmailSender>();
				var userService = scope.ServiceProvider.GetRequiredService<IUtilityService>();
				var userVms = await userService.GetListUserSendMailAsync();

				//var sendMail = new SendmailRequest()
				//{
				//	Displayname = "FA-Budget",
				//	ToDisplayname = "Test",
				//	Tomail = "hanoi-eng82@asahi-intecc.com",
				//	Frommail = "masahiro.urano@asahi-intecc.com",
				//	url = "http://172.16.33.123/",
				//	Data = "Update Data"
				//};
				//await emailService.SendEmail(sendMail);

				foreach (var item in userVms)
				{
					try
					{
						var sendMail = new SendmailRequest()
						{
							Displayname = "浦野 雅裕",
							ToDisplayname = item.User.Fullname,
							//	Tomail = item.User.Email,
							Tomail = "hanoi-eng82@asahi-intecc.com",
							Frommail = "masahiro.urano@asahi-intecc.com",
							url = "http://172.16.33.123/",
							Data = "Update Data",
							FABudgetVm = item.FABudgets.Select(p => new FABudgetVm() { No = p.No, AccountText = p.AccountText, AssetName = p.AssetName, PersonInCharge = p.PersonInCharge, PurchaseTimeEstimation = p.PurchaseTimeEstimation }).ToList(),
						};
						await emailService.SendEmail(sendMail);
						_logger.LogDebug(message: $"Sent Email: {JsonSerializer.Serialize(sendMail.ToDisplayname)}");
					}
					catch (Exception ex)
					{
						_logger.LogDebug(message: ex.Message);
					}

				}

			}

		}
	}
}
