using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using MediaDrip.Downloader.Event;
using MediaDrip.Downloader.Shared;

namespace MediaDrip
{
    public sealed partial class MediaDripDownloader
    {
        private void OnCollectionChanged_Event(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e?.NewItems != null)
            {
                var items = e.NewItems.Cast<DownloadObject>();

                NewQueuedItems?.Invoke(sender, new NotifyQueueChangedEventArgs(items));
            }

            if(e?.OldItems != null)
            {
                if(e.Action == NotifyCollectionChangedAction.Remove)
                {
                    var items = e.OldItems.Cast<DownloadObject>();

                    RemovedQueuedItems?.Invoke(sender, new NotifyQueueChangedEventArgs(items));
                }
            }
        }

        private void OnNewQueuedItems_Event(object sender, NotifyQueueChangedEventArgs e)
        {
            if(e?.Items != null)
            {
                var filterByAutoDownload = e.Items.Where(x => x.AutoDownload);

                ProcessDownloadObjects(filterByAutoDownload);
            }
        }
    }
}