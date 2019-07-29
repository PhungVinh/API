using AttributesManagement.Models;
using AttributesManagement.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AttributesManagement.DataAccess
{
    public class SP_Attributes
    {
        private CRM_MPContext db = new CRM_MPContext(new DbContextOptions<CRM_MPContext>());
        public SP_Attributes(CRM_MPContext context)
        {
            db = context;
        }
        public List<List<dynamic>> GetAllConstraint(string textSearch, int currPage, int recodperpage)
        {
            // Initialization.  
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@TextSearch", textSearch);
                para[1] = new SqlParameter("@currPage", currPage);
                para[2] = new SqlParameter("@recodperpage", recodperpage);
                return ExecuteMultipleResults("GetAllConstraint", para, typeof(TblAttributeConstraintViewModel), typeof(PagePaging));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }
        public List<List<dynamic>> GetAllCategory(string textSearch, int currPage, int recodperpage)
        {
            // Initialization.  
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] para = new SqlParameter[3];
                para[0] = new SqlParameter("@TextSearch", textSearch);
                para[1] = new SqlParameter("@currPage", currPage);
                para[2] = new SqlParameter("@recodperpage", recodperpage);
                return ExecuteMultipleResults("GetAllCategory_New", para, typeof(TblCategoryViewModel), typeof(PagePaging));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }
        public List<List<dynamic>> GetAllChildCategory(string category)
        {
            // Initialization.  
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@Category", category);
                return ExecuteMultipleResults("GetAllChildCategory", para, typeof(ChildCategory));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Info.  
        }
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

        public DataTable GetDataTable(string procedure, SqlParameter[] parameters)
        {
            DataTable datatable = new DataTable();
            using (System.Data.SqlClient.SqlConnection sqlConnection = new SqlConnection())
            {
                //SqlConnection sqlConnection = new SqlConnection(connection);
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = procedure;
                sqlCommand.CommandTimeout = 120;
                if (parameters != null)
                {
                    sqlCommand.Parameters.AddRange(parameters);
                }
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Connection = sqlConnection;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                try
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                    {
                        sqlConnection.Open();
                    }
                    sqlDataAdapter.Fill(datatable);
                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return datatable;
        }
        public DataTable ReturnDataTable(string sql)
        {
            SqlConnection conn = new SqlConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 600;
            DataTable t1 = new DataTable();
            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(t1);
                cmd.Connection.Close();
            }
            return t1;
        }
        public void ExecuteNonquery(string sql)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
        #region vudt
        public List<List<dynamic>> lstAttributes(int formId)
        { 
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@param", formId);
                return ExecuteMultipleResults("sp_lstAttributes", para, typeof(InfoAttribute));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
