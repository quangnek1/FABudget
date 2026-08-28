using ItemMaster.Shared.Model;

namespace ItemMaster.Server.Services
{
    public interface IMyF7782Repo
    {
        Task<IEnumerable<F7782CreateRequest>> GetMyF7782();
    }
}
