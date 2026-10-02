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
    [JsonPropertyName("isGlobalAdmin")] public bool IsGlobalAdmin { get; set; }
    [JsonPropertyName("allergies")] public List<string>? Allergies { get; set; }
    [JsonPropertyName("phone")] public string? Phone { get; set; }
    [JsonPropertyName("alcohol")] public string? Alcohol { get; set; }

    // Missing on users created before onboarding existed; they count as onboarded.
    [JsonPropertyName("onboardingCompleted")] public bool? OnboardingCompleted { get; set; }

    public UserProfile ToModel() => new()
    {
        Uid = Id,
        DisplayName = DisplayName ?? "",
        Email = Email ?? "",
        PhotoUrl = PhotoUrl ?? "",
        Phone = Phone ?? "",
        Theme = ThemeOptions.IsValid(Theme) ? Theme! : ThemeOptions.System,
        IsGlobalAdmin = IsGlobalAdmin,
        Allergies = AllergyNames.Distinct(Allergies),
        Alcohol = AlcoholPreferenceExtensions.ParseAlcoholPreference(Alcohol),
        OnboardingCompleted = OnboardingCompleted ?? true
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
    [JsonPropertyName("alcohol")] public string? Alcohol { get; set; }

    public TripMember ToModel() => new()
    {
        Uid = Id,
        DisplayName = DisplayName ?? "",
        Email = Email ?? "",
        PhotoUrl = PhotoUrl ?? "",
        Role = Role == MemberRoles.Admin ? MemberRoles.Admin : MemberRoles.Member,
        Alcohol = AlcoholPreferenceExtensions.ParseAlcoholPreference(Alcohol)
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
    [JsonPropertyName("comment")] public string? Comment { get; set; }
    [JsonPropertyName("ingredients")] public List<MealIngredientDocument>? Ingredients { get; set; }
    [JsonPropertyName("drinks")] public List<MealDrinkPayload>? Drinks { get; set; }

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
        Comment = Comment ?? "",
        Ingredients = Ingredients?.Select(i => i.ToModel()).ToList() ?? [],
        Drinks = Drinks?.Select(d => d.ToModel()).ToList() ?? []
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

internal sealed class BreakfastDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("ingredients")] public List<BreakfastIngredientDocument>? Ingredients { get; set; }

    public Breakfast ToModel(string tripId) => new()
    {
        TripId = tripId,
        Ingredients = Ingredients?.Select(i => i.ToModel()).ToList() ?? []
    };
}

internal sealed class TripDrinksDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("drinks")] public List<MealDrinkPayload>? Drinks { get; set; }
}

internal sealed class BreakfastIngredientDocument
{
    [JsonPropertyName("ingredientId")] public string? IngredientId { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("amount")] public double Amount { get; set; }
    [JsonPropertyName("unit")] public string? Unit { get; set; }
    [JsonPropertyName("scope")] public string? Scope { get; set; }

    public BreakfastIngredient ToModel() => new()
    {
        IngredientId = IngredientId ?? "",
        Name = Name ?? "",
        Amount = (decimal)Amount,
        Unit = Unit ?? "",
        Scope = BreakfastAmountScopeExtensions.ParseBreakfastAmountScope(Scope)
    };
}

internal sealed class BringItemDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("responsibleUid")] public string? ResponsibleUid { get; set; }
    [JsonPropertyName("responsibleName")] public string? ResponsibleName { get; set; }
    [JsonPropertyName("sectionId")] public string? SectionId { get; set; }

    public BringItem ToModel(string tripId) => new()
    {
        Id = Id,
        TripId = tripId,
        SectionId = SectionId ?? "",
        Name = Name ?? "",
        Quantity = Quantity < 1 ? 1 : Quantity,
        ResponsibleUid = ResponsibleUid ?? "",
        ResponsibleName = ResponsibleName ?? ""
    };
}

internal sealed class BringSectionDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("order")] public int Order { get; set; }

    public BringSection ToModel(string tripId) => new()
    {
        Id = Id,
        TripId = tripId,
        Name = Name ?? "",
        Order = Order
    };
}

internal sealed class PollDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("question")] public string? Question { get; set; }
    [JsonPropertyName("allowMultiple")] public bool AllowMultiple { get; set; }
    [JsonPropertyName("maxChoices")] public int? MaxChoices { get; set; }
    [JsonPropertyName("allowCustomOptions")] public bool AllowCustomOptions { get; set; }
    [JsonPropertyName("deadline")] public string? Deadline { get; set; }
    [JsonPropertyName("closed")] public bool Closed { get; set; }
    [JsonPropertyName("createdByUid")] public string? CreatedByUid { get; set; }
    [JsonPropertyName("createdByName")] public string? CreatedByName { get; set; }
    [JsonPropertyName("createdAt")] public string? CreatedAt { get; set; }

    public Poll ToModel(string tripId) => new()
    {
        Id = Id,
        TripId = tripId,
        Question = Question ?? "",
        AllowMultiple = AllowMultiple,
        MaxChoices = Math.Max(0, MaxChoices ?? 0),
        AllowCustomOptions = AllowCustomOptions,
        Deadline = DateTimeOffset.TryParse(Deadline, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var deadline)
            ? deadline
            : null,
        IsClosed = Closed,
        CreatedByUid = CreatedByUid ?? "",
        CreatedByName = CreatedByName ?? "",
        CreatedAt = CreatedAt ?? ""
    };
}

internal sealed class PollOptionDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("text")] public string? Text { get; set; }
    [JsonPropertyName("order")] public int Order { get; set; }
    [JsonPropertyName("addedByUid")] public string? AddedByUid { get; set; }
    [JsonPropertyName("addedByName")] public string? AddedByName { get; set; }

    public PollOption ToModel() => new()
    {
        Id = Id,
        Text = Text ?? "",
        Order = Order,
        AddedByUid = AddedByUid ?? "",
        AddedByName = AddedByName ?? ""
    };
}

internal sealed class PollVoteDocument
{
    [JsonPropertyName("id")] public string Id { get; set; } = "";
    [JsonPropertyName("voterName")] public string? VoterName { get; set; }
    [JsonPropertyName("optionIds")] public List<string>? OptionIds { get; set; }

    public PollVote ToModel() => new()
    {
        VoterUid = Id,
        VoterName = VoterName ?? "",
        OptionIds = OptionIds ?? []
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

internal sealed class AllergyDocument
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
