using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransferObject
{
    public class Invoice
    {
        public int ID { get; set; }

        public int TableID { get; set; }

        public int SectionID { get; set; }

        public DateTime DateCreated { get; set; }

        public int Total { get; set; }

        public int Cash { get; set; }

        public int Charge { get; set; }

        public bool Paid { get; set; }

        public Invoice()
        {
            Paid = false;
            Total = 0;
            Cash = 0;
            Charge = 0;
        }

        public Invoice(int id, int  tableID, DateTime dateCreated)
        {
            ID = id;
            TableID = tableID;
            DateCreated = dateCreated;
            Paid = false;
            Total = 0;
            Cash = 0;
            Charge = 0;
        }
    }
}
