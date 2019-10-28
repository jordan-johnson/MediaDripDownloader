using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using MediaDrip.Downloader.Shared;
using MediaDrip.Downloader.Event;
using MediaDrip.Downloader.Queue;
using MediaDrip.Downloader.Web;

namespace MediaDrip
{
    public sealed partial class MediaDripDownloader : IDownloadControls, ISourceControls, IQueueCollection<DownloadObject>
    {
        private bool _isDisposing;
        private SourceHandler _sourceHandler;
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
        /// Destructor to indicate 
        /// </summary>
        ~MediaDripDownloader()
        {
            Dispose(disposing: false);
        }

        /// <summary>
        /// Dispose of MediaDripDownloader and all of its underlying services.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The internal dispose method for the dispose pattern.
        /// 
        /// This method is for directly cleaning up events and other active services. All current downloads will be canceled.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if(!_isDisposing)
            {
                if(disposing)
                {
                    Console.WriteLine("disposing managed");
                }

                Console.WriteLine("disposing unmanaged");

                UnsubscribeEventListeners();

                _isDisposing = true;
            }
        }

        /// <summary>
        /// Automatically remove subscribers to OnCollectionChanged event handler, and unsubscribe internal OnCollectionChange_Event method.
        /// </summary>
        private void UnsubscribeEventListeners()
        {
            _queue.CollectionChanged -= OnCollectionChange_Event;

            foreach(var subscriber in OnCollectionChanged?.GetInvocationList())
            {
                OnCollectionChanged -= (subscriber as QueueCollectionEventHandler<DownloadObject>);
            }
        }

        /// <summary>
        /// Checks download's status to determine if download needs to be canceled.
        /// 
        /// If download is 
        /// </summary>
        /// <param name="download"></param>
        private void SafelyCancelThenRemoveDownload(DownloadObject download)
        {
            if(download.Status == DownloadStatus.InProgress)
            {
                // registers a callback with CancellationTokenSource to be executed when canceled
                download.Cancel(() => _queue.Remove(download));
            }
            else
            {
                _queue.Remove(download);
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