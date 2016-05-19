using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransferObject
{
    public class Account
    {
        public int ID { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public string Fullname { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public bool IsAdmin { get; set; }

        public Account()
        {

        }

        public Account(string userName, string passWord)
        {
             Username = userName;
             Password = passWord;
        }

        public Account(string userName, string passWord, string fullName,
                            DateTime birthDay, string address, string phone, bool isAdmin)
        {
             Username = userName;
             Password = passWord;
             Fullname = fullName;
             Birthday = birthDay;
             Address = address;
             Phone = phone;
             IsAdmin = isAdmin;
        }
    }
}
