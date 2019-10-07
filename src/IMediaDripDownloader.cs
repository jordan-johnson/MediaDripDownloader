using System;
using MediaDrip.Downloader.Web;
using MediaDrip.Downloader.Event;
using MediaDrip.Downloader.Shared;

namespace MediaDrip
{
    public interface IMediaDripDownloader : IDisposable
    {
        #region Controls

        void AddSource(ISource source);
        void CancelBySaveDestination(Uri destination);
        void CancelAll();

        #endregion
    }
}