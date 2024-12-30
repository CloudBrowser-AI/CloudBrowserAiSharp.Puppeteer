using CloudBrowserAiSharp.AI.Types;
using CloudBrowserAiSharp.Exceptions;
using CloudBrowserAiSharp.Puppeteer.Extensions;
using PuppeteerSharp;
using System.Threading.Tasks;

namespace CloudBrowserAiSharp.Puppeteer.AIExtensions;
/// <summary>
/// Extension class to use AIService implicitly, allowing for seamless AI interactions within web pages and frames.
/// </summary>
public static class AIExtensions {

    static AIService _svc;

    /// <summary>
    /// Configures the global settings for AIService, enabling the use of extension methods.
    /// </summary>
    /// <param name="apiToken">CloudBrowser.AI's API token for authentication.</param>
    /// <param name="options">The AI options to be used for requests.</param>
    public static void SetGlobalSettings(string apiToken, AIOptions options = null) {
        _svc?.Dispose();
        _svc = new(apiToken, options);
    }

    /// <summary>
    /// Configures the global settings for AIService, enabling the use of extension methods.
    /// </summary>
    /// <param name="apiToken">The CloudBrowser.AI API token for authentication.</param>
    /// <param name="openAiToken">The OpenAI token for authentication</param>
    /// <param name="openAiModel">The OpenAI model to use for requests</param>
    public static void SetGlobalSettings(string apiToken, string openAiToken, string openAiModel = null) {
        _svc?.Dispose();
        _svc = new(apiToken, openAiToken, openAiModel);
    }

