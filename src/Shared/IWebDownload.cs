using System;

namespace MediaDrip.Downloader.Shared
{
    public interface IWebDownload
    {
        Uri InputAddress { get; }
        Uri OutputAddress { get; }
        DownloadStatus Status { get; }
        int Progress { get; set; }
    }
}