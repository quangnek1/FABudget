using ItemMaster.Shared.Model;

namespace ItemMaster.Server.Services
{
	public interface IFABudgetServices
	{
		Task<ServiceResponse<FABudgetObjectVm>> GetFAAsync();
		Task<ServiceResponse<DataVm>> GetDataAsync();
		Task<ServiceResponse<List<DataUploadVm>>> UploadDataAsync(IFormFile files);
		Task<ServiceResponse<HttpResponseMessage>> SavePOUploadDataAsync(List<DataUploadVm> dataUploadVms);
		Task<ServiceResponse<FABudgetRequestEditVm>> GetDetailById(int id);
		Task<ServiceResponse<byte[]>> Export();
		Task<ServiceResponse<byte[]>> ExportPO();
		Task<ServiceResponse<HttpResponseMessage>> PutAsync(FABudgetRequestEditVm request);
		Task<ServiceResponse<HttpResponseMessage>> PostAsync(FABudgetRequestVm request);

		Task<bool> UpdateDataMonthly();

		//Pr
	}
}
