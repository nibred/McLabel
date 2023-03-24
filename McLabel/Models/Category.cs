using McLabel.Models.Interfaces;
using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Models
{
    internal class Category : NotifyBase, ICategory
    {
        private string _color;
        private string _name;
        private ICollection<IItem> _items;
        public string Name { get => _name; set => Set(ref _name, value); }
        public string Color { get => _color; set => Set(ref _color, value); }
        public ICollection<IItem> Items { get => _items; set => Set(ref _items, value); }
        public string PrintTemplate { get; set; }
        public string Printer { get; set; }
    }
}
