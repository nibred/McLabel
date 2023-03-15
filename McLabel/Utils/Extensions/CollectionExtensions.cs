using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Utils.Extensions
{
    internal static class CollectionExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> sequence)
        {
            return !sequence.Any();
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> sequence)
        {
            ObservableCollection<T> collection = new ObservableCollection<T>();
            if (sequence is null)
                return null;
            foreach (var item in sequence)
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}
