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
        public DownloadErrorType ErrorType { get; private set; }

        /// <summary>
        /// Download options for things like overwriting existing files, downloading immediately, etc.
        /// </summary>
        /// <value></value>
        public DownloadOptions Options { get; private set; }

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
        /// Constructs an object representing information on a download.
        /// </summary>
        /// <param name="input">URI input address/file to download.</param>
        /// <param name="output">URI output address/save destination.</param>
        /// <param name="options">Download options for determining if the file should be downloaded immediately, should it overwrite existing files, etc.</param>
        public DownloadObject(Uri input, Uri output, DownloadOptions options = null)
        {
            InputAddress = input;
            OutputAddress = output;
            Options = options ?? new DownloadOptions().UseDefaults();

            _cancelToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Set error status and assign error type.
        /// 
        /// If error is already set, it will not be overwritten.
        /// </summary>
        /// <param name="type"></param>
        public void SetError(DownloadErrorType type)
        {
            if(Status != DownloadStatus.Error)
            {
                Status = DownloadStatus.Error;

                ErrorType = type;
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