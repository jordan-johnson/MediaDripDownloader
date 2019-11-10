using System;
using System.Linq;
using System.Collections.Generic;

namespace MediaDrip.Downloader.Web
{
    internal sealed class SourceHandler : ISourceControls
    {
        private List<ISource> _sources;

        public SourceHandler()
        {
            _sources = new List<ISource>();
        }

        public void AddSource(ISource source)
        {
            if(source.Client == null)
                throw new NullReferenceException($"Source client null; check source using lookup address {source.LookupAddress.ToString()}");

            var check = GetByAddressComparison(source.LookupAddress);

            if(check == null)
            {
                _sources.Add(source);
            }
        }

        public void RemoveSource(Func<ISource, bool> predicate)
        {
            var source = GetSource(predicate);

            if(source != null)
            {
                _sources.Remove(source);
            }
        }

        public ISource GetSource(Func<ISource, bool> predicate)
        {
            return _sources.FirstOrDefault(predicate);
        }

        public bool SourceExistsByAddressMatch(Uri address)
        {
            return GetByAddressComparison(address) != null;
        }

        public void RunSourceFromAddressLookup(Uri address)
        {
            var lookupSource = GetByAddressComparison(address);

            lookupSource?.RunAsync(address);
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