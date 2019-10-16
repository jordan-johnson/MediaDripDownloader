using MediaDrip.Downloader.Queue;

namespace MediaDrip.Downloader.Event
{
    public interface INotifyQueueCollectionChanged<T> where T : IQueueable
    {
        event QueueCollectionEventHandler<T> OnCollectionChanged;
    }
}