
namespace CloudBrowser.Types.Request;

internal class CloseRequest {
    public string Address { get; set; }

    public CloseRequest() { }

    public CloseRequest(string address) {
        Address = address;
    }
}
