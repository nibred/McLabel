using McLabel.Commands;
using McLabel.Models;
using McLabel.Services.Interfaces;
using McLabel.Utils.Extensions;
using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using System.Xaml;

namespace McLabel.ViewModels
{
    internal class MainEditorViewModel : ViewModelBase
    {
        #region private fields
        private readonly IFileService _xmlService;
        private ObservableCollection<Item> _items;
        private ObservableCollection<Category> _categories;
        private Item _selectedItem;
        private Category _selectedCategory;

        private string[] _allProperties = new string[]
        {
            nameof(IsItemSelected),
            nameof(DateTimeNow),
            nameof(ExpiredDays1),
            nameof(ExpiredHours1),
            nameof(ExpiredMinutes1),
            nameof(ExpiredDays2),
            nameof(ExpiredHours2),
            nameof(ExpiredMinutes2),
            nameof(DateTimeReady1),
            nameof(DateTimeReady2)
        };
        private string[] _dateTimeProperties = new string[]
        {
            nameof(DateTimeNow),
            nameof(DateTimeReady1),
            nameof(DateTimeReady2)
        };
        #endregion

        #region observable collections
        public ObservableCollection<Item> Items
        {
            get
            {
                if (SelectedCategory is null)
                {
                    AddColorsInItems();
                    return _items;
                }
                return _items.Where(x => x.Category == SelectedCategory.Name).Select(x => { x.Color = SelectedCategory.Color; return x; }).ToObservableCollection();
            }
        }
        public ObservableCollection<Category> Categories => _categories;
        #endregion

        #region public fields
        public bool IsItemSelected => SelectedItem != null;
        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
                Notify(ref _allProperties);
            }
        }
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                Set(ref _selectedCategory, value);
                OnPropertyChanged(nameof(Items));
            }
        }
        public DateTime DateTimeNow => DateTime.Now;

        public string ExpiredDays1
        {
            get => IsItemSelected ? SelectedItem.Exp1Days : "0";
            set
            {
                SelectedItem.Exp1Days = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredHours1
        {
            get => IsItemSelected ? SelectedItem.Exp1Hours : "0";
            set
            {
                SelectedItem.Exp1Hours = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredMinutes1
        {
            get => IsItemSelected ? SelectedItem.Exp1Minutes : "0";
            set
            {
                SelectedItem.Exp1Minutes = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredDays2
        {
            get => IsItemSelected ? SelectedItem.Exp2Days : "0";
            set
            {
                SelectedItem.Exp2Days = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredHours2
        {
            get => IsItemSelected ? SelectedItem.Exp2Hours : "0";
            set
            {
                SelectedItem.Exp2Hours = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredMinutes2
        {
            get => IsItemSelected ? SelectedItem.Exp2Minutes : "0";
            set
            {
                SelectedItem.Exp2Minutes = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public DateTime DateTimeReady1
        {
            get
            {
                if (IsItemSelected)
                    return RecalculateDateTime(SelectedItem.Exp1Days, SelectedItem.Exp1Hours, SelectedItem.Exp1Minutes);
                return DateTimeNow;
            }
        }
        public DateTime DateTimeReady2
        {
            get
            {
                if (IsItemSelected)
                    return RecalculateDateTime(SelectedItem.Exp2Days, SelectedItem.Exp2Hours, SelectedItem.Exp2Minutes);
                return DateTimeNow;
            }
        }
        #endregion

        public void AddItems(IEnumerable<Item> items) //TODO: one generic method (items/categories)?
        {
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }
        public void AddCategories(IEnumerable<Category> categories)
        {
            foreach (var category in categories)
            {
                _categories.Add(category);
            }
        }

        #region commands
        public ICommand AddNewItemCommand => new RelayCommand(o =>
        {
            _items.Add(new Item()
            {
                Category = SelectedCategory.Name,
                Name = "New Item",
                Color = SelectedCategory.Color,
                Format = "",
                Exp1Days = "0",
                Exp1Hours = "0",
                Exp1Message = "",
                Exp1Minutes = "0",
                Exp2Days = "0",
                Exp2Hours = "0",
                Exp2Message = "",
                Exp2Minutes = "0",
                Line1st = "",
                Line2nd = "Item"
            });
            OnPropertyChanged(nameof(Items));
        }, o => SelectedCategory != null);

        public ICommand AddNewCategoryCommand => new RelayCommand(o =>
        {
            Categories.Add(new Category()
            {
                Name = "New category",
                Color = "#aabbcc",
                Printer = "",
                PrintTemplate = ""
            });
        });
        #endregion
        public MainEditorViewModel(IFileService xmlService)
        {
            _xmlService = xmlService;
            _items = new ObservableCollection<Item>();
            _categories = new ObservableCollection<Category>();
        }
        private DateTime RecalculateDateTime(string days, string hours, string minutes)
        {
            if (!Double.TryParse(days, out var d) ||
                !Double.TryParse(hours, out var h) ||
                !Double.TryParse(minutes, out var m))
                return DateTimeNow;
            try
            {
                DateTime newDateTime = DateTimeNow.AddDays(d).AddHours(h).AddMinutes(m);
                return newDateTime;
            }
            catch
            {
                return DateTimeNow;
            }
        }
        public MainEditorViewModel() : base() // design time constructor
        {
            _items = new ObservableCollection<Item>();
            for (int i = 0; i < 40; i++)
            {
                Items.Add(new Item
                {
                    Name = $"Item {i + 1}",
                    Category = "chleb",
                    Exp1Days = "0",
                    Exp1Hours = "4",
                    Exp1Message = "gotowe",
                    Exp1Minutes = "0",
                    Exp2Message = "koniec",
                    Exp2Days = "2",
                    Line1st = $"Item {i}",
                    Line2nd = "Line2nd",
                    Format = ""
                });
            }
            _categories = new ObservableCollection<Category>();
            for (int i = 0; i < 7; i++)
            {
                Categories.Add(new Category
                {
                    Color = $"#{RandomColorGenerator()}",
                    Name = $"Category {i + 1}",
                    Printer = "",
                    PrintTemplate = ""
                });
            }
        }

        private void AddColorsInItems()
        {
            if (!_items.IsEmpty() && !_categories.IsEmpty())
            {
                foreach ((Item item, Category category) in _items
                    .SelectMany(item => _categories
                    .Where(category => item.Category == category.Name)
                    .Select(category => (item, category))))
                {
                    item.Color = category.Color;
                }
            }
        }
        private string RandomColorGenerator() // only for design time
        {
            Random random = new Random();
            string chars = "123567890abcdef";
            string result = string.Empty;
            for (int i = 0; i < 6; i++)
                result += chars[random.Next(chars.Length)];
            return result;
        }
    }
}
