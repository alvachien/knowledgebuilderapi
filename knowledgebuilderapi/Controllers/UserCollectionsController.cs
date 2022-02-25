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
using Microsoft.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace knowledgebuilderapi.Controllers
{
    [Authorize]
    public class UserCollectionsController : ODataController
    {
        private readonly kbdataContext _context;

        public UserCollectionsController(kbdataContext context)
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
        public IQueryable<UserCollection> Get()
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new UnauthorizedAccessException("Failed ID");

            return _context.UserCollections.Where(p => p.User == usrId);
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
        public SingleResult<UserCollection> Get([FromODataUri] int key)
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new UnauthorizedAccessException("Failed ID");

            return SingleResult.Create(_context.UserCollections.Where(p => p.User == usrId && p.ID == key));
        }

        //// GET: /UserCollections
        //// [EnableQuery]
        //public IActionResult Get(ODataQueryOptions<UserCollection> queryOptions)
        //{
        //    // var querable = accounts.AsQueryable<Account>();
        //    if (queryOptions.Filter == null)
        //        return BadRequest("Must specify ther User");
        //    else
        //    {
        //        if (!queryOptions.Filter.RawValue.Contains("User EQ", StringComparison.OrdinalIgnoreCase))
        //            return BadRequest("Must specify ther User");
        //    }

        //    // queryOptions.Validate(new ODataValidationSettings())
        //    var finalQuery = queryOptions.ApplyTo(_context.UserCollections);
        //    return Ok(finalQuery);
        //    //return _context.UserCollections;
        //}

        //// GET: /UserCollections(:id)
        //// [EnableQuery]
        //public IActionResult Get([FromODataUri] int key, ODataQueryOptions<UserCollection> queryOptions)
        //{
        //    // return SingleResult.Create(_context.UserCollections.Where(p => p.ID == key));
        //    var accountQuery = _context.UserCollections.Where(c => c.ID == key);
        //    if (!accountQuery.Any())
        //    {
        //        return NotFound();
        //    }

        //    var finalQuery = queryOptions.ApplyTo(accountQuery.AsQueryable<UserCollection>()) as IQueryable<dynamic>;
        //    var result = finalQuery.FirstOrDefault();

        //    if (result == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(result);
        //}

        // POST: /UserCollections
        /// <summary>
        /// Support for creating user collection
        /// </summary>
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

                return BadRequest(ModelState);
            }

            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId) || coll.User != usrId)
                return new UnauthorizedResult();

            coll.CreatedAt = DateTime.Now;            
            _context.UserCollections.Add(coll);
            await _context.SaveChangesAsync();

            return Created(coll);
        }

        // PUT: /UserCollections(5)
        /// <summary>
        /// Support for updating user collection
        /// </summary>
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] UserCollection update)
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

                return BadRequest(ModelState);
            }

            if (key != update.ID)
                return BadRequest("Invalid key");

            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId) || update.User != usrId)
                return new UnauthorizedResult();

            // Check existes
            var isexist = await _context.UserCollections.AsNoTracking().Where(p => p.ID == key).CountAsync();
            if (isexist <= 0)
                return NotFound();

            update.ModifiedAt = DateTime.Now;
            _context.Entry(update).State = EntityState.Modified;

            // Items
            var itemsindb = _context.UserCollectionItems.AsNoTracking().Where(p => p.ID == update.ID).ToList();
            foreach (var ditem in update.Items)
            {
                var itemindb = itemsindb.Find(p => p.RefType == ditem.RefType && p.RefID == ditem.RefID);
                if (itemindb == null)
                {
                    _context.UserCollectionItems.Add(ditem);
                }
            }
            foreach (var ditem2 in itemsindb)
            {
                var nitem = update.Items.FirstOrDefault(p => p.RefType == ditem2.RefType && p.RefID == ditem2.RefID);
                if (nitem == null)
                {
                    _context.UserCollectionItems.Remove(ditem2);
                }
            }

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

        // DELETE: /UserCollections(5)
        /// <summary>
        /// Support for deleting user collection
        /// </summary>

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var coll = await _context.UserCollections.FindAsync(key);
            if (coll == null)
                return NotFound();

            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId) || coll.User != usrId)
                return new UnauthorizedResult();

            _context.UserCollections.Remove(coll);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
