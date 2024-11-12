using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace CloudBrowserPuppeteerClient.Client;

internal static class Json {
    public const string MediaType = "application/json";

    static readonly JsonSerializerOptions opts = new () {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        AllowTrailingCommas = true
    };

    public static T ReadFrom<T>(Stream reader) {
        using StreamReader streamReader = new (reader);
        return JsonSerializer.Deserialize<T>(streamReader.ReadToEnd(), opts);
    }

    public static void WriteTo<T>(Stream writer, T body) {
        using StreamWriter streamWriter = new (writer);
        streamWriter.Write(JsonSerializer.Serialize(body, typeof(T), opts));
    }

    public static async Task<T> ReadFromAsync<T>(Stream reader, CancellationToken ct) => await JsonSerializer.DeserializeAsync<T>(reader, opts, ct).ConfigureAwait(false);

    public static Task WriteToAsync<T>(Stream writer, T body, CancellationToken ct) => JsonSerializer.SerializeAsync(writer, body, typeof(T), opts, ct);
}

internal abstract class ClientBase (Uri baseAddress) {

    protected HttpClient httpClient { get; set; } = new () {
        BaseAddress = baseAddress
    };

    protected string BaseUrl => GetClient()?.BaseAddress?.ToString().TrimEnd('/') ?? "";

    static HttpRequestMessage GenerateMessage(string url, IDictionary<string, string> addedHeaders = null, HttpMethod method = null) {
        HttpRequestMessage httpRequestMessage = new () {
            Method = method ?? HttpMethod.Get,
            RequestUri = new Uri(url)
        };

        httpRequestMessage.Headers.AcceptEncoding.TryParseAdd("deflate,br,gzip");
        httpRequestMessage.Headers.Accept.TryParseAdd("application/json");

        if (addedHeaders != null) {
            foreach (KeyValuePair<string, string> addedHeader in addedHeaders) {
                httpRequestMessage.Headers.TryAddWithoutValidation(addedHeader.Key, addedHeader.Value);
            }
        }

        return httpRequestMessage;
    }

    static async Task<TOutput> ReadResponse<TOutput>(HttpResponseMessage rs, CancellationToken ct) {
        using Stream stream = await rs.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        Stream reader = await DecompressStream(rs, stream).ConfigureAwait(false);
        return await Json.ReadFromAsync<TOutput>(reader, ct).ConfigureAwait(false);
    }

    static async Task<Stream> DecompressStream(HttpResponseMessage rs, Stream stream) {
        string text = rs.Content?.Headers?.ContentEncoding?.FirstOrDefault()?.ToLowerInvariant();
        if (text == null) {
            return stream;
        }

        Stream stream2 = text switch {
            "gzip" => new GZipStream(stream, CompressionMode.Decompress, false),
            "deflate" => new DeflateStream(stream, CompressionMode.Decompress, false),
            "br" => new BrotliStream(stream, CompressionMode.Decompress, false),
            _ => stream,
        };
        MemoryStream copy = new ();
        await stream2.CopyToAsync(copy).ConfigureAwait(false);
        copy.Position = 0L;
        return copy;
    }

    static async Task<HttpResponseMessage> ReadLogic(HttpClient cli, HttpRequestMessage rq,  CancellationToken ct, Type expectedResponseType = null) {
        Stopwatch sw = Stopwatch.StartNew();
        try {
            Task<HttpResponseMessage> rsT = cli.SendAsync(rq, HttpCompletionOption.ResponseContentRead, ct);
            return await PollTaskWithCancellationToken(ct, rsT).ConfigureAwait(false);
        } catch (OperationCanceledException) {
            throw new TaskCanceledException($"Timeout after {sw.Elapsed.TotalSeconds} seconds");
        } catch (Exception ex2) when (ex2.InnerException != null) {
            for (Exception innerException = ex2.InnerException; innerException != null; innerException = innerException.InnerException) {
                if (innerException is OperationCanceledException) {
                    throw new TaskCanceledException($"Timeout after {sw.Elapsed.TotalSeconds} seconds");
                }
            }

            throw;
        }
    }

