using Xunit;
using System;
using System.Threading.Tasks;
using MediaDrip.Downloader.Web;

namespace MediaDrip.Downloader.Test
{
    public class SourceUtilitiesTest
    {
        public struct TestObject
        {
            string testKey;
            string testKey2;
        }

        public enum ExpectedDownloadReturnType
        {
            AsByteArray,
            AsFile,
            AsJson
        }

        public class BasicSourceTest : SourceUtilities
        {

            private ExpectedDownloadReturnType _expectedReturnType;

            public override Uri LookupAddress => new Uri("https://www.jsontest.com/");

            public BasicSourceTest(ExpectedDownloadReturnType expectedReturnType)
            {
                _expectedReturnType = expectedReturnType;
            }

            public override async Task<Uri> RunAsync(Uri initialAddress)
            {
                
            }
        }

        [Fact]
        public void EnqueueDownloadConvertToJSONObjectPassingTest()
        {
            var jsonTest = new Uri("http://echo.jsontest.com/testKey/testValue/testKey2/testValue2/");
            var downloader = new MediaDripDownloader();
            var source = new BasicSourceTest(ExpectedDownloadReturnType.AsJson);

            downloader.AddSource(source);

            // the output is useless since we're only passing the data into an object
            // so we'll just pass the input address to the output
            downloader.Enqueue(jsonTest, jsonTest);
        }
    }
}