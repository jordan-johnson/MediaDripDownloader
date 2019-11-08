using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaDrip.Downloader.Web
{
    /// <summary>
    /// Contract for sources to include a property for source lookup, a unique http client, and a method for processing a download.
    /// </summary>
    public interface ISource
    {
        /// <summary>
        /// Unique identifier for processing future DownloadObjects.
        /// 
        /// This address will be checked for similarity to a DownloadObject's input address.
        /// 
        /// TODO: Update to allow multiple domains.
        /// </summary>
        Uri LookupAddress { get; }

        /// <summary>
        /// An instance of HttpClient.
        /// 
        /// This client should be unique to its lookup address or overall domain.
        /// 
        /// DO NOT put the client in a using statement due to possible socket exhaustion.
        /// </summary>
        /// <value></value>
        HttpClient Client { get; }

        /// <summary>
        /// Process an address asynchronously.
        /// </summary>
        /// <param name="address">File to be downloaded.</param>
        Task<Uri> RunAsync(Uri address);
    }
}