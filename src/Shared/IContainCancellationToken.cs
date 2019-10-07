using System.Threading;

namespace MediaDrip.Downloader.Shared
{
    public interface IContainCancellationToken
    {
        CancellationTokenSource CancellationToken { get; }
    }
}