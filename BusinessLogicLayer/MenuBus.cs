 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using DataTransferObject;
using System.Collections;

namespace BusinessLogicLayer
{
    public class MenuBus
    {
        private static MenuDao menuDao = new MenuDao();

        public static ArrayList GetMenuList()
        {
            return menuDao.GetMenuList();
        }

        public static void Insert(Menu menu)
        {
            menuDao.Insert(menu);
        }

        public static void Update(Menu menu)
        {
            menuDao.Update(menu);
        }

        public static void Delete(Menu menu)
        {
            menuDao.Delete(menu);
        }

        public static ArrayList GetMenuByName(string nameToLookUp)
        {
            return menuDao.GetMenuByName(nameToLookUp);
        }

        public static int GetNextID()
        {
            return menuDao.GetNextID();
        }
    }
}
