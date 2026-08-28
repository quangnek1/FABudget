using ItemMaster.Server.Data.Entities;
using ItemMaster.Shared.Model;

namespace ItemMaster.Server.Services
{
    public interface IWorkFlowServices
    {
        Task<IEnumerable<WorkFlowVm>> Get();
        Task<IEnumerable<WorkFlowVm>> GetByUser();
        Task<ServiceResponse<string>> Create(WorkFlowVm workFlowVm);
        Task<ServiceResponse<bool>> Put(WorkFlowVm workFlowVm);
        Task<ServiceResponse<bool>> Delete(string Id);
    }
}
