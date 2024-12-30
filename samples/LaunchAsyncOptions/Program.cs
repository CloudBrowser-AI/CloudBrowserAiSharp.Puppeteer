using CloudBrowserAiSharp.Exceptions;
using CloudBrowserAiSharp.Puppeteer.Browser;
using CloudBrowserAiSharp.Puppeteer.Extensions;
using PuppeteerSharp;

namespace LaunchAsyncOptions;

internal class Program {
    static async Task Main(string[] args) {
        IBrowser browser = null;
        try {
            //Alternatively you can use a previously created service to launch it
            //using BrowserService svc = new(token);
            //browser = await svc.LaunchAsync(options).ConfigureAwait(false);

            browser = await BrowserExtension.LaunchAsync(
                "YOUR CLOUDBROWSER.AI TOKEN",
                new() {
                    Label = "MyCustomBrowser",
                    //Chromium is supported but we recommend Chrome for best stealth
                    Browser = CloudBrowserAiSharp.Browser.Types.SupportedBrowser.Chromium,
                    KeepOpen = 10 * 60,//This browser will close after 10 minutes without any Puppeteer connected.
                    Proxy = new() {
                        Host = "IP.0.0.0.1",
                        Port = "3000",
                        Password = "password",
                        Username = "username",
                    }
                }
                ).ConfigureAwait(false);
        } catch (AuthorizationException) {
            Console.WriteLine("Wrong token");
            return;
        } catch (NoSubscriptionException) {
            Console.WriteLine("No subscription");
            return;
        } catch (NoUnitsException) {
            Console.WriteLine("No enought units");
            return;
        } catch (BrowserLimitException) {
            Console.WriteLine("Too many browsers open");
            return;
        } catch (UnknownException) {
            Console.WriteLine("Unknown error");
            return;
        }
        Console.WriteLine("Browser connected");

        var page = await browser.FirstPage().ConfigureAwait(false);

        await page.GoToAsync("http://www.cloudbrowser.ai").ConfigureAwait(false);
        Console.WriteLine("Web visited");

        await Task.Delay(5000).ConfigureAwait(false);

        await browser.CloseAsync().ConfigureAwait(false);
        Console.WriteLine("Browser closed");
    }
}
