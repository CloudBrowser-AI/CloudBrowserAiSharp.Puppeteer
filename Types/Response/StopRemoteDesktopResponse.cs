namespace CloudBrowser.Types.Response;

internal class StoptRemoteDesktopResponse {
    public bool Success { get; set; }
    public ErrorRemoteDesktop Error { get; set; }
}