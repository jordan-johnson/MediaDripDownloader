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
    public sealed partial class MediaDripDownloader : ISourceControls, IQueueItems<DownloadObject>
    {
        private bool _isDisposing;
        private SourceHandler _sourceHandler;

        public IQueueCollection<DownloadObject> Queue { get; }

        public MediaDripDownloader()
        {
            _sourceHandler = new SourceHandler();

            Queue = new DownloadQueue();
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
                }

                _isDisposing = true;
            }
        }
    }
}