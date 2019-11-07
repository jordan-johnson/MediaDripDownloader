using System;
using System.Threading;
using MediaDrip.Downloader.Queue;

namespace MediaDrip.Downloader.Web
{
    public class DownloadObject : IWebDownload, IQueueable, IEquatable<DownloadObject>
    {
        /// <summary>
        /// Download Progress field which is set and validated in the Progress property.
        /// </summary>
        private int _progress;

        /// <summary>
        /// Cancellation token field which is initialized in constructor.
        /// </summary>
        private CancellationTokenSource _cancelToken;  

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
        /// Useful for determining if something went wrong with the download.
        /// </summary>
        public DownloadErrorType Error { get; private set; }

        /// <summary>
        /// Flag to determine if DownloadObject will be processed as soon as it's queued.
        /// </summary>
        public bool DownloadImmediately { get; set; }

        /// <summary>
        /// Current progress (0-100) of download.
        /// </summary>
        /// <value>Progress property gets/sets the value of the _progress field.</value>
        public int Progress
        {
            get => _progress;
            set
            {
                // value is between 0-100
                if(value >= 0 && value <= 100)
                {
                    if(value > 0 && value < 100)
                    {
                        Status = DownloadStatus.InProgress;
                    }
                    else if(value == 100)
                    {
                        Status = DownloadStatus.Success;
                    }

                    _progress = value;
                }
                else
                {
                    SetError(DownloadErrorType.ProgressOutOfRange);
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

            _cancelToken = new CancellationTokenSource();
        }

        public void SetError(DownloadErrorType type)
        {
            if(Status != DownloadStatus.Error)
            {
                Status = DownloadStatus.Error;

                Error = type;
            }
        }

        /// <summary>
        /// Cancel download
        /// </summary>
        public void Cancel()
        {
            if(Status != DownloadStatus.InProgress && !_cancelToken.Token.CanBeCanceled)
                return;

            Status = DownloadStatus.Canceled;
                
            _cancelToken.Cancel();
        }

        /// <summary>
        /// Register a callback before cancelling the download.
        /// </summary>
        /// <param name="callback"></param>
        public void Cancel(Action callback)
        {
            _cancelToken.Token.Register(callback);

            Cancel();
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