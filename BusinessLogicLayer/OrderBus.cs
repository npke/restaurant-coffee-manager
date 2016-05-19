using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using DataTransferObject;
using System.Data;
using System.Collections;

namespace BusinessLogicLayer
{
    public class OrderBus
    {
        private static OrderDao orderDao = new OrderDao();
        private static ItemDao itemDao = new ItemDao();

        public static DataTable GetOrder(int invoiceID)
        {
            DataTable itemOrder = orderDao.GetOrder(invoiceID);

            DataTable orderList = new DataTable();
            orderList.Columns.Add("ID");
            orderList.Columns.Add("TenMon");
            orderList.Columns.Add("SoLuong");
            orderList.Columns.Add("DonViTinh");
            orderList.Columns.Add("DonGia");
            orderList.Columns.Add("ThanhTien");

            for (int i = 0; i < itemOrder.Rows.Count; i++)
            {
                Item item = itemDao.GetTable(int.Parse(itemOrder.Rows[i]["ItemID"].ToString()));

                DataRow newRow = orderList.NewRow();
                newRow["ID"] = item.ID;
                newRow["TenMon"] = item.Name;
                newRow["SoLuong"] = itemOrder.Rows[i][1];
                newRow["DonViTinh"] = item.Unit;
                newRow["DonGia"] = item.Price;
                newRow["ThanhTien"] = int.Parse(itemOrder.Rows[i][1].ToString()) * item.Price;
                
                orderList.Rows.Add(newRow);
            }

            return orderList;
        }

        public static void Insert(Order order)
        {
            orderDao.Insert(order);
        }

        public static void Delete(Order order)
        {
            orderDao.Delete(order);
        }

        public static int GetNextID()
        {
            return orderDao.GetNextID();
        }

        internal static ArrayList GetOrderByInvoice(Invoice invoice)
        {
            return orderDao.getGetOrderByInvoice(invoice);
        }

        internal static void DeleteByItem(Item item)
        {
            orderDao.DeleteByItem(item);
        }
    }
}
