using System.ComponentModel.DataAnnotations;

namespace Raski.Features.Trips;

/// <summary>
/// Form model for the trip editor. Uses DateTime because InputDate does not
/// bind DateOnly with the same validation behaviour across browsers.
/// </summary>
public sealed class TripFormModel
{
    [Required(ErrorMessage = "Ange ett namn.")]
    [StringLength(80, ErrorMessage = "Namnet får vara högst 80 tecken.")]
    public string Name { get; set; } = "";

    [StringLength(80)]
    public string Destination { get; set; } = "";

    [StringLength(2000, ErrorMessage = "Länken får vara högst 2000 tecken.")]
    [RegularExpression(@"^https?://\S+$", ErrorMessage = "Ange en giltig länk som börjar med http:// eller https://.")]
    public string DestinationUrl { get; set; } = "";

    [StringLength(2000, ErrorMessage = "Länken får vara högst 2000 tecken.")]
    [RegularExpression(@"^https?://\S+$", ErrorMessage = "Ange en giltig länk som börjar med http:// eller https://.")]
    public string AccommodationUrl { get; set; } = "";

    [StringLength(30, ErrorMessage = "Telefonnumret får vara högst 30 tecken.")]
    [RegularExpression(@"^\+?[0-9 ()\-]+$", ErrorMessage = "Ange ett giltigt telefonnummer.")]
    public string AccommodationPhone { get; set; } = "";

    [Required(ErrorMessage = "Ange startdatum.")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Ange slutdatum.")]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(2);

    [StringLength(MaxNotesLength, ErrorMessage = "Övrig info får vara högst 20 000 tecken.")]
    public string Notes { get; set; } = "";

    // Firestore caps a document at ~1 MiB; this keeps the trip well below that.
    public const int MaxNotesLength = 20_000;
}
