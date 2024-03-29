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
        public void AddSource(ISource source) => _sourceHandler.AddSource(source);

        /// <summary>
        /// Remove source from handler using filter criteria.
        /// </summary>
        public void RemoveSource(Func<ISource, bool> predicate) => _sourceHandler.RemoveSource(predicate);

        /// <summary>
        /// Retrieve source from handler using filter criteria.
        /// </summary>
        public ISource GetSource(Func<ISource, bool> predicate) => _sourceHandler.GetSource(predicate);

        /// <summary>
        /// Add a DownloadObject to queue.
        /// </summary>
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
        public DownloadObject Enqueue(Uri input, DownloadOptions options = null)
        {
            var downloadObject = new DownloadObject(input, null, options);

            Enqueue(downloadObject);

            return downloadObject;
        }

        /// <summary>
        /// Create a DownloadObject that saves to disk then enqueue.
        /// </summary>
        public DownloadObject Enqueue(Uri input, Uri output, DownloadOptions options = null)
        {
            var downloadObject = new DownloadObject(input, output, options);

            Enqueue(downloadObject);

            return downloadObject;
        }

        /// <summary>
        /// Dequeues a single DownloadObject based on filter criteria. If the download is active, it will be canceled.
        /// </summary>
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
        public void Cancel(IWebDownload download) => download.Cancel();

        /// <summary>
        /// Cancel all active downloads but do not remove from queue.
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