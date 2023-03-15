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
        public override bool Equals(object obj)
        {
            Category category = obj as Category;
            if (category is null) return false;
            return Name == category.Name;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
