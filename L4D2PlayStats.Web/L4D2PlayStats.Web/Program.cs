using L4D2PlayStats.DependencyInjection;
using L4D2PlayStats.Sdk.DependencyInjection;
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

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddApp();
builder.Services.AddPlayStatsSdk();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseWebMarkupMin();
app.MapControllers();
app.MapControllerRoute("default", "{controller=Ranking}/{action=Index}/{id?}");
app.Run();