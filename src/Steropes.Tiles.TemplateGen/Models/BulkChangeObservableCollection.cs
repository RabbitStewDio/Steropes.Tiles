using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Steropes.Tiles.TemplateGen.Models
{
    public class BulkChangeObservableCollection<T> : ObservableCollection<T>
    {
        public BulkChangeObservableCollection()
        { }

        public BulkChangeObservableCollection([NotNull] List<T> list) : base(list)
        { }

        public BulkChangeObservableCollection([NotNull] IEnumerable<T> collection) : base(collection)
        { }

        public void AddRange(IEnumerable<T> dataToAdd)
        {
            this.CheckReentrancy();

            var changedItems = new List<T>(dataToAdd);

            //
            // We need the starting index later
            //
            int startingIndex = this.Count;

            //
            // Add the items directly to the inner collection
            //
            foreach (var data in changedItems)
            {
                this.Items.Add(data);
            }

            //
            // Now raise the changed events
            //
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startingIndex);
            this.OnCollectionChanged(args);
        }

        public void ReplaceAll(IEnumerable<T> dataToAdd)
        {
            this.CheckReentrancy();
            var removedItems = new List<T>(Items);
            Items.Clear();

            var changedItems = new List<T>(dataToAdd);
            foreach (var data in changedItems)
            {
                this.Items.Add(data);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, changedItems, removedItems, 0);
            this.OnCollectionChanged(args);
        }
    }
}
