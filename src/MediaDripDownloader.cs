using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using MediaDrip.Downloader.Shared;
using MediaDrip.Downloader.Event;
using MediaDrip.Downloader.Web;

namespace MediaDrip
{
    public sealed partial class MediaDripDownloader : IMediaDripDownloader
    {
        private bool _isDisposing;
        private ObservableCollection<DownloadObject> _queue;
        private SourceHandler _sourceHandler;

        public event DownloadQueueEventHandler NewQueuedItems;
        public event DownloadQueueEventHandler RemovedQueuedItems;

        public MediaDripDownloader()
        {
            _sourceHandler = new SourceHandler();

            InitializeQueueCollectionAndListeners();
        }

        ~MediaDripDownloader()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(!_isDisposing)
            {
                if(disposing)
                {
                    Console.WriteLine("dispose");

                    TerminateQueueCollectionAndListeners();
                }

                _isDisposing = true;
            }
        }

        private void InitializeQueueCollectionAndListeners()
        {
            _queue = new ObservableCollection<DownloadObject>();

            _queue.CollectionChanged += OnCollectionChanged_Event;

            NewQueuedItems += OnNewQueuedItems_Event;
        }

        private void TerminateQueueCollectionAndListeners()
        {
            if(_queue != null)
                _queue.CollectionChanged -= OnCollectionChanged_Event;
            
            _queue = null;

            NewQueuedItems = null;
            RemovedQueuedItems = null;
        }

        private void ThrowIfDuplicateQueuedItem(DownloadObject download)
        {
            if(_queue.Contains(download))
            {
                throw new Exception();
            }
        }

        private void ProcessDownloadObjects(DownloadObject download)
        {
            _sourceHandler.Run(download.DownloadAddress);
        }

        private void ProcessDownloadObjects(IEnumerable<DownloadObject> downloads)
        {
            foreach(var download in downloads)
            {
                _sourceHandler.Run(download.DownloadAddress);
            }
        }
    }
}