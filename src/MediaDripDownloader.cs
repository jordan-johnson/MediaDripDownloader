using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using MediaDrip.Downloader.Web;
using MediaDrip.Downloader.Event;
using MediaDrip.Downloader.Queue;
using MediaDrip.Downloader.Shared;

namespace MediaDrip
{
    public sealed partial class MediaDripDownloader : DisposableObject, IDownloadControls, ISourceControls, IQueueCollection<DownloadObject>
    {
        /// <summary>
        /// Repository of sources for processing DownloadObjects.
        /// </summary>
        private SourceHandler _sourceHandler;

        /// <summary>
        /// Queue of DownloadObjects.
        /// 
        /// OnCollectionChanged event is a wrapper for handling changes to this collection.
        /// </summary>
        private ObservableCollection<DownloadObject> _queue;

        /// <summary>
        /// Returns a read-only collection of queued items.
        /// </summary>
        public ReadOnlyCollection<DownloadObject> Items => _queue?.ToList().AsReadOnly();

        /// <summary>
        /// Occurs when a DownloadObject is added or removed from queue.
        /// 
        /// Internally, ObservableCollection is used; but this event handler is a clean wrapper for the model being used.
        /// </summary>
        public event QueueCollectionEventHandler<DownloadObject> OnCollectionChanged;

        /// <summary>
        /// Initializes an instance of MediaDripDownloader that initializes a source handler and queue collection.
        /// </summary>
        public MediaDripDownloader()
        {
            _sourceHandler = new SourceHandler();

            _queue = new ObservableCollection<DownloadObject>();
            _queue.CollectionChanged += OnCollectionChange_Event;
        }

        /// <summary>
        /// Dispose of objects that inherit IDisposable in this method.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            Console.WriteLine("disposing managed");
        }

        /// <summary>
        /// Dispose of unmanaged resources (i.e. events) in this method.
        /// </summary>
        protected override void DisposeUnmanagedResources()
        {
            Console.WriteLine("disposing unmanaged");

            UnsubscribeEventListeners();
        }

        /// <summary>
        /// Automatically remove subscribers to OnCollectionChanged event handler, and unsubscribe internal OnCollectionChange_Event method.
        /// </summary>
        private void UnsubscribeEventListeners()
        {
            _queue.CollectionChanged -= OnCollectionChange_Event;

            if(OnCollectionChanged != null)
            {
                foreach(var subscriber in OnCollectionChanged.GetInvocationList())
                {
                    OnCollectionChanged -= (subscriber as QueueCollectionEventHandler<DownloadObject>);
                }
            }
        }

        /// <summary>
        /// Invokes OnCollectionChanged event when ObservableCollection's event is invoked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChange_Event(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = new List<DownloadObject>();
            var oldItems = new List<DownloadObject>();

            if(e.NewItems != null) {
                newItems = CastToList(e.NewItems);

                foreach(var download in newItems.Where(x => x.Options.DownloadImmediately))
                {
                    _sourceHandler.RunSourceFromAddressLookup(download.InputAddress);
                }
            }

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