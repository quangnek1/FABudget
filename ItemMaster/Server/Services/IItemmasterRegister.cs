using ItemMaster.Server.Data.Entities;
using ItemMaster.Shared.Extensions;
using ItemMaster.Shared.Model;
using ItemMaster.Shared.Model.Wrapper;

namespace ItemMaster.Server.Services
{
    public interface IItemmasterRegister : IService
    {
        Task<List<ItemmasterVm>> GetAll();
        Task<List<ItemmasterVm>> GetByUserOwner();
        Task<List<ItemmasterVm>> GetListUserConfirmed();
        Task<List<SettingEmailRequestVm>> GetListSettingEmail();
        Task<F7782Details> Get7782Detail(string id);
        Task<ServiceResponse<int>> Post(F7782CreateRequest f7782CreateRequest);
        Task<ServiceResponse<int>> Confirm(int idTracking, TrackingUpdateRequest trackingUpdateRequest);
        Task<Shared.Model.Wrapper.IResult> GetItemmaster(string Id);
        Task<Shared.Model.Wrapper.IResult> Update(string Id);
        Task<IEnumerable<UserVm>> GetlistUserAsync();

    }
}
