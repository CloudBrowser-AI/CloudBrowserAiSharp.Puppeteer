using System;
using System.Threading;
using System.Threading.Tasks;
using CloudBrowser.Client;
using CloudBrowser.Types;
using CloudBrowser.Types.Response;
using PuppeteerSharp;

namespace CloudBrowser;

public class CloudBrowserPuppeteer(string _apiToken) {

    ApiClient _client = new();

    public async Task<IBrowser> LaunchAsync(BrowserOptions options = null) {
        OpenResponse rp;
        if (options != null) {
            rp = await _client.OpenAdvanced(_apiToken, options).ConfigureAwait(false);
        } else {
            rp = await _client.Open(_apiToken).ConfigureAwait(false);
        }
        switch (rp.Status) {
            case BrowserStatus.Succes:
                break;
            case BrowserStatus.Unknown:
                throw new UnknownException();
            case BrowserStatus.AuthorizationError:
                throw new AuthorizationException();
            case BrowserStatus.NoSubscription:
                throw new NoSubscriptionException();
            case BrowserStatus.NoUnits:
                throw new NoUnitsException();
            case BrowserStatus.BrowserLimit:
                throw new BrowserLimitException();
        }
        return await Puppeteer.ConnectAsync(new ConnectOptions {
            BrowserWSEndpoint = rp.Address,
            DefaultViewport = null,
            AcceptInsecureCerts = true,
            SlowMo = 0
        }).ConfigureAwait(false);
    }

    public Task<GetResponse> Get(TimeSpan? timeout = null, CancellationToken ct = default) => _client.Get(_apiToken, timeout: timeout, ct: ct);

    public Task<SimpleResponse> Close(string address, TimeSpan? timeout = null, CancellationToken ct = default) => _client.Close(_apiToken, new(address), timeout: timeout, ct: ct);

    public Task<StartRemoteDesktopResponse> StartRemoteDesktop(string address, TimeSpan? timeout = null, CancellationToken ct = default) => _client.StartRemoteDesktop(_apiToken, new (address), timeout: timeout, ct: ct);

    public Task<StoptRemoteDesktopResponse> StopRemoteDesktop(string address, TimeSpan? timeout = null, CancellationToken ct = default) => _client.StopRemoteDesktop(_apiToken, new(address), timeout: timeout,  ct: ct);

}
