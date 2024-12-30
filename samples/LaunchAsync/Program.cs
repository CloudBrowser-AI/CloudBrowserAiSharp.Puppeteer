using CloudBrowserAiSharp.Puppeteer.Browser;
using CloudBrowserAiSharp.Puppeteer.Extensions;

namespace LaunchAsync;
internal class Program {
    static async Task Main(string[] args) {
        //Alternatively you can use a previously created service to launch it
        //using BrowserService svc = new(token);
        //var browser = await svc.LaunchAsync().ConfigureAwait(false);

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
