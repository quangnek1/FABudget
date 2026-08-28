using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemMaster.Shared.Model;

namespace ItemMaster.Shared.Extensions
{
    public class SendmailRequest
    {
        public string Data { get; set; }
        public string Message { get; set; }
        public string Frommail { get; set; }
        public string Tomail { get; set; }
        public string Displayname { get; set; }
        public string ToDisplayname { get; set; }
        public string url { get; set; }
        public List<FABudgetVm>? FABudgetVm { get; set; }
    }
}
