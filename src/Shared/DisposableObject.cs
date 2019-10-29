using System;

namespace MediaDrip.Downloader.Shared
{
    public abstract class DisposableObject : IDisposable
    {
        public bool IsDisposalRequested { get; protected set; }

        ~DisposableObject()
        {
            Dispose(disposing: false);
        }

        public virtual void Dispose()
        {
            Dispose(disposing: true);

            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if(!IsDisposalRequested)
            {
                if(disposing)
                {
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();

                IsDisposalRequested = true;
            }
        }

        protected abstract void DisposeManagedResources();
        protected abstract void DisposeUnmanagedResources();
    }
}