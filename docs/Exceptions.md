# Exceptions

Below are a list of exceptions defined in the MediaDripDownloader library.

## SourceNotFoundException

Catches an attempt to use a `Source` that doesn't exist.

### Properties

* `Uri` Address

### Use Case

In your application, you could catch the exception and display an error message stating the source is not supported.

## DuplicateDownloadException

Catches any attempts to queue a download where an existing download might be overwritten.

### Properties

* `DownloadObject` Attempted
* `DownloadObject` Existing

### Use Case

In your application, you could catch the exception and confirm if the user wants to stop the existing download and queue the new one.