using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOC.Constant;
using VOC.ContextFactory;
using VOC.Models;
using VOC.Repositories;

namespace VOC.DataAccess
{
    public class CategoryDA : IcategoryRepository
    {
        private CRM_MPContext db = new CRM_MPContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MPContext>());
        private IDistributedCache _distributedCache;
        public string strconnect { get; set; }

        public object GetAllCategory(string TextSearch, int currPage, int recodperpage)
        {
            try
            {
                //List<List<dynamic>> obj = sp.GetAllCategory(TextSearch, currPage, recodperpage);
                //var response = new { data = obj[0], paging = obj[1] };
                //return response;
                //var data = db.TblCategory.Where(c => c.IsDelete == false);
                return null;
            }
            catch (Exception ex)
            {
                return new { code = 500, messsage = ex.Message };
            }
        }

        public void LoadContext(string orgCode, IDistributedCache distributedCache)
        {
            if (db == null || strconnect != orgCode)
            {
                string conn = VOCConstant.SQL_CONNECTION;
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
    }
}
