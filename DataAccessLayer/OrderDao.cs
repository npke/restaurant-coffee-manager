using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataTransferObject;
using System.Collections;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class OrderDao : DataProvider
    {
        public DataTable GetOrder(int invoiceID)
        {
            Connect();
            
            string sqlQuery = string.Format(@"SELECT ItemID, SUM(Quantity) FROM R_ORDER WHERE InvoiceID = {0} GROUP BY ItemID", invoiceID);
            DataTable dataTable = ExecuteQueryDT(sqlQuery);

            Disconnect();

            return dataTable;
        }

        public int GetNextID()
        {
            string sqlQuery = String.Format(@"SELECT COUNT(ID) FROM R_ORDER");
            int nextID = (int)ExecuteScalar(sqlQuery) + 1;

            if (nextID != 1)
            {
                sqlQuery = String.Format(@"SELECT MAX(ID) FROM R_ORDER");
                nextID = (int)ExecuteScalar(sqlQuery) + 1;
            }

            return nextID;
        }

        public void Insert(Order order)
        {
            string sqlQuery = string.Format(@"INSERT INTO R_ORDER VALUES({0}, {1}, {2}, {3})",
                                            order.ID, order.ItemID, order.Quantity, order.InvoiceID);
            ExecuteNonQuery(sqlQuery);
        }

        public void Delete(Order order)
        {
            string sqlQuery = String.Format(@"DELETE FROM R_ORDER WHERE ItemID = {0} AND InvoiceID = {1}",
                                                    order.ItemID, order.InvoiceID);
            ExecuteNonQuery(sqlQuery);
        }

        public ArrayList getGetOrderByInvoice(Invoice invoice)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT * FROM R_ORDER WHERE InvoiceID = {0}", invoice.ID);
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        protected override object GetDataFromDataRow(DataTable dt, int i)
        {
            Order order = new Order();

            order.ID = Int16.Parse(dt.Rows[i]["ID"].ToString());
            order.InvoiceID = Int16.Parse(dt.Rows[i]["InvoiceID"].ToString());
            order.ItemID = Int16.Parse(dt.Rows[i]["ItemID"].ToString());
            order.Quantity = Int16.Parse(dt.Rows[i]["Quantity"].ToString());

            return (object)order;
        }

        public void DeleteByItem(Item item)
        {
            string sqlQuery = String.Format(@"DELETE FROM R_ORDER WHERE ItemID = {0}", item.ID);
            ExecuteNonQuery(sqlQuery);
        }
    }
}
