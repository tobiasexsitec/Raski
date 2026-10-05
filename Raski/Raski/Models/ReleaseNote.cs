namespace Raski.Models;

public sealed record ReleaseNote(string Version, IReadOnlyList<string> Notes, DateOnly? Date = null);
