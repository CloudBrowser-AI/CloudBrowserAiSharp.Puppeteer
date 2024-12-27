using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBrowserAiSharp.Puppeteer.Extensions;
public static class HelperExtensions {
    #region Click
    /// <summary>
    /// The method runs <c>element.querySelector</c> within the page. Scrolls element into view if needed, and then uses <see cref="IPage.Mouse"/> to click in the center of the element.
    /// </summary>
    /// <param name="selector">A selector to query element for.</param>
    /// <returns>Task which resolves to <see cref="IElementHandle"/> pointing to the frame element.</returns>
    public static async Task ClickAsync(this IElementHandle element, string selector, ClickOptions options = null) {
        await (
            await element.QuerySelectorAsync(selector).ConfigureAwait(false)
        ).ClickAsync(options).ConfigureAwait(false);
    }
    #endregion

    #region DragAndDrop
    /// <summary>
    /// The method waits for the specified origin and destination selectors to be available on the page, and then performs a drag-and-drop operation from the origin to the destination.
    /// </summary>
    /// <param name="originSelector">A selector to query the origin element for.</param>
    /// <param name="destinationSelector">A selector to query the destination element for.</param>
    public static async Task DragAndDrop(this IPage page, string originSelector, string destinationSelector) {
        await page.WaitForSelectorAsync(originSelector).ConfigureAwait(false);
        await page.WaitForSelectorAsync(destinationSelector).ConfigureAwait(false);

        var origin = await page.QuerySelectorAsync(originSelector).ConfigureAwait(false);
        var destination = await page.QuerySelectorAsync(destinationSelector).ConfigureAwait(false);

        var originBox = await origin.BoundingBoxAsync().ConfigureAwait(false);
        var destinationBox = await destination.BoundingBoxAsync().ConfigureAwait(false);

        await page.Mouse.MoveAsync(originBox.X + originBox.Width / 2, originBox.Y + originBox.Height / 2).ConfigureAwait(false);
        await page.Mouse.DownAsync().ConfigureAwait(false);
        await page.Mouse.MoveAsync(destinationBox.X + destinationBox.Width / 2, destinationBox.Y + destinationBox.Height / 2).ConfigureAwait(false);
        await page.Mouse.UpAsync().ConfigureAwait(false);
    }

    #endregion

    #region ExistsSelector
    /// <summary>
    /// Checks if the specified selector exists on the page.
    /// </summary>
    /// <param name="selector">A selector to query elements for.</param>
    /// <returns>True if the selector exists; otherwise, false.</returns>
    public async static Task<bool> ExistsSelector(this IPage page, string selector) {
        return (await page.QuerySelectorAllAsync(selector).ConfigureAwait(false)).Length > 0;
    }

    /// <summary>
    /// Checks if the specified selector exists within the frame.
    /// </summary>
    /// <param name="selector">A selector to query elements for.</param>
    /// <returns>True if the selector exists; otherwise, false.</returns>
    public async static Task<bool> ExistsSelector(this IFrame page, string selector) {
        return (await page.QuerySelectorAllAsync(selector).ConfigureAwait(false)).Length > 0;
    }

    /// <summary>
    /// Checks if the specified selector exists within the element.
    /// </summary>
    /// <param name="selector">A selector to query elements for.</param>
    /// <returns>True if the selector exists; otherwise, false.</returns>
    public async static Task<bool> ExistsSelector(this IElementHandle e, string selector) {
        return (await e.QuerySelectorAllAsync(selector).ConfigureAwait(false)).Length > 0;
    }

    #endregion

    #region FirstPage
    /// <summary>
    /// Retrieves the first page of the browser.
    /// </summary>
    /// <returns>The first page of the browser.</returns>
    public static async Task<IPage> FirstPage(this IBrowser browser) {
        var p = await browser.PagesAsync().ConfigureAwait(false);
        return p[0];
    }

    #endregion

