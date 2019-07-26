using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using VOC.Constant;
using VOC.ContextFactory;
using VOC.Models;
using VOC.Repositories;
using VOC.ViewModel;

namespace VOC.DataAccess
{
    public class VOCProcessDA : IVOCProcessRepository
    {
        private CRM_MPContext db = new CRM_MPContext(new DbContextOptions<CRM_MPContext>());
        private IDistributedCache _distributedCache;
        public string strconnect { get; set; }
        private SP_VOCProcess sp;

        public VOCProcessDA()
        {
            sp = new SP_VOCProcess(db);
        }

        public VOCProcessDA(string orgCode, IDistributedCache distributedCache)
        {
            LoadContext(orgCode, distributedCache);
            sp = new SP_VOCProcess(db);
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public object SearchVOCProcess(int userId, string orgCode, string isShowAll, string textSearch, string isActive, int currPage, int recordperpage)
        {
            int checkCurrentVersion = 0;
            try
            {
                string connection = GetConnectionByOrgCode(orgCode);
                List<List<dynamic>> obj = sp.SearchVOCProcess(userId, orgCode, isShowAll, textSearch, isActive, currPage, recordperpage, connection);
                List<TblVocprocess> lstVOCProcess = obj[0].OfType<TblVocprocess>().ToList();
                PagePaging pagePaging = obj[1].OfType<PagePaging>().FirstOrDefault();
                if (lstVOCProcess.Count <= 0)
                {
                    return new { data = lstVOCProcess, pagePaging = pagePaging };
                }
                List<TblCategory> lstCategories = db.TblCategory.Where(c => c.IsDelete == false && c.CategoryTypeCode == "Incident").ToList();
                if (lstVOCProcess != null && lstCategories != null)
                {
                    foreach (var item in lstVOCProcess)
                    {


                        // Lấy loại sự vụ
                        if (!string.IsNullOrEmpty(item.VOCProcessType))
                        {
                            TblCategory category = lstCategories.Where(cate => cate.CategoryCode == item.VOCProcessType).FirstOrDefault();
                            if (category != null)
                            {
                                item.VOCProcessType = category.CategoryName;
                            }
                        }
                    }
                }
                List<ObjectVOCProccess> lstSearch = new List<ObjectVOCProccess>();
                foreach (var item in lstVOCProcess)
                {
                    checkCurrentVersion = 0;

                    ObjectVOCProccess objResult = ObjectSearch(item.VOCProcessCode, item.CurrentVersion.Value, orgCode);
                    objResult.CreateBy = item.CreateBy;
                    objResult.CreateDate = item.CreateDate;
                    objResult.UpdateDate = item.UpdateDate;
                    objResult.UpdateBy = item.UpdateBy;
                    // Sửa phiên bản hoạt động
                    List<TblVocprocessSteps> lstStep = db.TblVocprocessSteps.Where(s => s.VocprocessCode == item.VOCProcessCode).ToList();
                    foreach (var itemStep in lstStep)
                    {
                        if (itemStep.IsActive.HasValue)
                        {
                            if (itemStep.IsActive.Value)
                            {
                                checkCurrentVersion = itemStep.Version.Value;
                            }
                        }
                    }
                    if (checkCurrentVersion > 0)
                    {
                        objResult.CurrentVersion = checkCurrentVersion;
                    }
                    lstSearch.Add(objResult);
                }

                var response = new { data = lstSearch, pagePaging = pagePaging };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public object SearchVersionVOCProcess()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public int AddVOCProcess(ObjectVOCProccess model, string userName)
        {
            int resultAdd = VOCProcessConstant.AddVOCProcessFail;
            try
            {
                //TblVocprocess newVOCProcess = model.vocprocess;
                // Kiểm tra có bị trùng hay không
                TblVocprocess checkObject = db.TblVocprocess.Where(v => v.IsDelete == false && String.Compare(model.VOCProcessCode, v.VOCProcessCode, true) == 0).FirstOrDefault();
                if (checkObject == null)
                {
                    try
                    {
                        using (var ts = new TransactionScope())
                        {
                            // Thêm quy trình
                            checkObject = new TblVocprocess();
                            checkObject.VOCProcessName = model.VOCProcessName;
                            checkObject.VOCProcessCode = model.VOCProcessCode;
                            checkObject.VOCProcessType = model.VOCProcessType;
                            checkObject.Description = model.Description;
                            checkObject.IsActive = model.IsActive;
                            checkObject.CreateBy = userName;
                            checkObject.CreateDate = DateTime.Now;
                            checkObject.UpdateBy = userName;
                            checkObject.UpdateDate = DateTime.Now;
                            checkObject.IsDelete = false;
                            checkObject.CurrentVersion = 1;
                            checkObject.DurationDay = model.DurationDay;
                            checkObject.DurationHour = model.DurationHour;
                            checkObject.DurationMinute = model.DurationMinute;
                            db.TblVocprocess.Add(checkObject);
                            db.SaveChanges();
                            // Thêm Người vào quy trình
                            if (model.userViewModels != null && model.userViewModels.Count > 0 && checkObject.ID > 0)
                            {
                                AssigneeUser(model.userViewModels, checkObject.VOCProcessCode, checkObject.CurrentVersion.Value, null);
                            }
                            // Thêm bước của quy trình
                            if (model.objectSteps != null && model.objectSteps.Count > 0 && checkObject.ID > 0)
                            {
                                AddVOCProcessSteps(model.objectSteps, checkObject.VOCProcessCode, checkObject.CurrentVersion.Value, userName, null, null, true, false, null, null);
                            }
                            else
                            {
                                VOCProcessNoStep(checkObject, userName, checkObject.CurrentVersion.Value);
                            }
                            resultAdd = VOCProcessConstant.AddVOCProcessSuccess;
                            ts.Complete();
                        }
                    }
                    catch (TransactionAbortedException tae)
                    {
                        Console.WriteLine(tae.Message);
                        return VOCProcessConstant.AddVOCProcessFail;
                    }

                }
                else
                {
                    return VOCProcessConstant.AddDuplicateCode;
                }
                return resultAdd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return resultAdd;
            }
        }

        /// <summary>
        /// Hàm thực hiện thêm step rỗng khi quy trình không có step nào cả
        /// @author: HaiHM
        /// @createdDate: 25/07/2019
        /// </summary>
        private void VOCProcessNoStep(TblVocprocess vocprocess, string userName, int version)
        {
            TblVocprocessSteps step = db.TblVocprocessSteps.Where(s => s.IsNoStep == true && s.VocprocessCode == vocprocess.VOCProcessCode && s.Version == version).FirstOrDefault();
            if(step == null)
            {
                step = new TblVocprocessSteps();
                step.VocprocessCode = vocprocess.VOCProcessCode;
                step.StepCode = VOCProcessConstant.PARENT;
                step.ParentCode = VOCProcessConstant.PARENT;
                step.StepName = VOCProcessConstant.PARENT;
                step.FormId = 0;
                step.ConditionId = 0;
                step.OrderNum = 0;
                step.Version = version;
                step.CreateBy = userName;
                step.CreateDate = DateTime.Now;
                step.UpdateBy = userName;
                step.UpdateDate = DateTime.Now;
                step.IsActive = vocprocess.IsActive.Value;
                step.FinishDate = DateTime.Now;
                step.IsFinish = false;
                step.InProgress = false;
                step.DurationStepDay = 0;
                step.DurationStepHour = 0;
                step.DurationStepMinute = 0;
                step.IsNoStep = true;
                db.TblVocprocessSteps.Add(step);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Hàm thực hiện thêm các bước cho quy trình
        /// @author: HaiHM
        /// @createdDate: 18/07/2019
        /// </summary>
        /// <param name="lstSteps"></param>
        /// <param name="idVOCProcess"></param>
        /// <param name="version"></param>
        private void AddVOCProcessSteps(List<ObjectSteps> lstSteps, string codeVOCP, int version, string userName, int? formId, int? coditionId, bool IsActive, bool IsFinish, DateTime? dateFinish, bool? InProgress)
        {
            try
            {
                TblVocprocessSteps step = new TblVocprocessSteps();

                foreach (var item in lstSteps)
                {
                    // Thêm bước cha
                    step = new TblVocprocessSteps();
                    //TblVocprocessSteps stepParent = item.stepParent;

                    step.VocprocessCode = codeVOCP;
                    step.StepCode = item.StepCode;
                    step.ParentCode = item.ParentCode;
                    step.StepName = item.StepName;
                    step.FormId = formId;
                    step.ConditionId = coditionId;
                    step.OrderNum = item.OrderNum;
                    step.Version = version;
                    step.CreateBy = userName;
                    step.CreateDate = DateTime.Now;
                    step.UpdateBy = userName;
                    step.UpdateDate = DateTime.Now;
                    step.IsActive = IsActive;
                    step.FinishDate = dateFinish;
                    step.IsFinish = IsFinish;
                    step.InProgress = InProgress;
                    step.DurationStepDay = item.DurationStepDay;
                    step.DurationStepHour = item.DurationStepHour;
                    step.DurationStepMinute = item.DurationStepMinute;
                    db.TblVocprocessSteps.Add(step);
                    db.SaveChanges();
                    //Thêm tất cả các bước con
                    List<TblVocprocessSteps> lstChild = item.stepChilds;
                    if (lstChild != null && lstChild.Count > 0)
                    {
                        foreach (var itemChild in lstChild)
                        {
                            step = new TblVocprocessSteps();
                            step.VocprocessCode = codeVOCP;
                            step.StepCode = itemChild.StepCode;
                            step.ParentCode = itemChild.ParentCode;
                            step.StepName = itemChild.StepName;
                            step.FormId = formId;
                            step.ConditionId = coditionId;
                            step.OrderNum = itemChild.OrderNum;
                            step.Version = version;
                            step.CreateBy = userName;
                            step.CreateDate = DateTime.Now;
                            step.UpdateBy = userName;
                            step.UpdateDate = DateTime.Now;
                            step.IsActive = IsActive;
                            step.FinishDate = dateFinish;
                            step.IsFinish = IsFinish;
                            step.InProgress = InProgress;
                            step.DurationStepDay = itemChild.DurationStepDay;
                            step.DurationStepHour = itemChild.DurationStepHour;
                            step.DurationStepMinute = itemChild.DurationStepMinute;
                            db.TblVocprocessSteps.Add(step);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Hàm thực hiện phần quyền cho người dùng vào quy trình hoặc các bước của quy trình
        /// @author: HaiHM
        /// @createdDate: 18/07/2019
        /// </summary>
        /// <param name="lstUser"></param>
        /// <param name="idVOCProcess"></param>
        /// <param name="version"></param>
        /// <param name="StepCode"></param>
        private void AssigneeUser(List<UserViewModel> lstUser, string codeVOCP, int version, string StepCode)
        {
            try
            {
                List<TblVocprocessAssignee> lstVPA = db.TblVocprocessAssignee.Where(va => va.VocprocessCode == codeVOCP && va.Version == version & va.StepCode == null).ToList();
                if (lstVPA != null && lstVPA.Count > 0)
                {
                    //Xóa các bản ghi
                    foreach (var item in lstVPA)
                    {
                        //db.TblVocprocessAssignee.Remove(item);
                        //db.SaveChanges();
                    }
                }
                // Thêm mới
                if (lstUser != null && lstUser.Count > 0)
                {
                    TblVocprocessAssignee vpa = new TblVocprocessAssignee();

                    foreach (var itemAdd in lstUser)
                    {
                        vpa = new TblVocprocessAssignee();
                        vpa.VocprocessCode = codeVOCP;
                        vpa.Version = version;
                        vpa.UserId = itemAdd.ID;
                        vpa.StepCode = StepCode;
                        db.TblVocprocessAssignee.Add(vpa);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public object ConditionStepVOCProcess()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public object ConfigFormVOCProcess()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public object ConfigStepVOCProcess()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public object ConfigVOCProcess()
        {
            throw new NotImplementedException();
        }

        private ObjectVOCProccess ObjectSearch(string VOCProcessCode, int version, string orgCode)
        {
            ObjectVOCProccess objResult = new ObjectVOCProccess();
            // Lấy dữ liệu của quy trình
            TblVocprocess vocp = db.TblVocprocess.Where(p => p.VOCProcessCode == VOCProcessCode && p.IsDelete == false).FirstOrDefault();
            if (vocp != null)
            {
                objResult.VOCProcessCode = vocp.VOCProcessCode;
                objResult.VOCProcessName = vocp.VOCProcessName;
                objResult.VOCProcessType = vocp.VOCProcessType;
                objResult.Description = vocp.Description;
                objResult.IsActive = vocp.IsActive.Value;
                if (vocp.DurationDay.HasValue)
                {
                    objResult.DurationDay = vocp.DurationDay.Value;
                }
                if (vocp.DurationHour.HasValue)
                {
                    objResult.DurationHour = vocp.DurationHour.Value;

                }
                if (vocp.DurationMinute.HasValue)
                {
                    objResult.DurationMinute = vocp.DurationMinute.Value;
                }
                //Custom VOCProcessType
                List<TblCategory> lstCategories = db.TblCategory.Where(c => c.IsDelete == false && c.CategoryTypeCode == "Incident").ToList();
                TblCategory category = lstCategories.Where(cate => cate.CategoryCode == objResult.VOCProcessType).FirstOrDefault();
                if (category != null)
                {
                    objResult.VOCProcessType = category.CategoryName;
                }

                // Lấy người dùng được phân quyền theo VOCProcessCode-
                List<TblVocprocessAssignee> lstUsers = db.TblVocprocessAssignee.Where(va => va.VocprocessCode == VOCProcessCode && va.Version == version).ToList();
                List<List<dynamic>> dataUsers = sp.GetUserAssignee(String.Empty, orgCode, VOCProcessCode, version, null, VOCConstant.SQL_CONNECTION);
                List<UserViewModel> lstUserAll = dataUsers[0].OfType<UserViewModel>().OrderByDescending(u => u.CategoryCodeDepartment).ToList();
                List<UserViewModel> lstUserResult = new List<UserViewModel>();
                if (lstUsers != null && lstUsers.Count > 0)
                {
                    foreach (var item in lstUserAll)
                    {
                        foreach (var itemResult in lstUsers)
                        {
                            if (itemResult.UserId == item.ID)
                            {
                                lstUserResult.Add(item);
                            }
                        }
                    }
                }
                objResult.userViewModels = lstUserResult;

                // Lấy các bước của quy trình
                List<TblVocprocessSteps> steps = db.TblVocprocessSteps.Where(s => s.VocprocessCode == VOCProcessCode && s.Version == version).ToList();
                List<ObjectSteps> lstObjetStep = new List<ObjectSteps>();
                ObjectSteps objetStep = new ObjectSteps();
                foreach (var item in steps.Where(s => s.ParentCode == VOCProcessConstant.PARENT && s.Version == version && s.VocprocessCode == VOCProcessCode))
                {
                    TblVocprocessSteps stepParent = new TblVocprocessSteps();
                    List<TblVocprocessSteps> stepChild = new List<TblVocprocessSteps>();
                    objetStep.VocprocessCode = item.VocprocessCode;
                    objetStep.StepCode = item.StepCode;
                    objetStep.ParentCode = item.ParentCode;
                    objetStep.StepName = item.StepName;
                    objetStep.FormId = item.FormId;
                    objetStep.ConditionId = item.ConditionId;
                    objetStep.OrderNum = item.OrderNum;
                    objetStep.Version = item.Version;
                    objetStep.FinishDate = item.FinishDate;
                    objetStep.IsFinish = item.IsFinish;
                    objetStep.InProgress = item.InProgress;
                    objetStep.DurationStepDay = item.DurationStepDay;
                    objetStep.DurationStepHour = item.DurationStepHour;
                    objetStep.DurationStepMinute = item.DurationStepMinute;
                    foreach (var itemChild in steps.Where(sc => sc.ParentCode == item.StepCode && sc.Version == version && sc.VocprocessCode == VOCProcessCode))
                    {
                        stepChild.Add(itemChild);
                    }
                    objetStep.stepChilds = stepChild;

                    lstObjetStep.Add(objetStep);
                }

                objResult.objectSteps = lstObjetStep;

                return objResult;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public async Task<object> CopyVOCProcess(string VOCProcessCode, int version, string orgCode)
        {
            bool checkHasStep = true;
            try
            {
                ObjectVOCProccess objResult = new ObjectVOCProccess();

                // Lấy dữ liệu của quy trình
                TblVocprocess vocp = await db.TblVocprocess.Where(p => p.VOCProcessCode == VOCProcessCode && p.IsDelete == false).FirstOrDefaultAsync();
                if (vocp != null)
                {
                    objResult.VOCProcessCode = vocp.VOCProcessCode;
                    objResult.VOCProcessName = vocp.VOCProcessName;
                    objResult.VOCProcessType = vocp.VOCProcessType;
                    objResult.Description = vocp.Description;
                    objResult.IsActive = vocp.IsActive.Value;
                    objResult.CreateBy = vocp.CreateBy;
                    objResult.CreateDate = vocp.CreateDate;
                    objResult.UpdateDate = vocp.UpdateDate;
                    objResult.UpdateBy = vocp.UpdateBy;
                    if (vocp.DurationDay.HasValue)
                    {
                        objResult.DurationDay = vocp.DurationDay.Value;
                    }
                    if (vocp.DurationHour.HasValue)
                    {
                        objResult.DurationHour = vocp.DurationHour.Value;

                    }
                    if (vocp.DurationMinute.HasValue)
                    {
                        objResult.DurationMinute = vocp.DurationMinute.Value;
                    }
                    //Custom VOCProcessType
                    List<TblCategory> lstCategories = await db.TblCategory.Where(c => c.IsDelete == false && c.CategoryTypeCode == "Incident").ToListAsync();
                    TblCategory category = lstCategories.Where(cate => cate.CategoryCode == objResult.VOCProcessType).FirstOrDefault();
                    if (category != null)
                    {
                        objResult.VOCProcessType = category.CategoryName;
                    }

                    // Lấy người dùng được phân quyền theo VOCProcessCode-
                    //List<TblVocprocessAssignee> lstUsers = await db.TblVocprocessAssignee.Where(va => va.VocprocessCode == VOCProcessCode && va.Version == version).ToListAsync();
                    //List<List<dynamic>> dataUsers = sp.GetUserAssignee(String.Empty, orgCode, VOCProcessCode, version, null, VOCConstant.SQL_CONNECTION);
                    //List<UserViewModel> lstUserAll = dataUsers[0].OfType<UserViewModel>().OrderByDescending(u => u.CategoryCodeDepartment).ToList();
                    //List<UserViewModel> lstUserResult = new List<UserViewModel>();
                    //if (lstUsers != null && lstUsers.Count > 0)
                    //{
                    //    foreach (var item in lstUserAll)
                    //    {
                    //        foreach (var itemResult in lstUsers)
                    //        {
                    //            if (itemResult.UserId == item.ID)
                    //            {
                    //                lstUserResult.Add(item);
                    //            }
                    //        }
                    //    }
                    //}
                    //objResult.userViewModels = lstUserResult;

                    // Lấy các bước của quy trình
                    List<TblVocprocessSteps> steps = await db.TblVocprocessSteps.Where(s => s.VocprocessCode == VOCProcessCode && s.Version == version).ToListAsync();
                    List<ObjectSteps> lstObjetStep = new List<ObjectSteps>();
                    ObjectSteps objetStep = new ObjectSteps();
                    foreach (var item in steps.Where(s => s.ParentCode == VOCProcessConstant.PARENT && s.Version == version && s.VocprocessCode == VOCProcessCode))
                    {
                        TblVocprocessSteps stepParent = new TblVocprocessSteps();
                        List<TblVocprocessSteps> stepChild = new List<TblVocprocessSteps>();
                        // Set Value vào bước cha
                        objetStep.VocprocessCode = item.VocprocessCode;
                        objetStep.StepCode = item.StepCode;
                        objetStep.ParentCode = item.ParentCode;
                        objetStep.StepName = item.StepName;
                        objetStep.FormId = item.FormId;
                        objetStep.ConditionId = item.ConditionId;
                        objetStep.OrderNum = item.OrderNum;
                        objetStep.Version = item.Version;
                        objetStep.FinishDate = item.FinishDate;
                        objetStep.IsFinish = item.IsFinish;
                        objetStep.InProgress = item.InProgress;
                        objetStep.DurationStepDay = item.DurationStepDay;
                        objetStep.DurationStepHour = item.DurationStepHour;
                        objetStep.DurationStepMinute = item.DurationStepMinute;
                        objetStep.CreateBy = item.CreateBy;
                        objetStep.CreateDate = item.CreateDate;
                        objetStep.UpdateBy = item.UpdateBy;
                        objetStep.UpdateDate = item.UpdateDate;

                        //objetStep.stepParent = item;
                        foreach (var itemChild in steps.Where(sc => sc.ParentCode == item.StepCode && sc.Version == version && sc.VocprocessCode == VOCProcessCode))
                        {
                            stepChild.Add(itemChild);
                        }
                        objetStep.IsNoStep = item.IsNoStep;
                        if (objetStep.IsNoStep.HasValue)
                        {
                            checkHasStep = false;
                        }
                        objetStep.stepChilds = stepChild;
                        lstObjetStep.Add(objetStep);
                    }
                    if (checkHasStep)
                    {
                        objResult.objectSteps = lstObjetStep;
                    }
                    // Lấy danh sách user
                    var lstUserCustom = GetUserAssignee("", orgCode, VOCProcessCode, version, "");
                    //Use reflection
                    var response = new { data = objResult, lstUserCustom = lstUserCustom };
                    return response;
                }
                else
                {
                    var response = new { data = vocp, message = VOCProcessConstant.MS0001 };
                    return response;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var response = new { data = String.Empty, message = ex.Message };
                return null;
            }
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public int DeleteVOCProcess(int idVOCProcess, string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Hàm thực hiện kiểm tra xem có tăng version hay không
        /// @author: HaiHM
        /// @createdDate: 22/07/2019
        /// </summary>
        /// <param name="clientObject"></param>
        /// <param name="databaseObject"></param>
        /// <returns></returns>
        private bool CheckChangeVersion(List<ObjectSteps> clientObject, List<ObjectSteps> databaseObject)
        {
            bool checkVersion = false;
            if (clientObject.Count != databaseObject.Count)
            {
                // Tăng version
                checkVersion = true;
                return checkVersion;
            }
            else
            {
                // Thay đổi tên hay các thông tin cấu hình hay không {FormId, ConditionId}
                foreach (var item in clientObject)
                {
                    foreach (var itemReal in databaseObject)
                    {
                        // So sánh tên bước cha và Thông tin cấu hình
                        if (String.Compare(item.StepName, itemReal.StepName, true) != 0
                            || item.FormId != itemReal.FormId
                            || item.ConditionId != itemReal.ConditionId
                            || item.ConditionId < 0
                            || item.FormId < 0)
                        {
                            // Có thay đổi
                            checkVersion = true;
                            return checkVersion;
                        }
                        // Kiểm tra các bước con
                        // - Kiểm tra số lượng
                        if (item.stepChilds != null && itemReal.stepChilds != null && (item.stepChilds.Count != itemReal.stepChilds.Count))
                        {
                            checkVersion = true;
                            return checkVersion;
                        }
                        else
                        {
                            // Số lượng các bước con = nhau
                            //- Kiểm tra dữ liệu xem có thay đổi không
                            if (item.stepChilds != null && itemReal.stepChilds != null)
                            {
                                foreach (var itemChild in item.stepChilds)
                                {
                                    foreach (var itemRealChild in itemReal.stepChilds)
                                    {
                                        if (String.Compare(itemChild.StepName, itemRealChild.StepName, true) != 0
                                                || itemChild.FormId != itemRealChild.FormId
                                                || itemChild.ConditionId != itemRealChild.ConditionId
                                                || itemChild.ConditionId < 0
                                                || itemChild.FormId < 0)
                                        {
                                            // Có thay đổi
                                            checkVersion = true;
                                            return checkVersion;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                checkVersion = false;
                            }
                        }
                    }
                }
            }
            return checkVersion;
        }

        /// <summary>
        /// Function add VOC process
        /// @author: HaiHM
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public int EditVOCProcess(ObjectVOCProccess model, string userName)
        {
            try
            {
                int resultEdit = VOCProcessConstant.EditVOCProcessFail;
                bool checkVersion = false;
                // Kiểm tra có trong database hay không
                TblVocprocess checkObject = db.TblVocprocess.Where(v => v.IsDelete == false && String.Compare(model.VOCProcessCode, v.VOCProcessCode, true) == 0).FirstOrDefault();
                if (checkObject != null)
                {
                    List<TblVocprocessSteps> lstParent = new List<TblVocprocessSteps>();
                    try
                    {
                        using (var ts = new TransactionScope())
                        {
                            //Kiểm tra các bước xem có sự thay đổi gì không
                            // Dữ liệu mới -- { model.objectSteps }
                            // Lấy dữ liệu cũ
                            List<TblVocprocessSteps> steps = db.TblVocprocessSteps.Where(s => s.VocprocessCode == model.VOCProcessCode && s.Version == model.CurrentVersion.Value).ToList();
                            List<ObjectSteps> lstObjetStep = new List<ObjectSteps>();
                            ObjectSteps objetStep = new ObjectSteps();
                            foreach (var item in steps.Where(s => s.ParentCode == VOCProcessConstant.PARENT && s.Version == model.CurrentVersion.Value && s.VocprocessCode == model.VOCProcessCode))
                            {
                                TblVocprocessSteps stepParent = new TblVocprocessSteps();
                                List<TblVocprocessSteps> stepChild = new List<TblVocprocessSteps>();
                                //M.stepParent = item;
                                //Add Parent
                                objetStep.VocprocessCode = item.VocprocessCode;
                                objetStep.StepCode = item.StepCode;
                                objetStep.ParentCode = item.ParentCode;
                                objetStep.StepName = item.StepName;
                                objetStep.FormId = item.FormId;
                                objetStep.ConditionId = item.ConditionId;
                                objetStep.OrderNum = item.OrderNum;
                                objetStep.Version = item.Version;
                                objetStep.FinishDate = item.FinishDate;
                                objetStep.IsFinish = item.IsFinish;
                                objetStep.InProgress = item.InProgress;
                                objetStep.DurationStepDay = item.DurationStepDay;
                                objetStep.DurationStepHour = item.DurationStepHour;
                                objetStep.DurationStepMinute = item.DurationStepMinute;
                                objetStep.CreateBy = item.CreateBy;
                                objetStep.CreateDate = item.CreateDate;
                                objetStep.UpdateBy = item.UpdateBy;
                                objetStep.UpdateDate = item.UpdateDate;
                                foreach (var itemChild in steps.Where(sc => sc.ParentCode == item.StepCode && sc.Version == model.CurrentVersion.Value && sc.VocprocessCode == model.VOCProcessCode))
                                {
                                    stepChild.Add(itemChild);
                                }
                                objetStep.stepChilds = stepChild;

                                lstObjetStep.Add(objetStep);
                            }
                            checkObject.VOCProcessName = model.VOCProcessName;
                            //checkObject.VOCProcessCode = model.VOCProcessCode;
                            checkObject.VOCProcessType = model.VOCProcessType;
                            checkObject.Description = model.Description;
                            checkObject.IsActive = model.IsActive;
                            checkObject.CreateBy = userName;
                            checkObject.CreateDate = DateTime.Now;
                            checkObject.UpdateBy = userName;
                            checkObject.UpdateDate = DateTime.Now;
                            checkObject.IsDelete = false;
                            checkObject.DurationDay = model.DurationDay;
                            checkObject.DurationHour = model.DurationHour;
                            checkObject.DurationMinute = model.DurationMinute;
                            // Kiểm tra xem có phải nâng phiên bản lên hay không
                            //** Change version here   
                            if((model.objectSteps != null && lstObjetStep == null) || (model.objectSteps == null && lstObjetStep != null))
                            {
                                checkVersion = true;
                            }
                            else
                            {
                                if (CheckChangeVersion(model.objectSteps, lstObjetStep))
                                {
                                    checkVersion = true;
                                }
                            }
                            //Tìm phiên bản mới nhất trong các phiên bản của quy trình
                            // Check change step process
                            TblVocprocessSteps step = db.TblVocprocessSteps.Where(s => s.VocprocessCode == model.VOCProcessCode).OrderByDescending(sort => sort.Version).FirstOrDefault();
                            // Tăng version để nâng phiên bản
                            if (checkVersion)
                            {
                                checkObject.CurrentVersion = step.Version + 1;
                            }
                            // Assigne người vào quy trình
                            if (model.userViewModels != null && model.userViewModels.Count > 0 && checkObject.ID > 0)
                            {
                                AssigneeUser(model.userViewModels, checkObject.VOCProcessCode, checkObject.CurrentVersion.Value, null);
                            }
                            // Thêm bước của quy trình
                            if (model.objectSteps != null && model.objectSteps.Count > 0 && checkObject.ID > 0)
                            {
                                checkVersion = checkVersion ? true : checkObject.IsActive.Value;
                                AddVOCProcessSteps(model.objectSteps, checkObject.VOCProcessCode, checkObject.CurrentVersion.Value, userName, null, null, checkVersion, false, null, null);
                            }
                            else
                            {
                                // Trường hợp không có bước nào
                                VOCProcessNoStep(checkObject, userName, checkObject.CurrentVersion.Value);
                            }
                            // Update quy trình
                            db.Entry(checkObject).State = EntityState.Modified;
                            db.SaveChanges();
                            resultEdit = VOCProcessConstant.EditVOCProcessSuccess;
                            ts.Complete();
                        }
                        return resultEdit;
                    }
                    catch (TransactionAbortedException tae)
                    {
                        Console.WriteLine(tae.Message);
                        return VOCProcessConstant.EditVOCProcessFail;
                    }
                }
                else
                {
                    return VOCProcessConstant.EditDuplicateCode;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return VOCProcessConstant.EditVOCProcessFail;
            }
        }


        /// <summary>
        /// Function get connection của đơn vị
        /// HaiHM
        /// 15/07/2019
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        private string GetConnectionByOrgCode(string orgCode)
        {
            string connection = "";
            List<List<dynamic>> obj = sp.GetAllConnection();
            List<TblConnectionConfig> lstUser = obj[0].OfType<TblConnectionConfig>().ToList();
            if (lstUser.Count() > 0)
            {
                foreach (var item in lstUser)
                {
                    if (item.ConnectionKey.Contains(orgCode + VOCConstant.ConnectionKey))
                    {
                        connection = item.ConnectionValue;
                        break;
                    }
                }
            }
            return connection;
        }

        /// <summary>
        /// Hàm thực hiện lấy ra danh sách user để phân quyền cho các bước và quy trình
        /// @author: HaiHM
        /// @createdDate: 17/07/2019
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="orgCode"></param>
        /// <param name="vocProcessCode"></param>
        /// <param name="version"></param>
        /// <param name="stepCode"></param>
        /// <returns></returns>
        public object GetUserAssignee(string textSearch, string orgCode, string vocProcessCode, int version, string stepCode)
        {
            List<Object> lstObject = new List<object>();
            // biến checkSearchDepartment sử dụng để tìm kiếm theo tên phòng ban
            bool checkSearchDepartment = false;
            string search = "";
            try
            {

                List<List<dynamic>> lstCategorySP = sp.GetAllCategory(VOCConstant.Department, GetConnectionByOrgCode(orgCode));
                List<TblCategory> lstCategory = lstCategorySP[0].OfType<TblCategory>().OrderByDescending(c => c.CategoryCode).ToList();
                foreach (var itemCategory in lstCategory)
                {
                    if (String.Compare(itemCategory.CategoryName.Trim(), textSearch.Trim(), true) == 0)
                    {
                        checkSearchDepartment = true;
                    }
                }
                if (checkSearchDepartment || String.Compare(VOCConstant.DepartmentNULL.Trim(), textSearch.Trim(), true) == 0)
                {
                    // Tìm kiếm theo phòng ban
                    search = textSearch;
                    textSearch = null;

                }
                List<List<dynamic>> dataUsers = sp.GetUserAssignee(textSearch, orgCode, vocProcessCode, version, stepCode, VOCConstant.SQL_CONNECTION);
                List<UserViewModel> lstUser = dataUsers[0].OfType<UserViewModel>().OrderByDescending(u => u.CategoryCodeDepartment).ToList();

                if (!string.IsNullOrEmpty(vocProcessCode))
                {
                    // Lấy người dùng được phân quyền theo VOCProcessCode-
                    List<TblVocprocessAssignee> dblstUsers = db.TblVocprocessAssignee.Where(va => va.VocprocessCode == vocProcessCode && va.Version == version).ToList();
                    List<UserViewModel> lstUserResult = new List<UserViewModel>();
                    if (dblstUsers != null && dblstUsers.Count > 0)
                    {
                        foreach (var item in lstUser)
                        {
                            bool checkSelected = true;
                            foreach (var itemResult in dblstUsers)
                            {
                                if (item.ID == itemResult.UserId)
                                {
                                    item.Select = true;
                                    checkSelected = false;
                                    lstUserResult.Add(item);
                                }
                            }
                            if (checkSelected)
                            {
                                lstUserResult.Add(item);
                            }
                        }
                    }
                    List<UserViewModel> userNoDepartment = new List<UserViewModel>();
                    var obj = new object();
                    checkSearchDepartment = false;
                    bool selectDepartment = false;
                    bool selectNoDepartment = false;

                    foreach (var item in lstCategory)
                    {
                        selectDepartment = false;
                        if (String.Compare(item.CategoryName.Trim(), search.Trim(), true) == 0)
                        {
                            checkSearchDepartment = true;
                        }
                        obj = new object();
                        List<UserViewModel> userOfDepartment = new List<UserViewModel>();
                        foreach (var user in lstUserResult)
                        {

                            if (user.CategoryCodeDepartment == item.CategoryCode)
                            {
                                userOfDepartment.Add(user);
                            }
                            else
                            {
                                bool checkHasDepartment = true;
                                foreach (var category in lstCategory)
                                {
                                    if (user.CategoryCodeDepartment == category.CategoryCode)
                                    {
                                        checkHasDepartment = false;
                                    }
                                }
                                if (checkHasDepartment)
                                {
                                    if (userNoDepartment.Where(u => u.ID == user.ID).Count() == 0)
                                    {
                                        if (user.Select)
                                        {
                                            selectNoDepartment = true;
                                        }
                                        userNoDepartment.Add(user);
                                    }
                                }
                            }
                        }
                        foreach (var checkHasValue in userOfDepartment)
                        {
                            if (checkHasValue.Select)
                            {
                                selectDepartment = true;
                            }
                        }
                        obj = new { Department = item.CategoryName, selectDepartment = selectDepartment, UserViewModel = userOfDepartment };
                        if (userOfDepartment.Count > 0)
                        {
                            lstObject.Add(obj);
                        }
                        if (checkSearchDepartment)
                        {
                            return new { Department = item.CategoryName, selectDepartment = selectDepartment, UserViewModel = userOfDepartment };
                        }
                    }
                    if (String.Compare(VOCConstant.DepartmentNULL.Trim(), textSearch.Trim(), true) == 0)
                    {
                        return new { Department = VOCConstant.DepartmentNULL, selectDepartment = true, UserViewModel = userNoDepartment };
                    }
                    obj = new { Department = VOCConstant.DepartmentNULL, selectNoDepartment = selectNoDepartment, UserViewModel = userNoDepartment };
                    if (userNoDepartment.Count > 0)
                    {
                        lstObject.Add(obj);
                    }
                    return lstObject;
                }
                if (lstUser != null && lstCategory != null)
                {
                    List<UserViewModel> userNoDepartment = new List<UserViewModel>();
                    var obj = new object();
                    foreach (var item in lstCategory)
                    {
                        checkSearchDepartment = false;
                        if (String.Compare(item.CategoryName.Trim(), search.Trim(), true) == 0)
                        {
                            checkSearchDepartment = true;
                        }
                        obj = new object();
                        List<UserViewModel> userOfDepartment = new List<UserViewModel>();
                        foreach (var user in lstUser)
                        {
                            if (user.CategoryCodeDepartment == item.CategoryCode)
                            {
                                userOfDepartment.Add(user);
                            }
                            else
                            {
                                bool checkHasDepartment = true;
                                foreach (var category in lstCategory)
                                {
                                    if (user.CategoryCodeDepartment == category.CategoryCode)
                                    {
                                        checkHasDepartment = false;
                                    }
                                }
                                if (checkHasDepartment)
                                {
                                    if (userNoDepartment.Where(u => u.ID == user.ID).Count() == 0)
                                    {
                                        userNoDepartment.Add(user);
                                    }
                                }
                            }

                        }
                        obj = new { Department = item.CategoryName, selectNoDepartment = false, UserViewModel = userOfDepartment };
                        if (userOfDepartment.Count > 0)
                        {
                            lstObject.Add(obj);
                        }
                        if (checkSearchDepartment)
                        {
                            return new { Department = item.CategoryName, selectDepartment = false, UserViewModel = userOfDepartment };
                        }
                    }
                    obj = new { Department = VOCConstant.DepartmentNULL, selectNoDepartment = false, UserViewModel = userNoDepartment };
                    if (userNoDepartment.Count > 0)
                    {
                        lstObject.Add(obj);
                    }
                    if (String.Compare(VOCConstant.DepartmentNULL.Trim(), search.Trim(), true) == 0)
                    {
                        return new { Department = VOCConstant.DepartmentNULL, selectDepartment = true, UserViewModel = userNoDepartment };
                    }
                }
                return lstObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void LoadContext(string orgCode, IDistributedCache distributedCache)
        {
            if (db == null || strconnect != orgCode)
            {
                string conn = VOCConstant.MP_CONNECTION;
                var optionsBuilder = new DbContextOptionsBuilder<CRM_MPContext>();
                optionsBuilder.UseSqlServer(conn.Replace("MP", orgCode), providerOptions => providerOptions.CommandTimeout(300));
                db = new CRM_MPContext(optionsBuilder.Options);
            }
            _distributedCache = distributedCache;
        }

        public void SetContextFactory(TblConnectionConfig connectionStrings)
        {
            DbContextFactory.AddChildContext(connectionStrings);
        }

        public void SetStringCache(string cacheKey, object obj)
        {
            _distributedCache.GetString(cacheKey);
        }

        /// <summary>
        /// Hàm thực hiện lấy data từ cache theo key
        /// @author: HaiHM
        /// @createdDate: 17/07/2019
        /// </summary>
        /// <param name="key">KEY-VALUE</param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(string key)
        {
            return await _distributedCache.GetStringAsync(key);
        }

        public object SearchVersion(string userName, string VOCProcessCode, string isActive, int currPage, int recordperpage, string orgCode)
        {
            //List<VOCProcessStepsViewModel> lstResult = new List<VOCProcessStepsViewModel>();
            try
            {
                List<List<dynamic>> obj = sp.SearchVersion(userName, VOCProcessCode, isActive, currPage, recordperpage, GetConnectionByOrgCode(orgCode));
                List<VOCProcessStepsViewModel> lstResult = obj[0].OfType<VOCProcessStepsViewModel>().ToList();
                PagePaging pagePaging = obj[1].OfType<PagePaging>().FirstOrDefault();
                //List<VOCProcessStepsViewModel> lstSearch = obj[0].OfType<VOCProcessStepsViewModel>().ToList();
                //List<int> lstCheck = lstSearch.Select(s => s.Version.Value).Distinct().ToList<int>();
                //if (lstCheck != null)
                //{
                //    foreach(var item in lstCheck)
                //    {
                //        VOCProcessStepsViewModel result = lstSearch.Where(r => r.Version.Value == item).FirstOrDefault();
                //        if(result != null)
                //        {
                //            lstResult.Add(result);
                //        }

                //    }
                //}
                //PagePaging pagePaging = obj[1].OfType<PagePaging>().FirstOrDefault();
                //pagePaging.TotalPage = lstResult.Count();
                //pagePaging.CurrPage = currPage;
                //if(recordperpage > 0)
                //{
                //    int? RecordPerPage = pagePaging.TotalPage / recordperpage;
                //    if(RecordPerPage == 0)
                //    {
                //        pagePaging.PageSize = 1;
                //    }
                //    else
                //    {
                //        pagePaging.PageSize = pagePaging.TotalPage / recordperpage;
                //    }
                //}
                //else
                //{
                //    pagePaging.PageSize = 0;
                //}
                var response = new { data = lstResult, pagePaging = pagePaging };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Hàm lấy danh sách Account của phiên bản
        /// @author: HaiHM
        /// @createdDate: 23/07/2019
        /// </summary>
        /// <param name="VOCProcessCode"></param>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        public async Task<object> GetListUserSearchVersion(string VOCProcessCode, string orgCode)
        {

            List<UserViewModel> lstUser = new List<UserViewModel>();
            try
            {
                //List<List<dynamic>> dataUsers = sp.GetUserAssignee(String.Empty, orgCode, VOCProcessCode, 1, String.Empty, VOCConstant.SQL_CONNECTION);
                //List<UserViewModel> allUser = dataUsers[0].OfType<UserViewModel>().OrderByDescending(u => u.CategoryCodeDepartment).ToList();

                var lstResult = await db.TblVocprocessSteps.Where(s => s.VocprocessCode == VOCProcessCode).Select(x => x.UpdateBy).Distinct().ToListAsync();
                return lstResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// Hàm thực hiện switch trạng thái của phiên bản
        /// @author: HaiHM
        /// @createdDate: 23/07/2019
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int SwitchStatus(VOCProcessStepsViewModel step)
        {
            int resultActive = VOCProcessConstant.SwitchStatusFail;
            try
            {
                using (var ts = new TransactionScope())
                {
                    TblVocprocessSteps stepInDatabase = db.TblVocprocessSteps.Where(s => s.VocprocessCode == step.VOCProcessCode && s.Id == step.Id && s.Version == step.Version).FirstOrDefault();
                    List<TblVocprocessSteps> lstStepCheckActive = db.TblVocprocessSteps.Where(s => s.VocprocessCode == step.VOCProcessCode && s.Version != step.Version).ToList();
                    bool checkAtive = false;
                    int versionActive = 0;
                    if (stepInDatabase.IsActive.HasValue && step.IsActive.HasValue)
                    {
                        if (stepInDatabase.IsActive.Value == step.IsActive.Value)
                        {
                            // không thay đổi trạng thái
                            return VOCProcessConstant.SwitchStatusNotChange;
                        }
                    }
                    if (stepInDatabase != null)
                    {
                        if (!step.IsActive.Value)
                        {
                            List<TblVocprocessSteps> lstStep = db.TblVocprocessSteps.Where(s => s.VocprocessCode == step.VOCProcessCode && s.Version == step.Version).ToList();
                            UpdateStep(lstStep, step.VOCProcessCode, step.Version.Value, false);
                            resultActive = VOCProcessConstant.SwitchStatusSuccess;
                        }
                        else
                        {
                            //Kiểm tra xem có thằng nào đang Active hay không
                            foreach (var item in lstStepCheckActive)
                            {
                                if (item.IsActive.HasValue && item.IsActive.Value)
                                {
                                    checkAtive = true;
                                    versionActive = item.Version.Value;
                                    break;
                                }
                            }
                            if (checkAtive)
                            {
                                // có version đã active
                                //UpdateStep Version active => unactive
                                List<TblVocprocessSteps> lstStepActive = db.TblVocprocessSteps.Where(s => s.VocprocessCode == step.VOCProcessCode && s.Version == versionActive).ToList();
                                UpdateStep(lstStepActive, step.VOCProcessCode, versionActive, false);
                                // Update version unactive => active 
                                List<TblVocprocessSteps> lstStepUnActive = db.TblVocprocessSteps.Where(s => s.VocprocessCode == step.VOCProcessCode && s.Version == step.Version).ToList();
                                UpdateStep(lstStepUnActive, step.VOCProcessCode, step.Version.Value, true);
                                resultActive = VOCProcessConstant.SwitchStatusSuccess;

                                // Update VOCProcess
                                TblVocprocess tblVocprocess = db.TblVocprocess.Where(p => p.VOCProcessCode == step.VOCProcessCode && p.IsDelete == false).FirstOrDefault();
                                if (tblVocprocess != null)
                                {
                                    tblVocprocess.IsActive = true;
                                    tblVocprocess.CurrentVersion = lstStepUnActive.FirstOrDefault().Version;
                                    db.Entry(tblVocprocess).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                // Update version unactive => active 
                                List<TblVocprocessSteps> lstStepUnActive = db.TblVocprocessSteps.Where(s => s.VocprocessCode == step.VOCProcessCode && s.Version == step.Version).ToList();
                                UpdateStep(lstStepUnActive, step.VOCProcessCode, step.Version.Value, true);
                                resultActive = VOCProcessConstant.SwitchStatusSuccess;
                                // Update VOCProcess
                                TblVocprocess tblVocprocess = db.TblVocprocess.Where(p => p.VOCProcessCode == step.VOCProcessCode && p.IsDelete == false).FirstOrDefault();
                                if (tblVocprocess != null)
                                {
                                    tblVocprocess.IsActive = true;
                                    tblVocprocess.CurrentVersion = lstStepUnActive.FirstOrDefault().Version;
                                    db.Entry(tblVocprocess).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        resultActive = VOCProcessConstant.SwitchStatusNotFound;
                    }
                    ts.Complete();
                }
                return resultActive;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return VOCProcessConstant.SwitchStatusFail;
            }
        }

        /// <summary>
        /// Hàm update status step process 
        /// @author: HaiHM
        /// @createdDate: 23/07/2019
        /// </summary>
        /// <param name="VOCProcessCode"></param>
        /// <param name="version"></param>
        /// <param name="Active"></param>
        private void UpdateStep(List<TblVocprocessSteps> lstStep, string VOCProcessCode, int version, bool Active)
        {
            // Update step 
            foreach (var item in lstStep)
            {
                TblVocprocessSteps itemUpdate = db.TblVocprocessSteps.Where(s => s.Id == item.Id).FirstOrDefault();
                if (itemUpdate != null)
                {
                    itemUpdate.IsActive = Active;
                    db.Entry(itemUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

        }

        /// <summary>
        /// Hàm thực hiện tìm kiếm phiên bản khi copy quy trình
        /// @author: HaiHM
        /// @createdDate: 24/7/2019
        /// </summary>
        /// <param name="VOCProcessCode"></param>
        /// <param name="VOCProcessName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public object VesionWhenAddVOC()
        {
            try
            {
                List<SearchVersionCopyVM> lstResult = new List<SearchVersionCopyVM>();
                string code = "";
                string name = "";
                int version = 0;
                List<string> lstCodeVOC = db.TblVocprocess.Select(s => s.VOCProcessCode).Distinct().ToList();
                if (lstCodeVOC != null)
                {
                    foreach (var item in lstCodeVOC)
                    {
                        SearchVersionCopyVM result = new SearchVersionCopyVM();
                        result.VOCProcessCode = item;
                        List<int> lstVersionOfCode = db.TblVocprocessSteps.Where(c => c.VocprocessCode == item).Select(s1 => s1.Version.Value).Distinct().ToList();
                        List<string> lstVOCProcessName = db.TblVocprocess.Where(c => c.VOCProcessCode == item).Select(s1 => s1.VOCProcessName).Distinct().ToList();
                        code = item;
                        foreach (var itemName in lstVOCProcessName)
                        {
                            name = itemName;
                            result.VOCProcessName = itemName;
                            foreach (var itemVersion in lstVersionOfCode)
                            {
                                version = itemVersion;

                                result = new SearchVersionCopyVM();
                                result.VOCProcessCode = code;
                                result.VOCProcessName = name;
                                result.Version = version;
                                lstResult.Add(result);
                            }
                        }
                    }
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
