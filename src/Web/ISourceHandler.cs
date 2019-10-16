using System;
using System.Collections.Generic;

namespace MediaDrip.Downloader.Web
{
    public interface ISourceHandler
    {
        void Add(ISource source);
        void RunSourceFromAddressLookup(Uri address);
    }
}