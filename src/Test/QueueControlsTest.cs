using Xunit;
using System;
using MediaDrip.Downloader.Web;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaDrip.Downloader.Test
{
    public class ExampleTestSource : ISource
    {
        public Uri LookupAddress => throw new NotImplementedException();

        public HttpClient Client => throw new NotImplementedException();

        public Task<Uri> RunAsync(Uri address)
        {
            throw new NotImplementedException();
        }
    }

    public class QueueControlsTestFixture
    {
        public MediaDripDownloader Downloader { get; private set; }

        public QueueControlsTestFixture()
        {
            Downloader = new MediaDripDownloader();
        }

        ~QueueControlsTestFixture()
        {
            Downloader.Dispose();

            Console.WriteLine("cleaned up");
        }
    }

    public class QueueControlsTest : IClassFixture<QueueControlsTestFixture>
    {
        private QueueControlsTestFixture _fixture;

        public QueueControlsTest(QueueControlsTestFixture fixture)
        {
            _fixture = fixture;
        }

        public void EnqueueWithoutSourceExceptionTest()
        {
            _fixture.Downloader.Enqueue(new Uri("https://www.jsontest.com/"));
        }

        [Fact]
        public void TestOne()
        {
            Assert.True(true);
        }

        [Fact]
        public void TestTwo()
        {
            Assert.True(true);
        }
    }
}