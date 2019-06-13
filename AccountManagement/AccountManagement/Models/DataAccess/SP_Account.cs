using AccountManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace AccountManagement.Models.DataAccess
{
    public class SP_Account
    {
        private CRM_MASTERContext db = new CRM_MASTERContext(new DbContextOptions<CRM_MASTERContext>());
        //public SP_Account(CRM_MASTERContext _context)
        //{
        //    db = _context;
        //}

        /// <summary>
        /// Function get list and search user
        /// CreatedBy: HaiHM
        /// CreatedDate: 16/04/2019
        /// </summary>
        /// <param name="strFilter">filter</param>
        /// <returns>object user and pagging</returns>
        public List<List<dynamic>> SearchUser(string strFilter) {
            // Initialization.  
            try
            {
                string[] arr = strFilter.Split(',');
                DataSet ds = new DataSet();
                SqlParameter[] para = new SqlParameter[6];
                para[0] = new SqlParameter("@UserId", arr[0]);
                para[1] = new SqlParameter("@TextSearch", arr[1]);
                para[2] = new SqlParameter("@IsLock", arr[2]);
                para[3] = new SqlParameter("@OrgCode", arr[3]);
                para[4] = new SqlParameter("@currPage", arr[4]);
                para[5] = new SqlParameter("@recodperpage", arr[5]);
                return ExecuteMultipleResults("sp_GetAllUserPermission", para, typeof(TblUserViewModel), typeof(PagePaging));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }

        public List<List<dynamic>> JoinRolePermission(int userId, string MenuCode)
        {
            // Initialization.  
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@UserId", userId);
                para[1] = new SqlParameter("@MenuCode", MenuCode);
                return ExecuteMultipleResults("sp_JoinRolePermission", para, typeof(TblRoleCheckViewModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }

        public List<List<dynamic>> GetAuthorityById(int userId, int orgId)
        {
            // Initialization.  
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@userId", userId);
                para[1] = new SqlParameter("@orgId", orgId);
                return ExecuteMultipleResults("GetAuthorityById", para, typeof(ListAuthorityOfUserVM));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }

        /// <summary>
        /// Function get all department of organization by OrganizationCode
        /// CreatedBy: HaiHM
        /// CreatedDate: 22/04/2019
        /// </summary>
        /// <param name="OrganizationCode"></param>
        /// <returns>List department of organization</returns>
        public List<List<dynamic>> GetDepartment(string OrganizationCode)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@OrganizationCode", OrganizationCode);
                return ExecuteMultipleResults("GetDepartment", para, typeof(TblOrganizationViewModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Function Executes the specified parameters.
        /// CreatedBy: System
        /// CreatedDate: xx/04/2019
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
        /// Function get menu by role
        /// CreatedBy: System
        /// CreatedDate: xx/04/2019
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public object GetMenuByRole(int roleId)
        {
            // Initialization.  
            List<TblMenuSP> lst = new List<TblMenuSP>();
            try
            {
                SqlParameter usernameParam = new SqlParameter("@RoleId", roleId.ToString() ?? (object)DBNull.Value);

                // Processing.  
                string sqlQuery = "EXEC [dbo].[sp_LoadMenu] " +
                                    "@RoleId";
                lst = db.Query<TblMenuSP>().FromSql(sqlQuery, usernameParam).ToListAsync().Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }

        /// <summary>
        /// Function get menu parent
        /// CreatedBy: HaiHm
        /// CreatedDate: 15/04/2019
        /// </summary>
        /// <returns>list menu parent</returns>
        public object GetTblMenuParent()
        {
            // Initialization.  
            List<TblMenu> lst = new List<TblMenu>();
            try
            {
               // SqlParameter usernameParam = new SqlParameter("@RoleId", roleId.ToString() ?? (object)DBNull.Value);

                // Processing.  
                string sqlQuery = "EXEC [dbo].[sp_LoadMenuParent] ";
                lst = db.TblMenu.FromSql(sqlQuery).ToList<TblMenu>();
               // lst = db.Query<TblMenu>().FromSql(sqlQuery).ToListAsync().Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Info.  
            return lst;
        }

        #region Service Pack

        /// <summary>
        /// Function get list and search service pack with paging
        /// CreatedBy: HaiHM
        /// CreatedDate: 5/5/2019
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public List<List<dynamic>> SearchServicePack(string strFilter)
        {
            // Initialization.  
            try
            {
                string[] arr = strFilter.Split(',');
                DataSet ds = new DataSet();
                SqlParameter[] para = new SqlParameter[4];
                para[0] = new SqlParameter("@TextSearch", arr[0]);
                para[1] = new SqlParameter("@IsActive", arr[1]);
                para[2] = new SqlParameter("@currPage", arr[2]);
                para[3] = new SqlParameter("@recodperpage", arr[3]);
                return ExecuteMultipleResults("SearchServicePack_Pag", para, typeof(TblServicePackViewModel), typeof(PagePaging));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }
        #endregion

        /// <summary>
        /// Get All GetAllCategory (Chức vụ) when add or edit user
        /// CreatedBy: HaiHM
        /// CreatedDate: 24/4/2019
        /// </summary>
        /// <returns></returns>
        public List<List<dynamic>> GetAllCategory(string CategoryTypeCode)
        {
            // Initialization.  
            try {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@CategoryTypeCode", CategoryTypeCode);
                return ExecuteMultipleResults("GetAllCategory", para, typeof(TblCategory));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }

    }
}
