using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VOC.Constant;
using VOC.Models;
using VOC.ViewModel;

namespace VOC.DataAccess
{
    public class SP_VOCProcess
    {
        private CRM_MPContext db = new CRM_MPContext(new DbContextOptions<CRM_MPContext>());

        public SP_VOCProcess(CRM_MPContext context)
        {
            db = context;
        }

        /// <summary>
        /// Executes the specified parameters.\
        /// @author: System
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameters"></param>
        /// <param name="types"></param>
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
        /// Executes the specified parameters.\
        /// @author: System
        /// @createdDate: 15/07/2019
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="returnTypes">The return types.</param>
        /// <returns></returns>
        public List<List<dynamic>> ExecuteMultipleResults(string spName, SqlParameter[] parameters, string connection, params Type[] types)
        {
            List<List<dynamic>> results = new List<List<dynamic>>();
            SqlConnection sqlConnection = new SqlConnection(connection);
            var command = sqlConnection.CreateCommand();
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
        /// Function get all connection
        /// CreatedBy: HaiHM
        /// CreatedDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public List<List<dynamic>> GetAllConnection()
        {
            return ExecuteMultipleResults("GetAllConnection", null, VOCConstant.SQL_CONNECTION, typeof(TblConnectionConfig));
        }

        /// <summary>
        /// Get All GetAllCategory (Chức vụ) when add or edit user
        /// CreatedBy: HaiHM
        /// CreatedDate: 15/07/2019
        /// </summary>
        /// <returns></returns>
        public List<List<dynamic>> GetAllCategory(string CategoryTypeCode, string connection)
        {
            // Initialization.  
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@CategoryTypeCode", CategoryTypeCode);
                return ExecuteMultipleResults(VOCConstant.sp_GetAllCategoryByTypeCode, para, connection, typeof(TblCategory));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }


        /// <summary>
        /// Hàm dùng để lấy và tìm kiếm dữ liệu danh sách quy trình sự vụ
        /// @author: HaiHM
        /// @createdDate: 16/07/2019
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgCode"></param>
        /// <param name="isShowAll"></param>
        /// <param name="textSearch"></param>
        /// <param name="isActive"></param>
        /// <param name="currPage"></param>
        /// <param name="recordperpage"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public List<List<dynamic>> SearchVOCProcess(int userId, string orgCode, string isShowAll, string textSearch, string isActive, int currPage, int recordperpage, string connection)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[7];
                para[0] = new SqlParameter("@userId", userId);
                para[1] = new SqlParameter("@orgCode", orgCode);
                para[2] = new SqlParameter("@isShowAll", isShowAll);
                para[3] = new SqlParameter("@textSearch", textSearch);
                para[4] = new SqlParameter("@isActive", isActive);
                para[5] = new SqlParameter("@currPage", currPage);
                para[6] = new SqlParameter("@recordperpage", recordperpage);
                
                return ExecuteMultipleResults(VOCConstant.sp_SearchVOCProcess, para, connection, typeof(TblVocprocess), typeof(PagePaging));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Hàm dùng để lấy danh sách người dùng để asignee cho quy trình
        /// @author: HaiHM
        /// @createdDate: 16/07/2019 
        /// </summary>
        /// <param name="textSearch"></param>
        /// <param name="orgCode"></param>
        /// <param name="vocProcessCode"></param>
        /// <param name="version"></param>
        /// <param name="stepCode"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public List<List<dynamic>> GetUserAssignee(string textSearch, string orgCode, string vocProcessCode, int version, string stepCode, string connection)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter("@textSearch", textSearch);
                para[1] = new SqlParameter("@orgCode", orgCode);

                return ExecuteMultipleResults(VOCConstant.sp_voc_GetUserAssignee, para, connection, typeof(UserViewModel));
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<List<dynamic>> SearchVersion(string userName, string VOCProcessCode, string isActive, int currPage, int recordperpage, string connection)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter("@userName", userName);
                para[1] = new SqlParameter("@VOCProcessCode", VOCProcessCode);
                para[2] = new SqlParameter("@isActive", isActive);
                para[3] = new SqlParameter("@currPage", currPage);
                para[4] = new SqlParameter("@recordperpage", recordperpage);

                return ExecuteMultipleResults("sp_SearchVersion", para, connection, typeof(VOCProcessStepsViewModel), typeof(PagePaging));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
