using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaDrip.Downloader.Web
{
    public interface ISource
    {
        Uri LookupAddress { get; }
        HttpClient Client { get; }

        Task<Uri> RunAsync(Uri address);
    }
}