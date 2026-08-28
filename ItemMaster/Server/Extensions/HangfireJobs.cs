using System.Text.Json;
using ItemMaster.Server.Services;
using ItemMaster.Shared.Extensions;
using ItemMaster.Shared.Model;
using Microsoft.Extensions.DependencyInjection;
using ILogger = Serilog.ILogger;

namespace ItemMaster.Server.Extensions
{
	public class HangfireJobs
	{
		private readonly IEmailSender _emailSender;
		private readonly ILogger _logger;
		private readonly IUtilityService _utilityService;
		private readonly IFABudgetServices _fABudgetServices;

		public HangfireJobs(IEmailSender emailSender, ILogger logger, IUtilityService utilityService, IFABudgetServices fABudgetServices)
		{
			_emailSender = emailSender;
			_logger = logger;
			_utilityService = utilityService;
			_fABudgetServices = fABudgetServices;
		}
		public async Task SendDailyEmail()
		{

			if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
				return; // Skip Chủ nhật

			var userVms = await _utilityService.GetListUserSendMailAsync();
			foreach (var item in userVms)
			{
				try
				{
					var sendMail = new SendmailRequest()
					{
						Displayname = "浦野 雅裕",
						ToDisplayname = item.User.Fullname,
						Tomail = item.User.Email,
						//	Tomail = "hanoi-eng82@asahi-intecc.com",
						Frommail = "masahiro.urano@asahi-intecc.com",
						url = "http://172.16.33.123/",
						Data = "Update Data",
						FABudgetVm = item.FABudgets.Select(p => new FABudgetVm() { No = p.No, AccountText = p.AccountText, AssetName = p.AssetName, PersonInCharge = p.PersonInCharge, PurchaseTimeEstimation = p.PurchaseTimeEstimation }).ToList(),
					};
					await _emailSender.SendEmail(sendMail);
					_logger.Information(messageTemplate: $"Sent Email: {JsonSerializer.Serialize(sendMail.Tomail)}");
				}
				catch (Exception ex)
				{
					_logger.Error(messageTemplate: ex.Message);
				}
			}
			_logger.Information(messageTemplate: $"Sent Email Done");
		}
		public async Task UpdateMonthlyData()
		{
			try
			{
				_logger.Information("Starting monthly data update...");
				await _fABudgetServices.UpdateDataMonthly();
				_logger.Information("Monthly data update completed.");
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
			}
		}
	}
}
