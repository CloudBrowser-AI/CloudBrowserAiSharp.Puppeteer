using CloudBrowserAiSharp.Browser.Types;
using CloudBrowserAiSharp;
using PuppeteerSharp;
using CloudBrowserAiSharp.Puppeteer.Browser;

namespace LaunchAsync {
    internal class Program {
        static async Task Main(string[] args) {

            var browser = await BrowserExtension.LaunchAsync("YOUR CLOUDBROWSER.AI TOKEN").ConfigureAwait(false);

            Console.WriteLine("Browser connected");

            var page = await browser.FirstPage().ConfigureAwait(false);

            await page.GoToAsync("http://www.cloudbrowser.ai").ConfigureAwait(false);
            Console.WriteLine("Web visited");

            await Task.Delay(5000).ConfigureAwait(false);

            await browser.CloseAsync().ConfigureAwait(false);

            Console.WriteLine("Browser closed");
        }
    }
}
