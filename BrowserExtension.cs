using CloudBrowserAiSharp.Browser.Types;
using CloudBrowserAiSharp.Exceptions;
using PuppeteerSharp;
using System.Threading.Tasks;

namespace CloudBrowserAiSharp.Puppeteer;
public static class BrowserExtension {
    /// <summary>
    /// Launches a browser asynchronously in CloudbRowser.AI
    /// </summary>
    /// <param name="options">Options for launching the browser.</param>
    /// <returns>An IBrowser instance.</returns>
    public static async Task<IBrowser> LaunchAsync(this BrowserService client, BrowserOptions options = null) {
        var rp = await client.Open(options).ConfigureAwait(false);
        ExceptionHelper.ToException(rp.Status, null);

        IBrowser browser = await PuppeteerSharp.Puppeteer.ConnectAsync(new ConnectOptions {
            BrowserWSEndpoint = rp.Address,
            DefaultViewport = null,
            AcceptInsecureCerts = true,
            SlowMo = 0
        }).ConfigureAwait(continueOnCapturedContext: false);

        return browser;
    }

}
