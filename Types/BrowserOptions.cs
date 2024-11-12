using PuppeteerSharp;

namespace CloudBrowserPuppeteerClient.Types;

public class BrowserOptions {
    public string[] Args { get; set; }
    public string[] IgnoredDefaultArgs { get; set; }
    public bool? Headless { get; set; }
    public byte[][] Extensions { get; set; }
    public bool? Stealth { get; set; }
    public SupportedBrowser? Browser { get; set; }
    public BrowserOptionsProxy Proxy { get; set; }
    public int? KeepOpen { get; set; }
    public string Label { get; set; }
}