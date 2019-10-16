using System;
using System.Linq;
using System.Collections.Generic;
using MediaDrip.Downloader.Exception;

namespace MediaDrip.Downloader.Web
{
    internal sealed class SourceHandler : ISourceHandler
    {
        private List<ISource> _sources;

        public SourceHandler()
        {
            _sources = new List<ISource>();
        }

        public void Add(ISource source)
        {
            if(source.Client == null)
                throw new NullReferenceException($"Source client null; check source using lookup address {source.LookupAddress.ToString()}");

            _sources.Add(source);
        }

        public void RunSourceFromAddressLookup(Uri address)
        {
            var lookupSource = GetByAddressComparison(address);

            if(lookupSource == null)
                throw new SourceNotFoundException(address, $"Source lookup failed {address.ToString()}");

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