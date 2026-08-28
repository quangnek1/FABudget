using ItemMaster.Shared.Model;

namespace ItemMaster.Server.Services
{
    public interface IworkFlowTemplateServices
    {
        Task<ServiceResponse<string>> Create(WorkFlowTemplateVm workFlowTemplateVm);
        Task<IEnumerable<WorkflowtemplateRequestVm>> Get();
    }
}
