using McLabel.Models;
using McLabel.Services.Interfaces;
using System;
using System.Collections.Generic;
using McLabel.Utils.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using McLabel.Models.Interfaces;
using System.Xml.Linq;

namespace McLabel.Services
{
    internal class XmlFileService : IFileService
    {
        private const string DEFAULT_XML_DATA = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>" +
            "<LabelDataFile><HeaderPC><AppVersion>2.0.5.1</AppVersion><MenuVersion>97284</MenuVersion><FileVersion>3.19</FileVersion></HeaderPC><HeaderPrinter>" +
            "<CurrentFirmware>F 1.10.06</CurrentFirmware></HeaderPrinter><Shared><SubsectionTag1><Modify>false</Modify><CreationDate>5/21/2014</CreationDate></SubsectionTag1>" +
            "<SubsectionTag2><ModifiedDate>4-26-2022</ModifiedDate><ModifiedBy>FST Ithaca-9700</ModifiedBy></SubsectionTag2><SupportedLabelFields><Półprodukty/><Categories/>" +
            "<Exp1_dni/><Exp1_godzin/><Exp1_protokół/><Exp1_wiadomość/><Exp2_dni/><Exp2_godzin/><Exp2_protokół/><Exp2_wiadomość/><Format/><linia_1st/><linia_2nd/>" +
            "</SupportedLabelFields></Shared><Categories/><Labels/></LabelDataFile>";
        private const string BASE_NODE = "LabelDataFile";
        private const string CATEGORY_NODE = "LabelDataFile/Categories";
        private const string ITEM_NODE = "LabelDataFile/Labels";

        private readonly FileDialogService _fileDialogService;
        private List<XmlDocument> _validXmlDocuments;
        private ICollection<ICategory> _categories;
        public ICollection<ICategory> Categories => _categories;

        public XmlFileService(FileDialogService fileDialogService)
        {
            _fileDialogService = fileDialogService;
            _validXmlDocuments = new List<XmlDocument>();
            _categories = new List<ICategory>();
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
        public bool SaveXmlFile(ICollection<ICategory> categories)
        {
            XmlDocument document = CreateXmlFromDefault(categories);
            if (_fileDialogService.SaveFile(document))
            {
                return true;
            }
            return false;
        }

        private void AddItemsInXml(ICollection<IItem> items, ref XmlDocument document)
        {
            XmlNode categoryBaseNode = document.SelectSingleNode(ITEM_NODE);
            foreach (var item in items)
            {
                XmlElement newItem = document.CreateElement("Item");
                Dictionary<string, string> itemElements = new Dictionary<string, string>()
                {
                    {"Półprodukty", item.Name },
                    {"AssignedCategory", item.Category },
                    {"Exp1_dni", item.Exp1Days },
                    {"Exp1_godzin", item.Exp1Hours },
                    {"Exp1_wiadomość", item.Exp1Message },
                    {"Exp1_protokół", item.Exp1Minutes },
                    {"Exp2_dni", item.Exp2Days },
                    {"Exp2_godzin", item.Exp2Hours },
                    {"Exp2_wiadomość", item.Exp2Message },
                    {"Exp2_protokół", item.Exp2Minutes },
                    {"Format", null },
                    {"linia_1st", item.Line1st },
                    {"linia_2nd", item.Line2nd }
                };
                foreach (KeyValuePair<string, string> itemElement in itemElements)
                {
                    XmlNode newNode = document.CreateElement(itemElement.Key);
                    if (itemElement.Value != null)
                    {
                        newNode.InnerText = itemElement.Value;
                    }
                    newItem.AppendChild(newNode);
                }
                categoryBaseNode.AppendChild(newItem);
            }
        }
        private XmlDocument CreateXmlFromDefault(ICollection<ICategory> categories)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(DEFAULT_XML_DATA);
            XmlNode categoryBaseNode = document.SelectSingleNode(CATEGORY_NODE);
            foreach (var category in categories)
            {
                XmlElement newCategory = document.CreateElement("AssignedCategory");
                Dictionary<string, string> categoryNodes = new Dictionary<string, string>
                {
                    {"Name", category.Name },
                    {"Color", category.Color },
                    {"PrintTemplate", null },
                    {"Printer", null }
                };
                foreach (KeyValuePair<string, string> node in categoryNodes)
                {
                    XmlNode newNode = document.CreateElement(node.Key);
                    if (node.Value != null)
                    {
                        newNode.InnerText = node.Value;
                    }
                    newCategory.AppendChild(newNode);
                }
                categoryBaseNode.AppendChild(newCategory);
                AddItemsInXml(category.Items, ref document);
            }
            return document;
        }
        private void LoadItems()
        {
            foreach (XmlDocument xmlDocument in _validXmlDocuments)
            {
                var items = xmlDocument.SelectNodes($"{ITEM_NODE}/Item");
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
                var categories = xmlDocument.SelectNodes($"{CATEGORY_NODE}/AssignedCategory");
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
                        Items = new List<IItem>()
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
                bool checkRootElement = document.DocumentElement?.Name == BASE_NODE;
                bool checkCategories = document.SelectNodes(CATEGORY_NODE)?.Count > 0;
                bool checkLabels = document.SelectNodes(ITEM_NODE)?.Count > 0;
                if (checkRootElement && checkCategories && checkLabels)
                {
                    _validXmlDocuments.Add(document);
                }
            }
            return _validXmlDocuments.Any();
        }
    }
}
