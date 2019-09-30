# Source

> using `MediaDrip.Downloader.Web;`

A `Source` is an extension/child class for configuring and downloading files on a per-site basis. For example, you may have a YouTube source, SoundCloud source, etc.

Each `Source` should inherit `MediaDrip.Downloader.Web.ISource` or `MediaDrip.Downloader.Web.SourceUtilities`.

## MediaDrip.Downloader.Web.ISource

The `ISource` interface includes a contract of a Uri look-up address, an HttpClient, and an implementation of `Task<Uri> Run(Uri address)`.

### Properties / Methods

The `Uri LookupAddress` property is used internally when a download request is made by finding a similarity in the host addresses, and executing the `Run(Uri address)` task for that `Source`.

The `HttpClient Client` property is meant for re-use and should only be initialized once per `Source` during the application's lifetime. **_DO NOT_** wrap the HttpClient in a using block -- MediaDrip will handle disposing the client automatically once the application is closing. For asynchronous downloading, simply await a method from the client, and *make sure* to append `ConfigureAwait(false)` to prevent a deadlock on the synchronization context.

<< to do -- explain the Run() method >>

### Quick Example

```cs
public class SomeSiteSource : ISource
{
    // When a request to https://www.somesite.com/somefile.jpg is made,
    // this address will be used internally to find this source.
    public override Uri LookupAddress => new Uri("https://www.somesite.com");

    // Initialize only once!
    public HttpClient Client { get; private set; }

    public SomeSite()
    {
        Client = new HttpClient();
    }

    public override async Task<Uri> Run(Uri initialAddress)
    {
        // ConfigureAwait(false) will prevent a deadlock on the
        // synchronization context.
        var response = await Client.GetAsync(address).ConfigureAwait(false);

        // ... manipulate the data from the response to form a
        // ... Uri that will be returned.

        return new Uri("https://www.somesite.com/");
    }
}
```

## MediaDrip.Downloader.Web.SourceUtilities

The `SourceUtilities` class provides tools for common downloading methods. For example, you may need to download a JSON object online and route it a C# object for later use. [MORE LATER]