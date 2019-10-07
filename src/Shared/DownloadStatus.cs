namespace MediaDrip.Downloader.Shared
{
    public enum DownloadStatus
    {
        NotStarted = 0,
        InProgress,
        Canceled,
        Error,
        Success
    }
}