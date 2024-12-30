using CloudBrowserAiSharp.Puppeteer.Browser;
using CloudBrowserAiSharp.Puppeteer.Extensions;

namespace PuppetterHelpers;

internal class Program {
    static async Task Main(string[] args) {
        var browser = await BrowserExtension.LaunchAsync("YOUR CLOUDBROWSER.AI TOKEN").ConfigureAwait(false);
        Console.WriteLine("Browser connected");

        //When opening a new browser there is always a tab, so
        //we added this "FirstPage" function to get this tab easier
        var page = await browser.FirstPage().ConfigureAwait(false);

        await page.GoToAsync("http://www.cloudbrowser.ai").ConfigureAwait(false);
        Console.WriteLine("Web visited");

        //Scrolling
        var scrollHeight = await page.ScrollHeight().ConfigureAwait(false);
        Console.WriteLine("Scroll height is:{0}", scrollHeight);
        await page.ScrollToBottom().ConfigureAwait(false);
        Console.WriteLine("Scrolled to the bottom");
        await page.ScrollTop().ConfigureAwait(false);
        Console.WriteLine("Scrolled to the top");
        await page.ScrollToBottom(step_pixels:20, max_steps:50, milisecondsBetweenSteps:100).ConfigureAwait(false);
        Console.WriteLine("Scrolled to the bottom slowly");

        //Using a selector directly on ClickAsync
        var e = await page.QuerySelectorAsync("body").ConfigureAwait(false);
        await e.ClickAsync("h1").ConfigureAwait(false);
        Console.WriteLine("Title clicked");

        //This task will end when one selector stop being valid
        var t = page.WaitForSelectorDoesntExistAsync(".wp-element-button");
        await page.GoToAsync("https://app.cloudbrowser.ai/login");
        await t.ConfigureAwait(false);
        Console.WriteLine("Visited login page");

        //Check if a selector exists
        var b = await page.ExistsSelector(".wp-element-button");
        Console.WriteLine("Does .wp-element-button exists?:{a}", b);

        //Easier to obtain html and text
        e = await page.QuerySelectorAsync("#root").ConfigureAwait(false);
        var ihtml = await e.InnerHTML().ConfigureAwait(false);;
        var itext = await e.InnerText().ConfigureAwait(false);;
        var ohtml = await e.OuterHTML().ConfigureAwait(false); ;

        //Easier to obtain attributes
        e = await page.QuerySelectorAsync("a").ConfigureAwait(false);
        var a = await e.Attribute("href").ConfigureAwait(false);
        Console.WriteLine("A href:{0}",a);

        //Easier to obtain classes
        var classes = await e.Classes().ConfigureAwait(false);

        //Easier to select nodes, elements and ancestors
        var fe = await e.FirstChildElement().ConfigureAwait(false);
        var fn = await e.FirstChildNode().ConfigureAwait(false);
        var ancestor = await e.ClosestAncestor("body").ConfigureAwait(false);

        await browser.CloseAsync().ConfigureAwait(false);
        Console.WriteLine("Browser closed");
    }
}
