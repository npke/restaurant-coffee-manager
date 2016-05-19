using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTransferObject
{
    public class Section
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Section()
        {

        }

        public Section(int id, string name, string description)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
        }
    }
}
