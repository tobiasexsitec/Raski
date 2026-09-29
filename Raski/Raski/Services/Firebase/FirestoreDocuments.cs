using System.Globalization;
using System.Text.Json.Serialization;
using Raski.Models;

namespace Raski.Services.Firebase;

/// <summary>
/// Firestore document shapes. Kept separate from the domain models because
/// Firestore has no DateOnly/decimal/enum support: dates are stored as ISO
/// strings (sortable), amounts as doubles and enums as lowercase strings.
/// </summary>
internal static class FirestoreFormat
{
    public const string DateFormat = "yyyy-MM-dd";

    public static string ToIso(DateOnly date) =>
        date.ToString(DateFormat, CultureInfo.InvariantCulture);

    public static DateOnly ParseDate(string? value) =>
        DateOnly.TryParseExact(value, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : default;

    public static string UtcNow() =>
        DateTimeOffset.UtcNow.ToString("O", CultureInfo.InvariantCulture);
}

internal sealed class UserDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("displayName")] public string? DisplayName { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("photoUrl")] public string? PhotoUrl { get; set; }
    [JsonPropertyName("theme")] public string? Theme { get; set; }

    public UserProfile ToModel() => new()
    {
        Uid = Id,
        DisplayName = DisplayName ?? "",
        Email = Email ?? "",
        PhotoUrl = PhotoUrl ?? "",
        Theme = ThemeOptions.IsValid(Theme) ? Theme! : ThemeOptions.System
    };
}

internal sealed class TripDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("destination")] public string? Destination { get; set; }
    [JsonPropertyName("startDate")] public string? StartDate { get; set; }
    [JsonPropertyName("endDate")] public string? EndDate { get; set; }
    [JsonPropertyName("notes")] public string? Notes { get; set; }
    [JsonPropertyName("ownerUid")] public string? OwnerUid { get; set; }
    [JsonPropertyName("memberUids")] public List<string>? MemberUids { get; set; }

    public Trip ToModel() => new()
    {
        Id = Id,
        Name = Name ?? "",
        Destination = Destination ?? "",
        StartDate = FirestoreFormat.ParseDate(StartDate),
        EndDate = FirestoreFormat.ParseDate(EndDate),
        Notes = Notes,
        OwnerUid = OwnerUid ?? "",
        MemberUids = MemberUids ?? []
    };
}

internal sealed class TripMemberDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("displayName")] public string? DisplayName { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("photoUrl")] public string? PhotoUrl { get; set; }
    [JsonPropertyName("role")] public string? Role { get; set; }

    public TripMember ToModel() => new()
    {
        Uid = Id,
        DisplayName = DisplayName ?? "",
        Email = Email ?? "",
        PhotoUrl = PhotoUrl ?? "",
        Role = Role == MemberRoles.Admin ? MemberRoles.Admin : MemberRoles.Member
    };
}

internal sealed class MealDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("date")] public string? Date { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("responsibleUid")] public string? ResponsibleUid { get; set; }
    [JsonPropertyName("responsibleName")] public string? ResponsibleName { get; set; }
    [JsonPropertyName("recipeUrl")] public string? RecipeUrl { get; set; }
    [JsonPropertyName("ingredients")] public List<MealIngredientDocument>? Ingredients { get; set; }

    public Meal ToModel(string tripId) => new()
    {
        Id = Id,
        TripId = tripId,
        Date = FirestoreFormat.ParseDate(Date),
        Type = MealTypeExtensions.ParseMealType(Type),
        Title = Title ?? "",
        ResponsibleUid = ResponsibleUid ?? "",
        ResponsibleName = ResponsibleName ?? "",
        RecipeUrl = RecipeUrl ?? "",
        Ingredients = Ingredients?.Select(i => i.ToModel()).ToList() ?? []
    };
}

internal sealed class MealIngredientDocument
{
    [JsonPropertyName("ingredientId")] public string? IngredientId { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("amount")] public double Amount { get; set; }
    [JsonPropertyName("unit")] public string? Unit { get; set; }

    public MealIngredient ToModel() => new()
    {
        IngredientId = IngredientId ?? "",
        Name = Name ?? "",
        Amount = (decimal)Amount,
        Unit = Unit ?? ""
    };
}

internal sealed class IngredientDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("nameLower")] public string? NameLower { get; set; }
    [JsonPropertyName("tags")] public List<string>? Tags { get; set; }
    [JsonPropertyName("defaultUnit")] public string? DefaultUnit { get; set; }
    [JsonPropertyName("mergedIntoId")] public string? MergedIntoId { get; set; }

    public Ingredient ToModel() => new()
    {
        Id = Id,
        Name = Name ?? "",
        NameLower = NameLower ?? (Name ?? "").ToLowerInvariant(),
        Tags = Tags ?? [],
        DefaultUnit = DefaultUnit,
        MergedIntoId = MergedIntoId
    };
}

internal sealed class UnitDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("nameLower")] public string? NameLower { get; set; }
}

internal sealed class TagDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("nameLower")] public string? NameLower { get; set; }
}

internal sealed class ShoppingStateDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("checked")] public bool Checked { get; set; }
    [JsonPropertyName("checkedByUid")] public string? CheckedByUid { get; set; }
    [JsonPropertyName("checkedAt")] public string? CheckedAt { get; set; }
}

internal sealed class InviteDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("tripId")] public string? TripId { get; set; }
    [JsonPropertyName("tripName")] public string? TripName { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("invitedByUid")] public string? InvitedByUid { get; set; }
}
