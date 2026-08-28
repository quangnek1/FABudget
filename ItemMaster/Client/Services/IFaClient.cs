using ItemMaster.Shared.Model;

namespace ItemMaster.Client.Services
{
    public interface IFaClient
    {
        Task<ServiceResponse<FABudgetObjectVm>> GetFAAsync();
        Task<ServiceResponse<DataVm>> GetDataAsync();
        Task<HttpResponseMessage> PutAsync(FABudgetRequestEditVm fABudgetVm);
        Task<HttpResponseMessage> PostAsync(FABudgetRequestVm fABudgetRequestVm);
        Task<ServiceResponse<FABudgetRequestEditVm>> GetDetailByIdAsync(int id);
        Task<byte[]> ExportAsync();
        Task<byte[]> ExportPOAsync();

		Task<ServiceResponse<List<PrVm>>> GetPurchaseVmAsync(string ringisho);
		Task<ServiceResponse<List<PRDetailVm>>> GetPurchaseDetailVmAsync(string prNo);

	}
}
