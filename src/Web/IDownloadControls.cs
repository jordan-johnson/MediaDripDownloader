using System;

namespace MediaDrip.Downloader.Web
{
    public interface IDownloadControls
    {
        void Cancel(IWebDownload download);
        void CancelAll();
    }
}