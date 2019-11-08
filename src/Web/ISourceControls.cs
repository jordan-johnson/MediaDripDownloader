namespace MediaDrip.Downloader.Web
{
    /// <summary>
    /// Contract for basic source controls.
    /// </summary>
    public interface ISourceControls
    {
        void AddSource(ISource source);
    }
}