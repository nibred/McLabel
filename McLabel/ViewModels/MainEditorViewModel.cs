using McLabel.Commands;
using McLabel.Models;
using McLabel.Models.Interfaces;
using McLabel.Services.Interfaces;
using McLabel.Utils.Extensions;
using McLabel.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xaml;

namespace McLabel.ViewModels
{
    internal class MainEditorViewModel : NotifyBase
    {
        #region private fields
        private readonly IFileService _xmlService;
        private IItem _selectedItem;
        private ICategory _selectedCategory;
        #endregion

        #region observable collections
        public ObservableCollection<ICategory> Categories { get; private set; }
        public ObservableCollection<IItem> Items => SelectedCategory?.Items
            .Select(item =>
            {
                item.Color = SelectedCategory.Color;
                return item;
            }).ToObservableCollection();
        #endregion

        #region properties
        public ushort FontSize => 16;
        public DateTime DateTimeNow => DateTime.Now;
        public void AddCategories(ICollection<ICategory> categories) => Categories.AddRange(categories);
        public bool IsItemSelected => SelectedItem != null;
        public bool IsCategorySelected => SelectedCategory != null;
        public IItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
                OnPropertyChanged(nameof(IsItemSelected));
                OnPropertyChanged(nameof(DateTimeNow));
            }
        }
        public ICategory SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                Set(ref _selectedCategory, value);
                OnPropertyChanged(nameof(Items));
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
                Items = new List<IItem>()
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
            _xmlService.SaveXmlFile(Categories);
        }, o => Categories.Any());
        public ICommand GetRandomColorCommand => new RelayCommand(o =>
        {
            string color = GenerateRandomColor();
            SelectedCategory.Color = color;
            OnPropertyChanged(nameof(Items));
        }, o => IsCategorySelected);

        #endregion


        public MainEditorViewModel(IFileService xmlService)
        {
            _xmlService = xmlService;
            Categories = new ObservableCollection<ICategory>();
        }
        public MainEditorViewModel() : base() // design time constructor
        {
            Categories = new ObservableCollection<ICategory>();
            for (int i = 0; i < 10; i++)
            {
                SelectedCategory.Items.Add(new Item
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
