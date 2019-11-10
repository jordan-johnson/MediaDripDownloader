using System;

namespace MediaDrip.Downloader.Web
{
    /// <summary>
    /// Contract for basic source controls.
    /// </summary>
    public interface ISourceControls
    {
        void AddSource(ISource source);
        void RemoveSource(Func<ISource, bool> predicate);
        ISource GetSource(Func<ISource, bool> predicate);
    }
}