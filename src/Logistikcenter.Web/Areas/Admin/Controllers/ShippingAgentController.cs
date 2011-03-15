using System.Linq;
using System.Web.Mvc;
using Logistikcenter.Domain;
using Logistikcenter.Web.Areas.Admin.Models;
using Logistikcenter.Web.Services;

namespace Logistikcenter.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class ShippingAgentController : Controller
    {
        private readonly IRepository _repository;
        private readonly IUserService _userService;

        public ShippingAgentController(IRepository repository, IUserService userService)
        {
            _repository = repository;
            _userService = userService;
        }

        public ActionResult Index()
        {
            var shippingAgents = _repository.Query<ShippingAgent>()
                .OrderBy(s => s.CompanyName)
                .Select(s => new ShippingAgentModel {Id = s.Id, CompanyName = s.CompanyName, FirstName = s.FirstName, LastName = s.LastName});

            return View(shippingAgents);
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(ShippingAgentModel shippingAgentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(shippingAgentModel);

                _userService.AddShippingAgent(shippingAgentModel.Username, shippingAgentModel.Password);

                var shippingAgent = new ShippingAgent
                                        {
                                            Username = shippingAgentModel.Username,
                                            CompanyName = shippingAgentModel.CompanyName,
                                            FirstName = shippingAgentModel.FirstName,
                                            LastName = shippingAgentModel.LastName,
                                            CustomerType = CustomerType.Company                                           
                                        };

                _repository.Save(shippingAgent);

                return RedirectToAction("Index");
            }
            catch
            {
                _userService.Remove(shippingAgentModel.Username);
                return View();
            }
        }
        
        public ActionResult Edit(int id)
        {
            var shippingAgentModel = GetModel(id);

            return View(shippingAgentModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, ShippingAgentModel shippingAgentModel)
        {
            try
            {
                var shippingAgent = _repository.Query<ShippingAgent>().Where(s => s.Id == id).Single();
                
                shippingAgent.CompanyName = shippingAgentModel.CompanyName;
                shippingAgent.FirstName = shippingAgentModel.FirstName;
                shippingAgent.LastName = shippingAgentModel.LastName;
                
                _repository.Update(shippingAgent);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var shippingAgentModel = GetModel(id);
            return View(shippingAgentModel);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                // vad ska hända med ev Legs som denna har? Databasen kräver nog borttag, men är det rätt? 
                // ska det ens gå att ta bort?

                var shippingAgentModel = GetModel(id);
                _userService.Remove(shippingAgentModel.Username);

                _repository.Delete<ShippingAgent>(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private ShippingAgentModel GetModel(int id)
        {
            var shippingAgentModel = _repository.Query<ShippingAgent>().Where(s => s.Id == id)
                .Select(s => new ShippingAgentModel
                        {
                            Id = s.Id,
                            CompanyName = s.CompanyName,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Username = s.Username
                        })
                .Single();
            
            return shippingAgentModel;
        }

    }
}
