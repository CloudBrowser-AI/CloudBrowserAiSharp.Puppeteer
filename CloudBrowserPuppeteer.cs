using System.Linq.Expressions;
using System.Threading.Tasks;
using CloudBrowser.Client;
using CloudBrowser.Types;
using CloudBrowser.Types.Response;
using PuppeteerSharp;

namespace CloudBrowser;

public class CloudBrowserPuppeteer(string _apiToken) { 
    public async Task<IBrowser> LaunchAsync(BrowserOptions options = null) {
        ApiClient client = new();
        OpenResponse rp;
        if (options != null) {
            rp = await client.OpenAdvanced(_apiToken, options).ConfigureAwait(false);
        } else {
            rp = await client.Open(_apiToken).ConfigureAwait(false);
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
}
