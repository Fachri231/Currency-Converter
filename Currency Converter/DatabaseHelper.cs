using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currency_Converter
{
    internal class DatabaseHelper
    {
        private string stringConn = "Data Source=DESKTOP-RNFEO80\\SQLEXPRESS;Initial Catalog=CurrencyConverter;Integrated Security=True;";
        private SqlConnection GetConnection()
        {
            return new SqlConnection(stringConn);
        }

        public DataTable getData(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Terjadi Kesalahan Di Database : " + ex.Message, ex);
                } 
                catch (Exception ex)
                {
                    throw new Exception("Terjadi Kesalahan : " + ex.Message, ex);
                }
            }
            return dataTable;
        }

       public object excuteScalar(string query, SqlParameter[] parameters = null)
       {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Terjadi Kesalahan Database : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Terjadi Kesalahan : " + ex.Message, ex);
            }
       }

        public int executeNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Terjadi Kesalahan Database : " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Terjadi Kesalahan : " + ex.Message, ex);
            }
        }
    }
}
