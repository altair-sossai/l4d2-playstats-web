using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers
{
    public class PunishmentInfoController : Controller
    {
        // GET: PunishmentInfoController
        public ActionResult Index()
        {
            ViewBag.PunishmentInfo = "active";

            return View();
        }
    }
}