    #region InnerHTML
    /// <summary>
    /// Retrieves the inner HTML of the specified element.
    /// </summary>
    /// <returns>The inner HTML of the element.</returns>
    public static async Task<string> InnerHTML(this IElementHandle e) {
        try {
            return await e.EvaluateFunctionAsync<string>("e => e.innerHTML").ConfigureAwait(false);
        } catch {
            return null;
        }
    }

    /// <summary>
    /// The method runs <c>element.querySelector</c> within another element and then retrieves the inner HTML of the specified element.
    /// </summary>
    /// <param name="selector">A selector to query the target element for.</param>
    /// <returns>The inner HTML of the target element.</returns>
    public static async Task<string> InnerHTML(this IElementHandle e, string selector) {
        try {
            return await e.EvaluateFunctionAsync<string>("e => e.querySelector('" + selector + "').innerHTML").ConfigureAwait(false);
        } catch {
            return null;
        }
    }

    /// <summary>
    /// The method runs <c>document.querySelector</c> within the page and then retrieves the inner HTML of the specified element on the page.
    /// </summary>
    /// <param name="selector">A selector to query the element for.</param>
    /// <returns>The inner HTML of the element.</returns>
    public static async Task<string> InnerHTML(this IPage e, string selector) {
        try {
            return await e.EvaluateExpressionAsync<string>("document.querySelector('" + selector + "').innerHTML").ConfigureAwait(false);
        } catch {
            return null;
        }
    }

    #endregion

    #region InnerText
    /// <summary>
    /// The method runs <c>document.querySelector</c> within the page and then retrieves the inner text of the specified element on the page.
    /// </summary>
    /// <param name="selector">A selector to query the element for.</param>
    /// <returns>The inner text of the element.</returns>
    public static async Task<string> InnerText(this IPage page, string selector) {
        try {
            return await page.EvaluateExpressionAsync<string>("document.querySelector('" + selector + "').innerText").ConfigureAwait(false);
        } catch {
            return null;
        }
    }
    /// <summary>
    /// Retrieves the inner text of the page.
    /// </summary>
    /// <returns>The inner text of the page.</returns>
    public static async Task<string> InnerText(this IPage page) {
        try {
            return await page.EvaluateExpressionAsync<string>("e => e.innerText").ConfigureAwait(false);
        } catch {
            return null;
        }
    }
    /// <summary>
    /// The method runs <c>element.querySelector</c> within another element and then retrieves the inner text of the specified element.
    /// </summary>
    /// <param name="selector">A selector to query the target element for.</param>
    /// <returns>The inner text of the target element.</returns>
    public static async Task<string> InnerText(this IElementHandle e, string selector) {
        try {
            return await e.EvaluateFunctionAsync<string>("e => e.querySelector('" + selector + "').innerText").ConfigureAwait(false);
        } catch {
            return null;
        }
    }
    /// <summary>
    /// The method runs <c>element.querySelector</c> within another element and then retrieves the inner text of the specified element.
    /// </summary>
    /// <returns>The inner text of the target element.</returns>
    public static async Task<string> InnerText(this IElementHandle e) {
        try {
            return await e.EvaluateFunctionAsync<string>("e => e.innerText").ConfigureAwait(false);
        } catch {
            return null;
        }
    }

    #endregion

    #region OuterHTML
    /// <summary>
    /// Retrieves the outer HTML of the specified element.
    /// </summary>
    /// <returns>The outer HTML of the element.</returns>
    public static async Task<string> OuterHTML(this IElementHandle e) {
        try {
            return await e.EvaluateFunctionAsync<string>("e => e.outerHTML").ConfigureAwait(false);
        } catch {
            return null;
        }
    }

    /// <summary>
    /// The method runs <c>element.querySelector</c> within another element and then retrieves the outer HTML of the specified element.
    /// </summary>
    /// <param name="selector">A selector to query the target element for.</param>
    /// <returns>The outer HTML of the target element.</returns>
    public static async Task<string> OuterHTML(this IElementHandle e, string selector) {
        try {
            return await e.EvaluateFunctionAsync<string>("e => e.querySelector('" + selector + "').outerHTML").ConfigureAwait(false);
        } catch {
            return null;
        }
    }

