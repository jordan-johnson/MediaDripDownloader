using System;

namespace MediaDrip.Downloader.Web
{
    public interface IDownloadControls
    {
        void CancelAll();
        void RemoveAll();
        void RemoveWhere(Func<IWebDownload, bool> predicate);
    }
}