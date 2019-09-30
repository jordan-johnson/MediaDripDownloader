using System;
using MediaDrip.Downloader.Shared;

namespace MediaDrip.Downloader.Event
{
    public class NotifyDownloadProgressEventArgs : EventArgs
    {
        public DownloadObject Download { get; private set; }

        public int Progress
        {
            get
            {
                return Download.Progress;
            }
            set
            {
                Download.Progress = value;
            }
        }

        public NotifyDownloadProgressEventArgs(DownloadObject model, int progress)
        {
            Download = model;
            Progress = progress;
        }
    }
}