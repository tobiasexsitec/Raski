using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Raski;
using Raski.Services;
using Raski.Services.Firebase;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var firebaseOptions = builder.Configuration.GetSection(FirebaseOptions.SectionName).Get<FirebaseOptions>()
    ?? new FirebaseOptions();
builder.Services.AddSingleton(firebaseOptions);

var environmentOptions = builder.Configuration.GetSection(AppEnvironmentOptions.SectionName).Get<AppEnvironmentOptions>()
    ?? new AppEnvironmentOptions();
builder.Services.AddSingleton(environmentOptions);

builder.Services.AddSingleton<FirebaseInterop>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IThemeService, ThemeService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IAllergyService, AllergyService>();
builder.Services.AddSingleton<IHouseholdService, HouseholdService>();
builder.Services.AddSingleton<ITripService, TripService>();
builder.Services.AddSingleton<IIngredientService, IngredientService>();
builder.Services.AddSingleton<IMealService, MealService>();
builder.Services.AddSingleton<IBreakfastService, BreakfastService>();
builder.Services.AddSingleton<ITripDrinkService, TripDrinkService>();
builder.Services.AddSingleton<IBringItemService, BringItemService>();
builder.Services.AddSingleton<IBringSectionService, BringSectionService>();
builder.Services.AddSingleton<IPollService, PollService>();
builder.Services.AddSingleton<ITripPolicyService, TripPolicyService>();
builder.Services.AddSingleton<IShoppingListService, ShoppingListService>();
builder.Services.AddSingleton<IFeatureRequestService, FeatureRequestService>();
builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IReleaseNotesService, ReleaseNotesService>();

await builder.Build().RunAsync();
