using Xunit;
using System;
using MediaDrip.Downloader.Shared;

namespace MediaDrip.Downloader.Test
{
    public class DownloadObjectTest
    {
        private readonly Uri _address;
        private readonly DownloadObject _expectedObject;

        public DownloadObjectTest()
        {
            _address = new Uri("https://www.google.com");
            _expectedObject = new DownloadObject(_address, _address);
        }
    }
}