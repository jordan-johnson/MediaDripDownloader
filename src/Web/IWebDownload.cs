using System;

namespace MediaDrip.Downloader.Web
{
    /// <summary>
    /// Contract for necessary download information.
    /// </summary>
    public interface IWebDownload
    {
        Uri InputAddress { get; }
        Uri OutputAddress { get; }
        DownloadStatus Status { get; }
        DownloadErrorType ErrorType { get; }
        int Progress { get; set; }

        void Cancel();
        void Cancel(Action callback);
    }
}