using System;
using System.Threading.Tasks;
using MediaDrip.Downloader.Web;
using Newtonsoft.Json;

namespace MediaDrip.Downloader.Test
{
    public struct SoundCloudTrack
    {
        [JsonProperty("stream_url")]
        public String StreamUrl;
    }

    public class SoundCloudSource : SourceUtilities
    {
        private const String _clientId = "NmW1FlPaiL94ueEu7oziOWjYEzZzQDcK";
        private const String _initAddress = "https://api.soundcloud.com/resolve.json?url=";

        public override Uri LookupAddress => new Uri("https://www.soundcloud.com");

        public SoundCloudSource() : base() {}

        public override async Task<Uri> Run(Uri initialAddress)
        {
            var jsonAddress = BuildSoundCloudAddress(initialAddress.ToString());
            var task = await DownloadAsJSONObjectAsync<SoundCloudTrack>(jsonAddress);

            return new Uri(task.StreamUrl);
        }

        private Uri BuildSoundCloudAddress(String streamableUrl)
        {
            var finalAddress = _initAddress + streamableUrl + "&client_id=" + _clientId;

            return new Uri(finalAddress);
        }
    }
}