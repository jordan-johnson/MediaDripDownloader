using System;
using System.Collections.Generic;
using MediaDrip.Downloader.Queue;

namespace MediaDrip.Downloader.Event
{
    public class QueueCollectionChangedEventArgs<T> : EventArgs
        where T : IQueueable
    {
        public List<T> EnqueuedItems { get; private set; }
        public List<T> DequeuedItems { get; private set; }

        public QueueCollectionChangedEventArgs(List<T> enqueued, List<T> dequeued)
        {
            EnqueuedItems = enqueued;
            DequeuedItems = dequeued;
        }
    }
}