    /// <summary>
    /// The method runs <c>document.querySelector</c> within the page and then retrieves the outer HTML of the specified element on the page.
    /// </summary>
    /// <param name="selector">A selector to query the element for.</param>
    /// <returns>The outer HTML of the element.</returns>
    public static async Task<string> OuterHTML(this IPage e, string selector) {
        try {
            return await e.EvaluateExpressionAsync<string>("document.querySelector('" + selector + "').outerHTML").ConfigureAwait(false);
        } catch {
            return null;
        }
    }

    #endregion

    #region Variables
    /// <summary>
    /// Retrieves the value of the specified attribute of the element.
    /// </summary>
    /// <param name="e">The element whose attribute is to be retrieved.</param>
    /// <param name="name">The name of the attribute to retrieve.</param>
    /// <returns>The value of the attribute.</returns>
    public static async Task<string> Attribute(this IElementHandle e, string name) {
        try {
            return await e.EvaluateFunctionAsync<string>("(el, attr) => el.getAttribute(attr)", name).ConfigureAwait(false);
        } catch {
            return null;
        }
    }


    /// <summary>
    /// Retrieves the value of the "id" attribute of the specified element.
    /// </summary>
    /// <param name="e">The element whose "id" attribute is to be retrieved.</param>
    /// <returns>The "id" attribute.</returns>
    public static Task<string> Id(this IElementHandle e) => e.Attribute("id");


    /// <summary>
    /// Retrieves the class names of the specified element.
    /// </summary>
    /// <returns>Array of class names.</returns>
    public static async Task<string[]> Classes(this IElementHandle e) {
        try {
            string classes = await (await e.GetPropertyAsync("className").ConfigureAwait(false)).JsonValueAsync<string>();
            return classes.Split(' ');
        } catch {
            return null;
        }
    }

    /// <summary>
    /// Evaluates whether the specified element is checked.
    /// </summary>
    /// <returns>A boolean indicating whether the element is checked.</returns>
    public static Task<bool> IsChecked(this IElementHandle e) => e.EvaluateFunctionAsync<bool>("e => e.checked");


    /// <summary>
    /// Evaluates the number of child elements of the specified element.
    /// </summary>
    /// <returns>The number of child elements.</returns>
    public static Task<int> ChildElementCount(this IElementHandle element) => element.EvaluateFunctionAsync<int>("e=>e.childElementCount");

    #endregion

    #region WaitForSelectorDoesntExistAsync
    /// <summary>
    /// Waits for the specified selector to not exist on the page.
    /// </summary>
    /// <param name="selector">A selector to query element for.</param>
    /// <param name="options">Optional waiting parameters.</param>
    /// <returns>Task which resolves when the selector does not exist.</returns>
    /// <seealso cref="IPage.WaitForFunctionAsync(string, WaitForFunctionOptions, object[])"/>
    public static Task WaitForSelectorDoesntExistAsync(this IPage page, string selector, WaitForFunctionOptions options = null) {
        return page.WaitForFunctionAsync(
            "selector => !document.querySelector(selector)",
            new WaitForFunctionOptions {
                PollingInterval = options?.PollingInterval ?? 10,
                Timeout = options?.Timeout
            },
                selector
            );
    }

    /// <summary>
    /// Waits for the specified selector to not exist in the frame.
    /// </summary>
    /// <param name="selector">A selector to query element for.</param>
    /// <param name="options">Optional waiting parameters.</param>
    /// <returns>Task which resolves when the selector does not exist.</returns>
    /// <seealso cref="IFrame.WaitForFunctionAsync(string, WaitForFunctionOptions, object[])"/>
    public static Task WaitForSelectorDoesntExistAsync(this IFrame page, string selector, WaitForFunctionOptions options = null) {
        return page.WaitForFunctionAsync(
            "selector => !document.querySelector(selector)",
            new WaitForFunctionOptions {
                PollingInterval = options?.PollingInterval ?? 10,
                Timeout = options?.Timeout
            },
                selector
            );
    }
    #endregion

