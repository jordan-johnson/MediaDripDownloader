using System;

namespace MediaDrip.Downloader.Web
{
    public interface IWebDownload
    {
        Uri InputAddress { get; }
        Uri OutputAddress { get; }
        DownloadStatus Status { get; }
        int Progress { get; set; }

        void Cancel();
        void Cancel(Action callback);
    }
}