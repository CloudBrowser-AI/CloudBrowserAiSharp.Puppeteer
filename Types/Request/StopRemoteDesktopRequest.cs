
namespace CloudBrowserPuppeteerClient.Types.Request;

internal class StopRemoteDesktopRequest {
    public string Address { get; set; }

    public StopRemoteDesktopRequest() { }

    public StopRemoteDesktopRequest(string address) {
        Address = address;
    }
}
