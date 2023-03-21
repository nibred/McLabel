using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Models
{
    internal class Category
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string PrintTemplate { get; set; }
        public string Printer { get; set; }
        public List<Item> Items { get; set; }
    }
}
