using System;
using System.Linq;
using System.Collections.Generic;
using MediaDrip.Downloader.Exception;

namespace MediaDrip.Downloader.Web
{
    internal sealed class SourceHandler
    {
        private List<ISource> _sources;

        public SourceHandler()
        {
            _sources = new List<ISource>();
        }

        public void Add(ISource source)
        {
            // update later to log
            if(source.Client == null)
            {
                Console.WriteLine("Source Client is not set. Can't add.");

                return;
            }

            _sources.Add(source);
        }

        public void ThrowIfLookupFails(Uri address)
        {
            if(GetByAddressComparison(address) == null)
            {
                throw new SourceNotFoundException(address, $"Address lookup failed {address.ToString()}");
            }
        }

        public void Run(Uri address)
        {
            var lookupSource = GetByAddressComparison(address);

            if(lookupSource == null)
            {
                Console.WriteLine("Can't run source, not found");

                return;
            }

            lookupSource.Run(address);
        }

        private ISource GetByAddressComparison(Uri address)
        {
            var lookup = _sources
                .Where(x => Uri.Compare(x.LookupAddress, address, UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0)
                .FirstOrDefault();

            return lookup;
        }
    }
}