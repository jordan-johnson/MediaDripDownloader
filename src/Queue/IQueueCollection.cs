using System;
using System.Collections.ObjectModel;
using MediaDrip.Downloader.Event;

namespace MediaDrip.Downloader.Queue
{
    public interface IQueueCollection<T> : INotifyQueueCollectionChanged<T>
        where T : IQueueable
    {
        ReadOnlyCollection<T> Items { get; }

        void Enqueue(T item);
        void Dequeue(Func<T, bool> predicate);
    }
}