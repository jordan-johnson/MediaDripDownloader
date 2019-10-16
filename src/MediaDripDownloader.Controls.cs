using System;
using System.Linq;
using MediaDrip.Downloader.Web;
using MediaDrip.Downloader.Queue;
using MediaDrip.Downloader.Shared;
using MediaDrip.Downloader.Exception;

namespace MediaDrip
{
    public sealed partial class MediaDripDownloader
    {
        public void AddSource(ISource source) => _sourceHandler.Add(source);

        public void Enqueue(IQueueable item)
        {
        }

        public void Dequeue(IQueueable item)
        {

        }

        /// <summary>
        /// Adds a DownloadObject to queue for processing.
        /// 
        /// Throws DuplicateDownloadException if queue contains an object with a matching output address.
        /// </summary>
        public void Enqueue(DownloadObject obj)
        {
            var alreadyInQueue = Queue.Items.FirstOrDefault(x => x.OutputAddress == obj.OutputAddress);

            if(alreadyInQueue != null)
                throw new DuplicateDownloadException(alreadyInQueue, obj, "Queued item exists with matching output address.");

            Queue.Enqueue(obj);
        }

        /// <summary>
        /// Creates a DownloadObject and adds it to the queue for processing.
        /// </summary>
        /// <param name="input">File address to be downloaded.</param>
        /// <param name="output">Save destination.</param>
        /// <param name="immediatelyDownload">Automatically process once in queue?</param>
        public void Enqueue(Uri input, Uri output, bool immediatelyDownload = true)
        {
            var download = new DownloadObject(input, output, immediatelyDownload);

            Enqueue(download);
        }

        public void Dequeue(DownloadObject obj)
        {
            if(obj.Status == DownloadStatus.InProgress)
                obj.CancellationToken.Cancel();

            //Queue.Dequeue(obj);
        }

        public void DequeueBySaveDestination(Uri destination)
        {
            //var download = Queue.FirstOrDefault(x => x.OutputAddress == destination);

            //Dequeue(download);
        }

        public void CancelBySaveDestination(Uri destination)
        {

        }

        public void CancelAll()
        {

        }

        /*
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
        }*/
    }
}