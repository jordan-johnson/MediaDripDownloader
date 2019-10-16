namespace MediaDrip.Downloader.Queue
{
    public interface IQueueItems<T> : IQueueControls
        where T : IQueueable
    {
        IQueueCollection<T> Queue { get; }
    }
}