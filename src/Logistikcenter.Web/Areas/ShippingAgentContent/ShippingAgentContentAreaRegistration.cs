using System.Web.Mvc;

namespace Logistikcenter.Web.Areas.ShippingAgentContent
{
    public class ShippingAgentContentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ShippingAgentContent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ShippingAgentContent_default",
                "ShippingAgentContent/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
