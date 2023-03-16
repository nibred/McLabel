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
using System.Xaml;

namespace McLabel.ViewModels
{
    internal class MainEditorViewModel : ViewModelBase
    {
        private readonly IFileService _xmlService;
        private ObservableCollection<Item> _items;
        private ObservableCollection<Category> _categories;
        private Item _selectedItem;
        private Category _selectedCategory;
        public ObservableCollection<Item> Items
        {
            get
            {
                if (_selectedCategory is null)
                {
                    //AddColorsInItems();
                    return _items;
                }
                return _items.Where(x => x.Category == SelectedCategory.Name).Select(x => { x.Color = SelectedCategory.Color; return x; }).ToObservableCollection();
            }
        }
        public ObservableCollection<Category> Categories => _categories;
        public bool IsItemSelected => SelectedItem != null;
        public Item SelectedItem
        { 
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
                OnPropertyChanged(nameof(IsItemSelected));
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
        public MainEditorViewModel(IFileService xmlService)
        {
            _xmlService = xmlService;
            _items = new ObservableCollection<Item>();
            _categories = new ObservableCollection<Category>();
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
