using CloudBrowserPuppeteerClient.Types;
using CloudBrowserPuppeteerClient.Types.Request;
using CloudBrowserPuppeteerClient.Types.Response;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CloudBrowserPuppeteerClient.Client;
internal class ApiClient : ClientBase{
    public ApiClient() : base(new Uri("https://production.cloudbrowser.ai")) {

    }

    public Task<OpenResponse> Open(string token, BrowserOptions rq = null, TimeSpan? timeout = null, CancellationToken ct = default) => Post<OpenResponse, BrowserOptions>("Open", token, rq, timeout, ct);
    public Task<SimpleResponse> Close(string token, CloseRequest rq = null, TimeSpan? timeout = null, CancellationToken ct = default) => Post<SimpleResponse, CloseRequest>("Close", token, rq, timeout, ct);
    public Task<GetResponse> Get(string token, TimeSpan? timeout = null, CancellationToken ct = default) => Get<GetResponse>("Get", token, timeout, ct);
    public Task<StartRemoteDesktopResponse> StartRemoteDesktop(string token, StartRemoteDesktopRequest rq, TimeSpan? timeout = null, CancellationToken ct = default) => Post<StartRemoteDesktopResponse, StartRemoteDesktopRequest>("StartRemoteDesktop", token, rq, timeout, ct);
    public Task<StoptRemoteDesktopResponse> StopRemoteDesktop(string token, StopRemoteDesktopRequest rq, TimeSpan? timeout = null, CancellationToken ct = default) => Post<StoptRemoteDesktopResponse, StopRemoteDesktopRequest>("StopRemoteDesktop", token, rq, timeout, ct);


    Task<TRP> Post<TRP, TRQ>(string name, string token, TRQ rq, TimeSpan? timeout = null, CancellationToken ct = default) {
        var cli = GetClient();
        return DoPost<TRP, TRQ>(
            $"{cli.BaseAddress}api/v1/browser/{name}",
            rq,
            new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } },
            timeout: timeout, ct: ct
        );
    }
    Task<TRP> Get<TRP>(string name, string token, TimeSpan? timeout = null, CancellationToken ct = default) {
        var cli = GetClient();
        return DoGet<TRP>(
            $"{cli.BaseAddress}api/v1/browser/{name}",
            new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } },
            timeout: timeout, ct: ct
        );
    }
}
