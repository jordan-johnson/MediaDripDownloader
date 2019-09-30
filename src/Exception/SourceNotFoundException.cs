using System;

namespace MediaDrip.Downloader.Exception
{
    public class SourceNotFoundException : System.Exception
    {
        public Uri Address { get; private set; }

        public SourceNotFoundException(String message) : base(message) {}

        public SourceNotFoundException(Uri address, String message) : base(message)
        {
            Address = address;
        }
    }
}