    #region Query
    /// <summary>
    /// Performs an AI query on the HTML content of a page using the specified prompt.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="page">The page whose HTML content will be queried.</param>
    /// <param name="promt">The prompt for the AI query.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Query<T>(this IPage page, string promt) {
        var html = await page.GetContentAsync().ConfigureAwait(false);
        return await _svc.Query<T>(new() {
            Html = html,
            Prompt = promt
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Performs an AI query on the HTML content of a frame using the specified prompt.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="element">The frame whose HTML content will be queried.</param>
    /// <param name="promt">The prompt for the AI query.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Query<T>(this IFrame element, string promt) {
        var html = await element.GetContentAsync().ConfigureAwait(false);
        return await _svc.Query<T>(new() {
            Html = html,
            Prompt = promt
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Performs an AI query on the HTML content of an element using the specified prompt.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="element">The element whose HTML content will be queried.</param>
    /// <param name="promt">The prompt for the AI query.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Query<T>(this IElementHandle element, string promt) {
        var html = await element.OuterHTML().ConfigureAwait(false);
        return await _svc.Query<T>(new() {
            Html = html,
            Prompt = promt
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Performs an AI query on the content of a string using the specified prompt.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="value">The string whose content will be queried.</param>
    /// <param name="promt">The prompt for the AI query.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Query<T>(this string value, string promt) {
        return await _svc.Query<T>(new() {
            Html = value,
            Prompt = promt
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Performs an AI query on the content of a string using the specified prompt.
    /// </summary>
    /// <param name="value">The string whose content will be queried.</param>
    /// <param name="promt">The prompt for the AI query.</param>
    /// <returns>The AI response as a string.</returns>
    public static Task<string> Query(this string value, string promt) {
        return value.Query<string>(promt);
    }
    #endregion

    #region Summarize
    /// <summary>
    /// Summarizes the HTML content of a page using the specified language.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="page">The page whose HTML content will be summarized.</param>
    /// <param name="isoLanguage">The ISO language code for the summary (optional).</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Summarize<T>(this IPage page, string isoLanguage = null) {
        var html = await page.GetContentAsync().ConfigureAwait(false);
        return await _svc.Summarize<T>(new() {
            Html = html,
            IsoLang = isoLanguage
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Summarizes the HTML content of a frame using the specified language.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="frame">The frame whose HTML content will be summarized.</param>
    /// <param name="isoLanguage">The ISO language code for the summary (optional).</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Summarize<T>(this IFrame frame, string isoLanguage = null) {
        var html = await frame.GetContentAsync().ConfigureAwait(false);
        return await _svc.Summarize<T>(new() {
            Html = html,
            IsoLang = isoLanguage
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Summarizes the HTML content of an element using the specified language.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="element">The element whose HTML content will be summarized.</param>
    /// <param name="isoLanguage">The ISO language code for the summary (optional).</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Summarize<T>(this IElementHandle element, string isoLanguage = null) {
        var html = await element.OuterHTML().ConfigureAwait(false);
        return await _svc.Summarize<T>(new() {
            Html = html,
            IsoLang = isoLanguage
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Summarizes the content of a string using the specified language.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="value">The string whose content will be summarized.</param>
    /// <param name="isoLanguage">The ISO language code for the summary (optional).</param>
    /// <returns>The deserialized AI response.</returns>
    public static Task<T> Summarize<T>(this string value, string isoLanguage = null) {
        return _svc.Summarize<T>(new() {
            Html = value,
            IsoLang = isoLanguage
        });
    }
    /// <summary>
    /// Summarizes the content of a string using the specified language.
    /// </summary>
    /// <param name="value">The string whose content will be summarized.</param>
    /// <param name="isoLanguage">The ISO language code for the summary (optional).</param>
    /// <returns>The AI response as a string.</returns>
    public static Task<string> Summarize(this string value, string isoLanguage = null) {
        return value.Summarize<string>(isoLanguage);
    }
    #endregion

    #region Optimize
    /// <summary>
    /// Optimizes the HTML content of a page based on the provided instruction.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="page">The page whose HTML content will be optimized.</param>
    /// <param name="instruction">The instruction for the AI optimization.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Optimize<T>(this IPage page, string instruction) {
        var html = await page.GetContentAsync().ConfigureAwait(false);
        return await _svc.Optimize<T>(new() {
            Instruction = instruction,
            Text = html
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Optimizes the HTML content of a frame based on the provided instruction.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="frame">The frame whose HTML content will be optimized.</param>
    /// <param name="instruction">The instruction for the AI optimization.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Optimize<T>(this IFrame frame, string instruction) {
        var html = await frame.GetContentAsync().ConfigureAwait(false);
        return await _svc.Optimize<T>(new() {
            Instruction = instruction,
            Text = html
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Optimizes the HTML content of an element based on the provided instruction.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="element">The element whose HTML content will be optimized.</param>
    /// <param name="instruction">The instruction for the AI optimization.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Optimize<T>(this IElementHandle element, string instruction) {
        var html = await element.InnerText().ConfigureAwait(false);
        return await _svc.Optimize<T>(new() {
            Instruction = instruction,
            Text = html
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Optimizes the content of a string based on the provided instruction.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="variable">The string whose content will be optimized.</param>
    /// <param name="instruction">The instruction for the AI optimization.</param>
    /// <returns>The deserialized AI response.</returns>
    public static Task<T> Optimize<T>(this string variable, string instruction) {
        return _svc.Optimize<T>(new() {
            Instruction = instruction,
            Text = variable
        });
    }
    /// <summary>
    /// Optimizes the content of a string based on the provided instruction.
    /// </summary>
    /// <param name="variable">The string whose content will be optimized.</param>
    /// <param name="instruction">The instruction for the AI optimization.</param>
    /// <returns>The AI response as a string.</returns>
    public static Task<string> Optimize(this string variable, string instruction) {
        return variable.Optimize<string>(instruction);
    }
    #endregion

    #region Translate
    /// <summary>
    /// Translates the text content of a page to the specified language.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="page">The page whose text content will be translated.</param>
    /// <param name="isoLang">The ISO language code for the translation.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Translate<T>(this IPage page, string isoLang) {
        var text = await page.InnerText().ConfigureAwait(false);
        return await _svc.Translate<T>(new() {
            IsoLang = isoLang,
            Text = text
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Translates the text content of an element to the specified language.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="element">The element whose text content will be translated.</param>
    /// <param name="isoLang">The ISO language code for the translation.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Translate<T>(this IElementHandle element, string isoLang) {
        var text = await element.InnerText().ConfigureAwait(false);
        return await _svc.Translate<T>(new() {
            IsoLang = isoLang,
            Text = text
        }).ConfigureAwait(false);
    }
    /// <summary>
    /// Translates the content of a string to the specified language.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="text">The string whose content will be translated.</param>
    /// <param name="isoLang">The ISO language code for the translation.</param>
    /// <returns>The deserialized AI response.</returns>
    public static Task<T> Translate<T>(this string text, string isoLang) {
        return _svc.Translate<T>(new() {
            IsoLang = isoLang,
            Text = text
        });
    }
    /// <summary>
    /// Translates the content of a string to the specified language.
    /// </summary>
    /// <param name="text">The string whose content will be translated.</param>
    /// <param name="isoLang">The ISO language code for the translation.</param>
    /// <returns>The AI response as a string.</returns>
    public static Task<string> Translate(this string text, string isoLang) {
        return text.Translate<string>(isoLang);
    }
    #endregion

    #region To
    /// <summary>
    /// Converts the HTML content of a page to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the content should be converted.</typeparam>
    /// <param name="page">The page whose HTML content will be converted.</param>
    /// <returns>The converted content as the specified type.</returns>
    public static async Task<T> To<T>(this IPage page) {
        var html = await page.GetContentAsync().ConfigureAwait(false);
        return await _svc.To<T>(html).ConfigureAwait(false);
    }
    /// <summary>
    /// Converts the HTML content of a frame to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the content should be converted.</typeparam>
    /// <param name="frame">The frame whose HTML content will be converted.</param>
    /// <returns>The converted content as the specified type.</returns>
    public static async Task<T> To<T>(this IFrame frame) {
        var html = await frame.GetContentAsync().ConfigureAwait(false);
        return await _svc.To<T>(html).ConfigureAwait(false);
    }
    /// <summary>
    /// Converts the HTML content of an element to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the content should be converted.</typeparam>
    /// <param name="element">The element whose HTML content will be converted.</param>
    /// <returns>The converted content as the specified type.</returns>
    public static async Task<T> To<T>(this IElementHandle element) {
        var html = await element.OuterHTML().ConfigureAwait(false);
        return await _svc.To<T>(html).ConfigureAwait(false);
    }
    /// <summary>
    /// Converts the content of a string to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the content should be converted.</typeparam>
    /// <param name="value">The string whose content will be converted.</param>
    /// <returns>The converted content as the specified type.</returns>
    public static async Task<T> To<T>(this string value) {
        return await _svc.To<T>(value).ConfigureAwait(false);
    }
    #endregion

    #region ToJson
    /// <summary>
    /// Converts the content of a string to JSON format.
    /// </summary>
    /// <param name="value">The string whose content will be converted.</param>
    /// <returns>The content in JSON format.</returns>
    public static async Task<string> ToJSON(this string value) {
        var rp = await _svc.ToJSON(new() {
            Html = value
        }).ConfigureAwait(false);
        ExceptionHelper.ToException(rp.Status, rp.OpenAiError);
        return rp.Response;
    }
    /// <summary>
    /// Converts the HTML content of a page to JSON format.
    /// </summary>
    /// <param name="page">The page whose HTML content will be converted.</param>
    /// <returns>The content in JSON format.</returns>
    public static async Task<string> ToJSON(this IPage page) {
        var html = await page.GetContentAsync().ConfigureAwait(false);
        return await html.ToJSON().ConfigureAwait(false);
    }
    /// <summary>
    /// Converts the HTML content of a frame to JSON format.
    /// </summary>
    /// <param name="frame">The frame whose HTML content will be converted.</param>
    /// <returns>The content in JSON format.</returns>
    public static async Task<string> ToJSON(this IFrame frame) {
        var html = await frame.GetContentAsync().ConfigureAwait(false);
        return await html.ToJSON().ConfigureAwait(false);
    }
    /// <summary>
    /// Converts the HTML content of an element to JSON format.
    /// </summary>
    /// <param name="element">The element whose HTML content will be converted.</param>
    /// <returns>The content in JSON format.</returns>
    public static async Task<string> ToJSON(this IElementHandle element) {
        var html = await element.OuterHTML().ConfigureAwait(false);
        return await html.ToJSON().ConfigureAwait(false);
    }
    #endregion

    #region ToCSV
    /// <summary>
    /// Converts the content of a string to CSV format.
    /// </summary>
    /// <param name="value">The string whose content will be converted.</param>
    /// <returns>The content in CSV format.</returns>
    public static async Task<string> ToCSV(this string value) {
        var rp = await _svc.ToCSV(new() {
            Html = value
        }).ConfigureAwait(false);
        ExceptionHelper.ToException(rp.Status, rp.OpenAiError);
        return rp.Response;
    }
    /// <summary>
    /// Converts the HTML content of a page to CSV format.
    /// </summary>
    /// <param name="page">The page whose HTML content will be converted.</param>
    /// <returns>The content in CSV format.</returns>
    public static async Task<string> ToCSV(this IPage page) {
        var html = await page.GetContentAsync().ConfigureAwait(false);
        return await html.ToCSV().ConfigureAwait(false);
    }
    /// <summary>
    /// Converts the HTML content of a frame to CSV format.
    /// </summary>
    /// <param name="frame">The frame whose HTML content will be converted.</param>
    /// <returns>The content in CSV format.</returns>
    public static async Task<string> ToCSV(this IFrame frame) {
        var html = await frame.GetContentAsync().ConfigureAwait(false);
        return await html.ToCSV().ConfigureAwait(false);
    }
    /// <summary>
    /// Converts the HTML content of an element to CSV format.
    /// </summary>
    /// <param name="element">The element whose HTML content will be converted.</param>
    /// <returns>The content in CSV format.</returns>
    public static async Task<string> ToCSV(this IElementHandle element) {
        var html = await element.OuterHTML().ConfigureAwait(false);
        return await html.ToCSV().ConfigureAwait(false);
    }
    #endregion

    #region ToMarkdown
    /// <summary>
    /// Converts the content of a string to Markdown format.
    /// </summary>
    /// <param name="value">The string whose content will be converted.</param>
    /// <returns>The content in Markdown format.</returns>
    public static async Task<string> ToMarkdown(this string value) {
        var rp = await _svc.ToCSV(new() {
            Html = value
        }).ConfigureAwait(false);
        ExceptionHelper.ToException(rp.Status, rp.OpenAiError);
        return rp.Response;
    }
    /// <summary>
    /// Converts the HTML content of a page to Markdown format.
    /// </summary>
    /// <param name="page">The page whose HTML content will be converted.</param>
    /// <returns>The content in Markdown format.</returns>
    public static async Task<string> ToMarkdown(this IPage page) {
        var html = await page.GetContentAsync().ConfigureAwait(false);
        return await html.ToMarkdown().ConfigureAwait(false);
    }
    /// <summary>
    /// Converts the HTML content of a frame to Markdown format.
    /// </summary>
    /// <param name="frame">The frame whose HTML content will be converted.</param>
    /// <returns>The content in Markdown format.</returns>
    public static async Task<string> ToMarkdown(this IFrame frame) {
        var html = await frame.GetContentAsync().ConfigureAwait(false);
        return await html.ToMarkdown().ConfigureAwait(false);
    }
    /// <summary>
    /// Converts the HTML content of an element to Markdown format.
    /// </summary>
    /// <param name="element">The element whose HTML content will be converted.</param>
    /// <returns>The content in Markdown format.</returns>
    public static async Task<string> ToMarkdown(this IElementHandle element) {
        var html = await element.OuterHTML().ConfigureAwait(false);
        return await html.ToMarkdown().ConfigureAwait(false);
    }
    #endregion

    #region Describe
    /// <summary>
    /// Describes the content of an element based on the provided question.
    /// </summary>
    /// <typeparam name="T">The type to which the response should be deserialized.</typeparam>
    /// <param name="element">The element whose content will be described.</param>
    /// <param name="question">The question for the AI description.</param>
    /// <returns>The deserialized AI response.</returns>
    public static async Task<T> Describe<T>(this IElementHandle element, string question) {
        var src = await element.Attribute("src").ConfigureAwait(false);
        if (string.IsNullOrEmpty(src))
            return default;
        return await _svc.Describe<T>(new() {
            Question = question,
            ImageUrl = src
        }).ConfigureAwait(false);
    }
    #endregion
}
