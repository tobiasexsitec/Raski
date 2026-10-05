using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Raski.Shared;

/// <summary>Renders plain text with http(s) URLs as clickable links.</summary>
public sealed class LinkifiedText : ComponentBase
{
    [Parameter] public string? Text { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (string.IsNullOrEmpty(Text))
        {
            return;
        }

        var seq = 0;
        TextLinks.AddContent(builder, Text, ref seq);
    }
}
