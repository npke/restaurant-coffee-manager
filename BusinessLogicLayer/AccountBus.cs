using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Data.SqlClient;
using DataTransferObject;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class AccountBus
    {
        private static AccountDao accountDao = new AccountDao();

        // Phương thức đăng nhập
        // Trả về một đối tượng Account nếu đăng nhập thành công
        // Trả về null nếu đăng nhập thất bại
        public static Account Login(Account account)
        {
            return accountDao.Login(account);
        }

        // Phương thức mã hóa mật khẩu người dùng theo mã hash MD5
        public static string GetMD5Hash(string sourceString)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sourceString));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }



        public static System.Collections.ArrayList GetAccountList()
        {
            return accountDao.GetAccountList();
        }

        public static void Insert(Account account)
        {
            accountDao.Insert(account);
        }

        public static int GetNextID()
        {
            return accountDao.GetNextID();
        }

        public static void Update(Account account)
        {
            accountDao.Update(account);
        }

        public static void Delete(Account account)
        {
            accountDao.Delete(account);
        }
    }
}
