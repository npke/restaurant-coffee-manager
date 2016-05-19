using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataTransferObject;
using System.Collections;

namespace DataAccessLayer
{
    public class AccountDao : DataProvider
    {
        public AccountDao()
        {
            
        }

        public Account Login(Account account)
        {
            Account loginAccount = null;

            try
            {
                Connect();
                string sqlQuery = string.Format(@"SELECT * FROM R_ACCOUNT WHERE Username = '{0}' AND Password = '{1}'",
                                                                                        account.Username, account.Password);
                dataReader = ExecuteQuery(sqlQuery);
                loginAccount = (Account)GetObjectFromDataReader();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Disconnect();
            }

            return loginAccount;
        }

        protected override object GetObjectFromDataReader()
        {
            Account account = null;

            if (dataReader.Read())
            {
                account = new Account();

                account.Username = dataReader[1].ToString();
                account.Password = dataReader[2].ToString();
                account.Fullname = dataReader[3].ToString();
                account.Birthday = Convert.ToDateTime(dataReader[4].ToString());
                account.Address = dataReader[5].ToString();
                account.Phone = dataReader[6].ToString();
                account.IsAdmin = (bool)dataReader[7];
            }

            return account;
        }

        public System.Collections.ArrayList GetAccountList()
        {
            Connect();

            string sqlQuery = "SELECT * FROM R_ACCOUNT";
            dataAdapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            ArrayList arr = ConvertDataSetToArrayList(dataSet);

            Disconnect();

            return arr;
        }

        protected override object GetDataFromDataRow(DataTable dt, int i)
        {
            Account account = new Account();

            account.ID = int.Parse(dt.Rows[i]["ID"].ToString());
            account.Username = dt.Rows[i]["Username"].ToString();
            account.Fullname = dt.Rows[i]["Fullname"].ToString();
            account.Address = dt.Rows[i]["Address"].ToString();
            account.Phone = dt.Rows[i]["Phone"].ToString();
            account.Birthday = Convert.ToDateTime(dt.Rows[i]["Birthday"].ToString());
            account.IsAdmin = (bool)dt.Rows[i]["IsAdmin"];

            return (object)account;
        }

        public void Insert(Account account)
        {
            string sqlQuery = string.Format(@"INSERT INTO R_ACCOUNT VALUES({0}, '{1}', '{2}', N'{3}', '{4}', N'{5}', '{6}', {7})",
                                                account.ID, account.Username, account.Password, account.Fullname, account.Birthday.ToString("MM/dd/yyyy"),
                                                account.Address, account.Phone, (account.IsAdmin == true) ? 1 : 0);
            ExecuteNonQuery(sqlQuery);
        }

        public int GetNextID()
        {
            string sqlQuery = String.Format(@"SELECT COUNT(ID) FROM R_ACCOUNT");
            int nextID = (int)ExecuteScalar(sqlQuery) + 1;

            if (nextID != 1)
            {
                sqlQuery = String.Format(@"SELECT MAX(ID) FROM R_ACCOUNT");
                nextID = (int)ExecuteScalar(sqlQuery) + 1;
            }

            return nextID;
        }

        public void Update(Account account)
        {
            string sqlQuery = string.Format(@"UPDATE R_ACCOUNT SET ");

            // Nếu reset tài khoản
            if (account.Password == "#reset")
                sqlQuery += "Password = 'e10adc3949ba59abbe56e057f20f883e', "; // Hash MD5 của 123456
            else if (account.Password != "")
                sqlQuery += string.Format(@"Password = '{0}', ", account.Password);

            sqlQuery += string.Format(@"Fullname = N'{0}', Birthday = '{1}', Address = N'{2}', Phone = '{3}', IsAdmin = {4} WHERE Username = '{5}'",
                                                account.Fullname, account.Birthday.ToString("MM/dd/yyyy"), account.Address, account.Phone, (account.IsAdmin == true) ? 1 : 0, account.Username);
            ExecuteNonQuery(sqlQuery);
        }

        public void Delete(Account account)
        {
            string sqlQuery = string.Format(@"DELETE FROM R_ACCOUNT WHERE ID = {0}", account.ID);
            ExecuteNonQuery(sqlQuery);
        }
    }
}
