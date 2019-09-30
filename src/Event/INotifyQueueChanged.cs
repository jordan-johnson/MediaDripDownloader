namespace MediaDrip.Downloader.Event
{
    public interface INotifyQueueChanged
    {
        event DownloadQueueEventHandler NewQueuedItems;
    }
}