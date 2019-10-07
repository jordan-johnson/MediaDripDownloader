using System;
using MediaDrip.Downloader.Shared;

namespace MediaDrip.Downloader.Exception
{
    public class DuplicateDownloadException : System.Exception
    {
        public DownloadObject Existing { get; private set; }
        public DownloadObject Attempted { get; private set; }

        public DuplicateDownloadException(DownloadObject existing, DownloadObject attempted, String message) :
            base(message)
        {
            Existing = existing;
            Attempted = attempted;
        }
    }
}