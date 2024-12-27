using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBrowserAiSharp.Puppeteer.Extensions;
/// <summary>
/// Options for scrolling an element into a view
/// </summary>
public class ScrollIntoViewOptions {
    /// <summary>
    /// The horizontal offset for scrolling.
    /// </summary>
    public int OffsetX { get; set; } = 0;

    /// <summary>
    /// The vertical offset for scrolling.
    /// </summary>
    public int OffsetY { get; set; } = 0;
}

