using McLabel.Commands;
using McLabel.Models;
using McLabel.Models.Interfaces;
using McLabel.Services;
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
using System.Windows;
using System.Windows.Input;
using System.Xaml;

namespace McLabel.ViewModels
{
    internal class MainEditorViewModel : NotifyBase
    {
        #region private fields
        private readonly IFileService _xmlService;
        private readonly FileDialogService _fileDialogService;
        private IItem _selectedItem;
        private ICategory _selectedCategory;
        private readonly Random _random = new Random();
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
                _changeCategory = SelectedCategory;
                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(ChangeCategory));
            }
        }
        private ICategory _changeCategory;
        public ICategory ChangeCategory
        {
            get => _changeCategory;
            set
            {
                Set(ref _changeCategory, value);
                _changeCategory?.Items.Add(SelectedItem);
                _selectedCategory.Items.Remove(SelectedItem);
                SelectedCategory = _changeCategory;
                OnPropertyChanged(nameof(SelectedCategory));
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
                Name = $"New category {Categories.Count + 1}",
                Color = $"{GenerateRandomColor()}",
                Printer = "",
                PrintTemplate = "",
                Items = new List<IItem>()
            });
        });
        public ICommand RemoveElementCommand => new RelayCommand(o =>
        {
            if (_fileDialogService.ShowConfirmationDialog("Are you sure you want to delete?") != true)
                return;
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
            _xmlService.SaveFile(Categories);
        }, o => Categories.Any());
        public ICommand GetRandomColorCommand => new RelayCommand(o =>
        {
            string color = GenerateRandomColor();
            SelectedCategory.Color = color;
            OnPropertyChanged(nameof(Items));
        }, o => IsCategorySelected);
        #endregion


        public MainEditorViewModel(IFileService xmlService, FileDialogService fileDialogService)
        {
            _xmlService = xmlService;
            _fileDialogService = fileDialogService; // TODO! resolve for test
            Categories = new ObservableCollection<ICategory>();
        }
        private string GenerateRandomColor()
        {
            string chars = "123567890abcdef";
            string result = "#";
            for (int i = 0; i < 6; i++)
                result += chars[_random.Next(chars.Length)];
            return result;
        }
    }
}
