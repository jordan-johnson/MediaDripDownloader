# DownloadObject

> using `MediaDrip.Downloader.Shared;`

A `DownloadObject` is a simple model used for storing information about a download.

## Properties

`Uri DownloadAddress` refers to the end-user's address to later be processed by a `Source`.

`Uri SaveDestination` is the full path (directory, file name, and extension) where this download will be saved.

`bool AutoDownload` is an option to automatically begin processing and downloading. If false, the `DownloadObject` will remain in the queue until manually called via `MEDIADRIPDOWNLOADER.FORCESTART -- method name here`.

`bool IsDownloading` checks if the download is currently active.

`int Progress` refers to the current download progress percentage.

## Methods

`void RequestCancellation()` will attempt to cancel a private CancellationTokenSource thus stopping the download process.