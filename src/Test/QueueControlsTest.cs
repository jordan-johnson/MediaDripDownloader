using Xunit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MediaDrip.Downloader.Web;
using MediaDrip.Downloader.Event;

namespace MediaDrip.Downloader.Test
{
    public class ExampleTestSource : SourceUtilities
    {
        public override Uri LookupAddress => new Uri("https://www.jsontest.com/");

        public ExampleTestSource() : base() {}

        public override async Task<Uri> RunAsync(Uri address)
        {
            await Task.Delay(500);

            return address;
        }
    }

    /// <summary>
    /// Maintains a single instance of MDD throughout all tests then disposes on finish.
    /// </summary>
    public class QueueControlsTestFixture
    {
        public MediaDripDownloader Downloader { get; private set; }

        public DownloadObject EnqueuedItem { get; set; }

        public QueueControlsTestFixture()
        {
            Downloader = new MediaDripDownloader();

            Downloader.OnCollectionChanged += OnCollectionChanged_Event;
        }

        private void OnCollectionChanged_Event(object sender, QueueCollectionChangedEventArgs<DownloadObject> e)
        {
            if(e.EnqueuedItems != null)
            {
                EnqueuedItem = e.EnqueuedItems[0];
            }
        }

        ~QueueControlsTestFixture()
        {
            Downloader.OnCollectionChanged -= OnCollectionChanged_Event;
            Downloader.Dispose();

            Console.WriteLine("cleaned up");
        }
    }

    public class QueueControlsTest : IClassFixture<QueueControlsTestFixture>
    {
        private QueueControlsTestFixture _fixture;

        private Uri _jsonTestAddress = new Uri("https://www.jsontest.com/");

        private DownloadOptions _options;

        public QueueControlsTest(QueueControlsTestFixture fixture)
        {
            _fixture = fixture;

            _options = new DownloadOptions()
            {
                DownloadImmediately = false,
                Overwrite = false
            };
        }

        /// <summary>
        /// This tests that the download could not be enqueued due to lack of a source match.
        /// </summary>
        [Fact]
        public void FailEnqueueWithoutSourceMatchPassing()
        {
            _fixture.Downloader.RemoveSource(x => x.LookupAddress == new Uri("https://www.jsontest.com/"));

            var download = _fixture.Downloader.Enqueue(_jsonTestAddress);

            Assert.True(download.Status == DownloadStatus.Error);
            Assert.True(download.ErrorType == DownloadErrorType.SourceNotFound);
        }

        [Fact]
        public void EnqueueWithSourceMatchPassing()
        {
            _fixture.EnqueuedItem = null;
            _fixture.Downloader.AddSource(new ExampleTestSource());

            var download = _fixture.Downloader.Enqueue(_jsonTestAddress, _options);

            Assert.NotNull(_fixture.EnqueuedItem);
            Assert.True(download.Status == DownloadStatus.NotStarted);
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