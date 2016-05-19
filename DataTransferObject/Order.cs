using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransferObject
{
    public class Order
    {
        public int ID { get; set; }

        public int InvoiceID { get; set; }

        public int ItemID { get; set; }

        public int Quantity { get; set; }

        public Order()
        {

        }

        public Order(int id, int invoiceID, int itemID, int quantity)
        {
            ID = id;
            InvoiceID = invoiceID;
            ItemID = itemID;
            Quantity = quantity;
        }
    }
}
