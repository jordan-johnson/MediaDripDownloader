using System;
using MediaDrip.Downloader.Web;
using MediaDrip.Downloader.Event;
using MediaDrip.Downloader.Shared;

namespace MediaDrip
{
    public interface IMediaDripDownloader : INotifyQueueChanged, IDisposable
    {
        #region Controls

        void AddSource(ISource source);

        void Enqueue(String address, String destination, bool autoDownload = true);
        
        void ForceStart(DownloadObject download);
        void ForceStartAll();

        void CancelBySaveDestination(String destination);
        void CancelAll();

        #endregion
    }
}