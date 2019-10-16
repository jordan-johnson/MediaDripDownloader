using System;
using MediaDrip.Downloader.Shared;

namespace MediaDrip.Downloader.Exception
{
    public class QueuedItemNotFoundException : System.Exception
    {
        public QueuedItemNotFoundException(String message) :
            base(message)
        {
        }
    }
}