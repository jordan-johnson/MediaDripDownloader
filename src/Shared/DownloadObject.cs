using System;
using System.Threading;

namespace MediaDrip.Downloader.Shared
{
    public class DownloadObject : IEquatable<DownloadObject>
    {
        private int _progress;
        private CancellationTokenSource _token;

        public Uri DownloadAddress { get; set; }
        public Uri SaveDestination { get; set; }
        public bool AutoDownload { get; set; }
        public bool IsDownloading { get; set; }

        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                if(value >= 0 && value <= 100)
                {
                    _progress = value;
                }
            }
        }

        public DownloadObject(Uri address, Uri destination, bool autoDownload = true)
        {
            DownloadAddress = address;
            SaveDestination = destination;
            AutoDownload = autoDownload;

            _token = new CancellationTokenSource();
        }

        public void RequestCancellation() => _token.Cancel();

        public bool Equals(DownloadObject comparison)
        {
            if(comparison == null)
                throw new ArgumentNullException("Cannot compare DownloadObject; comparison object is null.");

            if(comparison.SaveDestination == null)
                throw new NullReferenceException($"Cannot compare DownloadObject; comparison object's {nameof(SaveDestination)} property is null.");

            if(SaveDestination == null)
                throw new NullReferenceException($"Cannot compare DownloadObject; base object's {nameof(SaveDestination)} property is null.");

            return SaveDestination == comparison.SaveDestination;
        }
    }
}