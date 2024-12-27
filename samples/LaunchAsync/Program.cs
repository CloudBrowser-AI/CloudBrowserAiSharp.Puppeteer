using CloudBrowserAiSharp.Browser.Types;
using CloudBrowserAiSharp;
using PuppeteerSharp;
using CloudBrowserAiSharp.Puppeteer.Browser;

namespace LaunchAsync {
    internal class Program {
        static async Task Main(string[] args) {

            //Request Cloud Browser AI to open a browser
            //using default settings
            var browser = await BrowserExtension.LaunchAsync("YOUR CLOUDBROWSER.AI TOKEN").ConfigureAwait(false);

            Console.WriteLine("Browser connected");

            var page = (await browser.PagesAsync().ConfigureAwait(false))[0];

            await page.GoToAsync("http://www.cloudbrowser.ai").ConfigureAwait(false);
            Console.WriteLine("Web visited");

            await Task.Delay(5000).ConfigureAwait(false);

            //You can close the browser using puppetter or CloudBrowser AI api
            //await svc.Close(rp.Address).ConfigureAwait(false);
            await browser.CloseAsync().ConfigureAwait(false);

            Console.WriteLine("Browser closed");
        }
    }
}
