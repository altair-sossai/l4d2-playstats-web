using System.Globalization;
using L4D2PlayStats.DependencyInjection;
using L4D2PlayStats.Sdk.DependencyInjection;
using Microsoft.AspNetCore.Localization;
using WebMarkupMin.AspNetCore8;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", false, true);

builder.Services
    .AddWebMarkupMin(options =>
    {
        options.AllowMinificationInDevelopmentEnvironment = true;
        options.AllowCompressionInDevelopmentEnvironment = true;
    })
    .AddHtmlMinification()
    .AddHttpCompression();

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
    new CultureInfo("en-us"),
    new CultureInfo("pt-br")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-us"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseWebMarkupMin();
app.MapControllers();
app.MapControllerRoute("default", "{controller=Ranking}/{action=Index}/{id?}");
app.Run();