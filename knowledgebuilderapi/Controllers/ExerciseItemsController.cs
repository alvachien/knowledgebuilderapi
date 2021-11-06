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
using Microsoft.AspNetCore.Authorization;

namespace knowledgebuilderapi.Controllers
{
    [Authorize]
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

        /// GET: /ExerciseItems(:id)
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
                return BadRequest("Key is not matched");
            }

            // Check item need be updated
            //var contentChanged = false;
            // var execitem = await _context.ExerciseItems.FindAsync(key);
            var execitem = await _context.ExerciseItems
                    .Include(i => i.Answer)
                    .Include(i => i.Tags)
                    .SingleOrDefaultAsync(x => x.ID == key);
            if (execitem == null)
            {
                return NotFound();
            }

            execitem.UpdateData(update);
            if (execitem.Answer == null)
            {
                if (update.Answer != null)
                {
                    execitem.Answer = new ExerciseItemAnswer(update.Answer);
                    execitem.Answer.ExerciseItem = execitem;
                }
            }
            else
            {
                if (update.Answer != null)
                {
                    execitem.Answer.UpdateData(update.Answer);
                }
                else
                {
                    execitem.Answer = null;
                }
            }
            if (execitem.Tags.Count > 0)
            {
                if (update.Tags.Count > 0)
                {
                    execitem.Tags.Clear();

                    foreach (var tag in update.Tags)
                    {
                        var newtag = new ExerciseTag(tag);
                        newtag.CurrentExerciseItem = execitem;
                        execitem.Tags.Add(newtag);
                    }
                }
                else
                {
                    // Delete all
                    execitem.Tags.Clear();
                }
            }
            else
            {
                if (update.Tags.Count > 0)
                {
                    foreach(var tag in update.Tags)
                    {
                        var newtag = new ExerciseTag(tag);
                        newtag.CurrentExerciseItem = execitem;
                        execitem.Tags.Add(newtag);
                    }
                }
            }

            //// Check the exercise item
            //if (execitem.Equals(update))
            //{
            //    // Do nothing
            //}
            //else
            //{
            //    contentChanged = true;
            //    execitem.UpdateData(update);
            //    _context.Update(execitem);
            //}

            //// Check the associated object need be updated - Answer
            //if (execitem.Answer == null)
            //{
            //    if (update.Answer != null)
            //    {
            //        contentChanged = true;

            //        ExerciseItemAnswer eia = new ExerciseItemAnswer(update.Answer);
            //        eia.ExerciseItem = execitem;
            //        _context.Update(eia);
            //    }
            //}
            //else
            //{
            //    if (update.Answer != null)
            //    {
            //        if (!execitem.Answer.Equals(update.Answer))
            //        {
            //            ExerciseItemAnswer eia = new ExerciseItemAnswer(update.Answer);
            //            eia.ExerciseItem = execitem;
            //            _context.Update(eia);
            //        }
            //    }
            //    else
            //    {
            //        contentChanged = true;

            //    }
            //}

            //// Check the associated object need be updated - Tags
            //if (execitem.Tags.Count > 0)
            //{

            //}
            //else
            //{

            //}

            try
            {
                //if (contentChanged)
                //{
                    await _context.SaveChangesAsync();
                //}
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
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