    #region Scroll
    /// <summary>
    /// Scrolls the element into view with optional smooth behavior and offset.
    /// </summary>
    /// <param name="element">The element to scroll into view.</param>
    /// <param name="options">Optional scroll options including offset.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task ScrollIntoView(this IElementHandle element, ScrollIntoViewOptions options = null) {
        if (options == null) {
            return element.EvaluateFunctionAsync("(element,) => element.scrollIntoView({behavior: 'smooth'})");//Behaviour smooth helps to simulate one human
        } else {
            return element.EvaluateFunctionAsync(@"
                (element, offsetX, offsetY) => {
                    var elementPosition = element.getBoundingClientRect();
                    var offsetPositionX = elementPosition.left + window.pageXOffset - offsetX;
                    var offsetPositionY = elementPosition.top + window.pageYOffset - offsetY;

                    window.scrollTo({
                        left: offsetPositionX,
                        top: offsetPositionY,
                        behavior: 'smooth'
                    });
                }", options.OffsetX, options.OffsetY);
        }
    }
    /// <summary>
    /// Scrolls the element matching the selector into view with optional smooth behavior and offset.
    /// </summary>
    /// <param name="page">The page containing the element.</param>
    /// <param name="selector">The selector of the element to scroll into view.</param>
    /// <param name="options">Optional scroll options including offset.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ScrollIntoView(this IPage page, string selector, ScrollIntoViewOptions options = null) {
        var element = await page.QuerySelectorAsync(selector).ConfigureAwait(false);
        await element.ScrollIntoView(options).ConfigureAwait(false);
    }
    /// <summary>
    /// Scrolls the page down by the specified number of pixels.
    /// </summary>
    /// <param name="page">The page to scroll.</param>
    /// <param name="pixels">The number of pixels to scroll down.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task ScrollDownByAsync(this IPage page, int pixels) => page.EvaluateExpressionAsync($"window.scrollTo(0, {pixels})");
    /// <summary>
    /// Gets the scroll height of the page.
    /// </summary>
    /// <param name="page">The page to get the scroll height of.</param>
    /// <returns>The scroll height of the page.</returns>
    public static Task<int> ScrollHeight(this IPage page) => page.EvaluateExpressionAsync<int>("document.body.scrollHeight");
    /// <summary>
    /// Gets the scroll top position of the page.
    /// </summary>
    /// <param name="page">The page to get the scroll top position of.</param>
    /// <returns>The scroll top position of the page.</returns>
    public static Task<int> ScrollTop(this IPage page) => page.EvaluateExpressionAsync<int>("document.documentElement.scrollTop");
    /// <summary>
    /// Scrolls the page to the bottom with optional step size and delay between steps.
    /// </summary>
    /// <param name="page">The page to scroll.</param>
    /// <param name="step_pixels">The number of pixels to scroll per step. If less than or equal to 0, scrolls to the bottom in one step.</param>
    /// <param name="max_steps">The maximum number of steps to take.</param>
    /// <param name="milisecondsBetweenSteps">The delay in milliseconds between steps.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ScrollToBottom(this IPage page, int step_pixels = -1, uint max_steps = 30, int milisecondsBetweenSteps = 100) {
        if (step_pixels <= 0) {
            await page.EvaluateExpressionAsync("window.scrollBy(0, document.body.scrollHeight)").ConfigureAwait(false);
            return;
        }
        var posBefore = await page.ScrollTop().ConfigureAwait(false);
        int stepCount = 0;
        while (stepCount < max_steps) {
            await page.ScrollDownByAsync(step_pixels).ConfigureAwait(false);
            if (milisecondsBetweenSteps >= 0) {
                await Task.Delay(milisecondsBetweenSteps).ConfigureAwait(false);
            }
            var posNow = await page.ScrollTop().ConfigureAwait(false);
            if (posNow == posBefore)
                break;
            posBefore = posNow;
            stepCount++;
        }
    }
    #endregion

    #region Selection
    /// <summary>
    /// Retrieves the first child node of the element, which can be an element or text.
    /// </summary>
    /// <param name="element">The element whose first child node will be retrieved.</param>
    /// <returns>The first child node as a MultipleElementHandle.</returns>
    public static async Task<MultipleElementHandle> FirstChildNode(this IElementHandle element) => await MultipleElementHandle.FromData(await element.EvaluateFunctionHandleAsync("element => element.firstChild").ConfigureAwait(false)).ConfigureAwait(false);

    /// <summary>
    /// Retrieves all child nodes of the element, which can include elements and text.
    /// </summary>
    /// <param name="element">The element whose child nodes will be retrieved.</param>
    /// <returns>An array of child nodes as MultipleElementHandle.</returns>
    public static async Task<MultipleElementHandle[]> ChildNodes(this IElementHandle element) => await ToMultipleElementHandleArray(await element.EvaluateFunctionHandleAsync("element => Array.from(element.childNodes)").ConfigureAwait(false)).ConfigureAwait(false);

    /// <summary>
    /// Retrieves the first child element of the element, excluding text nodes.
    /// </summary>
    /// <param name="element">The element whose first child element will be retrieved.</param>
    /// <returns>The first child element as an IElementHandle.</returns>
    public static async Task<IElementHandle> FirstChildElement(this IElementHandle element) => await element.EvaluateFunctionHandleAsync("element => element.firstElementChild").ConfigureAwait(false) as IElementHandle;

    /// <summary>
    /// Retrieves all child elements of the element, excluding text nodes.
    /// </summary>
    /// <param name="element">The element whose child elements will be retrieved.</param>
    /// <returns>An array of child elements as IElementHandle.</returns>
    public static async Task<IElementHandle[]> ChildElement(this IElementHandle element) => await ToIElementHandleArray(await element.EvaluateFunctionHandleAsync("element => Array.from(element.children)").ConfigureAwait(false)).ConfigureAwait(false);

    /// <summary>
    /// Retrieves the parent element of the specified element.
    /// </summary>
    /// <param name="element">The element whose parent will be retrieved.</param>
    /// <returns>The parent element as an IElementHandle.</returns>
    public static async Task<IElementHandle> Parent(this IElementHandle element) => await element.EvaluateFunctionHandleAsync("element => element.parentElement").ConfigureAwait(false) as IElementHandle;

    /// <summary>
    /// Retrieves the closest ancestor of the specified element that matches the selector.
    /// </summary>
    /// <param name="element">The element whose closest ancestor will be retrieved.</param>
    /// <param name="selector">The selector to match the ancestor.</param>
    /// <returns>The closest ancestor element as an IElementHandle.</returns>
    public static async Task<IElementHandle> ClosestAncestor(this IElementHandle element, string selector) => await element.EvaluateFunctionHandleAsync("(element, selector) => element.closest(selector)", selector).ConfigureAwait(false) as IElementHandle;

    /// <summary>
    /// Converts a JavaScript handle to an array of IElementHandle.
    /// </summary>
    /// <param name="arrayHandle">The JavaScript handle to convert.</param>
    /// <returns>An array of IElementHandle.</returns>
    static async Task<IElementHandle[]> ToIElementHandleArray(IJSHandle arrayHandle) {
        var properties = await arrayHandle.GetPropertiesAsync().ConfigureAwait(false);
        return properties.Values.Select(p => p as IElementHandle).Where(eh => eh != null).ToArray();
    }

    /// <summary>
    /// Converts a JavaScript handle to an array of MultipleElementHandle.
    /// </summary>
    /// <param name="arrayHandle">The JavaScript handle to convert.</param>
    /// <returns>An array of MultipleElementHandle.</returns>
    static async Task<MultipleElementHandle[]> ToMultipleElementHandleArray(IJSHandle arrayHandle) {
        var properties = await arrayHandle.GetPropertiesAsync().ConfigureAwait(false);
        var values = properties.Values.ToArray();

        MultipleElementHandle[] children = new MultipleElementHandle[values.Length];

        for (var i = 0; i < values.Length; i++) {
            children[i] = await MultipleElementHandle.FromData(values[i]).ConfigureAwait(false);
        }
        return children;
    }

    #endregion
}
