using System;
using System.Linq;
using MediaDrip.Downloader.Web;
using MediaDrip.Downloader.Shared;
using MediaDrip.Downloader.Exception;

namespace MediaDrip
{
    public sealed partial class MediaDripDownloader
    {
        public void AddSource(ISource source) => _sourceHandler.Add(source);

        public void Enqueue(String address, String destination, bool autoDownload = true)
        {
            try
            {
                var addrUri = new Uri(address);
                var destUri = new Uri(destination);

                _sourceHandler.ThrowIfLookupFails(addrUri);

                var download = new DownloadObject(addrUri, destUri, autoDownload);

                ThrowIfDuplicateQueuedItem(download);

                _queue.Add(download);
            }
            catch(UriFormatException)
            {
                // log
                Console.WriteLine("address oops");
            }
        }

        public void ForceStart(DownloadObject download)
        {
            ProcessDownloadObjects(download);
        }

        public void ForceStartAll()
        {
            var filterByManualDownload = _queue.Where(x => !x.AutoDownload);

            ProcessDownloadObjects(filterByManualDownload);
        }

        public void CancelBySaveDestination(String destination)
        {
            try
            {
                var destUri = new Uri(destination);
                var lookup = _queue.FirstOrDefault(x => x.SaveDestination == destUri);
                
                lookup?.RequestCancellation();
            }
            catch(UriFormatException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void CancelAll()
        {
            foreach(var download in _queue)
            {
                download.RequestCancellation();
            }
        }
    }
}