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
    [Authorize]
    public class KnowledgesController : ODataController 
    {
        private readonly kbdataContext _context;

        public KnowledgesController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /Knowledges
        /// <summary>
        /// Adds support for getting knowledges, for example:
        /// 
        /// GET /Knowledges
        /// GET /Knowledges?$filter=Name eq 'Windows 95'
        /// GET /Knowledges?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<Knowledge> Get()
        {
            return _context.Knowledges;
        }

        /// GET: /Knowledges(:id)
        /// <summary>
        /// Adds support for getting a knowledge by key, for example:
        /// 
        /// GET /Knowledges(1)
        /// </summary>
        /// <param name="key">The key of the Knowledge required</param>
        /// <returns>The Knowledge</returns>
        [EnableQuery]
        public SingleResult<Knowledge> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.Knowledges.Where(p => p.ID == key));
        }

        // POST: /Knowledges
        /// <summary>
        /// Support for creating knowledge
        /// </summary>
        public async Task<IActionResult> Post([FromBody] Knowledge knowledge)
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

            _context.Knowledges.Add(knowledge);
            await _context.SaveChangesAsync();

            return Created(knowledge);
        }

        // PUT: /Knowledges/5
        /// <summary>
        /// Support for updating Knowledges
        /// </summary>
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Knowledge update)
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
                if (!_context.Knowledges.Any(p => p.ID == key))
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

        // DELETE: /Knowledges/5
        /// <summary>
        /// Support for deleting knowledge by key.
        /// </summary>
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var knowledge = await _context.Knowledges.FindAsync(key);
            if (knowledge == null)
            {
                return NotFound();
            }

            _context.Knowledges.Remove(knowledge);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }

        // PATCH: /Knowleges
        /// <summary>
        /// Support for partial updates of knowledges
        /// </summary>
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<Knowledge> knowledge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _context.Knowledges.FindAsync(key);
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
                if (!_context.Knowledges.Any(p => p.ID == key))
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