using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Logistikcenter.Web.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ActionResult Index()
        {
            ViewBag.PageTitle = @Resources.Global.AppName + " - " + "About" ;
            ViewBag.title = @Resources.About.page_title;

            return View();
        }

    }
}