    static async Task<T> PollTaskWithCancellationToken<T>(CancellationToken ct, Task<T> rsT, int msDelay = 20) {
        while (!rsT.IsCompleted) {
            ct.ThrowIfCancellationRequested();
            await Task.Delay(msDelay).ConfigureAwait(false);
        }

        return rsT.Result;
    }

    static async Task SerializeRequest<TInput>(HttpRequestMessage rq, TInput body, CancellationToken ct) {
        MemoryStream ms = new ();
        Stream writer = ms;
        string encodingHeader = SetRequestContentAndStream(ref writer);
        await Json.WriteToAsync(writer, body, ct).ConfigureAwait(false);
        if (writer.CanWrite) {
            await writer.FlushAsync().ConfigureAwait(false);
            await writer.DisposeAsync().ConfigureAwait(false);
        }

        rq.Content = new ByteArrayContent(ms.ToArray());
        rq.Content.Headers.ContentType = new MediaTypeHeaderValue(Json.MediaType);
        if (encodingHeader != null) {
            rq.Content.Headers.ContentEncoding.Add(encodingHeader);
        }
    }

    static async Task<TOutput> Read<TOutput>(HttpClient cli, HttpRequestMessage rq, CancellationToken ct) {
        ct.ThrowIfCancellationRequested();
        HttpResponseMessage httpResponseMessage = await ReadLogic(cli, rq, ct, typeof(TOutput)).ConfigureAwait(false);
        if (httpResponseMessage == null) {
            return default;
        }

        return await ReadResponse<TOutput>(httpResponseMessage, ct).ConfigureAwait(false);
    }

    static string SetRequestContentAndStream(ref Stream writer) {
        writer = new DeflateStream(writer, CompressionLevel.Optimal);
        return "deflate";
    }

    TimeSpan Timeout(TimeSpan? timeout) {
        return timeout ?? TimeSpan.FromMinutes(2);
    }

    protected HttpClient GetClient() {
        return httpClient ?? new HttpClient();
    }

    protected HttpClient GetClient(TimeSpan? timeout) {
        if (!timeout.HasValue || timeout == TimeSpan.FromMinutes(2)) {
            return GetClient();
        }

        HttpClient httpClient = new ();
        httpClient.Timeout = timeout.GetValueOrDefault();
        return httpClient;
    }

    protected async Task<TOutput> DoGet<TOutput>(string url, IDictionary<string, string> addedHeaders = null, TimeSpan? timeout = null, CancellationToken ct = default) {
        HttpClient client = GetClient(timeout);
        HttpRequestMessage rq = GenerateMessage(url, addedHeaders);
        return await Read<TOutput>(client, rq, ct).ConfigureAwait(false);
    }

    protected async Task<TOutput> DoPost<TOutput, TInput>(string url, TInput body, IDictionary<string, string> addedHeaders = null, TimeSpan? timeout = null, CancellationToken ct = default) {
        HttpClient cli = GetClient(timeout);
        HttpRequestMessage rq = GenerateMessage(url, addedHeaders);
        rq.Method = HttpMethod.Post;
        await SerializeRequest(rq, body, ct).ConfigureAwait(false);
        return await Read<TOutput>(cli, rq, ct).ConfigureAwait(false);
    }

    protected async Task DoPost<TInput>(string url, TInput body, TimeSpan? timeout = null, CancellationToken ct = default(CancellationToken)) {
        HttpClient cli = GetClient(timeout);
        HttpRequestMessage rq = GenerateMessage(url);
        rq.Method = HttpMethod.Post;
        await SerializeRequest(rq, body, ct).ConfigureAwait(false);
        await ReadLogic(cli, rq, ct).ConfigureAwait(false);
    }
}