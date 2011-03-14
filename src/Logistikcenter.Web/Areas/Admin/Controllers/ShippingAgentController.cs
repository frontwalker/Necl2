using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Logistikcenter.Domain;
using Logistikcenter.Web.Areas.Admin.Models;

namespace Logistikcenter.Web.Areas.Admin.Controllers
{
    public class ShippingAgentController : Controller
    {
        private readonly IRepository _repository;

        public ShippingAgentController(IRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /Admin/ShippingAgent/

        public ActionResult Index()
        {
            var shippingAgents = _repository.Query<ShippingAgent>()
                .OrderBy(s => s.CompanyName)
                .Select(s => new ShippingAgentModel {Id = s.Id, CompanyName = s.CompanyName, FirstName = s.FirstName, LastName = s.LastName});

            return View(shippingAgents);
        }

        //
        // GET: /Admin/ShippingAgent/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Admin/ShippingAgent/Create

        [HttpPost]
        public ActionResult Create(ShippingAgentModel shippingAgentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(shippingAgentModel);

                //Membership.CreateUser(shippingAgentModel.Username, shippingAgentModel.Password);
                //Roles.AddUserToRole(shippingAgentModel.Username, "ShippingAgent");

                var shippingAgent = new ShippingAgent
                                        {
                                            Username = shippingAgentModel.Username,
                                            CompanyName = shippingAgentModel.CompanyName,
                                            FirstName = shippingAgentModel.FirstName,
                                            LastName = shippingAgentModel.LastName,
                                            CustomerType = CustomerType.Company,
                                            Password = shippingAgentModel.Password /* ska vi hantera inloggning mot denna tabell eller använda asp.net membership? */
                                        };

                _repository.Save(shippingAgent);

                return RedirectToAction("Index");
            }
            catch
            {
                //if (Membership.GetUser(shippingAgentModel.Username) != null)
                    //Membership.DeleteUser(shippingAgentModel.Username);

                return View();
            }
        }
        
        //
        // GET: /Admin/ShippingAgent/Edit/5
 
        public ActionResult Edit(int id)
        {
            var shippingAgentModel = GetModel(id);

            return View(shippingAgentModel);
        }

        //
        // POST: /Admin/ShippingAgent/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ShippingAgentModel shippingAgentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(shippingAgentModel);

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

        //
        // GET: /Admin/ShippingAgent/Delete/5
 
        public ActionResult Delete(int id)
        {
            var shippingAgentModel = GetModel(id);
            return View(shippingAgentModel);
        }

        //
        // POST: /Admin/ShippingAgent/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                // vad ska hända med ev Legs som denna har? Databasen kräver nog borttag, men är det rätt? 
                // ska det ens gå att ta bort?
 
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
