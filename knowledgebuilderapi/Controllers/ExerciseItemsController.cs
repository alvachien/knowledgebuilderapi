using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace knowledgebuilderapi.Controllers
{
    public class ExerciseItemsController : ODataController
    {
        private readonly kbdataContext _context;

        public ExerciseItemsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /ExerciseItems
        /// <summary>
        /// Adds support for getting knowledges, for example:
        /// 
        /// GET /ExerciseItems
        /// GET /ExerciseItems?$filter=Name eq 'Windows 95'
        /// GET /ExerciseItems?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<ExerciseItem> Get()
        {
            return _context.ExerciseItems;
        }

        /// GET: /ExerciseItem(:id)
        /// <summary>
        /// Adds support for getting a exercise by key, for example:
        /// 
        /// GET /ExerciseItem(1)
        /// </summary>
        /// <param name="key">The key of the exercise item</param>
        /// <returns>The exercise item</returns>
        [EnableQuery]
        public SingleResult<ExerciseItem> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.ExerciseItems.Where(p => p.ID == key));
        }

        // POST: /ExerciseItems
        /// <summary>
        /// Support for creating exercise item
        /// </summary>
        public async Task<IActionResult> Post([FromBody] ExerciseItem execitem)
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

            // Admin. fields
            execitem.CreatedAt = DateTime.Now;
            execitem.ModifiedAt = null;
            //if (execitem.Answer != null)
            //{
            //    execitem.Answer.ExerciseItem = execitem;
            //}
            //ExerciseItemAnswer execawr = null;
            //if (execitem.Answer != null)
            //{
            //    execawr = new ExerciseItemAnswer();
            //    execawr.Content = execitem.Answer.Content;
            //    execitem.Answer = null;
            //}
            if (execitem.Tags.Count > 0)
            {
                foreach (var tag in execitem.Tags)
                    tag.CurrentExerciseItem = execitem;
            }

            // Update db
            _context.ExerciseItems.Add(execitem);
            await _context.SaveChangesAsync();

            // Answer
            //if (execawr != null)
            //{
            //    execawr.ItemID = execitem.ID;
            //    _context.ExerciseItemAnswers.Add(execawr);
            //    await _context.SaveChangesAsync();
            //}

            return Created(execitem);
        }

        // PUT: /ExerciseItems/5
        /// <summary>
        /// Support for updating exercise items
        /// </summary>
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] ExerciseItem update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.ID)
            {
                return BadRequest();
            }

            update.ModifiedAt = DateTime.Now;
            _context.Entry(update).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ExerciseItems.Any(p => p.ID == key))
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

        // DELETE: /ExerciseItems/5
        /// <summary>
        /// Support for deleting knowledge item by key.
        /// </summary>
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var execitem = await _context.ExerciseItems.FindAsync(key);
            if (execitem == null)
            {
                return NotFound();
            }

            _context.ExerciseItems.Remove(execitem);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }

        // PATCH: /ExerciseItems
        /// <summary>
        /// Support for partial updates of knowledge items
        /// </summary>
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<ExerciseItem> execitem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _context.ExerciseItems.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            execitem.Patch(entity);
            entity.ModifiedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ExerciseItems.Any(p => p.ID == key))
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
