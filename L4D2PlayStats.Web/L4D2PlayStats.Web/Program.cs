using System.Globalization;
using System.Text.Json.Serialization;
using AspNet.Security.OpenId.Steam;
using L4D2PlayStats.Auth;
using L4D2PlayStats.DependencyInjection;
using L4D2PlayStats.Sdk.DependencyInjection;
using L4D2PlayStats.Web.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using WebMarkupMin.AspNetCoreLatest;

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
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = SteamAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddSteam();

builder.Services.AddApp();
builder.Services.AddPlayStatsSdk(builder.Configuration);

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseWebMarkupMin();
app.MapControllers();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();