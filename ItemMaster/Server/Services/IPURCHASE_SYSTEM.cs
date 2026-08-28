using ItemMaster.Server.Data.Entities;
using ItemMaster.Shared.Model;

namespace ItemMaster.Server.Services
{
	public interface IPURCHASE_SYSTEM
	{
		Task<ServiceResponse<List<PrVm>>> GetAsyncByRingishoNo(string? ringNo);
		Task<ServiceResponse<List<PRDetailVm>>> GetPRDetailByPRNo(string? prNo);
		Task<List<PrVm>> GetAsyncByBudgetNo(int? budgetNo);
		Task<List<DataUploadVm>> GetAsyncPRByPO(List<DataUploadVm> uploadVms);
		Task<int> CheckPO(PODataUpload pODataUpload);
		Task DownloadAllData();
	}
}
