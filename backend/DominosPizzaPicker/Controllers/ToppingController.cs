using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using DominosPizzaPicker.Backend.Models;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.OData;
using DominosPizzaPicker.Backend.DataObjects;
using Microsoft.Azure.Mobile.Server;

namespace DominosPizzaPicker.Backend.Controllers
{
    public class ToppingController : TableController<Topping>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DominosPizzaPickerContext context = new DominosPizzaPickerContext();
            DomainManager = new EntityDomainManager<Topping>(context, Request);
        }

        // GET tables/Sauce
        public IQueryable<Topping> GetAllToppings()
        {
            return Query();
        }

        // GET tables/Topping/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Topping> GetTopping(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Topping/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Topping> PatchTopping(string id, Delta<Topping> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Topping
        public async Task<IHttpActionResult> PostTopping(Topping item)
        {
            Topping current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Topping/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTopping(string id)
        {
            return DeleteAsync(id);
        }
    }
}