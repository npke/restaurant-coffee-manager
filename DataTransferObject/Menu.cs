using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransferObject
{
    public class Menu
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Menu()
        {

        }

        public Menu(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}
