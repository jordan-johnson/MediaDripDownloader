using System;
using System.Threading;
using MediaDrip.Downloader.Queue;

namespace MediaDrip.Downloader.Shared
{
    public class DownloadObject : IWebDownload, IContainCancellationToken, IQueueable, IEquatable<DownloadObject>
    {
        /// <summary>
        /// Download Progress field which is set and validated in the Progress property.
        /// </summary>
        private int _progress;

        /// <summary>
        /// File address to be downloaded.
        /// 
        /// Property is initialized in the constructor.
        /// </summary>
        public Uri InputAddress { get; private set; }

        /// <summary>
        /// Address to save destination.
        /// 
        /// Property is initialized in the constructor.
        /// </summary>
        public Uri OutputAddress { get; private set; }

        /// <summary>
        /// Current status of download.
        /// 
        /// Property is set internally.
        /// </summary>
        /// <value></value>
        public DownloadStatus Status { get; private set; }

        /// <summary>
        /// Flag to determine if DownloadObject will be processed as soon as it's queued.
        /// </summary>
        public bool DownloadImmediately { get; set; }

        /// <summary>
        /// Cancellation Token for asynchronous downloading.
        /// 
        /// Property is initialized in constructor.
        /// </summary>
        public CancellationTokenSource CancellationToken { get; private set; }

        /// <summary>
        /// Current progress (0-100) of download.
        /// </summary>
        /// <value>Progress property gets/sets the value of the _progress field.</value>
        public int Progress
        {
            get => _progress;
            set
            {
                if(value >= 0 && value <= 100)
                {
                    _progress = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="immediate"></param>
        public DownloadObject(Uri input, Uri output, bool immediate = true)
        {
            InputAddress = input;
            OutputAddress = output;
            DownloadImmediately = immediate;

            CancellationToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Comparison method to determine if two DownloadObjects are equal.
        /// 
        /// This is determined by both DownloadObjects' OutputAddress property.
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public bool Equals(DownloadObject comparison)
        {
            if(comparison == null)
                throw new ArgumentNullException("Cannot compare DownloadObject; comparison object is null.");

            if(comparison.OutputAddress == null)
                throw new NullReferenceException($"Cannot compare DownloadObject; comparison object's {nameof(OutputAddress)} property is null.");

            if(OutputAddress == null)
                throw new NullReferenceException($"Cannot compare DownloadObject; base object's {nameof(OutputAddress)} property is null.");

            return OutputAddress == comparison.OutputAddress;
        }
    }
}