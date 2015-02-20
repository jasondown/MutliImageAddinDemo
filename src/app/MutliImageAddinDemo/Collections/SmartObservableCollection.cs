using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Jason.Down.Blog.MutliImageAddinDemo.Collections
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications when items are added, removed, or when the
    /// whole collection is refreshed. The collection does not notify that it has changed until the entire range of
    /// items has been added, rather than after each item is added (which is the default <see cref="ObservableCollection{T}"/> behaviour).
    /// This class also provides notifications when an item within the collection has a property changed (that is hooked up to 
    /// <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SmartObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartObservableCollection{T}"/> class.
        /// </summary>
        public SmartObservableCollection()
        {
            base.CollectionChanged += SmartObservableCollectionChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartObservableCollection{T}"/> class that contains elements
        /// coped from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public SmartObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
            base.CollectionChanged += SmartObservableCollectionChanged;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartObservableCollection{T}"/> class that contains elements
        /// copied from the specified collection.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        public SmartObservableCollection(List<T> list)
            : base(list)
        {
            base.CollectionChanged += SmartObservableCollectionChanged;
        }

        /// <summary>
        /// Adds the range of items to the collection.
        /// </summary>
        /// <param name="range">The range.</param>
        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Items.Add(item);
            }

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Resets the collection by clearing the original collection and copying elements from a new one.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public void Reset(IEnumerable<T> collection)
        {
            Items.Clear();
            AddRange(collection);
        }

        /// <summary>
        /// Notifies when an items within the collection have changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void SmartObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (var notifyPropertyChanged in e.NewItems.OfType<INotifyPropertyChanged>())
                {
                    notifyPropertyChanged.PropertyChanged += ItemPropertyChanged;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (var notifyPropertyChanged in e.OldItems.OfType<INotifyPropertyChanged>())
                {
                    notifyPropertyChanged.PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Event handler used to notify when an item within the collection has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(a);
        }
    }
}
