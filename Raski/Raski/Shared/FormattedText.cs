using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Raski.Shared;

/// <summary>
/// Renders plain text with a small markdown subset: "#", "##", "###" headings, **bold** and clickable http(s) links.
/// Text is always
/// </summary>
public sealed class FormattedText : ComponentBase
{
    [Parameter] public string? Text { get; set; }

    // Mapped below the page's own h2 sections.
    [Parameter] public int BaseHeadingLevel { get; set; } = 3;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (string.IsNullOrEmpty(Text))
        {
            return;
        }

        var lines = Text.Replace("\r\n", "\n").Split('\n');
        List<string> paragraph = [];
        var seq = 0;

        foreach (var line in lines)
        {
            var level = GetHeadingLevel(line);
            if (level > 0)
            {
                FlushParagraph(builder, paragraph, ref seq);
                var tag = $"h{Math.Min(BaseHeadingLevel + level - 1, 6)}";
                builder.OpenElement(seq++, tag);
                builder.AddAttribute(seq++, "class", "formatted-heading");
                AddInline(builder, line[(level + 1)..].Trim(), ref seq);
                builder.CloseElement();
            }
            else
            {
                paragraph.Add(line);
            }
        }

        FlushParagraph(builder, paragraph, ref seq);
    }

    private static int GetHeadingLevel(string line)
    {
        var level = 0;
        while (level < line.Length && line[level] == '#')
        {
            level++;
        }

        return level is >= 1 and <= 3 && line.Length > level && line[level] == ' ' ? level : 0;
    }

    private static void FlushParagraph(RenderTreeBuilder builder, List<string> paragraph, ref int seq)
    {
        // Trim blank lines adjacent to headings; headings provide their own spacing.
        var start = 0;
        var end = paragraph.Count;
        while (start < end && string.IsNullOrWhiteSpace(paragraph[start])) start++;
        while (end > start && string.IsNullOrWhiteSpace(paragraph[end - 1])) end--;

        if (start < end)
        {
            builder.OpenElement(seq++, "p");
            AddInline(builder, string.Join('\n', paragraph[start..end]), ref seq);
            builder.CloseElement();
        }

        paragraph.Clear();
    }

    private static void AddInline(RenderTreeBuilder builder, string text, ref int seq)
    {
        var parts = text.Split("**");
        // An even part count means the last "**" is unmatched and should stay literal.
        var unmatched = parts.Length % 2 == 0;

        for (var i = 0; i < parts.Length; i++)
        {
            var isBold = i % 2 == 1 && !(unmatched && i == parts.Length - 1);
            if (unmatched && i == parts.Length - 1)
            {
                TextLinks.AddContent(builder, "**" + parts[i], ref seq);
            }
            else if (isBold && parts[i].Length > 0)
            {
                builder.OpenElement(seq++, "strong");
                TextLinks.AddContent(builder, parts[i], ref seq);
                builder.CloseElement();
            }
            else if (!isBold)
            {
                TextLinks.AddContent(builder, parts[i], ref seq);
            }
        }
    }
}
