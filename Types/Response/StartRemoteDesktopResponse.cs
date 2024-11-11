namespace CloudBrowser.Types.Response;

internal class StartRemoteDesktopResponse {
    public bool Success { get; set; }
    public ErrorRemoteDesktop Error { get; set; }
    public string Password { get; set; }
}