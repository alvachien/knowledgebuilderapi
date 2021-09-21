using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Deltas;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Query.Validator;

namespace knowledgebuilderapi.Controllers
{
    public class UserCollectionsController : ODataController
    {
        private readonly kbdataContext _context;

        public UserCollectionsController(kbdataContext context)
        {
            _context = context;
        }

        // GET: /UserCollections
        // [EnableQuery]
        public IActionResult Get(ODataQueryOptions<UserCollection> queryOptions)
        {
            // var querable = accounts.AsQueryable<Account>();
            if (queryOptions.Filter == null)
                return BadRequest("Must specify ther User");
            else
            {
                if (!queryOptions.Filter.RawValue.Contains("User EQ", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("Must specify ther User");
            }

            // queryOptions.Validate(new ODataValidationSettings())
            var finalQuery = queryOptions.ApplyTo(_context.UserCollections);
            return Ok(finalQuery);
            //return _context.UserCollections;
        }

        // GET: /UserCollections(:id)
        // [EnableQuery]
        public IActionResult Get([FromODataUri] int key, ODataQueryOptions<UserCollection> queryOptions)
        {
            // return SingleResult.Create(_context.UserCollections.Where(p => p.ID == key));
            var accountQuery = _context.UserCollections.Where(c => c.ID == key);
            if (!accountQuery.Any())
            {
                return NotFound();
            }

            var finalQuery = queryOptions.ApplyTo(accountQuery.AsQueryable<UserCollection>()) as IQueryable<dynamic>;
            var result = finalQuery.FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        public async Task<IActionResult> Post([FromBody] UserCollection coll)
        {
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var err in value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(err.Exception?.Message);
                    }
                }

                return BadRequest();
            }

            coll.CreatedAt = DateTime.Now;
            _context.UserCollections.Add(coll);
            await _context.SaveChangesAsync();

            return Created(coll);
        }

        // PUT: /UserCollections(5)
        /// <summary>
        /// Support for updating Knowledge items
        /// </summary>
        public async Task<IActionResult> Put([FromODataUri] int key, [FromODataBody] UserCollection update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.ID)
            {
                return BadRequest();
            }

            var coll = await _context.UserCollections
                    .Include(i => i.Items)
                    .SingleOrDefaultAsync(x => x.ID == key);
            if (coll == null)
            {
                return NotFound();
            }
            coll.UpdateData(update);
            // Items
            if (coll.Items.Count > 0)
            {
                if (update.Items.Count > 0)
                {
                    coll.Items.Clear();

                    foreach (var item in update.Items)
                    {
                        var nitem = new UserCollectionItem(item);
                        nitem.Collection = coll;
                        coll.Items.Add(nitem);
                    }
                }
                else
                {
                    // Delete all
                    coll.Items.Clear();
                }
            }
            else
            {
                if (update.Items.Count > 0)
                {
                    foreach (var item in update.Items)
                    {
                        var nitem = new UserCollectionItem(item);
                        nitem.Collection = coll;
                        coll.Items.Add(nitem);
                    }
                }
            }
            coll.ModifiedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(update);
        }

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var coll = await _context.UserCollections.FindAsync(key);
            if (coll == null)
            {
                return NotFound();
            }

            _context.UserCollections.Remove(coll);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
