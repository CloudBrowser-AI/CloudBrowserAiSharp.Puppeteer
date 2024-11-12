
namespace CloudBrowserPuppeteerClient.Types.Response;

public class StartRemoteDesktopResponse {
    public bool Success { get; set; }
    public ErrorRemoteDesktop Error { get; set; }
    public string Password { get; set; }
}