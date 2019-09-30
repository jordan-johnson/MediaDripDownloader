using Xunit;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediaDrip.Downloader.Web;
using MediaDrip.Downloader.Event;
using MediaDrip.Downloader.Shared;
using MediaDrip.Downloader.Exception;


namespace MediaDrip.Downloader.Test
{
    public class QueueTestFixture : IDisposable
    {
        public ISource Source { get; private set; }
        public IMediaDripDownloader Downloader { get; private set; }
        public List<DownloadObject> QueuedObjects { get; private set; }

        public QueueTestFixture()
        {
            Source = new SoundCloudSource();
            Downloader = new MediaDripDownloader();
            QueuedObjects = new List<DownloadObject>();
        }

        public void Dispose()
        {
            Downloader?.Dispose();
        }

        public void InitializeSource()
        {
            Downloader?.AddSource(Source);
        }

        public void InitializeEvents()
        {
            Downloader.NewQueuedItems += OnNewQueuedItems_Event;
        }

        public void TerminateEvents()
        {
            Downloader.NewQueuedItems -= OnNewQueuedItems_Event;
        }

        private void OnNewQueuedItems_Event(object sender, NotifyQueueChangedEventArgs e)
        {
            QueuedObjects?.AddRange(e?.Items);
        }
    }

    public class QueueTest : IClassFixture<QueueTestFixture>
    {
        private QueueTestFixture _fixture;

        public QueueTest(QueueTestFixture fixture)
        {
            _fixture = fixture;
        }

        ~QueueTest()
        {
            Console.WriteLine("disposed Downloader");
        }

        [Fact]
        public void PassIfEventFired()
        {
            _fixture.InitializeSource();
            _fixture.InitializeEvents();
            _fixture.Downloader.Enqueue("https://www.google.com", "https://www.google.com");
            _fixture.TerminateEvents();

            Assert.Single(_fixture.QueuedObjects);
        }

        [Fact]
        public void ErrorIfSourceNotFound()
        {
            Assert.Throws<SourceNotFoundException>(() => 
                _fixture.Downloader.Enqueue("https://www.google.com", "https://www.google.com")
            );
        }
    }
}