using Xunit;
using System;

namespace MediaDrip.Downloader.Test
{
    public class SoundCloudSourceTest
    {
        private SoundCloudSource _source;

        private const String _dlAddress = "https://soundcloud.com/koomaofficial/kooma-veela-sculpture-stripped";

        public SoundCloudSourceTest()
        {
            _source = new SoundCloudSource();
        }

        ~SoundCloudSourceTest()
        {
            Console.WriteLine("disposing test source");

            _source.Dispose();
        }

        [Fact]
        public void TestThing()
        {
        }
    }
}