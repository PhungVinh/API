using AttributesManagement.Constant;
using AttributesManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AttributesManagement.Common
{
    public class AttributeCommon
    {
        public static List<ConnectionStrings> ListConnectionStrings { get; set; }
        public List<List<dynamic>> GetAllConnection()
        {
            return ExecuteMultipleResults(AttributeConstant.MASTER_STORE_PROC, null, AttributeConstant.SQL_CONNECTION, typeof(ConnectionStrings));
        }

        /// <summary>
        /// Executes the specified parameters.
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
    }
}
