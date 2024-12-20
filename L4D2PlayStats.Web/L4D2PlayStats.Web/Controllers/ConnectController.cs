using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class ConnectController(IConfiguration configuration) : Controller
{
    private string[] ServerIPs => configuration.GetValue<string>("ServerIPs")?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];
    private string ServerIp => ServerIPs.First();

    [Route("connect")]
    public async Task<IActionResult> Index()
    {
        return Redirect($"steam://connect/{ServerIp}");
    }
}