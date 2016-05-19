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
    public class InvoiceBus
    {
        private static InvoiceDao invoiceDao = new InvoiceDao();

        public static Invoice GetLastestInvoice(int tableID)
        {
            return invoiceDao.GetLastestInvoice(tableID);
        }

        public static void Insert(Invoice invoice)
        {
            invoiceDao.Insert(invoice);
        }

        public static void Update(Invoice invoice)
        {
            invoiceDao.Update(invoice);
        }

        public static void DeleteByTable(Invoice invoice)
        {
            // Xóa các order của bàn này
            ArrayList invoiceList = InvoiceBus.GetInvoiceByTable(invoice.TableID);

            foreach (Invoice invc in invoiceList)
            {
                ArrayList orderList = OrderBus.GetOrderByInvoice(invc);
                foreach (Order order in orderList)
                {
                    OrderBus.Delete(order);
                }
            }

            invoiceDao.DeleteByTable(invoice);
        }

        private static ArrayList GetInvoiceByTable(int p)
        {
            return invoiceDao.GetInvoiceByTable(p);
        }

        public static int GetNextID()
        {
            return invoiceDao.GetNextID();
        }

        public static ArrayList GetInvoiceList(Invoice invoice)
        {
            return invoiceDao.GetInvoiceList(invoice);
        }

        public static DataTable GetInvoice(Invoice invoice)
        {
            return invoiceDao.GetInvoice(invoice);
        }

        public static void Delete(Invoice invoice)
        {
            ArrayList orderList = OrderBus.GetOrderByInvoice(invoice);
            foreach (Order order in orderList)
            {
                OrderBus.Delete(order);
            }
            invoiceDao.Delete(invoice);
        }

        public static void UpdateTotal(Invoice invoice)
        {
            invoiceDao.UpdateTotal(invoice);
        }
    }
}
