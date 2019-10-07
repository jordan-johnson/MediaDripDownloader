using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using MediaDrip.Downloader.Shared;
using MediaDrip.Downloader.Event;
using MediaDrip.Downloader.Queue;
using MediaDrip.Downloader.Web;

namespace MediaDrip
{
    public sealed partial class MediaDripDownloader : IMediaDripDownloader, IQueueItems<DownloadObject>
    {
        private bool _isDisposing;
        private SourceHandler _sourceHandler;

        public ObservableCollection<DownloadObject> Queue { get; private set; }

        public MediaDripDownloader()
        {
            _sourceHandler = new SourceHandler();

            Queue = new ObservableCollection<DownloadObject>();
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

                    Queue = null;
                }

                _isDisposing = true;
            }
        }

        private void ProcessDownloadObjects(DownloadObject download)
        {
            _sourceHandler.Run(download.InputAddress);
        }

        private void ProcessDownloadObjects(IEnumerable<DownloadObject> downloads)
        {
            foreach(var download in downloads)
            {
                _sourceHandler.Run(download.InputAddress);
            }
        }
    }
}