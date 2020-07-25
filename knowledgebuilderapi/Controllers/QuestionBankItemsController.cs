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
    public class QuestionBankItemsController : ODataController
    {
        private readonly kbdataContext _context;

        public QuestionBankItemsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /QuestionBankItems
        /// <summary>
        /// Adds support for getting question bank items, for example:
        /// 
        /// GET /QuestionBankItems
        /// GET /QuestionBankItems?$filter=Name eq 'Windows 95'
        /// GET /QuestionBankItems?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<QuestionBankItem> Get()
        {
            return _context.QuestionBankItems;
        }

        /// GET: /QuestionBankItems(:id)
        /// <summary>
        /// Adds support for getting a knowledge by key, for example:
        /// 
        /// GET /KnowledgeItems(1)
        /// </summary>
        /// <param name="key">The key of the Knowledge item required</param>
        /// <returns>The Knowledge item</returns>
        [EnableQuery]
        public SingleResult<QuestionBankItem> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.QuestionBankItems.Where(p => p.ID == key));
        }

        // POST: /QuestionBankItems
        /// <summary>
        /// Support for creating question bank item
        /// </summary>
        public async Task<IActionResult> Post([FromBody] QuestionBankItem qbitem)
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

            // Do initial check
            if (qbitem.KnowledgeItemID.HasValue)
            {
                if (!_context.KnowledgeItems.Any(p => p.ID == qbitem.KnowledgeItemID.Value))
                {
                    return BadRequest("Knowlege Item not exist");
                }
            }

            _context.QuestionBankItems.Add(qbitem);
            await _context.SaveChangesAsync();

            return Created(qbitem);
        }

        // PUT: /QuestionBankItems/5
        /// <summary>
        /// Support for updating question bank item
        /// </summary>
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] QuestionBankItem update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.ID)
            {
                return BadRequest();
            }

            // Do initial check
            if (update.KnowledgeItemID.HasValue)
            {
                if (!_context.KnowledgeItems.Any(p => p.ID == update.KnowledgeItemID.Value))
                {
                    return BadRequest("Knowlege Item not exist");
                }
            }

            _context.Entry(update).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.QuestionBankItems.Any(p => p.ID == key))
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

        // DELETE: /QuestionBankItems/5
        /// <summary>
        /// Support for deleting question bank item by key.
        /// </summary>
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var item = await _context.QuestionBankItems.FindAsync(key);
            if (item == null)
            {
                return NotFound();
            }

            _context.QuestionBankItems.Remove(item);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
