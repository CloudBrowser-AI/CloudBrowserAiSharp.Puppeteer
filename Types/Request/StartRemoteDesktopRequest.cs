
namespace CloudBrowserPuppeteerClient.Types.Request;

internal class StartRemoteDesktopRequest {
    public string Address { get; set; }

    public StartRemoteDesktopRequest() { }

    public StartRemoteDesktopRequest(string address) {
        Address = address;
    }
}
