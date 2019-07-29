using Encryption.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.ContextFactory
{
    public class DbContextFactory
    {
        private Dictionary<string, string> ConnectionStrings { get; set; }
        private Dictionary<string, CRM_MPContext> DbContextMap { get; set; }
        private static DbContextFactory dbContextFactory;
        private DbContextFactory()
        {
        }
        public Dictionary<string, CRM_MPContext> GetDbContextMap()
        {
            return DbContextMap;
        }
        public static DbContextFactory getInstance(Dictionary<string, string> ConnectionStrings)
        {
            if (dbContextFactory == null)
            {
                dbContextFactory = new DbContextFactory();
            }
            dbContextFactory.ConnectionStrings = ConnectionStrings;
            dbContextFactory.SetConnectionString(ConnectionStrings);
            return dbContextFactory;
        }

        public static CRM_MPContext getContextInstance(string orgCode)
        {
            if (dbContextFactory == null || dbContextFactory.DbContextMap == null)
            {
                return null;
            }

            return dbContextFactory.DbContextMap.Where(x => x.Key == orgCode).FirstOrDefault().Value;
        }

        public void SetConnectionString(Dictionary<string, string> connStrs)
        {
            ConnectionStrings = connStrs;
            if (DbContextMap == null)
            {
                DbContextMap = new Dictionary<string, CRM_MPContext>();
                foreach (var cons in this.ConnectionStrings)
                {
                    //CRM_MPContext dbcontext = new CRM_MPContext(cons.Value);
                    var optionsBuilder = new DbContextOptionsBuilder<CRM_MPContext>();
                    optionsBuilder.UseSqlServer(cons.Value);
                    CRM_MPContext dbcontext = new CRM_MPContext(optionsBuilder.Options);
                    DbContextMap.Add(cons.Key, dbcontext);
                }
            }
        }
    }
}
