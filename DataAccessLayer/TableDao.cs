using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataTransferObject;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public class TableDao : DataProvider
    {
        public ArrayList GetTableListInSection(int sectionID)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT * FROM R_TABLE WHERE SectionID = {0}", sectionID);
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        public ArrayList GetTableByName(string tableName, int sectionID)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT * FROM R_TABLE WHERE SectionID = {0} AND Name LIKE '%{1}%'", sectionID, tableName);
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        public Table GetTable(int tableID)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT * FROM R_TABLE WHERE ID = {0}", tableID);
            SqlDataReader dataReader = ExecuteQuery(sqlQuery);

            Table table = null;
            if (dataReader.Read())
            {
                table = new Table();
                table.ID = int.Parse(dataReader["ID"].ToString());
                table.Name = dataReader["Name"].ToString();
                table.Description = dataReader["Description"].ToString();
                table.SectionID = int.Parse(dataReader["SectionID"].ToString());
                table.Status = (bool)dataReader["Status"];
            }

            Disconnect();

            return table;
        }

        protected override object GetDataFromDataRow(DataTable dt, int i)
        {
            Table table = new Table();

            table.ID = Int16.Parse(dt.Rows[i]["ID"].ToString());
            table.SectionID = Int16.Parse(dt.Rows[i]["SectionID"].ToString());
            table.Name = dt.Rows[i]["Name"].ToString();
            table.Description = dt.Rows[i]["Description"].ToString();
            table.Status = (bool)dt.Rows[i]["Status"];

            return (object)table;
        }

        public void Update(Table table)
        {
            string sqlQuery = string.Format(@"UPDATE R_TABLE SET SectionID = {0}, Name = N'{1}', Description = N'{2}', Status = {3} WHERE ID = {4}",
                table.SectionID, table.Name, table.Description, (table.Status == true) ? 1 : 0, table.ID);
            ExecuteNonQuery(sqlQuery);
        }

        public void Delete(Table table)
        {
            string sqlQuery = string.Format(@"DELETE FROM R_TABLE WHERE ID = {0}", table.ID);
            ExecuteNonQuery(sqlQuery);
        }

        public int GetNextID()
        {
            string sqlQuery = String.Format(@"SELECT COUNT(ID) FROM R_TABLE");
            int nextID = (int)ExecuteScalar(sqlQuery) + 1;

            if (nextID != 1)
            {
                sqlQuery = String.Format(@"SELECT MAX(ID) FROM R_TABLE");
                nextID = (int)ExecuteScalar(sqlQuery) + 1;
            }

            return nextID;
        }

        public void Insert(Table table)
        {
            string sqlQuery = string.Format(@"INSERT INTO R_TABLE VALUES({0}, {1}, '{2}', '{3}', {4})",
                table.ID, table.SectionID, table.Name, table.Description, (table.Status == true) ? 1 : 0);
            ExecuteNonQuery(sqlQuery);
        }

        public void DeleteBySectionID(Table table)
        {
            string sqlQuery = string.Format(@"DELETE FROM R_TABLE WHERE SectionID = {0}", table.SectionID);
            ExecuteNonQuery(sqlQuery);
        }
    }
}
