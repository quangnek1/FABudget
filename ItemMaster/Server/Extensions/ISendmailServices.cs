using ItemMaster.Shared.Extensions;

namespace ItemMaster.Server.Extensions
{
    public interface ISendmailServices
    {
        Task SendEmail(SendmailRequest request);
    }
}
