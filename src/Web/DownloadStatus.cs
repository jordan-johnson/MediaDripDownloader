namespace MediaDrip.Downloader.Web
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