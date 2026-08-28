using ItemMaster.Server.Data;
using ItemMaster.Server.Data.Entities;
using ItemMaster.Server.Extensions;
using ItemMaster.Shared.Extensions;
using ItemMaster.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ItemMaster.Server.Services
{
    public class temmasterRegister : IItemmasterRegister
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityService _utilityService;
        private readonly ISendmailServices _sendmailServices;
        private readonly IImageServices _ImageServices;
        public temmasterRegister(ApplicationDbContext context, IUtilityService utilityService, ISendmailServices sendmailServices, IImageServices imageServices)
        {
            _context = context;
            _utilityService = utilityService;
            _sendmailServices = sendmailServices;
            _ImageServices = imageServices;
        }
        public async Task<List<ItemmasterVm>> GetAll()
        {
            var vm = await _context.ItemMasterRegisters.Join(_context.ItemDetails, item => item.Id, content => content.IdItemMasterRegister, (item, content) => new { ItemmasterVm = item, ItemmasterDetails = content })
                .ToListAsync();
            var records = vm.Select(p => new ItemmasterVm()
            {
                Id = p.ItemmasterVm.Id,
                CreateDate = p.ItemmasterVm.CreateDate,
                IdSignature = p.ItemmasterVm.IdSignature,
                RequestDate = p.ItemmasterVm.RequestDate,
                Status = p.ItemmasterVm.Status,
                Owner = p.ItemmasterVm.Own,
            }).ToList();


            return records;
        }

        public Task<Shared.Model.Wrapper.IResult> GetItemmaster(string Id)
        {
            throw new NotImplementedException();
        }
        public Task<Shared.Model.Wrapper.IResult> Update(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Post(F7782CreateRequest f7782CreateRequest)
        {
            var _userId = await _utilityService.GetUserAsync();
            using (var transaction = _context.Database.BeginTransaction())
            {
                var itemmaster = new ItemMasterRegister()
                {
                    RequestDate = f7782CreateRequest.RequestDate,
                    SAPCompletetionDate = f7782CreateRequest.SAPCompletetionDate,
                    Ver = f7782CreateRequest.Ver,
                    Own = _userId.Id,
                    //     IdSignature = f7782CreateRequest.IdTemplate,
                    CreateDate = DateTime.Now,
                    SectionReceive = f7782CreateRequest.SectionReceive
                };
                _context.ItemMasterRegisters.Add(itemmaster);
                await _context.SaveChangesAsync();

                await ProcessItemDetail(f7782CreateRequest, itemmaster);
                await TrackingProcess(f7782CreateRequest, itemmaster, _userId);
                //   await ProcessTemplateWorkflow(f7782CreateRequest, itemmaster);

                var result = await _context.SaveChangesAsync();
                transaction.Commit();
                if (result > 0)
                {
                    var se = new SendmailRequest()
                    {
                        Displayname = f7782CreateRequest.SendUser.Fullname,
                        Tomail = f7782CreateRequest.SendUser.Email,
                        Frommail = _userId.Email,
                        url = "http://172.16.34.150/F7782Details/" + itemmaster.Id.ToString(),
                        Data = "New Item Master Register"
                    };
                    _sendmailServices.SendEmail(se);
                    return new ServiceResponse<int>() { IsSuccess = true, Message = "OK", Data = itemmaster.Id };
                }
                else
                {
                    transaction.Rollback();
                    return new ServiceResponse<int>() { IsSuccess = false, Message = "false", Data = 0 };
                }
            }
        }
        async Task ProcessItemDetail(F7782CreateRequest request, ItemMasterRegister itemMaster)
        {
            int idItemmaster = itemMaster.Id;
            foreach (var item in request.ItemVms)
            {
                var res = new ItemDetails()
                {
                    IdItemMasterRegister = idItemmaster,
                    ItemCode = item.ItemCode,
                    CatalogeCode = item.CatalogCode,
                    ItemName = item.ItemName,
                    ETA = item.ETA,
                    Remark = item.Remark,
                    LastModifieDate = DateTime.Now,
                    Status = true
                };
                await _context.ItemDetails.AddAsync(res);
            }
        }
        async Task ProcessTemplateWorkflow(F7782CreateRequest request, ItemMasterRegister itemMaster)
        {
            string idSignature = request.IdtemplateWorkflow;
            int idTemplate = int.Parse(request.IdTemplate);

            if (request.signatureInItemMasterVms.Count() > 0)
            {
                _context.SignatureInItemMasters.RemoveRange(_context.SignatureInItemMasters.Where(x => x.IdItemMaster == itemMaster.Id));
                _context.SaveChanges();
            }
            var q = await _context.SignatureDetails.Where(p => p.SignatureId == int.Parse(idSignature)).ToListAsync();
            foreach (var item in q)
            {
                if (await _context.SignatureInItemMasters.Where(p => p.IdSignature == item.Id.ToString() && idTemplate == itemMaster.Id).SingleOrDefaultAsync() == null)
                {
                    var c = new SignatureInItemMaster()
                    {
                        IdItemMaster = itemMaster.Id,
                        IdSignature = item.Id.ToString(),
                        DateCreate = DateTime.Now,
                        InConfirm = false,
                    };
                    await _context.SignatureInItemMasters.AddAsync(c);
                }
            }


            var q2 = await _context.WorkflowTemplates.Where(p => p.IdTemplate == idTemplate).ToListAsync();
            List<TemplateInItemMaster> en = new List<TemplateInItemMaster>();
            foreach (var item in q2.OrderBy(o => o.Route))
            {
                //if (item == null) continue;
                //if (await _context.TemplateInItemMasters.Where(p => p.IdTemplate == item.Id && p.IdItemMaster == itemMaster.Id).SingleOrDefaultAsync() == null)
                //{

                //}
                var ob = new TemplateInItemMaster()
                {
                    IdItemMaster = itemMaster.Id,
                    IdTemplate = item.Id.ToString(),
                    Isconfirm = false,
                };
                en.Add(ob);
            }
            await _context.TemplateInItemMasters.AddRangeAsync(en);
        }
        async Task TrackingProcess(F7782CreateRequest request, ItemMasterRegister itemMaster, User user)
        {
            int idItemmaster = itemMaster.Id;
            var newMadeBy = new Tracking()
            {
                IdItemMaster = idItemmaster.ToString(),
                UserConfirm = itemMaster.Own,
                LastModifiedDate = DateTime.Now,
                IsConfirmed = true,
                Alias = HeadTracking.RequestDiv,
                HeadSign = HeadTracking.MadeBy,
                SendFrom = itemMaster.Own,
                Route = 1,
                Sign = _ImageServices.GenerateImage(user.Code, DateTime.Now.ToString("yyyy/MM/dd"), "Meo").Result,
                Type = HeadTracking.MadeBy
            };
            await _context.Trackings.AddAsync(newMadeBy);

            var newNextStep = new Tracking()
            {
                IdItemMaster = idItemmaster.ToString(),
                UserConfirm = request.SendUser.UserId,
                LastModifiedDate = DateTime.Now,
                IsConfirmed = false,
                Alias = HeadTracking.RequestDiv,
                HeadSign = HeadTracking.CheckedBy,
                SendFrom = itemMaster.Own,
                Route = 2,
                Type = HeadTracking.CheckedBy
            };
            await _context.Trackings.AddAsync(newNextStep);
        }
        /// <summary>
        /// Item master Detail
        /// </summary>
        /// <param name="id">Id Item master</param>
        /// <returns>F7782Details Object</returns>
        public async Task<F7782Details> Get7782Detail(string id)
        {
            var vm = await _context.ItemMasterRegisters.
                Join(_context.ItemDetails, item => item.Id, content => content.IdItemMasterRegister, (item, content) => new { ItemmasterVm = item, ItemmasterDetails = content }).
                Where(o => o.ItemmasterVm.Id == int.Parse(id))
                .ToListAsync();

            var query = (from l in _context.ItemMasterRegisters
                         where l.Id == int.Parse(id)
                         select l).SingleOrDefault();

            var obSuggest = GetSuggest(id, query.SectionReceive).Result;
            //    var listEmailSetting = _context.SettingEmails.ToList().Select(p=> new SettingEmailRequestVm() {id = p.id, Section = p.Section, UserId = p.UserId }).ToList();

            var ab = new F7782Details()
            {
                Id = query.Id,
                Status = query.Status,
                RequestDate = query.RequestDate,
                Ver = query.Ver,
                SAPCompletetionDate = query.SAPCompletetionDate,
                ItemVms = ItemVms(query.Id).Result,
                CreateDate = query.CreateDate,
                DisplayCofirm = DisplayConfirm(int.Parse(id)).Result,
                trackingVms = GetTrackingList(id).Result,
                suggestSend = obSuggest,
                //       settingEmailRequestVms = listEmailSetting
                //     signatureInItemMasterVms = ProcessGetSignatureInItemMasterVm(int.Parse(id)).Result,
                //     templateInItemMasterVms = ProcessGetWorkflowTemplate(int.Parse(id)).Result
            };
            return ab;
        }
        async Task<TrackingUpdateRequest> GetSuggest(string idItemmaster, string setting)
        {
            var user = await _utilityService.GetUserAsync(); // Lấy ra User
            int parentId = user.ParentId;


            try
            {
                var query = await _context.Trackings.Where(p => p.SendFrom == user.Id).OrderByDescending(o => o.Id).ToListAsync();
                if (query.Count() > 0)
                {
                    string userId = query.FirstOrDefault().UserConfirm;
                    var GetListPerrent = await _context.Trackings.Where(p => p.IdItemMaster == idItemmaster && p.HeadSign == HeadTracking.ApprovedBy).ToListAsync();
                    var dem = GetListPerrent.Count();
                    if (parentId == 0)
                    {
                        //Get next Step
                        if (dem == 1)
                        {
                            var getNextDiv = _context.SettingEmails.Where(p => p.Section == setting).SingleOrDefault().UserId;
                            userId = getNextDiv;
                        }
                        if (dem == 2)
                        {
                            var getNextDiv = _context.SettingEmails.Where(p => p.Section == setting).SingleOrDefault().UserId;
                            userId = getNextDiv;
                        }
                    }

                    var _nextUser = await _utilityService.GetByIdAsync(userId);
                    return new TrackingUpdateRequest()
                    {
                        SendUser = new UserVm() { UserId = _nextUser.UserId, Email = _nextUser.Email, Username = _nextUser.Username, Fullname = _nextUser.Fullname, Code = _nextUser.Code, ParrenId = _nextUser.ParrenId }
                    };
                }
                else
                {
                    var GetListPerrent = await _context.Trackings.Where(p => p.IdItemMaster == idItemmaster && p.HeadSign == HeadTracking.ApprovedBy).ToListAsync();
                    var dem = GetListPerrent.Count();
                    string userId = "";
                    if (parentId == 0)
                    {
                        //Get next Step
                        if (dem == 1)
                        {
                            var getNextDiv = _context.SettingEmails.Where(p => p.Section == setting).SingleOrDefault().UserId;
                            userId = getNextDiv;
                        }
                        if (dem == 2)
                        {
                            var getNextDiv = _context.SettingEmails.Where(p => p.Section == setting).SingleOrDefault().UserId;
                            userId = getNextDiv;
                        }
                    }

                    var _nextUser = await _utilityService.GetByIdAsync(userId);
                    return new TrackingUpdateRequest()
                    {
                        SendUser = new UserVm() { UserId = _nextUser.UserId, Email = _nextUser.Email, Username = _nextUser.Username, Fullname = _nextUser.Fullname, Code = _nextUser.Code, ParrenId = _nextUser.ParrenId }
                    };
                }

            }
            catch (Exception)
            {
                return new TrackingUpdateRequest()
                {
                    SendUser = new UserVm() { }
                };
            };
        }

        async Task<List<TrackingVm>> GetTrackingList(string idItemmaster)
        {
            var query = await _context.Trackings.Where(p => p.IdItemMaster == idItemmaster)
                .Select(o => new TrackingVm()
                {
                    Id = o.Id,
                    Alias = o.Alias,
                    HeadSign = o.HeadSign,
                    IdItemMaster = o.IdItemMaster,
                    IsConfirmed = o.IsConfirmed,
                    LastModifiedDate = o.LastModifiedDate,
                    Route = o.Route,
                    Sign = o.Sign,
                    Type = o.Sign,
                    UserConfirm = o.UserConfirm
                }).ToListAsync();

            return query;
        }
        async Task<bool> DisplayConfirm(int idItemmaster)
        {
            //var result = false;
            //var _userId = await _utilityService.GetUserId();

            //var query = await (
            // from l in _context.SignatureInItemMasters
            // join i in _context.Signatures on l.IdSignature equals i.Id.ToString()
            // join k in _context.SignatureDetails on i.Id equals k.SignatureId
            // where l.IdItemMaster == idItemmaster && k.UserCode == _userId
            // select new { l, i, k }).ToListAsync();

            //foreach (var item in query.OrderBy(p => p.k.Route))
            //{
            //    if (string.IsNullOrEmpty(item.l.Sign))
            //    {
            //        result = true;
            //        break;
            //    }
            //}
            //if (result == false)
            //{
            //    var res = CheckTemplate(idItemmaster, _userId).Result;
            //    result = res;
            //}
            var result = false;
            var _userId = await _utilityService.GetUserId();
            string _idItemmaster = idItemmaster.ToString();

            var query = await (
             from l in _context.Trackings
             where l.IdItemMaster == _idItemmaster && l.UserConfirm == _userId
             select l).ToListAsync();

            foreach (var item in query.OrderBy(p => p.Route))
            {
                if (string.IsNullOrEmpty(item.Sign) || item.IsConfirmed == false)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        async Task<bool> CheckTemplate(int idItemmaster, string _userId)
        {
            bool result = false;
            var query = await (
             from l in _context.TemplateInItemMasters
             join i in _context.Workflows on l.IdTemplate equals i.Id.ToString()
             join k in _context.WorkflowTemplates on i.Id equals k.IdTemplate
             where l.IdItemMaster == idItemmaster && k.UserConfirm == _userId
             select new { l, i, k }).ToListAsync();
            foreach (var item in query)
            {
                if (string.IsNullOrEmpty(item.l.Sign))
                {
                    result = true;
                    break;
                }
            }
            return result;

        }
        async Task<List<ItemVm>> ItemVms(int id)
        {
            var query = (from l in _context.ItemDetails
                         where l.IdItemMasterRegister == id
                         select new ItemVm()
                         {
                             Id = l.Id,
                             IdF7782 = l.Id,
                             ItemCode = l.ItemCode,
                             CatalogCode = l.CatalogeCode,
                             ItemName = l.ItemName,
                             ETA = l.ETA,
                             Group = l.Group,
                             Remark = l.Remark,
                         }).ToList();
            return query;
        }
        async Task<WorkFlowTemplateVm> ProcessWorkflowDetail(int id)
        {
            var o = await _context.WorkflowTemplates.Where(p => p.Id == id).SingleOrDefaultAsync();
            var ob = new WorkFlowTemplateVm()
            {
                Id = o.Id,
                Type = o.Type,
                Status = o.Status,
                LastModifieDate = o.LastModifieDate,
                Alias = o.Alias,
                Route = o.Route.ToString(),
                Userconfirm = o.UserConfirm
            };
            //.Select(o => new WorkFlowTemplateVm()
            //{
            //    Id = o.Id,
            //    Type = o.Type,
            //    Status = o.Status,
            //    LastModifieDate = o.LastModifieDate,
            //    Alias = o.Alias,
            //    Route = o.Route.ToString(),
            //    Userconfirm = o.UserConfirm
            //}).SingleOrDefaultAsync();

            return ob;
        }
        public SignatureDetailsVm ProcessSignatureDetail(int id)
        {
            var o = _context.SignatureDetails.SingleOrDefault(p => p.Id == id);
            var sign = new SignatureDetailsVm()
            {
                Id = o.Id,
                Type = o.Type,
                LastModifieDate = o.LastModifieDate,
                Route = o.Route.ToString(),
                Idsignature = o.SignatureId,
                Isconfirm = o.Isconfirmed,
                Usercode = o.UserCode
            };

            return sign;
        }
        public async Task<List<SignatureInItemMasterVm>> ProcessGetSignatureInItemMasterVm(int idItemmaster)
        {
            try
            {
                var tem = await (
               from l in _context.SignatureInItemMasters
               join i in _context.Signatures on l.IdSignature equals i.Id.ToString()
               join k in _context.SignatureDetails on i.Id equals k.SignatureId
               where l.IdItemMaster == idItemmaster
               select new { l, i, k }).ToListAsync();
                var res = tem.Select(p => new SignatureInItemMasterVm()
                {
                    IdItemMaster = idItemmaster.ToString(),
                    IdSignature = p.i.Id.ToString(),
                    InConfirm = p.l.InConfirm,
                    Sign = p.l.Sign,
                    DateCreate = p.l.DateCreate,
                    UserVm = _utilityService.GetByIdAsync(p.k.UserCode).Result,
                    SignatureDetailsVm = ProcessSignatureDetail(Convert.ToInt32(p.k.Id))
                }).ToList();
                return res;
            }
            catch (Exception ex)
            {
                return new List<SignatureInItemMasterVm>();
            }


        }
        async Task<List<TemplateInItemMasterVm>> ProcessGetWorkflowTemplate(int idItemmaster)
        {
            var tem = (
               from l in _context.TemplateInItemMasters
               join i in _context.Workflows on Convert.ToInt32(l.IdTemplate) equals i.Id
               join k in _context.WorkflowTemplates on i.Id equals k.IdTemplate
               where l.IdItemMaster == idItemmaster
               select new { l, i, k }).ToList().Select(p => new TemplateInItemMasterVm()
               {
                   IdItemmaster = idItemmaster.ToString(),
                   IdTemplate = p.i.Id.ToString(),
                   IsConfirm = p.l.Isconfirm,
                   Sign = p.l.Sign,
                   UserVm = _utilityService.GetByIdAsync(p.k.UserConfirm).Result,
                   WorkFlowTemplateVm = ProcessWorkflowDetail(p.k.Id).Result
               }).ToList();
            return tem;
        }
        public async Task<ServiceResponse<int>> Confirm(int idTracking, TrackingUpdateRequest trackingUpdateRequest)
        {
            string idItemmaster = idTracking.ToString();
            var user = await _utilityService.GetUserAsync();

            //Find tracking update
            var query = await _context.Trackings.Where(p => p.UserConfirm == user.Id && p.IdItemMaster == idItemmaster && p.IsConfirmed == false).FirstOrDefaultAsync();

            //update Current Tracking
            var currentTracking = _context.Trackings.Where(p => p.Id == query.Id).SingleOrDefault();
            try
            {
                string dt = _ImageServices.GenerateImage(user.Code, DateTime.Now.ToString("yyyy/MM/dd"), "Meo").Result;
                currentTracking.Sign = Convert.ToString(dt);
                currentTracking.IsConfirmed = true;
                currentTracking.LastModifiedDate = DateTime.Now;
            }
            catch (Exception)
            {
                currentTracking.Sign = "ABc";

            }
            _context.Trackings.Update(currentTracking);

            // Create Next Tracking
            if (currentTracking.Type != "Plan")
            {
                int idItemMaster = int.Parse(currentTracking.IdItemMaster);

                var userNextTracking = await _utilityService.GetByIdAsync(trackingUpdateRequest.SendUser.UserId);
                var propertyNextTracking = CheckApproveOrConfirm(userNextTracking, currentTracking.Type, idItemMaster.ToString()).Result;
                var nextTracking = new Tracking()
                {
                    IdItemMaster = currentTracking.IdItemMaster,
                    UserConfirm = trackingUpdateRequest.SendUser.UserId,
                    LastModifiedDate = DateTime.Now,
                    SendFrom = currentTracking.UserConfirm,
                    IsConfirmed = false,
                    Alias = propertyNextTracking.Alias,
                    HeadSign = propertyNextTracking.HeadSign,
                    Route = currentTracking.Route + 1,
                    Type = propertyNextTracking.Type
                };
                _context.Trackings.Add(nextTracking);
            }
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var se = new SendmailRequest()
                {
                    Displayname = trackingUpdateRequest.SendUser.Fullname,
                    Tomail = trackingUpdateRequest.SendUser.Email,
                    Frommail = user.Email,
                    url = "http://172.16.34.150/F7782Details/" + currentTracking.IdItemMaster,
                    Data = "Confirm Item Master Register"
                };

                _sendmailServices.SendEmail(se);

                return new ServiceResponse<int>() { Data = 1, IsSuccess = true, Message = "OK" };
            }
            else
            {
                return new ServiceResponse<int>() { Data = 1, IsSuccess = false, Message = "False" };
            }
        }
        public async Task<IEnumerable<UserVm>> GetlistUserAsync()
        {
            var data = await _utilityService.GetListUserAsync();
            return data;
        }
        public async Task<Tracking> CheckApproveOrConfirm(UserVm? user, string currentType, string idItemmaster)
        {
            var query = _context.Trackings.Where(p => p.IdItemMaster == idItemmaster && p.HeadSign == HeadTracking.ApprovedBy).ToList();
            var dem = query.Count();
            if (dem == 0 && user.ParrenId == 1)
            {
                return new Tracking() { Alias = HeadTracking.RequestDiv, HeadSign = HeadTracking.CheckedBy, Type = HeadTracking.CheckedBy };
            }
            if (dem == 0 && user.ParrenId == 0)
            {
                return new Tracking() { Alias = HeadTracking.RequestDiv, HeadSign = HeadTracking.ApprovedBy, Type = HeadTracking.ApprovedBy };
            }
            if (dem == 1 && user.ParrenId == 1 && currentType == HeadTracking.ApprovedBy)
            {
                return new Tracking() { Alias = HeadTracking.AnswerDiv, HeadSign = HeadTracking.MadeBy, Type = HeadTracking.MadeBy };
            }
            if (dem == 1 && user.ParrenId == 1 && currentType == HeadTracking.MadeBy)
            {
                return new Tracking() { Alias = HeadTracking.AnswerDiv, HeadSign = HeadTracking.CheckedBy, Type = HeadTracking.CheckedBy };
            }
            if (dem == 1 && user.ParrenId == 1 && currentType == HeadTracking.CheckedBy)
            {
                return new Tracking() { Alias = HeadTracking.AnswerDiv, HeadSign = HeadTracking.CheckedBy, Type = HeadTracking.CheckedBy };
            }
            if (dem == 1 && user.ParrenId == 0 && currentType == HeadTracking.CheckedBy)
            {
                return new Tracking() { Alias = HeadTracking.AnswerDiv, HeadSign = HeadTracking.ApprovedBy, Type = HeadTracking.ApprovedBy };
            }
            if (dem == 2 && user.ParrenId == 1 && currentType == HeadTracking.ApprovedBy)
            {
                return new Tracking() { Alias = HeadTracking.SAP, HeadSign = HeadTracking.CheckedBy, Type = HeadTracking.CheckedBy };
            }
            if (dem == 2 && user.ParrenId == 1 && currentType == HeadTracking.CheckedBy)
            {
                return new Tracking() { Alias = HeadTracking.SAP, HeadSign = HeadTracking.InputBy, Type = HeadTracking.InputBy };
            }
            if (dem == 2 && user.ParrenId == 1 && currentType == HeadTracking.InputBy)
            {
                return new Tracking() { Alias = HeadTracking.Plan, HeadSign = HeadTracking.ConfirmedBy, Type = HeadTracking.ConfirmedBy };
            }
            else
            {
                return new Tracking() { Alias = HeadTracking.Plan, HeadSign = HeadTracking.ConfirmedBy, Type = HeadTracking.ConfirmedBy };
            }
        }

        public async Task<List<ItemmasterVm>> GetByUserOwner()
        {
            var iduser = await _utilityService.GetUserId();

            var vm = await (from l in _context.ItemMasterRegisters
                            join i in _context.Trackings on l.Id.ToString() equals i.IdItemMaster
                            where i.UserConfirm == iduser && i.IsConfirmed == false
                            select new { l, i }).ToListAsync();

            //var trackingNotConfirmVm = await (from l in _context.Trackings
            //                                  where l.UserConfirm == iduser && l.IsConfirmed == false
            //                                  select l).ToListAsync();

            //var rere = from l in vm
            //           join i in trackingNotConfirmVm on l.l.Id equals i.
            //           select new { l, i };

            var records = vm.Select(p => new ItemmasterVm()
            {
                Id = p.l.Id,
                CreateDate = p.l.CreateDate,
                IdSignature = p.l.IdSignature,
                RequestDate = p.l.RequestDate,
                Status = p.l.Status,
                Owner = p.l.Own,
                SAPCompleteDate = p.l.SAPCompletetionDate,
                itemMasters = GetItemmasterDetails(p.l.Id).Result,
                trackingVms = GetTrackingList(p.l.Id.ToString()).Result
            }).ToList();


            return records;
        }
        async Task<List<ItemmasterDetails>> GetItemmasterDetails(int iditemmaster)
        {
            var query = await (from l in _context.ItemDetails
                               where l.IdItemMasterRegister == iditemmaster
                               select l).ToListAsync();
            var res = query.Select(p => new ItemmasterDetails()
            {
                Id = p.Id,
                IdItemMasterRegister = p.IdItemMasterRegister,
                CatalogeCode = p.CatalogeCode,
                ETA = p.ETA,
                ItemCode = p.ItemCode,
                ItemName = p.ItemName,
                LastModifieDate = p.LastModifieDate,
                Remark = p.Remark,
                Status = p.Status
            }).ToList();
            return res;
        }

        public async Task<List<ItemmasterVm>> GetListUserConfirmed()
        {
            var iduser = await _utilityService.GetUserId();

            var meo = from l in _context.Trackings
                      where l.UserConfirm == iduser && l.IsConfirmed == true
                      group l.IdItemMaster by l.IdItemMaster into g
                      select new { idItem = g.Key, list = g.ToList() };
            var meoVm = (from m in meo
                         join i in _context.ItemMasterRegisters on m.idItem equals i.Id.ToString()
                         select new { i }).ToList();

            //var vm = await (from l in _context.ItemMasterRegisters
            //                join i in _context.Trackings on l.Id.ToString() equals i.IdItemMaster
            //                where i.UserConfirm == iduser && i.IsConfirmed == true
            //                group i.IdItemMaster by l.CreateDate into g
            //                select new { itemid = g.Key, list = g.ToList() }).ToListAsync();

            var records = meoVm.Select(p => new ItemmasterVm()
            {
                Id = p.i.Id,
                CreateDate = p.i.CreateDate,
                IdSignature = p.i.IdSignature,
                RequestDate = p.i.RequestDate,
                Status = p.i.Status,
                TrangThaiHienTai = GetTrangThaiHienTai(p.i.Id.ToString()).Result,
                SAPCompleteDate = p.i.SAPCompletetionDate,
                //   itemMasters = GetItemmasterDetails(p.l.Id).Result,
                //   trackingVms = GetTrackingList(p.l.Id.ToString()).Result
            }).ToList();

            return records;
        }
        async Task<string> GetTrangThaiHienTai(string id)
        {
            var abc = _context.Trackings.Where(p => p.IdItemMaster == id && p.IsConfirmed == false).ToList();
            var getUserDisplay = "";
            if (abc.Count() > 0)
            {
                var dt = _utilityService.GetByIdAsync(abc.FirstOrDefault().UserConfirm).Result;
                getUserDisplay = dt.Fullname + " - Pending";
            }
            else
            {
                getUserDisplay = "Completed";

            }
            return getUserDisplay;
        }
        public async Task<List<SettingEmailRequestVm>> GetListSettingEmail()
        {
            var listEmailSetting = await _context.SettingEmails.ToListAsync();
            var a = listEmailSetting.Select(p => new SettingEmailRequestVm() { id = p.id, Section = p.Section, UserId = p.UserId }).ToList();
            return a;
        }
        public static string GetMyTable<T>(IEnumerable<T> list, params Func<T, object>[] fxns)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<TABLE>\n");
            foreach (var item in list)
            {
                sb.Append("<TR>\n");
                foreach (var fxn in fxns)
                {
                    sb.Append("<TD>");
                    sb.Append(fxn(item));
                    sb.Append("</TD>");
                }
                sb.Append("</TR>\n");
            }
            sb.Append("</TABLE>");

            return sb.ToString();
        }
    }
}
