using MediaDrip.Downloader.Queue;

namespace MediaDrip.Downloader.Event
{
    public delegate void QueueCollectionEventHandler<T>(object sender, QueueCollectionChangedEventArgs<T> e)
        where T : IQueueable;
}