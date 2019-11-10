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
        /// Download status field which is set and validated in the Status property.
        /// </summary>
        private DownloadStatus _status;

        /// <summary>
        /// File address to be downloaded.
        /// </summary>
        public Uri InputAddress { get; private set; }

        /// <summary>
        /// Address to save destination.
        /// </summary>
        public Uri OutputAddress { get; private set; }

        /// <summary>
        /// Current status of download.
        /// 
        /// Status will not be updated if previously set to error or success.
        /// </summary>
        public DownloadStatus Status
        {
            get => _status;
            set
            {
                if(_status == DownloadStatus.Error || _status == DownloadStatus.Success)
                    return;
                
                _status = value;
            }
        }

        /// <summary>
        /// Useful for determining if something went wrong with the download.
        /// </summary>
        public DownloadErrorType ErrorType { get; private set; }

        /// <summary>
        /// Download options for things like overwriting existing files, downloading immediately, etc.
        /// </summary>
        public DownloadOptions Options { get; private set; }

        /// <summary>
        /// Current progress (0-100) of download.
        /// </summary>
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
                    else if(value == 100 && Status != DownloadStatus.Error)
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
            if(Status != DownloadStatus.InProgress || !_cancelToken.Token.CanBeCanceled)
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