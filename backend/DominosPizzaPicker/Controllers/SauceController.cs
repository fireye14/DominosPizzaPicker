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
    public class SauceController : TableController<Sauce>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DominosPizzaPickerContext context = new DominosPizzaPickerContext();
            DomainManager = new EntityDomainManager<Sauce>(context, Request);
        }

        // GET tables/Sauce
        public IQueryable<Sauce> GetAllSauces()
        {
            return Query();
        }

        // GET tables/Sauce/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Sauce> GetSauce(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Sauce/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Sauce> PatchSauce(string id, Delta<Sauce> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Sauce
        public async Task<IHttpActionResult> PostSauce(Sauce item)
        {
            Sauce current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Sauce/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteSauce(string id)
        {
            return DeleteAsync(id);
        }
    }
}