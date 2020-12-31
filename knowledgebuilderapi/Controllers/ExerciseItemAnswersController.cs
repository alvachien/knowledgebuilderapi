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
    public class ExerciseItemAnswersController : ODataController
    {
        private readonly kbdataContext _context;

        public ExerciseItemAnswersController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /ExerciseItemAnswers
        /// <summary>
        /// Adds support for getting exercise item answer, for example:
        /// 
        /// GET /ExerciseItemAnswers
        /// GET /ExerciseItemAnswers?$filter=Name eq 'Windows 95'
        /// GET /ExerciseItemAnswers?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<ExerciseItemAnswer> Get()
        {
            return _context.ExerciseItemAnswers;
        }

        /// GET: /ExerciseItemAnswers(:id)
        /// <summary>
        /// Adds support for getting a exercise item answer by key, for example:
        /// 
        /// GET /ExerciseItemAnswers(1)
        /// </summary>
        /// <param name="key">The key of the exercise item answer</param>
        /// <returns>The exercise item answer</returns>
        [EnableQuery]
        public SingleResult<ExerciseItemAnswer> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.ExerciseItemAnswers.Where(p => p.ID == key));
        }

        // POST: /ExerciseItemAnswers
        /// <summary>
        /// Support for creating exercise item answer
        /// </summary>
        public async Task<IActionResult> Post([FromBody] ExerciseItemAnswer answer)
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

            _context.ExerciseItemAnswers.Add(answer);
            await _context.SaveChangesAsync();

            return Created(answer);
        }

        // PUT: /ExerciseItemAnswers/5
        /// <summary>
        /// Support for updating exercise item answer
        /// </summary>
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] ExerciseItemAnswer update)
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
                if (!_context.ExerciseItemAnswers.Any(p => p.ID == key))
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

        // DELETE: /ExerciseItemAnswers/5
        /// <summary>
        /// Support for deleting exercise item answer by key.
        /// </summary>
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var answer = await _context.ExerciseItemAnswers.FindAsync(key);
            if (answer == null)
            {
                return NotFound();
            }

            _context.ExerciseItemAnswers.Remove(answer);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }

        // PATCH: /ExerciseItemAnswers
        /// <summary>
        /// Support for partial updates of exercise item answer
        /// </summary>
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<ExerciseItemAnswer> answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await _context.ExerciseItemAnswers.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            answer.Patch(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ExerciseItemAnswers.Any(p => p.ID == key))
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

