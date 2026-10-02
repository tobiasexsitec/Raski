using Raski.Models;

namespace Raski.Features.Trips;

/// <summary>Editable state for the create/edit poll form.</summary>
public sealed class PollDraft
{
    public string Question { get; set; } = "";
    public bool AllowMultiple { get; set; }
    public int MaxChoices { get; set; }
    public bool AllowCustomOptions { get; set; }
    public bool HasDeadline { get; set; }
    public DateTime? DeadlineLocal { get; set; }
    public List<PollDraftOption> Options { get; set; } = [];

    public IEnumerable<PollDraftOption> FilledOptions => Options.Where(o => !string.IsNullOrWhiteSpace(o.Text));

    public DateTimeOffset? Deadline => HasDeadline && DeadlineLocal is { } local
        ? new DateTimeOffset(DateTime.SpecifyKind(local, DateTimeKind.Local))
        : null;

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Question)
        && (FilledOptions.Any() || AllowCustomOptions)
        && (!AllowMultiple || MaxChoices >= 0)
        && (!HasDeadline || DeadlineLocal is not null);

    public static PollDraft CreateNew() => new()
    {
        Options = [new(), new()],
        DeadlineLocal = DateTime.Today.AddDays(1).AddHours(18)
    };

    public static PollDraft FromPoll(Poll poll) => new()
    {
        Question = poll.Question,
        AllowMultiple = poll.AllowMultiple,
        MaxChoices = poll.MaxChoices,
        AllowCustomOptions = poll.AllowCustomOptions,
        HasDeadline = poll.Deadline is not null,
        DeadlineLocal = poll.Deadline?.ToLocalTime().DateTime ?? DateTime.Today.AddDays(1).AddHours(18),
        Options = [.. poll.Options.Select(o => new PollDraftOption { Id = o.Id, Text = o.Text })]
    };
}

public sealed class PollDraftOption
{
    /// <summary>Empty for options not yet saved. Saved options can't be removed.</summary>
    public string Id { get; set; } = "";
    public string Text { get; set; } = "";
    public string Key { get; } = Guid.NewGuid().ToString("N");

    public bool IsSaved => !string.IsNullOrEmpty(Id);
}
