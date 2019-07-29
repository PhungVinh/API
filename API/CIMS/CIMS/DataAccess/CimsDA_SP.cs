using CIMS.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CIMS.DataAccess
{
    public class CimsDA_SP
    {
        //private CimsDA cDA = new CimsDA();
        private IDistributedCache _distributedCache;
        public CimsDA_SP(CRM_MPContext context)
        {
            db = context;
        }
        public void LoadDistributedCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        private CRM_MPContext db;



        /// <summary>
        /// List thong tin 1 khach hang
        /// </summary>
        /// <param name="RecordId"></param>
        /// <returns></returns>
        public List<List<dynamic>> GetCustomerList_RecordId(string RecordId)
        {
            var para = new DynamicParameters();
            para.Add("@RecordId", RecordId);
            List<List<dynamic>> lstObject = new List<List<dynamic>>();
            lstObject = ReturnDynamicTableGetCustomerList_RecordId("cims_GetCustomerList_RecordId", para);
            return lstObject;
        }

        public List<List<dynamic>> ReturnDynamicTableGetCustomerList_RecordId(string procedure, DynamicParameters parameters)
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

        /// <summary>
        /// List danh sach CIMS
        /// </summary>
        /// <param name="ModuleParent"></param>
        /// <param name="currPage"></param>
        /// <param name="recodperpage"></param>
        /// <returns></returns>
        public List<List<dynamic>> GetCimsvalue(string ModuleParent, int currPage, int recodperpage)
        {
            var para = new DynamicParameters();
            para.Add("@Module", ModuleParent);
            para.Add("@CurrPage", currPage);
            para.Add("@PageSize", recodperpage);
            List<List<dynamic>> lstObject = new List<List<dynamic>>();
            lstObject = ReturnDynamicTable("cims_GetCustomerList", para);
            return lstObject;
        }

        /// <summary>
        /// Executes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="returnTypes">The return types.</param>
        /// <returns></returns>
        public List<List<dynamic>> ExecuteMultipleResults(string spName, SqlParameter[] parameters, List<string> lstCol, params Type[] types)
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
            //using (var reader = command.ExecuteReader())
            using (var reader = command.ExecuteReader())

            {
                do
                {
                    var innerResults = new List<dynamic>();
                    while (reader.Read())
                    {
                        var item = CimsBuilder.CreateNewObject(lstCol);
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
            }
            return results;
        }
        /// <summary>
        /// lay du lieu dong: tra ve nhieu ket qua
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Lay du lieu dong tra ve 1 ket qua
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<dynamic> ReturnDynamicTableSingle(string procedure, DynamicParameters parameters)
        {
            List<List<dynamic>> lstDynamic = new List<List<dynamic>>();
            var connection = db.Database.GetDbConnection() as SqlConnection;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            var grid = connection.Query(sql: procedure,
                                  param: parameters,
                                  commandType: CommandType.StoredProcedure);
            connection.Close();
            return grid.ToList();
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
    }
}