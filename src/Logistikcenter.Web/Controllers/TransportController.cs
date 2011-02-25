using System.Web.Mvc;
using Logistikcenter.Web.Models;

namespace Logistikcenter.Web.Controllers
{
    public class TransportController : Controller
    {
        public ActionResult Index()
        {                                 
            return View(new TransportModel());
        }

    }
}
