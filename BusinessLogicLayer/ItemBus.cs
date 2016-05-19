using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataTransferObject;
using DataAccessLayer;
using System.Collections;
using System.Data;

namespace BusinessLogicLayer
{
    public class ItemBus
    {
        private static ItemDao itemDao = new ItemDao();

        public static DataTable GetItemListInMenu(int menuID)
        {
            return itemDao.GetItemListInMenuDT(menuID);
        }

        public static ArrayList GetItemByName(string nameToLookUp, int menuID)
        {
            return itemDao.GetItemByName(nameToLookUp, menuID);
        }

        public static void Insert(Item item)
        {
            itemDao.Insert(item);
        }

        public static int GetNextID()
        {
            return itemDao.GetNextID();
        }

        public static void Delete(Item item)
        {
            OrderBus.DeleteByItem(item);
            itemDao.Delete(item);
        }

        public static void Update(Item item)
        {
            itemDao.Update(item);
        }
    }
}
