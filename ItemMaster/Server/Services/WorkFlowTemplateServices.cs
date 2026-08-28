using ItemMaster.Server.Data;
using ItemMaster.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace ItemMaster.Server.Services
{
    public class WorkFlowTemplateServices : IworkFlowTemplateServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        public WorkFlowTemplateServices(ApplicationDbContext context, IUtilityService utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }
        public async Task<ServiceResponse<string>> Create(WorkFlowTemplateVm workFlowTemplateVm)
        {
            var response = new ServiceResponse<string>() { };
            return response = new ServiceResponse<string> { Message = "Fail", IsSuccess = false };

        }

        public async Task<IEnumerable<WorkflowtemplateRequestVm>> Get()
        {
            var _userId = await _utilityService.GetUserAsync();

            var list = await (from l in _context.WorkflowTemplates select l).Select(o => new WorkflowtemplateRequestVm()
            {
                Id = o.Id,
                LastModifieDate = o.LastModifieDate,
                Alias = o.Alias,
                Status = o.Status,
                UserConfirm = o.UserConfirm,
                Route = o.Route,
                Type = o.Type,
            }).ToListAsync();


            return list;
        }
    }
}
