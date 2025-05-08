using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITBaza.Controllers.ReportingControlers
{
    [Authorize]
    public class ReportingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
