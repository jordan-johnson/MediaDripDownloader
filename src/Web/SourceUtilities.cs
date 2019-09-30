using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MediaDrip.Downloader.Web
{
    public abstract class SourceUtilities : ISource, IDisposable
    {
        private bool _isDisposing;

        public abstract Uri LookupAddress { get; }

        public HttpClient Client { get; private set; }

        public SourceUtilities()
        {
            Client = new HttpClient();
        }

        ~SourceUtilities()
        {
            Dispose(disposing: false);
        }

        public abstract Task<Uri> Run(Uri initialAddress);

        public void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(_isDisposing)
            {
                if(disposing)
                {
                    Console.WriteLine("disposing source");

                    Client.Dispose();
                }
            }
        }

        protected async Task<T> DownloadAsJSONObjectAsync<T>(Uri address, CancellationToken token = default(CancellationToken))
        {
            var response = await Client.GetAsync(address, token).ConfigureAwait(false);

            return await TryProcessingResponse<T>(response,
                async () =>
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    return JsonConvert.DeserializeObject<T>(content);
                },
                token
            );
        }

        protected async Task<bool> DownloadFileAsync(Uri address, Uri destination, bool overwrite = false, CancellationToken token = default(CancellationToken))
        {
            var destinationAsString = destination.ToString();
            var response = await Client.GetAsync(address, token).ConfigureAwait(false);

            return await TryProcessingResponse<bool>(response,
                async () =>
                {
                    var stream = new FileStream(destinationAsString, FileMode.Create, FileAccess.Write, FileShare.None);
                    
                    await response.Content.CopyToAsync(stream).ConfigureAwait(false);

                    return File.Exists(destinationAsString);
                },
                token
            );
        }

        protected async Task<byte[]> DownloadAsByteArrayAsync(Uri address)
        {
            return await Client.GetByteArrayAsync(address).ConfigureAwait(false);
        }

        /// <summary>
        /// Try/catch method for handling CancellationToken and HttpRequest
        /// exceptions. 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="continuation">Delegate parameter for async processing.</param>
        /// <param name="token">Cancellation token. </param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private async Task<T> TryProcessingResponse<T>(HttpResponseMessage response, Func<Task<T>> continuation, CancellationToken token = default(CancellationToken))
        {
            try
            {
                // nullref exception will not be thrown since CT is struct
                token.ThrowIfCancellationRequested();

                response.EnsureSuccessStatusCode();

                return await continuation().ConfigureAwait(false);
            }
            catch(OperationCanceledException)
            {
                Console.WriteLine("Operation canceled.");
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine($"Unsuccessful http request. Reason: {e.Message}");
            }

            return default(T);
        }
    }
}