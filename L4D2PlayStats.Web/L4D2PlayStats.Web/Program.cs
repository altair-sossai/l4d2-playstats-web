using L4D2PlayStats.DependencyInjection;
using L4D2PlayStats.Sdk.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", false, true);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddApp();
builder.Services.AddPlayStatsSdk();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Ranking}/{action=Index}/{id?}");
app.Run();