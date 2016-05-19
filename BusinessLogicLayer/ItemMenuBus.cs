using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataTransferObject;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class ItemMenuBus
    {
        private static ItemMenuDao itemMenuDao = new ItemMenuDao();

        public static void DeleteByItemID(ItemMenu itemMenu)
        {
           itemMenuDao.DeleteByItemID(itemMenu);
        }

        public static void DeleteByMenuID(ItemMenu itemMenu)
        {
           itemMenuDao.DeleteByMenuID(itemMenu);
        }

        public static void Insert(ItemMenu itemMenu)
        {
           itemMenuDao.Insert(itemMenu);
        }

        public static void Update(ItemMenu itemMenu)
        {
           itemMenuDao.Update(itemMenu);
        }
    }
}
