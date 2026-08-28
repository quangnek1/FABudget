using ItemMaster.Server.Data;
using ItemMaster.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace ItemMaster.Server.Services
{
    public class MyF7782Repo : IMyF7782Repo
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        public MyF7782Repo(ApplicationDbContext context, IUtilityService utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }
        public async Task<IEnumerable<F7782CreateRequest>> GetMyF7782()
        {
            var _userId = await _utilityService.GetUserAsync();

            var list = await (from l in _context.ItemMasterRegisters
                              where l.Own == _userId.Id
                              select new F7782CreateRequest
                              {
                                  Id = l.Id,
                                  RequestDate = l.RequestDate,
                                  SAPCompletetionDate = l.SAPCompletetionDate,
                                  Status = l.Status,
                                  ItemVms = (List<ItemVm>)_context.ItemDetails.Where(o => o.IdItemMasterRegister == l.Id).Select(p => new ItemVm
                                  {
                                      Id = p.Id,
                                      IdF7782 = l.Id,
                                      ItemCode = p.ItemCode,
                                      CatalogCode = p.CatalogeCode,
                                      ItemName = p.ItemName,
                                      ETA = p.ETA,
                                      Group = p.Group
                                  })
                              }).ToListAsync()
                            ;

            return list;
        }
    }
}
