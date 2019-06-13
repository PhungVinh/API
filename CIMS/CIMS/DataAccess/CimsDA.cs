using CIMS.Common;
using CIMS.Constant;
using CIMS.ContextFactory;
using CIMS.DTO;
using CIMS.Models;
using CIMS.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace CIMS.DataAccess
{

    public class CimsDA : CimsRepository
    {

        public CimsDA()
        {

        }
        private static CimsCommon cimsCommon = new CimsCommon();
        private IDistributedCache _distributedCache;
        private static string keyString = CimsConstant.keyString;
        private CimsDA_SP cimsDA;
        private string connectionKey;
        public string strconnect { get; set; }
        //public CRM_MPContext db { get; set; }
        private CRM_MPContext db = new CRM_MPContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MPContext>());

        /// <summary>
        ///  phongtv
        ///  List EncryptAtt ma hoa
        /// </summary>
        /// <param name="ModuleParent"></param>
        /// <returns></returns>

        public object EncryptAtt(string ModuleParent)
        {
            try
            {
                var lstAtt = (from a in db.TblVocattributes

                              where (a.IsDelete == false && a.ModuleParent == ModuleParent && a.Encyption == true)
                              select new
                              {
                                  a.AttributeCode,
                                  a.AttributeLabel

                              });
                return lstAtt;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        /// <summary>
        ///  /// phongtv
        /// //List Attr No ma hoa
        /// </summary>
        /// <param name="ModuleParent"></param>
        /// <returns></returns>

        public object NoEncryptAtt(string ModuleParent)
        {
            try
            {
                var lstAtt = (from a in db.TblVocattributes

                              where (a.IsDelete == false && a.ModuleParent == ModuleParent && a.Encyption == false)
                              select new
                              {
                                  a.AttributeCode,
                                  a.AttributeLabel

                              });
                return lstAtt;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        /// <summary>
        /// //Giai ma
        /// </summary>
        ///  /// phongtv
        /// <param name="ModuleParent"></param>
        /// <param name="AttributeCode"></param>
        /// <param name="keyString"></param>
        /// <returns></returns>
        public int DecryptAtt(string ModuleParent, string AttributeCode, string keyString)
        {
            int result = 0;
            try
            {
                List<TblCimsattributeValue> CimsChk = db.TblCimsattributeValue.Where(a => a.IsDelete == false && a.Module == ModuleParent && a.AttributeCode == AttributeCode).ToList();

                for (int i = 0; i < CimsChk.Count(); i++)
                {

                    TblVocattributes AttChk = db.TblVocattributes.Where(a => a.IsDelete == false && a.ModuleParent == ModuleParent && a.AttributeCode == AttributeCode).FirstOrDefault();
                    AttChk.Encyption = false;
                    db.Entry(AttChk).State = EntityState.Modified;
                    db.SaveChanges();

                    string text = CimsChk[i].AttributeValue;
                    CimsChk[i].AttributeValue = cimsCommon.DecryptStringAES(text, keyString);
                    db.Entry(CimsChk[i]).State = EntityState.Modified;
                    db.SaveChanges();
                    result = 2;
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }
        
        /// <summary>
        ///  // Ma hoa
        /// </summary>
        /// phongtv
        /// <param name="ModuleParent"></param>
        /// <param name="AttributeCode"></param>
        /// <param name="keyString"></param>
        /// <returns></returns>
        public int EncryptAtt(string ModuleParent, string AttributeCode, string keyString)
        {

            int result = 0;
            try
            {
                List<TblCimsattributeValue> CimsChk = db.TblCimsattributeValue.Where(a => a.IsDelete == false && a.Module == ModuleParent && a.AttributeCode == AttributeCode).ToList();

                for (int i = 0; i < CimsChk.Count(); i++)
                {

                    TblVocattributes AttChk = db.TblVocattributes.Where(a => a.IsDelete == false && a.ModuleParent == ModuleParent && a.AttributeCode == AttributeCode).FirstOrDefault();
                    AttChk.Encyption = true;

                    db.Entry(AttChk).State = EntityState.Modified;
                    db.SaveChanges();

                    string text = CimsChk[i].AttributeValue;
                    CimsChk[i].AttributeValue = cimsCommon.EncryptStringAES(text, keyString);
                    db.Entry(CimsChk[i]).State = EntityState.Modified;
                    db.SaveChanges();
                    result = 2;
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
                throw ex;
            }
        }


        /// <summary>
        /// Delete CIMs
        /// </summary>
        /// <param name="RecordId"></param>
        /// <returns></returns>
        public int DeleteCims(string RecordId)
        {
            int checkDelete = 0;
            try
            {
                List<TblCimsattributeValue> deleteAU = db.TblCimsattributeValue.Where(au => au.RecordId == RecordId).ToList<TblCimsattributeValue>();

                if(deleteAU != null)
                {
                    foreach (var item in deleteAU)
                    {
                        db.TblCimsattributeValue.Remove(item);
                        db.SaveChanges();
                    }
                    checkDelete = 1;
                }
                else
                {
                    checkDelete = 0;
                }


                return checkDelete;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        ///  // List truong thong tin ma hoa module
        /// </summary>
        /// phongtv
        /// <param name="ModuleParent"></param>
        /// <returns></returns>
        public object ListVocattributes(string ModuleParent)
        {
            try
            {
                var lstAtt = (from a in db.TblVocattributes
                              where (a.IsDelete == false && a.ModuleParent == ModuleParent)
                              select new
                              {
                                  a.AttributeCode,
                                  a.AttributeLabel

                              });
                return lstAtt;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }
        //Edit CIMS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstCustomer"></param>
        /// <param name="rowIdentify"></param>
        /// <returns></returns>
            public object EditCimsValue(List<TblCimsattributeValue> lstCustomer, string rowIdentify )
        {

            TblCimsattributeValue AttributesValue = db.TblCimsattributeValue.Where(a => a.RecordId == rowIdentify).FirstOrDefault();

            if(AttributesValue != null)
            {
                List<object> lstErrs = new List<object>();
                List<TblCimsattributeValue> lstAttributes = new List<TblCimsattributeValue>();
                for (int i = 0; i < lstCustomer.Count; i++)
                {
                    TblVocattributes tblVocattributesIsRequired = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsRequired == true).FirstOrDefault();
                    if (tblVocattributesIsRequired != null)
                    {
                        if (lstCustomer[i].AttributeValue.Length > 0)
                        {
                            TblVocattributes tblVocattributesIsDuplicate = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsDuplicate == true
                          ).FirstOrDefault();
                            if (tblVocattributesIsDuplicate != null)
                            {
                                TblCimsattributeValue cimsCheck = db.TblCimsattributeValue.Where(b => b.AttributeCode.TrimEnd() == lstCustomer[i].AttributeCode && b.AttributeValue.ToString() == lstCustomer[i].AttributeValue
                                && b.RecordId != rowIdentify).FirstOrDefault();
                                if (cimsCheck == null)
                                {
                                    TblCimsattributeValue cimsValue = db.TblCimsattributeValue.Where(b => b.AttributeCode == lstCustomer[i].AttributeCode
                                && b.RecordId == rowIdentify).FirstOrDefault();
                                    cimsValue.AttributeCode = lstCustomer[i].AttributeCode;
                                    cimsValue.AttributeValue = lstCustomer[i].AttributeValue;
                                    cimsValue.UpdatedDate = DateTime.Now;
                                    db.Entry(cimsValue).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    lstErrs.Add(new
                                    {
                                        field =  tblVocattributesIsDuplicate.AttributeCode,
                                        name = tblVocattributesIsDuplicate.AttributeLabel,
                                        message = "NotDuplicate"
                                    });
                                }
                            }
                            else
                            {
                                TblCimsattributeValue cimsValue = db.TblCimsattributeValue.Where(b => b.AttributeCode == lstCustomer[i].AttributeCode 
                                && b.RecordId == rowIdentify).FirstOrDefault();
                                cimsValue.AttributeCode = lstCustomer[i].AttributeCode;
                                cimsValue.AttributeValue = lstCustomer[i].AttributeValue;
                                cimsValue.UpdatedDate = DateTime.Now;
                                db.Entry(cimsValue).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            lstErrs.Add(new
                            {
                                field = tblVocattributesIsRequired.AttributeCode,
                                name = tblVocattributesIsRequired.AttributeLabel,
                                message = "NotNull"
                            });
                        }
                    }
                    else
                    {
                        TblVocattributes tblVocattributesIsDuplicate = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsDuplicate == true
                     ).FirstOrDefault();
                        if (tblVocattributesIsDuplicate != null)
                        {
                            TblCimsattributeValue cimsCheck = db.TblCimsattributeValue.Where(b => b.AttributeCode.TrimEnd() == lstCustomer[i].AttributeCode && b.AttributeValue.ToString() == lstCustomer[i].AttributeValue
                            && b.RecordId != rowIdentify).FirstOrDefault();
                            if (cimsCheck == null)
                            {
                                TblCimsattributeValue cimsValue = db.TblCimsattributeValue.Where(b => b.AttributeCode == lstCustomer[i].AttributeCode
                                && b.RecordId == rowIdentify).FirstOrDefault();
                                cimsValue.AttributeCode = lstCustomer[i].AttributeCode;
                                cimsValue.AttributeValue = lstCustomer[i].AttributeValue;
                                cimsValue.UpdatedDate = DateTime.Now;
                                db.Entry(cimsValue).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                lstErrs.Add(new
                                {
                                    field = tblVocattributesIsDuplicate.AttributeCode,
                                    name = tblVocattributesIsDuplicate.AttributeLabel,
                                    message = "NotDuplicate"

                                });
                            }
                        }
                        else
                        {
                            TblCimsattributeValue cimsValue = db.TblCimsattributeValue.Where(b => b.AttributeCode == lstCustomer[i].AttributeCode
                                 && b.RecordId == rowIdentify).FirstOrDefault();
                            cimsValue.AttributeCode = lstCustomer[i].AttributeCode;
                            cimsValue.AttributeValue = lstCustomer[i].AttributeValue;
                            cimsValue.UpdatedDate = DateTime.Now;
                            db.Entry(cimsValue).State = EntityState.Modified;
                            db.SaveChanges();
                            //lstAttributes.Add(cimsValue);
                        }
                    }
                }

                // list
                if (lstErrs.Count > 0)
                {
                    return lstErrs;
                }
                //var lst = db.TblVocattributes.Where(v => v.ModuleParent == "CIMS" &&
                //v.IsDelete == false && lstCustomer.Where(x => x.AttributeCode == v.AttributeCode).Count() == 0).
                //Select(x => new TblCimsattributeValue()
                //{
                //    AttributeCode = x.AttributeCode,
                   
                //    AttributeValue = "",
                //    IsDelete = false,
                //    RecordId = rowIdentify,
                //    CreatedDate = DateTime.Now,
                //});
                //lstAttributes.AddRange(lst);
                //db.TblCimsattributeValue.AddRange(lstAttributes);
                //db.Entry(lstAttributes).State = EntityState.Modified;
                //db.SaveChanges();
                return true;
                

            }
            else
            {
                return false;
            }
           
        }



        //Adddate CIMS
        /// <summary>
        ///Phongtv
        /// AddCimsValue
        /// </summary>
        /// <param name="lstCustomer"></param>
        /// <returns></returns>
        public object AddCimsValue(List<TblCimsattributeValue> lstCustomer)
        {
            var rowIdentify = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            
            const string module = "CIMS";
            try
            {
                List<object> lstErrs = new List<object>();
                List<TblCimsattributeValue> lstAttributes = new List<TblCimsattributeValue>();
                for (int i = 0; i < (lstCustomer.Count); i++)
                {
                    var tblVocattributesIsRequired = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsRequired == true).FirstOrDefault();
                    if (tblVocattributesIsRequired != null)
                    {
                        if (lstCustomer[i].AttributeValue.Length > 0)
                        {
                            TblVocattributes tblVocattributesIsDuplicate = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsDuplicate == true
                          ).FirstOrDefault();
                            if (tblVocattributesIsDuplicate != null)
                            {
                                TblCimsattributeValue cimsValue = db.TblCimsattributeValue.Where(b => b.AttributeCode == lstCustomer[i].AttributeCode && b.AttributeValue.ToString() == lstCustomer[i].AttributeValue).FirstOrDefault();
                                if (cimsValue == null)
                                {
                                    TblVocattributes attcheck = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsReuse == true).FirstOrDefault();

                                    if (attcheck == null)
                                    {
                                        TblCimsattributeValue CimsAdd = new TblCimsattributeValue();
                                        CimsAdd.AttributeCode = lstCustomer[i].AttributeCode;
                                        CimsAdd.AttributeValue = lstCustomer[i].AttributeValue;
                                        CimsAdd.Module = module;
                                        CimsAdd.IsDelete = false;
                                        CimsAdd.RecordId = rowIdentify;
                                        CimsAdd.CreatedDate = DateTime.Now;
                                        lstAttributes.Add(CimsAdd);
                                    }
                                    else
                                    {
                                        string currDate = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                                        TblCimsattributeValue CimsAdd = new TblCimsattributeValue();
                                        CimsAdd.AttributeCode = lstCustomer[i].AttributeCode;
                                        CimsAdd.AttributeValue = "CUS."+ currDate;
                                        CimsAdd.Module = module;
                                        CimsAdd.IsDelete = false;
                                        CimsAdd.RecordId = rowIdentify;
                                        CimsAdd.CreatedDate = DateTime.Now;
                                        lstAttributes.Add(CimsAdd);
                                    }

                                 
                                }
                                else
                                {
                                    lstErrs.Add(new
                                    {
                                        field = tblVocattributesIsDuplicate.AttributeCode,
                                        name = tblVocattributesIsDuplicate.AttributeLabel,
                                        message = "NotDuplicate"
                                    });
                                }
                            }
                            else
                            {
                                TblVocattributes attcheck = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsReuse == true).FirstOrDefault();

                                if (attcheck == null)
                                {
                                    TblCimsattributeValue CimsAdd = new TblCimsattributeValue();
                                    CimsAdd.AttributeCode = lstCustomer[i].AttributeCode;
                                    CimsAdd.AttributeValue = lstCustomer[i].AttributeValue;
                                    CimsAdd.Module = module;
                                    CimsAdd.IsDelete = false;
                                    CimsAdd.RecordId = rowIdentify;
                                    CimsAdd.CreatedDate = DateTime.Now;
                                    lstAttributes.Add(CimsAdd);
                                }
                                else
                                {
                                    string currDate = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                                    TblCimsattributeValue CimsAdd = new TblCimsattributeValue();
                                    CimsAdd.AttributeCode = lstCustomer[i].AttributeCode;
                                    CimsAdd.AttributeValue = "CUS." + currDate;
                                    CimsAdd.Module = module;
                                    CimsAdd.IsDelete = false;
                                    CimsAdd.RecordId = rowIdentify;
                                    CimsAdd.CreatedDate = DateTime.Now;
                                    lstAttributes.Add(CimsAdd);
                                }
                            }
                        }
                        else
                        {
                            lstErrs.Add(new
                            {
                                field = tblVocattributesIsRequired.AttributeCode,
                                name = tblVocattributesIsRequired.AttributeLabel,
                                message = "NotNull"
                            });
                        }
                    }
                    else
                    {

                        TblVocattributes tblVocattributesIsDuplicate = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsDuplicate == true
                        ).FirstOrDefault();
                        if (tblVocattributesIsDuplicate != null)
                        {
                            TblCimsattributeValue cimsValue = db.TblCimsattributeValue.Where(b => b.AttributeCode == tblVocattributesIsDuplicate.AttributeCode && b.AttributeValue.ToString() == lstCustomer[i].AttributeValue).FirstOrDefault();
                            if (cimsValue == null )
                            {
                                TblVocattributes attcheck = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsReuse == true).FirstOrDefault();

                                if (attcheck == null)
                                {
                                    TblCimsattributeValue CimsAdd = new TblCimsattributeValue();
                                    CimsAdd.AttributeCode = lstCustomer[i].AttributeCode;
                                    CimsAdd.AttributeValue = lstCustomer[i].AttributeValue;
                                    CimsAdd.Module = module;
                                    CimsAdd.IsDelete = false;
                                    CimsAdd.RecordId = rowIdentify;
                                    CimsAdd.CreatedDate = DateTime.Now;
                                    lstAttributes.Add(CimsAdd);
                                }
                                else
                                {
                                    string currDate = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                                    TblCimsattributeValue CimsAdd = new TblCimsattributeValue();
                                    CimsAdd.AttributeCode = lstCustomer[i].AttributeCode;
                                    CimsAdd.AttributeValue = "CUS." + currDate;
                                    CimsAdd.Module = module;
                                    CimsAdd.IsDelete = false;
                                    CimsAdd.RecordId = rowIdentify;
                                    CimsAdd.CreatedDate = DateTime.Now;
                                    lstAttributes.Add(CimsAdd);
                                }
                            }
                            else
                            {
                                lstErrs.Add(new
                                {
                                    field = tblVocattributesIsDuplicate.AttributeCode,
                                    name = tblVocattributesIsDuplicate.AttributeLabel,
                                    message = "NotDuplicate"
                                });
                            }
                        }
                        else
                        {
                            TblVocattributes attcheck = db.TblVocattributes.Where(a => a.AttributeCode == lstCustomer[i].AttributeCode && a.IsReuse == true).FirstOrDefault();

                            if (attcheck == null)
                            {
                                TblCimsattributeValue CimsAdd = new TblCimsattributeValue();
                                CimsAdd.AttributeCode = lstCustomer[i].AttributeCode;
                                CimsAdd.AttributeValue = lstCustomer[i].AttributeValue;
                                CimsAdd.Module = module;
                                CimsAdd.IsDelete = false;
                                CimsAdd.RecordId = rowIdentify;
                                CimsAdd.CreatedDate = DateTime.Now;
                                lstAttributes.Add(CimsAdd);
                            }
                            else
                            {
                                string currDate = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
                                TblCimsattributeValue CimsAdd = new TblCimsattributeValue();
                                CimsAdd.AttributeCode = lstCustomer[i].AttributeCode;
                                CimsAdd.AttributeValue = "CUS." + currDate;
                                CimsAdd.Module = module;
                                CimsAdd.IsDelete = false;
                                CimsAdd.RecordId = rowIdentify;
                                CimsAdd.CreatedDate = DateTime.Now;
                                lstAttributes.Add(CimsAdd);
                            }
                        }
                        
                    }
                }
               
                // list
                if (lstErrs.Count > 0)
                {
                    return lstErrs;
                }
                var lst = db.TblVocattributes.Where(v => v.ModuleParent == "CIMS" &&
                v.IsDelete == false && lstCustomer.Where(x => x.AttributeCode == v.AttributeCode).Count() == 0).
                Select(x => new TblCimsattributeValue()
                {
                    AttributeCode = x.AttributeCode,
                    Module = module,
                    AttributeValue = "",
                    IsDelete = false,
                    RecordId = rowIdentify,
                    CreatedDate = DateTime.Now,
                });
                lstAttributes.AddRange(lst);
                db.TblCimsattributeValue.AddRange(lstAttributes);
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public object GetCustomerList_RecordId(string RecordId)
        {
            try
            {
                List<List<dynamic>> obj = cimsDA.GetCustomerList_RecordId(RecordId);
                var response = obj[0];
                return response;
            }
            catch (Exception ex)
            {
                return new { code = 500, ex.Message };
            }
        }




        /// <summary>
        /// GetCimsvalue
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        public object GetCimsvalue(string ModuleParent, int currPage, int recodperpage)
        {
            try
            {
                List<List<dynamic>> obj = cimsDA.GetCimsvalue(ModuleParent, currPage, recodperpage);
                var response = new { search = obj[0], column = obj[1], data = obj[2], pagination = obj[3] };
                return response;
            }
            catch (Exception ex)
            {
                return new { code = 500, ex.Message };
            }
        }


        /// <summary>
        /// //List ten truong
        /// </summary>
        /// <returns></returns>
        public List<string> GetAtt()
        {
            try
            {
                var lstCimsatt = (from a in db.TblCimsattributeValue
                                  join b in db.TblVocattributes on a.AttributeCode equals b.AttributeCode
                                  where (a.Module == "CIMS" && b.IsTableShow == true)
                                  select new
                                  {
                                      b.AttributeLabel
                                  });

                return lstCimsatt.Select(s => s.AttributeLabel).Distinct().ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public void LoadContext(string orgCode, IDistributedCache distributedCache)
        {
            if (db == null || strconnect != orgCode)
            {
                string conn = CimsConstant.MP_CONNECTION;
                var optionsBuilder = new DbContextOptionsBuilder<CRM_MPContext>();
                optionsBuilder.UseSqlServer(conn.Replace(CimsConstant.DATABASE_MP, orgCode), providerOptions => providerOptions.CommandTimeout(300));
                db = new CRM_MPContext(optionsBuilder.Options);
            }
            cimsDA = new CimsDA_SP(db);
            _distributedCache = distributedCache;
        }
        #region vudt
        public async Task<ResponseDTO> GetListAttributesEncryption(string parentCode)
        {
            try
            {
                var lst = new AttributesEncryptionDTO();
                lst.tblEncrpytion = await db.TblEncryption.Where(x => x.ParentCode == parentCode).Select(x => new EncryptionDTO()
                {
                    AttributeCode = db.TblVocattributes.Where(c => c.AttributeCode == x.AttributeCode).FirstOrDefault().AttributeCode,
                    AttributeLabel = db.TblVocattributes.Where(c => c.AttributeCode == x.AttributeCode).FirstOrDefault().AttributeLabel,
                    ParentCode = db.TblVocattributes.Where(c => c.AttributeCode == x.AttributeCode).FirstOrDefault().ModuleParent,
                    CreateDate = x.CreateDate,
                    UpdateDate = cimsCommon.GetTimeExecuteEncrpytion(x.UpdateDate),
                    EncryptionStatus = x.EncryptionStatus
                }).AsNoTracking().ToListAsync();
                lst.tblAttributes = await db.TblVocattributes.Where(x => !lst.tblEncrpytion.Where(c => c.AttributeCode == x.AttributeCode && c.ParentCode == x.ModuleParent).Select(c => c.AttributeCode).Contains(x.AttributeCode)).Select(x => new TblVocattributes()
                {
                    AttributeCode = x.AttributeCode,
                    AttributeLabel = x.AttributeLabel
                }).AsNoTracking().ToListAsync();
                return new ResponseDTO() { StatusCode = 200, Response = lst };
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { StatusCode = 400, Response = ex.Message };
            }
        }

        public async Task<ResponseDTO> ExecuteEncrpytion(AttributesDTO model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var item in model.tblVocattributes)
                    {
                        
                        var label = await db.TblVocattributes.Where(x => x.AttributeCode == item.AttributeCode).AsNoTracking().FirstOrDefaultAsync();
                        var attributeEncryption = new TblEncryption();
                        attributeEncryption.AttributeCode = item.AttributeCode;
                        attributeEncryption.AttributeLabel = label.AttributeLabel;
                        attributeEncryption.CreatedBy = model.UserName;
                        attributeEncryption.CreateDate = DateTime.Now;
                        attributeEncryption.ParentCode = item.ModuleParent;
                        attributeEncryption.EncryptionStatus = true;
                        attributeEncryption.UpdateDate = DateTime.Now;
                        db.TblEncryption.Add(attributeEncryption);
                        
                    }
                    await db.SaveChangesAsync();
                    scope.Complete();
                    return new ResponseDTO() { StatusCode = 200, Response = CimsConstant.MS0013 };
                }
                catch (Exception ex)
                {
                    return new ResponseDTO() { StatusCode = 400, Response = ex.Message };
                }

            }
        }
        private void Encrpytion()
        {

        }
        #endregion
    }

}



