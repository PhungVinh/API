using Encryption.Common;
using Encryption.Constant;
using Encryption.ContextFactory;
using Encryption.DTO;
using Encryption.Models;
using Encryption.Repositories;
using Encryption.Scheduler;
using Encryption.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Encryption.DataAccess
{
    public class EncryptionDA : IEncryptionRepository
    {
        private static EncryptionCommon common = new EncryptionCommon();
        private IDistributedCache _distributedCache;
        private static readonly string keyString = EncryptionConstant.keyString;
        private List<string> lst;
        public string strConnect { get; set; }
        private CRM_MPContext db = new CRM_MPContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MPContext>());

        public EncryptionDA()
        {
            lst = new List<string>() { "Tài khoản người dùng",
                                        "Tên đầy đủ",
                                        "Email",
                                        "Số điện thoại",
                                        "Ảnh",
                                        "Địa chỉ",
                                        "Giới tính",
                                        "Ngày sinh",
                                        "Mã đơn vị"};

        }
        public void LoadContext(string orgCode, IDistributedCache distributedCache)
        {
            if (db == null || strConnect != orgCode)
            {
                string conn = EncryptionConstant.SQL_MP_CONNECTION;
                var optionsBuilder = new DbContextOptionsBuilder<CRM_MPContext>();
                optionsBuilder.UseSqlServer(conn.Replace(EncryptionConstant.MP, orgCode), providerOptions => providerOptions.CommandTimeout(300));
                db = new CRM_MPContext(optionsBuilder.Options);
            }
            _distributedCache = distributedCache;
        }
        #region vudt
        public async Task<ResponseDTO> GetListAttributesEncryption(string orgCode)
        {
            try
            {
                var moduleName = GetListModule(EncryptionConstant.MASTER).Response;
                List<TblMasterMenu> lstMenu = new List<TblMasterMenu>();
                foreach (var item in moduleName)
                {
                    TblMasterMenu tblMasterMenu = new TblMasterMenu();
                    tblMasterMenu = item as TblMasterMenu;
                    lstMenu.Add(tblMasterMenu);
                }
                var data = await db.TblEncryption.Select(x => new EncryptionDTO()
                {
                    AttributeCode = x.AttributeCode,
                    AttributeLabel = x.AttributeLabel,
                    ModuleName = x.ModuleName,
                    MenuCode = x.ParentCode,
                    UpdateDate = x.UpdateDate,
                    ExecuteTime = x.IsDone == true ? EncryptionConstant.MS0015 : common.GetTimeExecuteEncrpytion(x.UpdateDate),
                    EncryptionStatus = x.EncryptionStatus
                }).AsNoTracking().OrderByDescending(x => x.UpdateDate).ToListAsync();
                return new ResponseDTO() { StatusCode = 200, Response = data };
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { StatusCode = 500, Response = ex.Message };
            }
        }
        public ResponseDTO UpdateEncrpytion(AttributeModel model)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (var item in model.tblVocattributes)
                    {
                        var result = db.TblEncryption.Where(x => x.AttributeCode == item.AttributeCode && x.ParentCode == item.ParentCode).FirstOrDefault();
                        if (result != null)
                        {
                            // Kiểm tra xem trường dữ liệu đã được mã hóa lần nào chưa
                            if (result.IsFirst == true)
                            {
                                db.TblEncryption.Remove(result);
                            }
                            else
                            {
                                if (result.EncryptionStatus == result.FinalizationStatus)
                                {
                                    // Kiểm tra trạng thái mã hóa của trường dữ liệu
                                    if (result.EncryptionStatus == true)
                                    {
                                        result.UpdateDate = DateTime.Now;
                                        result.EncryptionStatus = false;
                                        result.UpdatedBy = model.UserName;
                                        result.IsDone = false;
                                        db.Entry(result).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        result.UpdateDate = DateTime.Now;
                                        result.EncryptionStatus = true;
                                        result.UpdatedBy = model.UserName;
                                        result.IsDone = false;
                                        db.Entry(result).State = EntityState.Modified;
                                    }
                                }
                                else
                                {
                                    result.IsDone = true;
                                    result.EncryptionStatus = result.FinalizationStatus;
                                }
                            }
                        }
                        else
                        {
                            // Thêm mới trường dữ liệu cần mã hóa
                            if (item.ParentCode == EncryptionConstant.ADMIN_USER)
                            {
                                var attributeEncryption = new TblEncryption();
                                attributeEncryption.AttributeCode = item.AttributeCode;
                                attributeEncryption.AttributeLabel = item.AttributeLabel;
                                attributeEncryption.CreatedBy = model.UserName;
                                attributeEncryption.CreateDate = DateTime.Now;
                                attributeEncryption.ParentCode = item.ParentCode;
                                attributeEncryption.ModuleName = item.ModuleName;
                                attributeEncryption.EncryptionStatus = true;
                                attributeEncryption.UpdateDate = DateTime.Now;
                                attributeEncryption.IsFirst = true;
                                attributeEncryption.IsDone = false;
                                db.TblEncryption.Add(attributeEncryption);
                            }
                            else
                            {
                                var label = db.TblVocattributes.Where(x => x.AttributeCode == item.AttributeCode && x.ModuleParent == item.ParentCode).AsNoTracking().FirstOrDefault();
                                if (label != null)
                                {
                                    var attributeEncryption = new TblEncryption();
                                    attributeEncryption.AttributeCode = item.AttributeCode;
                                    attributeEncryption.AttributeLabel = label.AttributeLabel;
                                    attributeEncryption.CreatedBy = model.UserName;
                                    attributeEncryption.CreateDate = DateTime.Now;
                                    attributeEncryption.ParentCode = item.ParentCode;
                                    attributeEncryption.ModuleName = item.ModuleName;
                                    attributeEncryption.EncryptionStatus = true;
                                    attributeEncryption.UpdateDate = DateTime.Now;
                                    attributeEncryption.IsFirst = true;
                                    attributeEncryption.IsDone = false;
                                    db.TblEncryption.Add(attributeEncryption);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseDTO() { StatusCode = 500, Response = ex.Message };
                }

                db.SaveChanges();
                scope.Complete();
                return new ResponseDTO() { StatusCode = 200, Response = EncryptionConstant.MS0013 };
            }
        }
        public async void SchedulerExecuteEncrpytion(IScheduler scheduler)
        {
            try
            {
                IJobDetail job = JobBuilder.Create<EncryptionJob>()
                    .WithIdentity("Encryption")
                    .Build();
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("Encryption")
                    .WithCronSchedule("	0 54 11 1/1 * ? *")
                    //.StartAt(new DateTimeOffset(DateTime.Now.AddSeconds(5)))
                    //.WithDailyTimeIntervalSchedule()
                    //.WithSimpleSchedule(x => x.WithIntervalInSeconds(15).RepeatForever())
                    .Build();
                await scheduler.ScheduleJob(job, trigger);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseDTO> GetListAttributesWithParentCode(string parentCode, string orgCode)
        {
            try
            {
                var moduleName = GetListModule(EncryptionConstant.MASTER).Response;
                List<TblMasterMenu> lstMenu = new List<TblMasterMenu>();
                foreach (var item in moduleName)
                {
                    TblMasterMenu tblMasterMenu = new TblMasterMenu();
                    tblMasterMenu = item as TblMasterMenu;
                    lstMenu.Add(tblMasterMenu);
                }
                if (parentCode == EncryptionConstant.ADMIN_USER)
                {
                    List<AttributeDTO> lstTblMasterMenus = new List<AttributeDTO>();
                    for (int i = 0; i < lst.Count; i++)
                    {
                        AttributeDTO tblMasterMenu = new AttributeDTO();
                        tblMasterMenu.AttributeCode = (i + 1).ToString();
                        tblMasterMenu.AttributeLabel = lst[i];
                        tblMasterMenu.ModuleName = lstMenu.Where(x => x.MenuCode == EncryptionConstant.ADMIN_USER).FirstOrDefault().MenuName;
                        lstTblMasterMenus.Add(tblMasterMenu);
                    }
                    return new ResponseDTO() { StatusCode = 200, Response = lstTblMasterMenus };
                }
                else
                {
                    var lstAttribute = await db.TblVocattributes.Where(x => x.ModuleParent == parentCode && x.IsReuse != true).AsNoTracking().ToListAsync();
                    var data = lstAttribute.Where(x => !db.TblEncryption.Where(c => c.AttributeCode == x.AttributeCode && c.ParentCode == x.ModuleParent).Select(c => c.AttributeCode).Contains(x.AttributeCode)).Select(x => new AttributeDTO()
                    {
                        AttributeCode = x.AttributeCode,
                        AttributeLabel = x.AttributeLabel,
                        ModuleName = lstMenu.Where(c => c.MenuCode == parentCode).FirstOrDefault().MenuName
                    }).ToList();
                    return new ResponseDTO() { StatusCode = 200, Response = data };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { StatusCode = 500, Response = ex.Message };
            }
        }
        public async void ExecuteEncrpytion()
        {
            try
            {
                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 1);
                DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                var lstEncrpytion = await db.TblEncryption.Where(x => x.UpdateDate > startDate && x.UpdateDate < endDate).AsNoTracking().ToListAsync();
                if (lstEncrpytion.Count > 0)
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        foreach (var item in lstEncrpytion)
                        {
                            var lstCimsAttributeValue = await db.TblCimsattributeValue.Where(x => x.AttributeCode == item.AttributeCode && x.Module == item.ParentCode).ToListAsync();
                            foreach (var cimsAttributeValue in lstCimsAttributeValue)
                            {
                                if (item.EncryptionStatus != item.FinalizationStatus)
                                {
                                    if (item.EncryptionStatus == true)
                                    {
                                        cimsAttributeValue.AttributeValue = common.EncryptStringAES(cimsAttributeValue.AttributeValue, keyString);
                                        db.Entry(cimsAttributeValue).State = EntityState.Modified;
                                        item.IsDone = true;
                                        item.IsFirst = false;
                                        item.FinalizationStatus = item.EncryptionStatus;
                                        db.Entry(item).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        cimsAttributeValue.AttributeValue = common.DecryptStringAES(cimsAttributeValue.AttributeValue, keyString);
                                        db.Entry(cimsAttributeValue).State = EntityState.Modified;
                                        item.IsDone = true;
                                        item.IsFirst = false;
                                        item.FinalizationStatus = item.EncryptionStatus;
                                        db.Entry(item).State = EntityState.Modified;
                                    }
                                }
                                else
                                {
                                    item.IsDone = true;
                                }
                            }
                        }
                        await db.SaveChangesAsync();
                        scope.Complete();
                        Trace.WriteLine(EncryptionConstant.SuccessEncryption);
                    }
                }
                Trace.WriteLine(EncryptionConstant.NotExecuteEncryption);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(EncryptionConstant.FailureEncryption + ex.Message);
            }
        }
        public ResponseDTO GetListModule(string orgCode)
        {
            try
            {
                var data = common.GetAllModule().ToList();
                return new ResponseDTO() { StatusCode = 200, Response = data[0] };
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { StatusCode = 500, Response = ex.Message };
            }
        }
        #endregion
    }
}
