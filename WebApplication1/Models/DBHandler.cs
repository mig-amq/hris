using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;

    public enum CRUD
    {
        CREATE,
        READ,
        UPDATE,
        DELETE
    }

    public class DBHandler
    {
        private SqlConnectionStringBuilder SqlConnectionBuilder;

        public DBHandler()
        {
            SqlConnectionBuilder = new SqlConnectionStringBuilder();
            this.SqlConnectionBuilder.DataSource = "localhost\\MSSQLSRVR";
            this.SqlConnectionBuilder.InitialCatalog = "master";
            this.SqlConnectionBuilder.IntegratedSecurity = true;
        }

        /**
         * <summary>Executes the specified sql command and
         * returns either the DataTable containing the Read Data or the Primary Key of the
         * first affected row</summary>
         */
        public T Execute<T>(CRUD type, string sql, Dictionary<string, dynamic> param = null)
        {
            try
            {

                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(this.SqlConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    Console.WriteLine(connection);
                    if (param != null)
                    {
                        foreach (KeyValuePair<string, dynamic> kv in param)
                        {
                            command.Parameters.AddWithValue(kv.Key, kv.Value);
                        }
                    }

                    switch (type)
                    {
                        case CRUD.READ:
                            dt.Load(command.ExecuteReaderAsync().Result);
                            connection.Close();
                            try
                            {
                                return (T)Convert.ChangeType(dt, typeof(T));
                            }
                            catch (InvalidCastException)
                            {
                                return default(T);
                            }
                        default:
                            if (type != CRUD.DELETE)
                            {
                                int recID = (int)command.ExecuteScalarAsync().Result;
                                connection.Close();
                                try
                                {
                                    return (T)Convert.ChangeType(recID, typeof(T));
                                }
                                catch (InvalidCastException)
                                {
                                    return default(T);
                                }
                            }
                            else
                            {
                                command.ExecuteScalarAsync();
                                connection.Close();
                                return default(T);
                            }

                            break;
                    }
                
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }
    }
}