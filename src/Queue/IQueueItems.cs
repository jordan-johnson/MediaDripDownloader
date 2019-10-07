using System.Collections.ObjectModel;
using MediaDrip.Downloader.Event;

namespace MediaDrip.Downloader.Queue
{
    public interface IQueueItems<T> where T : IQueueable
    {
        ObservableCollection<T> Queue { get; }

        void Enqueue(T obj);
        void Dequeue(T obj);
    }
}