using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using MediaDrip.Downloader.Event;
using MediaDrip.Downloader.Shared;
using MediaDrip.Downloader.Exception;

namespace MediaDrip.Downloader.Queue
{
    public class DownloadQueue : IQueueCollection<DownloadObject>
    {
        /// <summary>
        /// Field of queued DownloadObjects.
        /// </summary>
        private ObservableCollection<DownloadObject> _queue;

        /// <summary>
        /// Returns a read-only collection of queued items.
        /// </summary>
        public ReadOnlyCollection<DownloadObject> Items => _queue.ToList().AsReadOnly();

        /// <summary>
        /// Occurs when a DownloadObject is added or removed from queue.
        /// </summary>
        public event QueueCollectionEventHandler<DownloadObject> OnCollectionChanged;

        public DownloadQueue()
        {
            _queue = new ObservableCollection<DownloadObject>();

            _queue.CollectionChanged += OnCollectionChange_Event;
        }

        ~DownloadQueue()
        {
            // remove
            Console.WriteLine("dereg collection event");

            OnCollectionChanged = null;

            _queue.CollectionChanged -= OnCollectionChange_Event;
        }

        public void Enqueue(DownloadObject item)
        {
            var match = _queue.FirstOrDefault(x => x.OutputAddress == item.OutputAddress);

            if(match != null)
                throw new DuplicateDownloadException(match, item, "Queued item exists with matching output address.");
            
            _queue.Add(item);
        }

        public void Dequeue(Func<DownloadObject, bool> predicate)
        {
            var match = _queue.FirstOrDefault(predicate);

            if(match != null)
            {
                _queue.Remove(match);
            }
        }

        /// <summary>
        /// Invokes OnCollectionChanged event when ObservableCollection's event is invoked.
        /// 
        /// This is to create a front-facing event that 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChange_Event(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = new List<DownloadObject>();
            var oldItems = new List<DownloadObject>();

            if(e.NewItems != null)
                newItems = CastToList(e.NewItems);

            if(e.OldItems != null)
                oldItems = CastToList(e.OldItems);

            OnCollectionChanged?.Invoke(this, new QueueCollectionChangedEventArgs<DownloadObject>(newItems, oldItems));

            /// <summary>
            /// Local function to cast NotifyCollectionChangedEventArgs items to List of DownloadObject.
            /// </summary>
            List<DownloadObject> CastToList(System.Collections.IList collection)
            {
                var cast = collection.Cast<DownloadObject>().ToList();

                return new List<DownloadObject>(cast);
            }
        }
    }
}