using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Inventory_Demo;

namespace Inventory_Demo.Controllers
{
    [RoutePrefix("Api/Inventory")]
    [AllowAnonymous]
    public class InventoryController : ApiController
    {
        private InventoryEntitiesEx db = new InventoryEntitiesEx();

        // GET: api/Inventory
        [Route("AllInventoryDetails")]
        [HttpGet]
        public IQueryable<Inventory> GetInventories()
        {
            return db.Inventories;
        }

        // GET: api/Inventory/5
       
        [Route("GetInventoryDetailsById/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetInventory(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return NotFound();
            }

            return Ok(inventory);
        }

        // PUT: api/Inventory/5
        [HttpPut]
        [Route("UpdateInventoryDetails")]
        
        public IHttpActionResult PostInventory(int id, Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inventory.InventoryID)
            {
                return BadRequest();
            }

            db.Entry(inventory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Inventory
        [HttpPost]
        [Route("InsertInventoryDetails")]
        [AllowAnonymous]
        public IHttpActionResult PostInventory(Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Inventories.Add(inventory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = inventory.InventoryID }, inventory);
        }

        // DELETE: api/Inventory/5
        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteInventoryDetails/{id}")]
        public IHttpActionResult DeleteInventory(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return NotFound();
            }

            db.Inventories.Remove(inventory);
            db.SaveChanges();

            return Ok(inventory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InventoryExists(int id)
        {
            return db.Inventories.Count(e => e.InventoryID == id) > 0;
        }
    }
}