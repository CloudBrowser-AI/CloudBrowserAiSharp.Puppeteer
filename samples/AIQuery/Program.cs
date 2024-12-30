using CloudBrowserAiSharp.Puppeteer.AIExtensions;
using CloudBrowserAiSharp.Puppeteer.Browser;
using CloudBrowserAiSharp.Puppeteer.Extensions;

namespace AI;

internal class Program {

    const string cloudBrowserToken = "YOUR CLOUDBROWSER.AI TOKEN";
    const string openAiToken = "YOUR OPEN AI TOKEN";

    static async Task Main(string[] args) {
        AIExtensions.SetGlobalSettings(cloudBrowserToken, openAiToken);

        var browser = await BrowserExtension.LaunchAsync(cloudBrowserToken).ConfigureAwait(false);
        Console.WriteLine("Browser connected");

        var page = await browser.FirstPage().ConfigureAwait(false);

        await page.GoToAsync("http://www.cloudbrowser.ai").ConfigureAwait(false);
        Console.WriteLine("Web visited");

        //Query
        var price = await page.Query<decimal>("Give me the lowest price").ConfigureAwait(false);

        //Summarize
        var summary = await page.Summarize<string>().ConfigureAwait(false);

        //Translate
        var e = await page.QuerySelectorAsync("h1").ConfigureAwait(false);
        var translated = await e.Translate<string>("ES").ConfigureAwait(false);

        //Optimize
        var optimized = await e.Optimize<string>("Title").ConfigureAwait(false);

        //To
        var data = await page.To<CustomType>().ConfigureAwait(false);
        var json = await page.ToJSON().ConfigureAwait(false);
        var markdown = await page.ToMarkdown().ConfigureAwait(false);
        var csv = await page.ToCSV().ConfigureAwait(false);

        await browser.CloseAsync().ConfigureAwait(false);
        Console.WriteLine("Browser closed");
    }
}

class CustomType {
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Paragraphs { get; set; }
}
