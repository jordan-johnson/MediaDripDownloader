using System;
using System.Linq;
using MediaDrip.Downloader.Web;

namespace MediaDrip
{
    public sealed partial class MediaDripDownloader
    {
        public void AddSource(ISource source) => _sourceHandler.Add(source);

        /// <summary>
        /// Enqueue a DownloadObject.
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(DownloadObject item)
        {
            var match = _queue.FirstOrDefault(x => x.OutputAddress == item.OutputAddress);

            if(match == null)
            {
                _queue.Add(item);
            }
        }

        /// <summary>
        /// Create a DownloadObject that doesn't save to disk then enqueue.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="immediate"></param>
        public void Enqueue(Uri input, bool immediate = true)
        {
            var downloadObject = new DownloadObject(input, null, immediate);

            Enqueue(downloadObject);
        }

        /// <summary>
        /// Create a DownloadObject that saves to disk then equeue.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="immediate"></param>
        public void Enqueue(Uri input, Uri output, bool immediate = true)
        {
            var downloadObject = new DownloadObject(input, output, immediate);

            Enqueue(downloadObject);
        }

        /// <summary>
        /// Dequeues a DownloadObject based on filter criteria.
        /// </summary>
        /// <param name="predicate"></param>
        public void Dequeue(Func<DownloadObject, bool> predicate)
        {
            var match = _queue.FirstOrDefault(predicate);

            if(match != null)
            {
                SafelyCancelThenRemoveDownload(match);
            }
        }

        /// <summary>
        /// Removes all downloads from queue. Any active downloads will be canceled.
        /// </summary>
        public void DequeueAll()
        {
            for(var i = 0; i < _queue.Count; i++)
            {
                var download = _queue[i];

                SafelyCancelThenRemoveDownload(download);
            }
        }

        // continue working on this
        //
        //
        //

        /// <summary>
        /// Mirror method for cancellation of a download.
        /// </summary>
        /// <param name="download"></param>
        public void Cancel(IWebDownload download) => download.Cancel();

        /// <summary>
        /// Cancel all active downloads
        /// 
        /// DownloadObject's Cancel method checks if the download is in progress before canceling.
        /// </summary>
        public void CancelAll()
        {
            foreach(var download in _queue)
            {
                download.Cancel();
            }
        }
    }
}