
namespace CloudBrowser.Types;

public enum BrowserStatus {
    Unknown = 0,
    Succes = 200,
    AuthorizationError = 401,
    NoSubscription = 402,
    NoUnits = 403,
    BrowserLimit = 404
}