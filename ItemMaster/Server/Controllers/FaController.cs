using ItemMaster.Server.Services;
using ItemMaster.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ItemMaster.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FaController : ControllerBase
	{
		private readonly IFABudgetServices _FABudgetServices;
		private readonly IPURCHASE_SYSTEM _pURCHASE_SYSTEM;
		public FaController(IFABudgetServices fABudgetServices, IPURCHASE_SYSTEM pURCHASE_SYSTEM)
		{
			_FABudgetServices = fABudgetServices;
			_pURCHASE_SYSTEM = pURCHASE_SYSTEM;
		}
		[HttpGet("getAll")]
		public async Task<IActionResult> GetFA()
		{
			var response = await _FABudgetServices.GetFAAsync();

			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
		[HttpPost("uploadFile")]
		public async Task<IActionResult> UploadData([FromForm] IFormFile files)
		{
			var response = await _FABudgetServices.UploadDataAsync(files);

			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
		[HttpPost("savePOUpload")]
		public async Task<IActionResult> SavePOUpload([FromBody] List<DataUploadVm> DataUploadVms)
		{
			var response = await _FABudgetServices.SavePOUploadDataAsync(DataUploadVms);

			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
		[HttpGet("exportPO")]
		public async Task<IActionResult> ExportPO()
		{
			var response = await _FABudgetServices.ExportPO();
			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}
			byte[] reportBytes = response.Data;

			var file = File(reportBytes, response.Message, $"Export{DateTime.Now.ToString("MMddyyyhhmmss", CultureInfo.InvariantCulture)}.xlsx");
			return file;

		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetDetailById(int id)
		{
			var response = await _FABudgetServices.GetDetailById(id);
			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}
			return Ok(response);
		}

		[HttpGet("export")]
		public async Task<IActionResult> Export()
		{
			var response = await _FABudgetServices.Export();
			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}
			byte[] reportBytes = response.Data;

			var file = File(reportBytes, response.Message, $"Export{DateTime.Now.ToString("MMddyyyhhmmss", CultureInfo.InvariantCulture)}.xlsx");
			return file;

		}
		[HttpGet("getData")]
		public async Task<IActionResult> GetData()
		{
			var response = await _FABudgetServices.GetDataAsync();

			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
		[HttpPut("update")]
		public async Task<IActionResult> PutAsync([FromBody] FABudgetRequestEditVm request)
		{
			var response = await _FABudgetServices.PutAsync(request);

			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
		[HttpPost("add")]
		public async Task<IActionResult> PostAsync(FABudgetRequestVm request)
		{
			var response = await _FABudgetServices.PostAsync(request);

			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
		[HttpGet("getPr/{ringisho}")]
		public async Task<IActionResult> GetFA(string ringisho)
		{
			var response = await _pURCHASE_SYSTEM.GetAsyncByRingishoNo(ringisho);

			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
		[HttpGet("getPrDetail/{prNo}")]
		public async Task<IActionResult> GetPRDetail(string prNo)
		{
			var response = await _pURCHASE_SYSTEM.GetPRDetailByPRNo(prNo);

			if (!response.IsSuccess)
			{
				return BadRequest(response);
			}

			return Ok(response);
		}
	}
}
