using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace DataAccessLayer
{
    public class DataProvider
    {
        protected static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=RestaurantDB;Integrated Security=True";

        protected SqlConnection connection;
        protected SqlCommand command;
        protected SqlDataAdapter dataAdapter;
        protected SqlDataReader dataReader;

        public static string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public SqlCommand Command
        {
            get { return command; }
            set { command = value; }
        }

        public SqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        public SqlDataAdapter DataAdapter
        {
            get { return dataAdapter; }
            set { dataAdapter = value; }
        }

        public SqlDataReader DataReader
        {
            get { return dataReader; }
            set { dataReader = value; }
        }

        // Phương thức khởi tạo 
        public DataProvider()
        {
            connection = new SqlConnection(ConnectionString);

            command = new SqlCommand();
            command.Connection = connection;

            dataAdapter = new SqlDataAdapter();
        }

        // Phương thức kết nối cơ sở dữ liệu
        public void Connect()
        {
            connection.Open();
        }

        // Phương thức đóng kết nối cơ sở dữ liệu
        public void Disconnect()
        {
            connection.Close();
        }

        public SqlDataReader ExecuteQuery(string sqlQuery)
        {
            command.CommandText = sqlQuery;
            return command.ExecuteReader();
        }

        public DataTable ExecuteQueryDT(string sqlQuery)
        {
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            return ds.Tables[0];
        }

        public void ExecuteNonQuery(string sqlQuery)
        {
            try
            {
                connection.Open();
                command.CommandText = sqlQuery;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }

        public object ExecuteScalar(string sqlQuery)
        {
            try
            {
                connection.Open();
                command.CommandText = sqlQuery;
                return command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }

        protected ArrayList ConvertDataSetToArrayList(DataSet dataset)
        {
            ArrayList arr = new ArrayList();
            DataTable dt = dataset.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object obj = GetDataFromDataRow(dt, i);
                arr.Add(obj);
            }
            return arr;
        }

        protected virtual object GetDataFromDataRow(DataTable dt, int i)
        {
            return null;
        }

        protected virtual object GetObjectFromDataReader()
        {
            return null;
        }
    }
}
