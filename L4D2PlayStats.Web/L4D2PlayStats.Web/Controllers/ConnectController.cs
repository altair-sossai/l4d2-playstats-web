using L4D2PlayStats.Core.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace L4D2PlayStats.Web.Controllers;

public class ConnectController(IOptions<AppOptions> config) : Controller
{
    private string[] ServerIPs => config.Value.ServerIPs?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];
    private string ServerIp => ServerIPs.First();

    [Route("connect")]
    public IActionResult Index()
    {
        return Redirect($"steam://connect/{ServerIp}");
    }
}