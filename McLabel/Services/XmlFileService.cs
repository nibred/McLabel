using McLabel.Models;
using McLabel.Services.Interfaces;
using System;
using System.Collections.Generic;
using McLabel.Utils.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace McLabel.Services
{
    internal class XmlFileService : IFileService
    {
        private readonly FileDialogService _fileDialogService;
        private List<XmlDocument> _validXmlDocuments;
        private List<Category> _categories;
        private Dictionary<string, List<Item>> _itemsDictionary;
        public List<Category> Categories => _categories;

        public XmlFileService(FileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;
            _validXmlDocuments = new List<XmlDocument>();
            _categories = new List<Category>();
            _itemsDictionary = new Dictionary<string, List<Item>>();
        }
        public bool OpenXmlFiles()
        {
            if (_fileDialogService.OpenFiles(out IEnumerable<string> selectedXmlFiles))
            {
                if (IsValidFiles(selectedXmlFiles))
                {
                    LoadCategories();
                    LoadItems();
                    return true;
                }
            }
            return false;
        }
        private void LoadItems()
        {
            foreach (XmlDocument xmlDocument in _validXmlDocuments)
            {
                var items = xmlDocument.SelectNodes("LabelDataFile/Labels/Item");
                foreach (XmlNode item in items)
                {
                    Item newItem = new Item
                    {
                        Name = item["Półprodukty"].InnerText,
                        Category = item["AssignedCategory"].InnerText,
                        Exp1Days = item["Exp1_dni"].InnerText,
                        Exp1Hours = item["Exp1_godzin"].InnerText,
                        Exp1Message = item["Exp1_wiadomość"].InnerText,
                        Exp1Minutes = item["Exp1_protokół"].InnerText,
                        Exp2Days = item["Exp2_dni"].InnerText,
                        Exp2Hours = item["Exp2_godzin"].InnerText,
                        Exp2Message = item["Exp2_wiadomość"].InnerText,
                        Exp2Minutes = item["Exp2_protokół"].InnerText,
                        Format = item["Format"].InnerText,
                        Line1st = item["linia_1st"].InnerText,
                        Line2nd = item["linia_2nd"].InnerText
                    };
                    foreach (var category in _categories.Where(c => c.Name == newItem.Category))
                    {
                        category.Items.Add(newItem);
                    }
                }
            }
        }
        private void LoadCategories()
        {
            List<string> categoryNames = new List<string>();
            categoryNames.Capacity = 10;
            foreach (XmlDocument xmlDocument in _validXmlDocuments)
            {
                var categories = xmlDocument.SelectNodes("LabelDataFile/Categories/AssignedCategory");
                foreach (XmlNode category in categories)
                {
                    string categoryName = category["Name"].InnerText;
                    if (categoryNames.Contains(categoryName))
                        continue;
                    _categories.Add(new Category
                    {
                        Name = category["Name"].InnerText,
                        Color = category["Color"].InnerText,
                        PrintTemplate = category["PrintTemplate"].InnerText,
                        Printer = category["Printer"].InnerText,
                        Items = new List<Item>()
                    });
                    categoryNames.Add(categoryName);
                }
            }
        }

        private bool IsValidFiles(IEnumerable<string> xmlFiles)
        {
            _validXmlDocuments.Clear();
            foreach (var xmlFile in xmlFiles)
            {
                var document = new XmlDocument();
                document.Load(xmlFile);
                bool checkRootElement = document.DocumentElement?.Name == "LabelDataFile";
                bool checkCategories = document.SelectNodes("LabelDataFile/Categories")?.Count > 0;
                bool checkLabels = document.SelectNodes("LabelDataFile/Labels")?.Count > 0;
                if (checkRootElement && checkCategories && checkLabels)
                {
                    _validXmlDocuments.Add(document);
                }
            }
            return _validXmlDocuments.Any();
        }
    }
}
