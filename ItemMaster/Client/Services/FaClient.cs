using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using ItemMaster.Shared.Model;
using Microsoft.JSInterop;

namespace ItemMaster.Client.Services
{
    public class FaClient : IFaClient
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime JS;
        public FaClient(HttpClient httpClient, IJSRuntime _JS)
        {
            _http = httpClient;
            JS = _JS;
        }

        public async Task<byte[]> ExportAsync()
        {
            var response = await _http.GetAsync($"api/fa/Export");
            response.EnsureSuccessStatusCode();
            var fileBytes = await response.Content.ReadAsByteArrayAsync();
            return fileBytes;

        }

        public async Task<ServiceResponse<FABudgetRequestEditVm>> GetDetailByIdAsync(int id)
		{
			var response = await _http.GetFromJsonAsync<ServiceResponse<FABudgetRequestEditVm>>($"api/fa/{id}");
			return response;
		}

		public async Task<ServiceResponse<FABudgetObjectVm>> GetFAAsync()
        {
            var respone = await _http.GetFromJsonAsync<ServiceResponse<FABudgetObjectVm>>($"api/fa/getAll");
            return respone;
        }
        /// <summary>
        /// Get data: Section, Rate, Account
        /// </summary>
        /// <returns>Object</returns>
		public async Task<ServiceResponse<DataVm>> GetDataAsync()
		{
			var json = await _http.GetStringAsync($"api/fa/getData");
			Console.WriteLine(json); // Log JSON phản hồi
			var respone1 = JsonSerializer.Deserialize<ServiceResponse<DataVm>>(json);


			var respone = await _http.GetFromJsonAsync<ServiceResponse<DataVm>>($"api/fa/getData");

			if (respone == null)
			{
				return new ServiceResponse<DataVm> { IsSuccess = false, Message = "No data returned from server." };
			}

			return respone;
		}

		public async Task<HttpResponseMessage> PutAsync(FABudgetRequestEditVm fABudgetVm)
		{
			Console.WriteLine(fABudgetVm);
			var response = await _http.PutAsJsonAsync("api/fa/update", fABudgetVm);
			Console.WriteLine(response);
			return response;
		}

		public async Task<HttpResponseMessage> PostAsync(FABudgetRequestVm fABudgetRequestVm)
		{
			var response = await _http.PostAsJsonAsync("api/fa/add", fABudgetRequestVm);
			return response;
		}

		public async Task<ServiceResponse<List<PrVm>>> GetPurchaseVmAsync(string ringisho)
		{
			var respone = await _http.GetFromJsonAsync<ServiceResponse<List<PrVm>>>($"api/fa/getPr/{ringisho}");
			return respone;
		}

		public async Task<ServiceResponse<List<PRDetailVm>>> GetPurchaseDetailVmAsync(string prNo)
		{
		var respone = await _http.GetFromJsonAsync<ServiceResponse<List<PRDetailVm>>>($"api/fa/getPrDetail/{prNo}");
			return respone;
		}

		public async Task<byte[]> ExportPOAsync()
		{
			var response = await _http.GetAsync($"api/fa/exportPO");
			response.EnsureSuccessStatusCode();
			var fileBytes = await response.Content.ReadAsByteArrayAsync();
			return fileBytes;
		}
	}
}
