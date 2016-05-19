using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransferObject
{
    public class Table
    {
        public int ID { get; set; }

        public int SectionID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        public Table()
        {
            Status = false;
        }

        public Table(int id, int sectionID, string description, bool status)
        {
            ID = id;
            SectionID = sectionID;
            Description = description;
            Status = status;
        }
    }
}
