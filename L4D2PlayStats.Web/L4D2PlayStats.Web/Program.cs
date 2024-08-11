using System.Globalization;
using L4D2PlayStats.DependencyInjection;
using L4D2PlayStats.Sdk.DependencyInjection;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", false, true);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddApp();
builder.Services.AddPlayStatsSdk();

var app = builder.Build();

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("pt-BR"),
    new CultureInfo("es-ES")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();