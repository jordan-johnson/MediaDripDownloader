using System;
using System.Collections.Generic;
using MediaDrip.Downloader.Shared;

namespace MediaDrip.Downloader.Event
{
    public class NotifyQueueChangedEventArgs : EventArgs
    {
        public IEnumerable<DownloadObject> Items { get; private set; }

        public NotifyQueueChangedEventArgs(IEnumerable<DownloadObject> items)
        {
            Items = items;
        }
    }
}