namespace MediaDrip.Downloader.Queue
{
    public interface IQueueControls
    {
        void Enqueue(IQueueable item);
        void Dequeue(IQueueable item);
    }
}