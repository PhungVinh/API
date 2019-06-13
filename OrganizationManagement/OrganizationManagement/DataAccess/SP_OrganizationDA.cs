using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using OrganizationManagement.Common;
using OrganizationManagement.Constant;
using OrganizationManagement.Models;
using OrganizationManagement.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OrganizationManagement.DataAccess
{
    public class SP_OrganizationDA: IObjectContextAdapter
    {
        private IDistributedCache _distributedCache;
        public void LoadDistributedCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        private OrganizationCommon common = new OrganizationCommon();

        CRM_MASTERContext db = new CRM_MASTERContext(new Microsoft.EntityFrameworkCore.DbContextOptions<CRM_MASTERContext>());




        public object GetOrg_Page1(string strFilter)
        {
            string[] arr = strFilter.Split(',');
            DataSet ds = new DataSet();
            SqlParameter[] para = new SqlParameter[6];
            para[0] = new SqlParameter("@DateFrom", arr[0]);
            para[1] = new SqlParameter("@DateTo", arr[1]);
            para[2] = new SqlParameter("@TextSearch", arr[2]);
            para[3] = new SqlParameter("@IsActive", arr[3]);
            para[4] = new SqlParameter("@currPage", arr[4]);
            para[5] = new SqlParameter("@recodperpage", arr[5]);
            string sqlQuery = "EXEC [dbo].[GetOrg_Page] @DateFrom, @DateTo, @TextSearch, @IsActive, @currPage, @recodperpage";
            List<OrganizationSP> lst = db.Query<OrganizationSP>().FromSql(sqlQuery, para).ToListAsync().Result;
            
            return lst;
        }



        // List goi dich vu

        public List<List<dynamic>> GetServicePack(int OrganizationId)
        {
            var para = new DynamicParameters();
            para.Add("OrganizationId", OrganizationId);
          
            List<List<dynamic>> lstObject = new List<List<dynamic>>();
            lstObject = ReturnDynamicTable("GetServicePack", para);
            return lstObject;
        }


        /// <summary>
        /// Lay du lieu dong tra ve 1 ket qua
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// 
        public List<List<dynamic>> ReturnDynamicTable(string procedure, DynamicParameters parameters)
        {
            List<List<dynamic>> lstDynamic = new List<List<dynamic>>();
            var connection = db.Database.GetDbConnection() as SqlConnection;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            var grid = connection.QueryMultiple(sql: procedure,
                                  param: parameters,
                                  commandType: CommandType.StoredProcedure);
            while (!grid.IsConsumed)
            {
                lstDynamic.Add(grid.Read().ToList());
            }
            connection.Close();
            return lstDynamic;
        }


        public DataSet GetDataSet(string spName, SqlParameter[] para)
        {
            DataSet ds = new DataSet();
            var connection = db.Database.GetDbConnection();
            var command = connection.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            if (para != null && para.Any())
            {
                command.Parameters.AddRange(para);
            }

            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
            var result = command.ExecuteReader();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command as SqlCommand);
            sqlDataAdapter.Fill(ds);
            command.Connection.Close();
            return ds;
        }
        
        public List<List<dynamic>> GetOrg_Page(string strFilter)
        {
            string[] arr = strFilter.Split(',');
            DataSet ds = new DataSet();
            SqlParameter[] para = new SqlParameter[6];
            para[0] = new SqlParameter("@DateFrom", arr[0]);
            para[1] = new SqlParameter("@DateTo", arr[1]);
            para[2] = new SqlParameter("@TextSearch", arr[2]);
            para[3] = new SqlParameter("@IsActive", arr[3]);
            para[4] = new SqlParameter("@currPage", arr[4]);
            para[5] = new SqlParameter("@recodperpage", arr[5]);
            return ExecuteMultipleResults("GetOrg_Page",para, typeof(OrganizationSP), typeof(PagePaging));
        }

        /// <summary>
        /// Executes the specified parameters.
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
        /// Executes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="returnTypes">The return types.</param>
        /// <returns></returns>
        public void ExecuteNonQuerySql(string sql)
        {
            var connection = db.Database.GetDbConnection();
            string strConnect = connection.ConnectionString;
            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
            command.ExecuteNonQuery();
            command.Connection.Close();
        }
        public void ExecuteNonQueryStoreProcedure(string sql, string dbName)
        {
            string strConnect = OrganizationConstant.SQL_CONNECTION;
            SqlConnection sqlConnection = new SqlConnection(strConnect.Replace(OrganizationConstant.DATABASE_MASTER, dbName));
            SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        /// <summary>
        /// Function get Menu when create Orga
        /// CreatedBy: HaiHM
        /// CreatedDate: 22/5/2019
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public List<TblMenu> LoadMenuWhenAddOrg()
        {
            List<TblMenu> lst = new List<TblMenu>();
            try
            {
                // Processing.  
                string sqlQuery = "EXEC sp_LoadMenuWhenAddOrg";
                lst = db.TblMenu.FromSql(sqlQuery).ToListAsync().Result;
                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
