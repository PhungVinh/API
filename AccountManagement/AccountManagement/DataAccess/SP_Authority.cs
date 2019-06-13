using AccountManagement.Models;
using AccountManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace AccountManagement.DataAccess
{
    public class SP_Authority
    {
        private CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());
        //public SP_Authority(CRM_MASTERContext _context)
        //{
        //    db = _context;
        //}

        /// <summary>
        /// Function get list and search authority with paging
        /// CreatedBy: HaiHM
        /// CreatedDate: 3/5/2019
        /// </summary>
        /// <param name="strFilter">filter</param>
        /// <returns>list authority and paging</returns>
        public List<List<dynamic>> SearchAuthority(string strFilter)
        {
            // Initialization.  
            try
            {
                string[] arr = strFilter.Split(',');
                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter("@TextSearch", arr[0]);
                para[1] = new SqlParameter("@currPage", arr[1]);
                para[2] = new SqlParameter("@recodperpage", arr[2]);
                para[3] = new SqlParameter("@userId", arr[3]);
                para[4] = new SqlParameter("@OrganizationId", arr[4]);
                    
                return ExecuteMultipleResults("SearchAuthority", para, typeof(TblAuthorityViewModel), typeof(PagePaging));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }

        /// <summary>
        /// Function copy authority by id
        /// CreatedBy: HaiHM
        /// CreatedDate: 4/5/2019
        /// </summary>
        /// <param name="authorityId"></param>
        /// <returns> role in authority and role not in authority</returns>
        public List<List<dynamic>> CopyAuthority(int authorityId)
        {
            // Initialization.  
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@AuthorityId", authorityId);
                return ExecuteMultipleResults("CopyAuthority", para, typeof(MenuAndRoleViewModel), typeof(MenuAndRoleViewModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }

        /// <summary>
        /// Function executes the specified parameters.
        /// CreatedBy: System
        /// CreatedDate: xx/xx/2019
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="returnTypes">The return types.</param>
        /// <returns></returns>
        public List<List<dynamic>> ExecuteMultipleResults(string spName, SqlParameter[] parameters, params Type[] types)
        {
            List<List<dynamic>> results = new List<List<dynamic>>();

            var connection = db.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null && parameters.Any())
            {
                command.Parameters.AddRange(parameters);
            }

            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }

            int counter = 0;
            using (var reader = command.ExecuteReader())
            {
                do
                {
                    var innerResults = new List<dynamic>();

                    if (counter > types.Length - 1) { break; }

                    while (reader.Read())
                    {
                        var item = Activator.CreateInstance(types[counter]);

                        for (int inc = 0; inc < reader.FieldCount; inc++)
                        {
                            Type type = item.GetType();
                            string name = reader.GetName(inc);
                            PropertyInfo property = type.GetProperty(name);

                            if (property != null && name == property.Name)
                            {
                                var value = reader.GetValue(inc);
                                if (value != null && value != DBNull.Value)
                                {
                                    property.SetValue(item, Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType), null);
                                }
                            }
                        }
                        innerResults.Add(item);
                    }
                    results.Add(innerResults);
                    counter++;
                }
                while (reader.NextResult());
                reader.Close();
                command.Connection.Close();
            }
            return results;
        }

        /// <summary>
        /// Function get list menu(role) with paging
        /// CreatedBy: HaiHM
        /// CreatedDate: xx/05/2019
        /// Get List Role
        /// </summary>
        /// <returns></returns>
        public List<List<dynamic>> SearchMenuAndRole(string ParentCode)
        {
            // Initialization.  
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@ParentCode", ParentCode);
                return ExecuteMultipleResults("SearchMenuAndRole", para, typeof(MenuAndRoleViewModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.         
        }
        
        #region vudt
        /// <summary>
        /// vudt
        /// Get List Users Not Granted Authority
        /// </summary>
        /// <returns></returns>
        public object GetUserNotGrantedAuthority()
        {
            // Initialization.  
            try
            {
                return ExecuteMultipleResults("sp_GetUserNotGrantedAuthority", null, typeof(TblUserViewModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.         
        }
        #endregion
    }
}
