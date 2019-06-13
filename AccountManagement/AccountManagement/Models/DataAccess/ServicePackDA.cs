using AccountManagement.Constant;
using AccountManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountManagement.Models.DataAccess
{
    public class ServicePackDA : IServicePackRepository
    {
        private CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());
        private SP_Account sp = new SP_Account();
        private IDistributedCache _distributedCache;
        private IConfiguration _config;

        public ServicePackDA(IDistributedCache distributedCache, IConfiguration config)
        {
            _distributedCache = distributedCache;
            _config = config;
        }

        /// <summary>
        /// Function Lock And Active ServicePack By Id ServicePack
        /// CreatedBy: HaiHM
        /// CreatedDate: 6/5/2019
        /// </summary>
        /// <param name="id">id of Service Pack</param>
        /// <returns>1= success, 0 fail </returns>
        public int ActiveAndLockServicePack(int id)
        {
            int result = 0;
            try
            {
                TblServicePack pack = GetServicePackById(id);
                if (pack != null)
                {
                    if (pack.IsActive == true)
                    {
                        pack.IsActive = false;
                    }
                    else
                    {
                        pack.IsActive = true;
                    }

                    db.Entry(pack).State = EntityState.Modified;
                    db.SaveChanges();
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Function Add ServicePack
        /// CreatedBy: HaiHM
        /// CreatedDate: 6/5/2019
        /// Not same [CodeServicePack], [NameServicePack]
        /// </summary>
        /// <param name="servicePack">obj</param>
        /// <returns>1= success, 0 fail, 2 same name or code </returns>
        public int AddServicePack(TblServicePack servicePack)
        {
            try
            {
                TblServicePack packCheck = db.TblServicePack.Where(sp => sp.IsDelete == false &&
                        String.Compare(sp.CodeServicePack, servicePack.CodeServicePack, false) == 0 &&
                        String.Compare(sp.CodeServicePack, servicePack.CodeServicePack, false) == 0).FirstOrDefault();

                if (packCheck == null)
                {
                    packCheck = new TblServicePack();
                    //packCheck.public int Id { get; set; }
                    packCheck.CodeServicePack = servicePack.CodeServicePack;
                    packCheck.NameServicePack = servicePack.NameServicePack;
                    packCheck.Description = servicePack.Description;
                    packCheck.Avatar = servicePack.Avatar;
                    packCheck.MaxUser = servicePack.MaxUser;
                    packCheck.Price = servicePack.Price;
                    packCheck.IsActive = servicePack.IsActive;
                    packCheck.IsDelete = servicePack.IsDelete;
                    packCheck.Discount = servicePack.Discount;
                    packCheck.Hot = servicePack.Hot;
                    packCheck.CreatedBy = servicePack.CreatedBy;
                    packCheck.CreatedDate = DateTime.Now;
                    packCheck.UpdatedBy = servicePack.UpdatedBy;
                    packCheck.UpdatedDate = DateTime.Now;

                    db.TblServicePack.Add(packCheck);
                    db.SaveChanges();
                    return ServicePackConstant.AddServicePackSuccess;
                }
                else
                {
                    return ServicePackConstant.AddServicePackFail;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// CreatedBy: HaiHM
        /// CreatedDate: 7/5/2019
        /// Delete ServicePack By Id ServicePack
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1= success, 0 fail </returns>
        public int DeleteServicePack(int id)
        {
            int result = ServicePackConstant.DeleteServicePackkFail;
            try
            {
                TblServicePack pack = GetServicePackById(id);
                if (pack != null)
                {
                    pack.IsDelete = true;
                    pack.UpdatedDate = DateTime.Now;
                    db.Entry(pack).State = EntityState.Modified;
                    db.SaveChanges();
                    result = ServicePackConstant.DeleteServicePackSuccess;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Function Edit service pack
        /// CreatedBy: HaiHM
        /// CreatedDate: 7/5/2019
        /// </summary>
        /// <param name="servicePack"></param>
        /// <returns>1= success, 0 fail, 2 same name or code </returns>
        public int EditServicePack(TblServicePack servicePack)
        {
            try
            { 
                TblServicePack packCheck = db.TblServicePack.Where(sp => sp.IsDelete == false && sp.Id == servicePack.Id).FirstOrDefault();

                if (packCheck != null)
                {
                    //packCheck = new TblServicePack();
                    //packCheck.public int Id { get; set; }
                    packCheck.CodeServicePack = servicePack.CodeServicePack;
                    packCheck.NameServicePack = servicePack.NameServicePack;
                    packCheck.Description = servicePack.Description;
                    packCheck.Avatar = servicePack.Avatar;
                    packCheck.MaxUser = servicePack.MaxUser;
                    packCheck.Price = servicePack.Price;
                    packCheck.IsActive = servicePack.IsActive;
                    packCheck.IsDelete = servicePack.IsDelete;
                    packCheck.Discount = servicePack.Discount;
                    packCheck.Hot = servicePack.Hot;
                    packCheck.UpdatedBy = servicePack.UpdatedBy;
                    packCheck.UpdatedDate = DateTime.Now;

                    db.Entry(packCheck).State = EntityState.Modified;
                    db.SaveChanges();
                    return ServicePackConstant.EditServicePackSuccess;
                }
                else
                {
                    return ServicePackConstant.EditServicePackFail;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Function get list and search service pack
        /// CreatedBy: HaiHM
        /// CreatedDate: 7/5/2019
        /// </summary>
        /// <param name="strFilter">filter parameter</param>
        /// <returns>object service pack and paging</returns>
        public object GetListServicePack(string strFilter)
        {
            try
            {
                List<List<dynamic>> obj = sp.SearchServicePack(strFilter);
                var response = new { data =obj[0], paging =obj[1] };
                //Active comment for Unitest
                //int r = obj[0].Count();
                //return r;
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Function get service pack by id
        /// CreatedBy: HaiHM
        /// CreatedDate: 7/5/2019
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null or service pack has same id</returns>
        public TblServicePack GetServicePackById(int id)
        {
            try
            {
                TblServicePack obj = db.TblServicePack.Where(u => u.Id == id).FirstOrDefault();
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
    }
}
