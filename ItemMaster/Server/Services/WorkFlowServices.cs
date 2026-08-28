using ItemMaster.Server.Data;
using ItemMaster.Server.Data.Entities;
using ItemMaster.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ItemMaster.Server.Services
{
    public class WorkFlowServices : IWorkFlowServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        public WorkFlowServices(ApplicationDbContext context, IUtilityService utilityService)
        {
            _context = context;
            _utilityService = utilityService;
        }
        public async Task<ServiceResponse<string>> Create(WorkFlowVm request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var workflow = new Signature()
                    {
                        Id = int.Parse(request.Id),
                        CreateBy = request.CreateBy,
                        CreateDate = request.CreateDate,
                        Status = request.Status,
                    };
                    if (request.workFolowDetailVms.Count > 0)
                    {
                        await ProcessCreateWorkFlowDetals(request, workflow);
                    }

                    var result = await _context.SaveChangesAsync();
                    transaction.Commit();

                    if (result > 0)
                    {
                        return new ServiceResponse<string> { Message = "Successfuly", IsSuccess = true };
                    }
                    else
                    {
                        transaction.Rollback();
                        return new ServiceResponse<string> { Message = "fald", IsSuccess = false };
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return new ServiceResponse<string> { Message = "failed", IsSuccess = false };
                }
            }
        }
        private async Task ProcessCreateWorkFlowDetals(WorkFlowVm request, Signature signature)
        {
            #region conment
            //if (request.signatureInItemMasterVms.Count() > 0)
            //{
            //    _context.SignatureInItemMasters.RemoveRange(_context.SignatureInItemMasters.Where(x => x.IdSignature == signature.Id.ToString()));
            //   await _context.SaveChangesAsync();
            //}
            //foreach (var sign in request.signatureInItemMasterVms)
            //{
            //    if (sign == null) continue;
            //    string signId = sign.
            //    if (await _context.SignatureInItemMasters.FindAsync(improvement.Id, tagId) == null)
            //    {
            //        _db.TagInImprovementProposals.Add(new TagInImprovementProposal()
            //        {
            //            ImprovementProposalId = improvement.Id,
            //            TagId = tagId.ToString()
            //        });
            //    }
            //}
            #endregion
            foreach (var item in request.workFolowDetailVms)
            {
                var workFolowDetailVm = new SignatureDetails()
                {
                    Id = int.Parse(item.Id),
                    Isconfirmed = item.IsConfirmed,
                    LastModifieDate = item.LastModifieDate,
                    SignatureId = int.Parse(request.Id),
                    Type = item.Type,
                    UserCode = item.UserCode,
                    Route = item.Route
                };
                await _context.SignatureDetails.AddAsync(workFolowDetailVm);

            }
        }
        public async Task<ServiceResponse<bool>> Delete(string Id)
        {
            var query = _context.Signatures.Where(o => o.Id == int.Parse(Id)).SingleOrDefault();
            if (query != null)
            {
                _context.Signatures.Remove(query);
                await _context.SaveChangesAsync();
                return new ServiceResponse<bool>() { Data = true, IsSuccess = true, Message = "Ok" };
            }
            return new ServiceResponse<bool>() { Data = false, IsSuccess = true, Message = "Fail" };
        }
        public async Task<ServiceResponse<bool>> Put(WorkFlowVm request)
        {
            var _object = await _context.Signatures.FindAsync(request.Id);
            if (request.workFolowDetailVms.Count() > 0)
            {
                _context.SignatureDetails.RemoveRange(_context.SignatureDetails.Where(x => x.SignatureId == int.Parse(request.Id)));
                _context.SaveChanges();
            }
            foreach (var item in request.workFolowDetailVms)
            {
                var _detailob = new SignatureDetails() { UserCode = item.UserCode, Route = item.Route, LastModifieDate = DateTime.Now, SignatureId = int.Parse(item.SignatureId), Type = item.Type };
                await _context.SignatureDetails.AddAsync(_detailob);
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ServiceResponse<bool>() { IsSuccess = true, Message = "Update Ok" };
            }
            else
            {
                return new ServiceResponse<bool>() { IsSuccess = false, Message = "Fald rồi" };
            }

        }
        public async Task<IEnumerable<WorkFlowVm>> Get()
        {
            var list = await (from l in _context.Signatures
                              select new WorkFlowVm
                              {
                                  Id = l.Id.ToString(),
                                  CreateBy = l.CreateBy,
                                  CreateDate = l.CreateDate,
                                  Status = l.Status,
                                  Name = l.Name,
                                  workFolowDetailVms = (List<WorkFolowDetailVm>)_context.SignatureDetails.Select(p => new WorkFolowDetailVm
                                  {
                                      Id = p.Id.ToString(),
                                      SignatureId = p.SignatureId.ToString(),
                                      UserCode = p.UserCode,
                                      Type = p.Type,
                                      LastModifieDate = p.LastModifieDate,
                                      IsConfirmed = p.Isconfirmed,
                                      Route = p.Route
                                  })
                              }).ToListAsync()
                            ;

            //var result = new ServiceResponse<List<WorkFlowVm>>()
            //{
            //    Data = list,
            //    IsSuccess = true,
            //    Message = "Ok"
            //};
            return list;
        }
        public async Task<IEnumerable<WorkFlowVm>> GetByUser()
        {
            var _userId = await _utilityService.GetUserAsync();

            var list = await (from l in _context.Signatures
                              where l.CreateBy == _userId.Id
                              select new WorkFlowVm
                              {
                                  Id = l.Id.ToString(),
                                  CreateBy = l.CreateBy,
                                  CreateDate = l.CreateDate,
                                  Status = l.Status,
                                  Name = l.Name,
                                  workFolowDetailVms = (List<WorkFolowDetailVm>)_context.SignatureDetails.Where(o => o.SignatureId == l.Id).Select(p => new WorkFolowDetailVm
                                  {
                                      Id = p.Id.ToString(),
                                      SignatureId = p.SignatureId.ToString(),
                                      UserCode = p.UserCode,
                                      Type = p.Type,
                                      LastModifieDate = p.LastModifieDate,
                                      IsConfirmed = p.Isconfirmed,
                                      Route = p.Route
                                  })
                              }).ToListAsync()
                            ;

            return list;
        }
    }
}
