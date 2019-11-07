using System;
using System.Linq;
using MediaDrip.Downloader.Web;

namespace MediaDrip
{
    public sealed partial class MediaDripDownloader
    {
        /// <summary>
        /// Add source to handler for processing expected DownloadObjects.
        /// </summary>
        /// <param name="source"></param>
        public void AddSource(ISource source) => _sourceHandler.Add(source);

        /// <summary>
        /// Add a DownloadObject to queue.
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(DownloadObject item)
        {
            if(_sourceHandler.SourceExistsByAddressMatch(item.InputAddress))
            {
                var match = _queue.FirstOrDefault(x => x.OutputAddress == item.OutputAddress);

                if(match == null)
                {
                    _queue.Add(item);
                }
                else
                {
                    item.SetError(DownloadErrorType.DuplicateInQueue);
                }
            }
            else
            {
                item.SetError(DownloadErrorType.SourceNotFound);
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
        /// Create a DownloadObject that saves to disk then enqueue.
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
        /// Dequeues a single DownloadObject based on filter criteria. If the download is active, it will be canceled.
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

        /// <summary>
        /// Dequeue all DownloadObjects based on filter criteria. Any active downloads will be canceled.
        /// </summary>
        /// <param name="predicate"></param>
        public void DequeueAllWhere(Func<DownloadObject, bool> predicate)
        {
            var matches = _queue.Where(predicate);

            foreach(var download in matches)
            {
                SafelyCancelThenRemoveDownload(download);
            }
        }

        /// <summary>
        /// Dequeue all DownloadObjects with a canceled status.
        /// </summary>
        public void DequeueCanceledDownloads()
        {
            DequeueAllWhere(x => x.Status == DownloadStatus.Canceled);
        }

        /// <summary>
        /// Cancel a DownloadObject but do not remove from queue.
        /// </summary>
        /// <param name="download"></param>
        public void Cancel(IWebDownload download) => download.Cancel();

        /// <summary>
        /// Cancel all active downloads.
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

        /// <summary>
        /// Checks the download's status to determine if it needs to be canceled before removing it from queue.
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
    }
}