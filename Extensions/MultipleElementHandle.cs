using PuppeteerSharp;
using System.Threading.Tasks;

namespace CloudBrowserAiSharp.Puppeteer.Extensions;
/// <summary>
/// Represents a handle that can contain an element, text, or other types of nodes.
/// </summary>
public class MultipleElementHandle {
    /// <summary>
    /// Gets or sets the text content of the node.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the element handle of the node.
    /// </summary>
    public IElementHandle Element { get; set; }

    /// <summary>
    /// Gets or sets the handle for other types of nodes.
    /// </summary>
    public IJSHandle Other { get; set; }

    /// <summary>
    /// </summary>
    public MultipleElementHandle() { }

    /// <summary>
    /// Sets the data for the handle based on the node type.
    /// </summary>
    /// <param name="jsHandle">The JavaScript handle to evaluate.</param>
    public async Task SetData(IJSHandle jsHandle) {
        var nodeType = await jsHandle.EvaluateFunctionAsync<string>("node => node.nodeType === 1 ? 'Element' : node.nodeType === 3 ? 'Text' : 'Other'").ConfigureAwait(false);

        switch (nodeType) {
            case "Element":
                Element = jsHandle as IElementHandle;
                break;
            case "Text":
                Text = await jsHandle.EvaluateFunctionAsync<string>("node => node.textContent").ConfigureAwait(false);
                break;
            default:
                Other = jsHandle;
                break;
        }
    }

    /// <summary>
    /// Creates a MultipleElementHandle from the provided JavaScript handle.
    /// </summary>
    /// <param name="jsHandle">The JavaScript handle to convert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the MultipleElementHandle.</returns>
    public static async Task<MultipleElementHandle> FromData(IJSHandle jsHandle) {
        MultipleElementHandle m = new();
        await m.SetData(jsHandle).ConfigureAwait(false);
        return m;
    }
}
