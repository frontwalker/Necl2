using System.Collections.Generic;
using System.Web.Mvc;
using Logistikcenter.Web.Models;

namespace Logistikcenter.Web.Controllers
{
    public class TransportController : Controller
    {
        public ActionResult Index()
        {
            SetUpViewBag();

            return View(new TransportModel());
        }

        private void SetUpViewBag()
        {
            ViewBag.DateRestrictionTypes = new List<SelectListItem>
                           {
                               new SelectListItem {Text = Resources.Global.Godset_skall_vara_framme_senast, Value = "0", Selected = true},
                               new SelectListItem {Text = Resources.Global.Godset_skall_skickas_efter, Value = "1", Selected = false},                               
                           };

            ViewBag.PackageTypes = new List<SelectListItem>
                           {
                               new SelectListItem {Text = Resources.Global.Paket, Value = "0", Selected = true},
                               new SelectListItem {Text = Resources.Global.Kolli, Value = "1", Selected = false},
                               new SelectListItem {Text = Resources.Global.Pall, Value = "2", Selected = false}
                           };

            ViewBag.Hours = Hours;

            ViewBag.SelectedVolumeTypes = new[] { Resources.Global.volym_m3 , Resources.Global.flakmeter ,Resources.Global.pallplats };

            ViewBag.Destination = Resources.Global.Destination;
            ViewBag.Tid_och_datum = Resources.Global.Tid_och_datum;
            ViewBag.Godsinformation = Resources.Global.Godsinformation;

            ViewBag.PageTitle = Resources.Global.Transport_sök_transport;
        }

        private static IEnumerable<SelectListItem> Hours
        {
            get
            {
                for (int i = 0; i < 24; i++)
                {
                    yield return new SelectListItem { Text = i.ToString("00") + ":00", Value = i.ToString() };
                }
            }
        }
    }
}
