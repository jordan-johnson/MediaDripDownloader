namespace MediaDrip.Downloader.Web
{
    public class DownloadOptions
    {
        /// <summary>
        /// Flag to determine if download will be processed as soon as it's queued.
        /// </summary>
        public bool DownloadImmediately;

        /// <summary>
        /// Flag to determine if download should overwrite any existing files.
        /// </summary>
        public bool Overwrite;

        /// <summary>
        /// Assign default values to options and return this object.
        /// </summary>
        /// <returns></returns>
        public DownloadOptions UseDefaults()
        {
            DownloadImmediately = true;
            Overwrite = false;

            return this;
        }
    }
}