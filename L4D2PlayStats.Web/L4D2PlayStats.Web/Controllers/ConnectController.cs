using L4D2PlayStats.Core.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class ConnectController(IAppOptionsWraper config) : Controller
{
    [Route("connect")]
    public IActionResult Index()
    {
        return Redirect($"steam://connect/{config.ServerIp}");
    }
}