using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using knowledgebuilderapi.Models;

namespace knowledgebuilderapi.Controllers 
{
    //[Authorize]
    public class KnowledgeItemsController : ODataController 
    {
        private readonly kbdataContext _context;

        public KnowledgeItemsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /KnowledgeItems
        /// <summary>
        /// Adds support for getting knowledges, for example:
        /// 
        /// GET /KnowledgeItems
        /// GET /KnowledgeItems?$filter=Title eq 'Windows 95'
        /// GET /KnowledgeItems?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<KnowledgeItem> Get()
        {
            return _context.KnowledgeItems;
        }

        /// GET: /KnowledgeItems(:id)
        /// <summary>
        /// Adds support for getting a knowledge by key, for example:
        /// 
        /// GET /KnowledgeItems(1)
        /// </summary>
        /// <param name="key">The key of the Knowledge item required</param>
        /// <returns>The Knowledge item</returns>
        [EnableQuery]
        public SingleResult<KnowledgeItem> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.KnowledgeItems.Where(p => p.ID == key));
        }

        // POST: /KnowledgeItems
        /// <summary>
        /// Support for creating knowledge item
        /// </summary>
        public async Task<IActionResult> Post([FromBody] KnowledgeItem knowledge)
        {
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach(var err in value.Errors) 
                    {
                        System.Diagnostics.Debug.WriteLine(err.Exception?.Message);
                    }
                }

                return BadRequest();
            }

            _context.KnowledgeItems.Add(knowledge);
            await _context.SaveChangesAsync();

            return Created(knowledge);
        }

        // PUT: /KnowledgeItems/5
        /// <summary>
        /// Support for updating Knowledge items
        /// </summary>
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] KnowledgeItem update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.ID)
            {
                return BadRequest();
            }

            _context.Entry(update).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.KnowledgeItems.Any(p => p.ID == key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(update);
        }

        // DELETE: /KnowledgeItems/5
        /// <summary>
        /// Support for deleting knowledge item by key.
        /// </summary>
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var knowledge = await _context.KnowledgeItems.FindAsync(key);
            if (knowledge == null)
            {
                return NotFound();
            }

            _context.KnowledgeItems.Remove(knowledge);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }

        // PATCH: /KnowledgeItems
        /// <summary>
        /// Support for partial updates of knowledge items
        /// </summary>
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<KnowledgeItem> knowledge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _context.KnowledgeItems.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            knowledge.Patch(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.KnowledgeItems.Any(p => p.ID == key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }
    }
}