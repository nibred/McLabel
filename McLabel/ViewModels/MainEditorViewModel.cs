using McLabel.Commands;
using McLabel.Models;
using McLabel.Services.Interfaces;
using McLabel.Utils.Extensions;
using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        public ObservableCollection<Category> Categories { get => _categories; set => Set(ref _categories, value); }
        public ObservableCollection<Item> Items => SelectedCategory?.Items?
            .Select(i => 
            { 
                i.Color = SelectedCategory?.Color; 
                return i; 
            })
            .ToObservableCollection();
        #endregion

        #region properties
        public ushort FontSize => 16;
        public bool IsItemSelected => SelectedItem != null;
        public bool IsCategorySelected => SelectedCategory != null;
        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
                OnPropertyChanged(nameof(IsItemSelected));
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
            get => IsItemSelected ? SelectedItem.Exp1Days : string.Empty;
            set
            {
                SelectedItem.Exp1Days = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredHours1
        {
            get => IsItemSelected ? SelectedItem.Exp1Hours : string.Empty;
            set
            {
                SelectedItem.Exp1Hours = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredMinutes1
        {
            get => IsItemSelected ? SelectedItem.Exp1Minutes : string.Empty;
            set
            {
                SelectedItem.Exp1Minutes = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredDays2
        {
            get => IsItemSelected ? SelectedItem.Exp2Days : string.Empty;
            set
            {
                SelectedItem.Exp2Days = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredHours2
        {
            get => IsItemSelected ? SelectedItem.Exp2Hours : string.Empty;
            set
            {
                SelectedItem.Exp2Hours = value;
                Notify(ref _dateTimeProperties);
            }
        }
        public string ExpiredMinutes2
        {
            get => IsItemSelected ? SelectedItem.Exp2Minutes : string.Empty;
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

        #region commands
        public ICommand AddNewItemCommand => new RelayCommand(o =>
        {
            SelectedCategory.Items.Add(new Item()
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
                Color = $"{GenerateRandomColor()}",
                Printer = "",
                PrintTemplate = "",
                Items = new List<Item>()
            });
        });
        public ICommand RemoveElementCommand => new RelayCommand(o =>
        {
            if (o is Item)
            {
                SelectedCategory.Items.Remove(o as Item);
            }
            else
            {
                Categories.Remove(o as Category);
            }
            OnPropertyChanged(nameof(Items));
        });
        public ICommand SaveCommand => new RelayCommand(o =>
        {

        }, o => Categories.Any());
        public ICommand GetRandomColorCommand => new RelayCommand(o =>
        {
            string color = GenerateRandomColor();
            SelectedCategory.Color = color;
            foreach(var item in SelectedCategory.Items)
            {
                item.Color = color;
            }
            OnPropertyChanged(nameof(SelectedCategory));
            OnPropertyChanged(nameof(Items));
        }, o => IsCategorySelected);
        #endregion

        public void AddCategories(IEnumerable<Category> categories) => Categories.AddRange(categories);

        public MainEditorViewModel(IFileService xmlService)
        {
            _xmlService = xmlService;
            Categories = new ObservableCollection<Category>();
        }
        private DateTime RecalculateDateTime(string days, string hours, string minutes)
        {
            if (!Double.TryParse(days, out var d))
                d = 0;
            if (!Double.TryParse(hours, out var h))
                h = 0;
            if (!Double.TryParse(minutes, out var m))
                m = 0;
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
            Categories = new ObservableCollection<Category>();
            for (int i = 0; i < 10; i++)
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
                    Line1st = $"Item {i + 1}",
                    Line2nd = "Line2nd",
                    Format = ""
                });
            }
            for (int i = 0; i < 7; i++)
            {
                Categories.Add(new Category
                {
                    Color = $"{GenerateRandomColor()}",
                    Name = $"Category {i + 1}",
                    Printer = "",
                    PrintTemplate = ""
                });
            }
        }
        private string GenerateRandomColor()
        {
            Random random = new Random();
            string chars = "123567890abcdef";
            string result = "#";
            for (int i = 0; i < 6; i++)
                result += chars[random.Next(chars.Length)];
            return result;
        }
    }
}
