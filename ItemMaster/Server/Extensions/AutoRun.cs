using ItemMaster.Server.Data;

namespace ItemMaster.Server.Extensions
{
    public class AutoRun : IAutoRun
    {
        private readonly ApplicationDbContext _context;   
        public AutoRun(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Run()
        {
            var query = _context.ItemDetails.Where(p => p.ETA == DateTime.Now);
        }
    }
}
