using Xunit;
using System;
using MediaDrip.Downloader.Web;
using MediaDrip.Downloader.Event;

namespace MediaDrip.Downloader.Test
{
    /// <summary>
    /// Tests that MediaDripDownloader properly disposes all resources.
    /// </summary>
    public class ServiceDisposalTest
    {
        private Uri _testAddress = new Uri("https://www.google.com");

        /// <summary>
        /// Asserts that MediaDripDownloader is disposable, and that it has been disposed automatically after the using block.
        /// </summary>
        [Fact]
        public void SimpleAutomaticDisposePassingTest()
        {
            MediaDripDownloader downloader;

            Assert.True(typeof(IDisposable).IsAssignableFrom(typeof(MediaDripDownloader)));

            using(downloader = new MediaDripDownloader()){}

            Assert.True(downloader.IsDisposalRequested);
        }

        /// <summary>
        /// Asserts that MediaDripDownloader is disposable, and that it has been disposed properly after a manual call.
        /// </summary>
        [Fact]
        public void SimpleManualDisposePassingTest()
        {
            var downloader = new MediaDripDownloader();

            Assert.True(typeof(IDisposable).IsAssignableFrom(typeof(MediaDripDownloader)));
            Assert.False(downloader.IsDisposalRequested);

            downloader.Dispose();

            Assert.True(downloader.IsDisposalRequested);
        }

        [Fact]
        public void QueueChangedEventDisposePassingTest()
        {
            var downloader = new MediaDripDownloader();

            downloader.OnCollectionChanged += OnQueueChanged_Event;
            downloader.Enqueue(new DownloadObject(_testAddress, _testAddress));

            Assert.NotEmpty(downloader.Items);

            downloader.Dispose();

            Assert.True(downloader.IsDisposalRequested);
            //Assert.True(downloader.OnCollectionChanged == null);
        }

        private void OnQueueChanged_Event(object sender, QueueCollectionChangedEventArgs<DownloadObject> e)
        {
            Assert.True(e.EnqueuedItems != null || e.DequeuedItems != null);
        }
    }
}