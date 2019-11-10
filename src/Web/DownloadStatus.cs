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

    public enum DownloadErrorType
    {
        None = 0,
        ProgressOutOfRange,
        SourceNotFound,
        DuplicateInQueue
    }
}