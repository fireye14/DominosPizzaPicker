using DominosPizzaPicker.Backend.DataObjects;
using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using DominosPizzaPicker.Backend.Models;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.OData;

namespace DominosPizzaPicker.Backend.Controllers
{
    public class PizzaController : TableController<Pizza>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DominosPizzaPickerContext context = new DominosPizzaPickerContext();
            DomainManager = new EntityDomainManager<Pizza>(context, Request);            
        }

        // GET tables/Pizza
        [EnableQuery(PageSize = 1000)]
        public IQueryable<Pizza> GetAllPizzas()
        {            
            return Query();
        }

        // GET tables/Pizza/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Pizza> GetPizza(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Pizza/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Pizza> PatchPizza(string id, Delta<Pizza> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Pizza
        public async Task<IHttpActionResult> PostPizza(Pizza item)
        {
            Pizza current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Pizza/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeletePizza(string id)
        {
            return DeleteAsync(id);
        }
    }
}