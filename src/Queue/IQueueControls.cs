using System;

namespace MediaDrip.Downloader.Queue
{
    public interface IQueueControls<T> where T : IQueueable
    {
        void Enqueue(T item);
        void Dequeue(Func<T, bool> predicate);
        void DequeueAll();
    }
}