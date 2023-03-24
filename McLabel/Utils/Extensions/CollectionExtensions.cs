using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Utils.Extensions
{
    internal static class CollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> sequence)
        {
            foreach (T item in sequence)
            {
                collection.Add(item);
            }
        }
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> sequence) => new ObservableCollection<T>(sequence);
    }
}
