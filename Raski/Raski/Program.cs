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

builder.Services.AddSingleton<FirebaseInterop>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IThemeService, ThemeService>();
builder.Services.AddSingleton<ITripService, TripService>();
builder.Services.AddSingleton<IIngredientService, IngredientService>();
builder.Services.AddSingleton<IMealService, MealService>();
builder.Services.AddSingleton<IShoppingListService, ShoppingListService>();

await builder.Build().RunAsync();
