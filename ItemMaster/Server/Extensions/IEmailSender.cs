using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;
using ItemMaster.Shared.Extensions;

public interface IEmailSender
{
	Task SendEmail(SendmailRequest request);
}

public class EmailSender : IEmailSender
{



	public async Task SendEmail(SendmailRequest request)
	{
		Thread thread = new Thread(async () =>
		{
			bool checkstatus = await Send(request);
		}
			);
		thread.Start();
	}

	private async Task<bool> Send(SendmailRequest request)
	{
		try
		{
			string url = request.url;
			string subject = "Update on Planned Purchase of Fixed Assets(設備投資の予測更新)";
			string body1 = "Good morning," + " <br /> <br />";
			string body1_1 = "The planned purchase of fixed assets is now scheduled to take place in 3 months.  <br />";
			string body1_2 = "Please keep us informed of any changes to this schedule.  <br /><br />";
			// Tạo bảng HTML cho `budget`
			string contentTable = "<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;'>";
			contentTable += "<tr><th>No</th><th>Account Text</th><th>Asset Name</th><th>Person in charge</th><th>Purchase time Estimation</th></tr>";
			foreach (var item in request.FABudgetVm)
			{
				contentTable += $"<tr><td>{item.No}</td><td>{item.AccountText}</td><td>{item.AssetName}</td><td>{item.PersonInCharge}</td><td>{Convert.ToDateTime(item.PurchaseTimeEstimation).ToString("MM/yyyy") }</td></tr>";
			}
			contentTable += "</table><br />";

			string body1_3 = "You can expect to receive this email everyday until the update is completed..  <br />";

			string body2 = "Please access by click below link detail:  <br />" +
					 $"<a href='{HtmlEncoder.Default.Encode(url)}'>clicking here</a> <br /> <br />";
			string body3 = "Urano <br />";
			//	string body4 = "Email is sent automatically on: " + Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
			string body = body1 + body1_1 + body1_2 + contentTable + body1_3 + body2 + body3;

			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(request.Frommail, request.Displayname);

			mail.To.Add(new MailAddress(request.Tomail));
			mail.Subject = subject;
			mail.Body = body;
			mail.IsBodyHtml = true;
			mail.Priority = MailPriority.High;

			SmtpClient smtp = new SmtpClient();
			smtp.Host = "sbox.asahi-intecc.com";
			smtp.Send(mail);

			return true;

		}
		catch (Exception ex)
		{
			// MessageBox.Show(ex.ToString());
			return false;
		}
	}
}
