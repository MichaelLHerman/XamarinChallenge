using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace XamarinChallenge
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Updates the observable collection to be equivalent to another collection.
        /// </summary>
        public static void UpdateCollection<T>(this ObservableCollection<T> collection, IEnumerable<T> newCollection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (newCollection == null)
            {
                throw new ArgumentNullException(nameof(newCollection));
            }

            var itemsToDelete = collection.Except(newCollection).ToList();
            foreach (T itemToDelete in itemsToDelete)
            {
                collection.Remove(itemToDelete);
            }

            for (var sourceIndex = 0; sourceIndex < newCollection.Count(); sourceIndex++)
            {
                T item = newCollection.ElementAt(sourceIndex);
                if (collection.Contains(item))
                {
                    int destIndex = collection.IndexOf(item);
                    if (destIndex != sourceIndex)
                    {
                        collection.Move(destIndex, sourceIndex);
                    }
                }
                else
                {
                    collection.Insert(sourceIndex, item);
                }
            }
        }
    }

}
