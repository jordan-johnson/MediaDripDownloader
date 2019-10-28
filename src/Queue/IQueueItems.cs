namespace MediaDrip.Downloader.Queue
{
    public interface IQueueItems<T> : IQueueControls<T>
        where T : IQueueable
    {
        IQueueCollection<T> Queue { get; }
    }
}