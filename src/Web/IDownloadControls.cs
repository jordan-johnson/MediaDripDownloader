namespace MediaDrip.Downloader.Web
{
    /// <summary>
    /// Contract for basic download controls.
    /// </summary>
    public interface IDownloadControls
    {
        void Cancel(IWebDownload download);
        void CancelAll();
    }
}