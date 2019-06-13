using AccountManagement.Repositories;
using System;
using Microsoft.EntityFrameworkCore;
using AccountManagement.Models;

namespace AccountManagement.DataAccess
{
    public class LogUserLogDA : ILogUserRepository
    {
        private CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());
        public LogUserLogDA()
        {

        }

        /// <summary>
        /// Fuction add log
        /// CreatedBy: HaiHM
        /// CreatedDate: 6/5/2019
        /// </summary>
        /// <param name="log">log</param>
        public void AddLog(TblLogUser log)
        {
            try
            {
                db.TblLogUser.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
    }
}
