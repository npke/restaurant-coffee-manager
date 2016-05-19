using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DataTransferObject;
using System.Data;

namespace DataAccessLayer
{
    public class ItemDao : DataProvider
    {
        public ArrayList GetItemListInMenu(int menuID)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT ID, Name, Price, Unit, Description 
                                              FROM R_ITEM, ITEM_MENU WHERE ID = ItemID AND MenuID = {0}", menuID);
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        public Item GetTable(int itemID)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT * FROM R_ITEM WHERE ID = {0}", itemID);
            SqlDataReader dataReader = ExecuteQuery(sqlQuery);

            Item item = null;
            if (dataReader.Read())
            {
                item = new Item();
                item.ID = int.Parse(dataReader["ID"].ToString());
                item.Name = dataReader["Name"].ToString();
                item.Description = dataReader["Description"].ToString();
                item.Unit = dataReader["Unit"].ToString();
                item.Price = int.Parse(dataReader["Price"].ToString());
            }

            Disconnect();

            return item;
        }

        public DataTable GetItemListInMenuDT(int menuID)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT ID, Name, Price, Unit, Description
                                              FROM R_ITEM, ITEM_MENU WHERE ID = ItemID AND MenuID = {0}", menuID);
            DataTable dataTable = ExecuteQueryDT(sqlQuery);

            Disconnect();

            return dataTable;
        }

        protected override object GetDataFromDataRow(DataTable dt, int i)
        {
            Item item = new Item();

            item.ID = Int16.Parse(dt.Rows[i]["ID"].ToString());
            item.Name = dt.Rows[i]["Name"].ToString();
            item.Price = Int32.Parse(dt.Rows[i]["Price"].ToString());
            item.Unit = dt.Rows[i]["Unit"].ToString();
            item.Description = dt.Rows[i]["Description"].ToString();

            return (object)item;
        }

        public ArrayList GetItemByName(string nameToLookUp, int menuID)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT ID, Name, Price, Unit, Description 
                                              FROM R_ITEM, ITEM_MENU WHERE ID = ItemID AND Name LIKE '%{0}%' AND MenuID = {1}", 
                                                nameToLookUp, menuID);
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        public void Insert(Item item)
        {
            string sqlQuery = String.Format(@"INSERT INTO R_ITEM VALUES({0}, N'{1}', {2}, N'{3}', N'{4}')",
                                                    item.ID, item.Name, item.Price, item.Unit, item.Description);
            ExecuteNonQuery(sqlQuery);
        }

        public int GetNextID()
        {
            string sqlQuery = String.Format(@"SELECT COUNT(ID) FROM R_ITEM");
            int nextID = (int)ExecuteScalar(sqlQuery) + 1;

            if (nextID != 1)
            {
                sqlQuery = String.Format(@"SELECT MAX(ID) FROM R_ITEM");
                nextID = (int)ExecuteScalar(sqlQuery) + 1;
            }

            return nextID;
        }

        public void Delete(Item item)
        {
            string sqlQuery = string.Format(@"DELETE FROM R_ITEM WHERE ID = {0}", item.ID);
            ExecuteNonQuery(sqlQuery);
        }

        public void Update(Item item)
        {
            string sqlQuery = String.Format(@"UPDATE R_ITEM SET Name = N'{0}', Price = {1}, Unit = N'{2}', Description = N'{3}' WHERE ID = {4}",
                                                item.Name, item.Price, item.Unit, item.Description, item.ID);
            ExecuteNonQuery(sqlQuery);
        }
    }
}
