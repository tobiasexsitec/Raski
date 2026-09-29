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

    [Required(ErrorMessage = "Ange startdatum.")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Ange slutdatum.")]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(2);

    [StringLength(2000)]
    public string Notes { get; set; } = "";
}
