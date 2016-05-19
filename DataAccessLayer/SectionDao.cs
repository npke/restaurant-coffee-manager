using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataTransferObject;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace DataAccessLayer
{
    public class SectionDao : DataProvider
    {
        public SectionDao()
        {
        }

        public void Insert(Section section)
        {
            string sqlQuery = string.Format(@"INSERT INTO R_Section VALUES({0}, N'{1}', N'{2}')",
                                              section.ID, section.Name, section.Description);
            ExecuteNonQuery(sqlQuery);
        }

        public int GetNextID()
        {
            string sqlQuery = String.Format(@"SELECT COUNT(ID) FROM R_SECTION");
            int nextID = (int)ExecuteScalar(sqlQuery) + 1;

            if (nextID != 1)
            {
                sqlQuery = String.Format(@"SELECT MAX(ID) FROM R_SECTION");
                nextID = (int)ExecuteScalar(sqlQuery) + 1;
            }

            return nextID;
        }


        public void Update(Section section)
        {
            string sqlQuery = string.Format(@"UPDATE R_SECTION SET Name = N'{0}', Description = N'{1}' WHERE ID = {2}",
                                            section.Name, section.Description, section.ID);
            ExecuteNonQuery(sqlQuery);
        }

        public void Delete(Section section)
        {
            string sqlQuery = string.Format(@"DELETE FROM R_SECTION WHERE ID = {0}", section.ID);
            ExecuteNonQuery(sqlQuery);
        }

        public ArrayList GetSectionList()
        {
            Connect();

            string sqlQuery = "SELECT * FROM R_SECTION";
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        protected override object GetDataFromDataRow(DataTable dt, int i)
        {
            Section section = new Section();

            section.ID = Int16.Parse(dt.Rows[i]["ID"].ToString());
            section.Name = dt.Rows[i]["Name"].ToString();
            section.Description = dt.Rows[i]["Description"].ToString();

            return (object)section;
        }

        public ArrayList GetSectionListByName(string p)
        {
            Connect();

            string sqlQuery = String.Format(@"SELECT * FROM R_SECTION WHERE Name LIKE '%{0}%'", p);
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }
    }
}
