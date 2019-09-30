using System;
using System.IO;

namespace MediaDrip.Downloader.Shared
{
    public static class PathUtility
    {
        public static bool CreateDirectoryIfNotExists(Uri path)
        {
            var getDirectoryName = Path.GetDirectoryName(path.AbsolutePath);

            if(!Directory.Exists(getDirectoryName))
            {
                Directory.CreateDirectory(getDirectoryName);
            }

            return Directory.Exists(getDirectoryName);
        }
    }
}