using McLabel.Models.Interfaces;
using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Models
{
    internal class Item : NotifyBase, IItem
    {
        private string _name;
        private string _category;
        private string _exp1Days;
        private string _exp1Hours;
        private string _exp1Minutes;
        private string _exp1Message;
        private string _exp2Days;
        private string _exp2Hours;
        private string _exp2Minutes;
        private string _exp2Message;
        private string _format;
        private string _line1st;
        private string _line2nd;
        private string _color;
        public string Name { get => _name; set => Set(ref _name, value); }
        public string Category { get => _category; set => Set(ref _category, value); }
        public string Exp1Days { get => _exp1Days; set => Set(ref _exp1Days, value); }
        public string Exp1Hours { get => _exp1Hours; set => Set(ref _exp1Hours, value); }
        public string Exp1Minutes { get => _exp1Minutes; set => Set(ref _exp1Minutes, value); }
        public string Exp1Message { get => _exp1Message; set => Set(ref _exp1Message, value); }
        public string Exp2Days { get => _exp2Days; set => Set(ref _exp2Days, value); }
        public string Exp2Hours { get => _exp2Hours; set => Set(ref _exp2Hours, value); }
        public string Exp2Minutes { get => _exp2Minutes; set => Set(ref _exp2Minutes, value); }
        public string Exp2Message { get => _exp2Message; set => Set(ref _exp2Message, value); }
        public string Format { get => _format; set => Set(ref _format, value); }
        public string Line1st { get => _line1st; set => Set(ref _line1st, value); }
        public string Line2nd { get => _line2nd; set => Set(ref _line2nd, value); }
        public string Color { get => _color; set => Set(ref _color, value); }
    }
}
