using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using DataTransferObject;
using System.Collections;

namespace BusinessLogicLayer
{
    public class TableBus
    {
        private static TableDao tableDao = new TableDao();

        // Phương thức lấy danh sách các bàn trong một khu theo ID khu
        public static ArrayList GetTableListInSection(int sectionID)
        {
            return tableDao.GetTableListInSection(sectionID);
        }

        // Phương thức tìm kiếm danh sách bàn theo tên
        public static ArrayList GetTableByName(string tableName, int sectionID)
        {
            return tableDao.GetTableByName(tableName, sectionID);
        }

        public static Table GetTable(int tableID)
        {
            return tableDao.GetTable(tableID);
        }

        public static void Update(Table table)
        {
            tableDao.Update(table);
        }

        public static void Delete(Table table)
        {
            tableDao.Delete(table) ;
        }

        public static void Insert(int sectionID, int quantity, string tableName, int from)
        {
            Table table = new Table();
            table.SectionID = sectionID;

            for (int i = from; i < quantity + from; i++)
            {
                table.ID = TableBus.GetNextID();
                table.Name = tableName + " " + i.ToString();
                table.Status = true;
                TableBus.Insert(table);
            }
        }

        private static int GetNextID()
        {
            return tableDao.GetNextID();
        }

        public static void Insert(Table table)
        {
            tableDao.Insert(table);
        }

        public static void DeleteBySectionID(Table table)
        {
            ArrayList deletedTable = TableBus.GetTableListInSection(table.SectionID);

            // Xóa các hóa đơn của bàn
            foreach (Table dTable in deletedTable)
            {
                Invoice invoice = new Invoice();
                invoice.TableID = dTable.ID;
                InvoiceBus.DeleteByTable(invoice);
            }

            // Xóa các bàn thuộc khu đó
            tableDao.DeleteBySectionID(table);
        }
    }
}
