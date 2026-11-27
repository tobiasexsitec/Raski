namespace Raski.Models;

/// <summary>A trip member or a household that can be responsible for something on a trip.</summary>
public sealed record ResponsibleOption(string Key, string Name, bool IsHousehold);

public static class ResponsibleOptions
{
    // Households are stored in the existing responsibleUid field with a prefix,
    // so documents and listeners don't need a separate field.
    public const string HouseholdPrefix = "household:";

    public static string HouseholdKey(string householdId) => HouseholdPrefix + householdId;

    public static bool IsHousehold(string key) => key.StartsWith(HouseholdPrefix, StringComparison.Ordinal);

    /// <summary>
    /// Members first, then households with at least two adults where someone is on the trip.
    /// </summary>
    public static IReadOnlyList<ResponsibleOption> Build(IEnumerable<TripMember> members, IEnumerable<Household> households)
    {
        var memberList = members.ToList();
        var namesByUid = memberList
            .GroupBy(m => m.Uid)
            .ToDictionary(g => g.Key, g => g.First().DisplayName);

        var householdOptions = households
            .Where(h => h.MemberUids.Count >= 2 && h.MemberUids.Any(namesByUid.ContainsKey))
            .Select(h => new ResponsibleOption(
                HouseholdKey(h.Id),
                h.DisplayName(h.MemberUids.Where(namesByUid.ContainsKey).Select(uid => namesByUid[uid])),
                true))
            .Where(o => o.Name.Length > 0)
            .OrderBy(o => o.Name, StringComparer.CurrentCultureIgnoreCase);

        return [
            .. memberList.Select(m => new ResponsibleOption(m.Uid, m.DisplayName, false)),
            .. householdOptions
        ];
    }
}
