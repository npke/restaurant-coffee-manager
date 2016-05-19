using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataTransferObject;

namespace DataAccessLayer 
{
    public class ItemMenuDao : DataProvider
    {
        public ItemMenuDao()
        {

        }

        public void DeleteByItemID(ItemMenu itemMenu)
        {
            string sqlQuery = string.Format(@"DELETE FROM ITEM_MENU WHERE ItemID = {0}", itemMenu.ItemID);
            ExecuteNonQuery(sqlQuery);
        }

        public void DeleteByMenuID(ItemMenu itemMenu)
        {
            string sqlQuery = string.Format(@"DELETE FROM ITEM_MENU WHERE MenuID = {0}", itemMenu.MenuID);
            ExecuteNonQuery(sqlQuery);
        }

        public void Insert(ItemMenu itemMenu)
        {
            string sqlQuery = String.Format(@"INSERT INTO ITEM_MENU VALUES({0}, {1})",
                                                itemMenu.ItemID, itemMenu.MenuID);
            ExecuteNonQuery(sqlQuery);
        }

        public void Update(ItemMenu itemMenu)
        {
            string sqlQuery = String.Format(@"UPDATE ITEM_MENU SET ItemID = {0}, MenuID = {1} WHERE ItemID = {2} AND MenuID = {3}",
                                                itemMenu.ItemID, itemMenu.MenuID, itemMenu.ItemID, itemMenu.OldMenuID);
            ExecuteNonQuery(sqlQuery);
        }
    }
}
