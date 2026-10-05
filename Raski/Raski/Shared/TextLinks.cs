using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Rendering;

namespace Raski.Shared;

/// <summary>
/// Renders plain text where http(s) URLs become clickable links.
/// Text is always added as encoded content, never as raw HTML.
/// </summary>
public static partial class TextLinks
{
    [GeneratedRegex(@"https?://[^\s<>""]+", RegexOptions.IgnoreCase)]
    private static partial Regex UrlPattern();

    // Punctuation that usually ends a sentence rather than belonging to the URL.
    private static readonly char[] TrailingPunctuation = ['.', ',', ';', ':', '!', '?', ')', ']', '\'', '"'];

    public static void AddContent(RenderTreeBuilder builder, string text, ref int seq)
    {
        var position = 0;

        foreach (Match match in UrlPattern().Matches(text))
        {
            var url = match.Value.TrimEnd(TrailingPunctuation);
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || uri.Host.Length == 0)
            {
                continue;
            }

            if (match.Index > position)
            {
                builder.AddContent(seq++, text[position..match.Index]);
            }

            builder.OpenElement(seq++, "a");
            builder.AddAttribute(seq++, "class", "text-link");
            builder.AddAttribute(seq++, "href", url);
            builder.AddAttribute(seq++, "target", "_blank");
            builder.AddAttribute(seq++, "rel", "noopener noreferrer");
            builder.AddContent(seq++, url);
            builder.CloseElement();

            position = match.Index + url.Length;
        }

        if (position < text.Length)
        {
            builder.AddContent(seq++, text[position..]);
        }
    }
}
