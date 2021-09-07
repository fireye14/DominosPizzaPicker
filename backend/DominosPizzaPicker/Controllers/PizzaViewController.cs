using DominosPizzaPicker.Backend.DataObjects;
using DominosPizzaPicker.Backend.Models;
using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace DominosPizzaPicker.Backend.Controllers
{
    public class PizzaViewController : TableController<PizzaView>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DominosPizzaPickerContext context = new DominosPizzaPickerContext();
            DomainManager = new EntityDomainManager<PizzaView>(context, Request);
        }

        // GET tables/Pizza
        [EnableQuery(PageSize = 1000)]
        public IQueryable<PizzaView> GetAllPizzaView()
        {
            return Query();
        }

        // GET tables/Pizza/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<PizzaView> GetPizzaView(string id)
        {
            return Lookup(id);
        }
    }
}