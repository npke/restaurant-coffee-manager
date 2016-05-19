using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using DataTransferObject;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public class MenuDao : DataProvider
    {
        public ArrayList GetMenuList()
        {
            Connect();

            string sqlQuery = "SELECT * FROM R_MENU";
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        protected override object GetDataFromDataRow(DataTable dt, int i)
        {
            Menu menu = new Menu();

            menu.ID = Int16.Parse(dt.Rows[i]["ID"].ToString());
            menu.Name = dt.Rows[i]["Name"].ToString();
            menu.Description = dt.Rows[i]["Description"].ToString();

            return (object)menu;
        }

        public int GetNextID()
        {
            string sqlQuery = String.Format(@"SELECT COUNT(ID) FROM R_MENU");
            int nextID = (int)ExecuteScalar(sqlQuery) + 1;

            if (nextID != 1)
            {
                sqlQuery = String.Format(@"SELECT MAX(ID) FROM R_MENU");
                nextID = (int)ExecuteScalar(sqlQuery) + 1;
            }

            return nextID;
        }

        public void Insert(Menu menu)
        {
            string sqlQuery = String.Format(@"INSERT INTO R_MENU VALUES({0}, N'{1}', N'{2}')",
                                                    menu.ID, menu.Name, menu.Description);
            ExecuteNonQuery(sqlQuery);
        }

        public void Update(Menu menu)
        {
            string sqlQuery = string.Format(@"UPDATE R_MENU SET Name = N'{0}', Description = N'{1}' WHERE ID = {2}",
                                                  menu.Name, menu.Description, menu.ID);
            ExecuteNonQuery(sqlQuery);
        }

        public void Delete(Menu menu)
        {
            string sqlQuery = string.Format(@"DELETE FROM R_MENU WHERE ID = {0}", menu.ID);
            ExecuteNonQuery(sqlQuery);
        }

        public ArrayList GetMenuByName(string nameToLookUp)
        {
            Connect();

            string sqlQuery = String.Format(@"SELECT * FROM R_MENU WHERE Name LIKE '%{0}%'", nameToLookUp);
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        public int GetMinMenuID()
        {
            string sqlQuery = String.Format(@"SELECT MIN(ID) FROM R_MENU");
            int nextID = (int)ExecuteScalar(sqlQuery);

            return nextID;
        }
    }
}
