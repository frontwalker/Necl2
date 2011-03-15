using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Logistikcenter.Domain;

namespace Logistikcenter.Web.Areas.API.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class DestinationController : Controller
    {
        private readonly IRepository _repository;

        public DestinationController(IRepository repository )
        {
            _repository = repository;
        }
      
        [HttpGet]
        public ActionResult Index()
        {
            var term = HttpContext.Request.QueryString["term"];

            var destinations = _repository.Query<Destination>();

            if (!string.IsNullOrEmpty(term))
                destinations = destinations.Where(d => d.Name.ToLower().Contains(term));
            try
            {
                return new JsonResult
                {
                    Data = destinations.OrderBy(d => d.Name).Select(d => d.Name).ToList(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            }
            catch (Exception)
            {
                // SqlCe fixar inte denna linq-query
                return new JsonResult
                {
                    Data = new List<Destination>(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };                                
            }
        }        
    }
}
