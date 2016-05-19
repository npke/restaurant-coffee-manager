using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransferObject
{
    public class Item
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Unit { get; set; }

        public string Description { get; set; }

        public Item()
        {

        }

        public Item(int id, string name, int price, string unit, string description)
        {
             ID = id;
             Name = name;
             Price = price;
             Unit = unit;
             Description = description;
        }
    }
}
