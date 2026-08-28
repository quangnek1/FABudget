using ItemMaster.Server.Data;
using ItemMaster.Server.Data.Entities;
using ItemMaster.Server.Extensions;
using ItemMaster.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItemMaster.Server.Services
{
	public class UtilityService : IUtilityService
	{
		private readonly UserDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ApplicationDbContext _applicationDbContext;
		public UtilityService(UserDbContext context, IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_applicationDbContext = applicationDbContext;
		}
		/// <summary>
		/// Trả về đối tượng user
		/// </summary>
		/// <param name="id">idUser của User muốn lấy</param>
		/// <returns>Trả về đối tượng user</returns>
		public async Task<UserVm> GetByIdAsync(string id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			return new UserVm()
			{
				UserId = user.Id,
				Username = user.UserName,
				Email = user.UserName,
				Fullname = user.FullName,
				Code = user.Code,
				ParrenId = user.ParentId
			};
		}

		public async Task<string> GetDivisionName(string id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<UserVm>> GetListUserAsync()
		{
			var user = await _context.Users.Select(p => new UserVm()
			{
				UserId = p.Id,
				Username = p.UserName,
				Email = p.Email,
				Fullname = p.FullName,
				Code = p.Code,
				ParrenId = p.ParentId
			}).ToListAsync();

			return user;
		}

		public async Task<User> GetUserAsync()
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

			return user;
		}

		public async Task<string> GetUserId()
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			return userId;
		}
		//public async Task<User> GetUserWithDivision()
		//{
		//    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
		//    var user = from l in _context.Users
		//       .FirstOrDefaultAsync(u => u.Id == userId);

		//    return user;
		//}
		public async Task<IEnumerable<EmailSendVm>> GetListUserSendMailAsync()
		{
			//var queryBudget = await _applicationDbContext.FABudgets.Where(p => p.RingishoProcess == false || string.IsNullOrEmpty(p.RingishoNo) && Convert.ToDateTime(p.PurchaseTimeEstimation).Month < Convert.ToDateTime(p.PurchaseTimeEstimation).AddMonths(2).Month).ToListAsync();
			var user = await _context.Users.Select(p => new UserVm()
			{
				UserId = p.Id,
				Username = p.UserName,
				Email = p.Email,
				Fullname = p.FullName,
				Code = p.Code,
				ParrenId = p.ParentId
			}).ToListAsync();

			var today = DateTime.Now;
			var startOfMonth = new DateTime(today.Year, today.Month, 1);
			var endOfRange = startOfMonth.AddMonths(3).AddDays(-1); // Ngày cuối của tháng thứ 3

			var queryBudget = await _applicationDbContext.FABudgets
				.Where(p => (string.IsNullOrEmpty(p.RingishoNo) && p.RingishoProcess == false) &&
							p.PurchaseTimeEstimation.HasValue &&
							p.PurchaseTimeEstimation.Value >= startOfMonth &&
							p.PurchaseTimeEstimation.Value <= endOfRange)
				.ToListAsync();

			var groupedTasks = queryBudget
				.GroupBy(task => task.PersonInCharge) // Nhóm theo tên người
				.ToList();

			var listEmailSendVm = new List<EmailSendVm>();
			foreach (var group in groupedTasks)
			{
				Console.WriteLine($"Người phụ trách: {group.Key}");
				string[] name = group.Key.Split(',');
				foreach (var item in name)
				{
					listEmailSendVm.Add(new EmailSendVm() { User = await GetUser(item, user), FABudgets = group.ToList() });
				}

			}


			return listEmailSendVm;
		}
		private async Task<UserVm> GetUser(string name, List<UserVm?> users)
		{
			var groupSendmails = await _applicationDbContext.GroupEmailSends.Where(p=>p.Content == name).ToArrayAsync();

			//var sendMailList = (from l in groupSendmails
			//					join u in users on l.Id equals u.UserId
			//					where l.Content == name
			//					select new { u.Fullname, u.Email }).ToList();

			var sendMailList = (from l in groupSendmails
								join u in users on l.Id equals u.UserId
								select new { u.Fullname, u.Email }).ToList();

			var userVm = sendMailList.Select(l => new UserVm() { Fullname = l.Fullname, Email = l.Email }).SingleOrDefault();

			return userVm;
		}
	}

}
