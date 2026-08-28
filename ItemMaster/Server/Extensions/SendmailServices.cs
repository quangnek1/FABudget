using ItemMaster.Shared.Extensions;
using System.Net.Mail;
using System.Text.Encodings.Web;

namespace ItemMaster.Server.Extensions
{
	public class SendmailServices : ISendmailServices
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
                string subject = "【FA Budget】 " + request.Data;
                string body1 = "Dear Mr/Ms " + request.ToDisplayname + ", <br /> <br />";
                string body2 = "Please approve by click below link detail:  <br />" +
                         $"<a href='{HtmlEncoder.Default.Encode(url)}'>clicking here</a> <br /> <br />";
                string body3 = "Thanks and Best regards.<br />";
                string body4 = "Email is sent automatically on: " + Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                string body = body1 + body2 + body3 + body4;
                //string body5 = "<table style='width:100 %; zoom: 87 % '>< thead >< tr >< th colspan = '2' style = 'text-align:center' > (Estimated Effectiveness)</ th > < th colspan = '2' style = 'text-align: center; border-color:@color'> Hiệu quả thu được sau khi Áp dụng cải tiến< br />(Effectiveness after finishing improvement)</ th ></ tr ></ thead >< tbody >< tr >< td colspan = '2' style = 'text-align:center' >Application Date Plan on @fG7722RequestVm.ApplicationDatePlan</ td >  < td colspan = '2' style = 'text-align:center' >  Actual application date on @fG7722RequestVm.ActualApplicationDate</ td > </ tr > < tr > < td > Estimated cost </ td > < td > @fG7722RequestVm.EstimatedCost </ td > < td > Actual cost </ td >  < td > @fG7722RequestVm.ActualCost </ td > </ tr > < tr > < td > Before improvement </ td > < td > @fG7722RequestVm.EstimatedEfftivenesBefore @fG7722RequestVm.UnitBefore </ td > < td rowspan = '2' style = 'margin: 0 auto; padding-top: 26px;' > Actual After improvement</ td > < td rowspan = '2' style = 'margin: 0 auto; padding-top: 26px;' > @fG7722RequestVm.FinishingAfterImp @fG7722RequestVm.UnitActual </ td > </ tr > < tr >  < td > After improvement </ td >   < td >  @fG7722RequestVm.EstimatedEfftivenesAfter @fG7722RequestVm.UnitAfter </ td ></ tr >< tr > < td > Expected reduce values</ td > < td > @fG7722RequestVm.EstimatedEfftivenesExpected </ td > < td > Actual reduce value</ td >< td >@fG7722RequestVm.FinishingReduceImp </ td ></ tr > < tr >< td > Costdown value(USD / Year) </ td > < td > @fG7722RequestVm.EstimatedEfftivenesCostDownValue USD </ td > < td > Costdown value(USD / Year) </ td >< td > @fG7722RequestVm.FinishingCostDownValueImp USD </ td > </ tr ></ tbody > </ table > ";

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
}
