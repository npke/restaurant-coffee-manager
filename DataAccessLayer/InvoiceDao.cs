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
    public class InvoiceDao : DataProvider
    {
        public Invoice GetLastestInvoice(int tableID)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT * FROM R_INVOICE WHERE TableID = {0} ORDER BY DateCreated DESC", tableID);
            SqlDataReader dataReader = ExecuteQuery(sqlQuery);

            Invoice invoice = null;
            if (dataReader.Read())
            {
                invoice = new Invoice();
                invoice.ID = int.Parse(dataReader["ID"].ToString());
                invoice.DateCreated = Convert.ToDateTime(dataReader["DateCreated"].ToString());
                invoice.TableID = int.Parse(dataReader["TableID"].ToString());
                invoice.Total = int.Parse(dataReader["Total"].ToString());
                invoice.Cash = int.Parse(dataReader["Cash"].ToString());
                invoice.Charge = int.Parse(dataReader["Charge"].ToString());
                invoice.Paid = (bool)dataReader["Paid"];
            }

            Disconnect();

            return invoice;
        }

        public int GetNextID()
        {
            string sqlQuery = String.Format(@"SELECT COUNT(ID) FROM R_INVOICE");
            int nextID = (int)ExecuteScalar(sqlQuery) + 1;

            if (nextID != 1)
            {
                sqlQuery = String.Format(@"SELECT MAX(ID) FROM R_INVOICE");
                nextID = (int)ExecuteScalar(sqlQuery) + 1;
            }

            return nextID;
        }

        public void Insert(Invoice invoice)
        {
            string sqlQuery = string.Format(@"INSERT INTO R_INVOICE VALUES({0}, '{1}', {2}, {3}, {4}, {5}, {6}, {7})",
                invoice.ID, invoice.DateCreated.ToString("MM/dd/yyyy HH:mm:ss"), invoice.TableID, (invoice.Paid == true) ? 1 : 0, invoice.Total, invoice.Cash, invoice.Charge, invoice.SectionID);
            ExecuteNonQuery(sqlQuery);
        }

        public void Update(Invoice invoice)
        {
            string sqlQuery = string.Format(@"UPDATE R_INVOICE SET Paid = {0}, Total = {1}, Cash = {2}, Charge = {3} WHERE ID = {4}",
                (invoice.Paid == true) ? 1 : 0, invoice.Total, invoice.Cash, invoice.Charge, invoice.ID);
            ExecuteNonQuery(sqlQuery);
        }

        public void DeleteByTable(Invoice invoice)
        {
            string sqlQuery = string.Format(@"DELETE FROM R_INVOICE WHERE TableID = {0}", invoice.TableID);
            ExecuteNonQuery(sqlQuery);
        }

        public System.Collections.ArrayList GetInvoiceByTable(int p)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT * FROM R_INVOICE WHERE TableID = {0}", p);
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        protected override object GetDataFromDataRow(DataTable dt, int i)
        {
            Invoice invoice = new Invoice();

            invoice.ID = Int16.Parse(dt.Rows[i]["ID"].ToString());
            invoice.DateCreated = Convert.ToDateTime(dt.Rows[i]["DateCreated"].ToString());
            invoice.TableID = Int16.Parse(dt.Rows[i]["TableID"].ToString());
            invoice.Total = int.Parse(dt.Rows[i]["Total"].ToString());
            invoice.Cash = int.Parse(dt.Rows[i]["Cash"].ToString());
            invoice.Charge = int.Parse(dt.Rows[i]["Charge"].ToString());
            invoice.Paid = (bool)dt.Rows[i]["Paid"];
            invoice.SectionID = Int16.Parse(dt.Rows[i]["SectionID"].ToString());

            return (object)invoice;
        }

        public ArrayList GetInvoiceList(Invoice invoice)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT R_SECTION.Name, R_TABLE.Name, R_INVOICE.* FROM R_INVOICE, R_SECTION, R_TABLE WHERE R_INVOICE.TableID = R_TABLE.ID AND 
                                            R_INVOICE.SectionID = R_SECTION.ID AND ");

            if (invoice.TableID != 0)
                sqlQuery += string.Format(@"TableID = {0} AND ", invoice.TableID);

            if (invoice.SectionID != 0)
                sqlQuery += string.Format(@"R_INVOICE.SectionID = {0} AND ", invoice.SectionID);

            DateTime dt = Convert.ToDateTime("01/01/0001 00:00:00");
            if (invoice.DateCreated != dt)
                sqlQuery += string.Format(@"DateCreated = '{0}' AND DateCreated <= ", invoice.DateCreated.ToString("MM/dd/yyyy HH:mm:ss"));

            sqlQuery += string.Format(@"Total >= {0}", invoice.Total);

            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        public DataTable GetInvoice(Invoice invoice)
        {
            Connect();

            string sqlQuery = string.Format(@"SELECT R_SECTION.Name, R_TABLE.Name, R_INVOICE.* FROM R_INVOICE, R_SECTION, R_TABLE WHERE R_INVOICE.TableID = R_TABLE.ID AND 
                                            R_INVOICE.SectionID = R_SECTION.ID AND ");

            if (invoice.TableID != 0)
                sqlQuery += string.Format(@"TableID = {0} AND ", invoice.TableID);

            if (invoice.SectionID != 0)
                sqlQuery += string.Format(@"R_INVOICE.SectionID = {0} AND ", invoice.SectionID);

            DateTime dt = Convert.ToDateTime("01/01/0001 00:00:00");
            DateTime dateBefore = invoice.DateCreated.AddDays(0);
            DateTime dateAfter = invoice.DateCreated.AddDays(1);

            if (invoice.DateCreated != dt)
                sqlQuery += string.Format(@"DateCreated >= '{0}' AND DateCreated < '{1}' AND ", dateBefore.ToString("MM/dd/yyyy"), dateAfter.ToString("MM/dd/yyyy"));

            sqlQuery += string.Format(@"Total >= {0}", invoice.Total);

            DataTable dataTable = ExecuteQueryDT(sqlQuery);

            Disconnect();

            return dataTable;
        }

        public void Delete(Invoice invoice)
        {
            string sqlQuery = string.Format(@"DELETE FROM R_INVOICE WHERE ID = {0}", invoice.ID);
            ExecuteNonQuery(sqlQuery);
        }

        public void UpdateTotal(Invoice invoice)
        {
            string sqlQuery = string.Format(@"UPDATE R_INVOICE SET Total = {0} WHERE ID = {1}",
                invoice.Total, invoice.ID);
            ExecuteNonQuery(sqlQuery);
        }
    }
}
