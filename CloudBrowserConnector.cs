using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PuppeteerSharp;

public class CloudBrowserConnector
{
    private readonly string _apiToken;
    private readonly string _serverUrl = "https://production.cloudbrowser.ai/api/v1/Browser/OpenAdvanced";
    private readonly object _body;

    public CloudBrowserConnector(string apiToken, object body = null)
    {
        _apiToken = apiToken;
        _body = body;
    }

    public async Task<IBrowser> ConnectAsync() // Cambiado a IBrowser
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

        // Configurar la solicitud JSON
        var content = _body != null
            ? new StringContent(JsonSerializer.Serialize(_body), Encoding.UTF8, "application/json")
            : null;

        var response = await client.PostAsync(_serverUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch WebSocket URL: {response.ReasonPhrase}");
        }

        var webSocketDebuggerUrl = await response.Content.ReadAsStringAsync();

        // Conectar PuppeteerSharp al navegador remoto
        var browser = await Puppeteer.ConnectAsync(new ConnectOptions
        {
            BrowserWSEndpoint = webSocketDebuggerUrl
        });

        return browser; // Devuelve IBrowser
    }
}
