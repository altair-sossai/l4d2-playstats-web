using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class PatentController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Patent = "active";

        return View();
    }
}