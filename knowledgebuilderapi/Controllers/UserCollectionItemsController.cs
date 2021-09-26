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

namespace knowledgebuilderapi.Controllers
{
    public class UserCollectionItemsController : ODataController
    {
        private readonly kbdataContext _context;

        public UserCollectionItemsController(kbdataContext context)
        {
            _context = context;
        }

        // GET: /UserCollectionItems
        [EnableQuery]
        public IQueryable<UserCollectionItem> Get()
        {
            return _context.UserCollectionItems;
        }

        // GET: /UserCollectionItems(:id)
        [EnableQuery]
        public SingleResult<UserCollectionItem> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.UserCollectionItems.Where(p => p.ID == key));
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCollection([FromBody] ODataActionParameters parameters)
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

            String user = (String)parameters["User"];
            int collid = (int)parameters["ID"];
            int refid = (int)parameters["RefID"];
            TagRefType reftype = (TagRefType)parameters["RefType"];
            var createAtValue = parameters["CreatedAt"];
            DateTime? createdAt = null;
            if (createAtValue != null)
                createdAt = ((DateTimeOffset)createAtValue).DateTime;
            else
                createdAt = DateTime.Today;
            if (String.IsNullOrEmpty(user) || refid <= 0)
                return BadRequest("Invalid input");

            // Check collection header
            var collcnt = (from collheader in this._context.UserCollections
                             where collheader.ID == collid && collheader.User == user select collheader.ID).Count();
            if (collcnt != 1)
                return BadRequest("Invalid collection");

            // Check existence of item
            var itemcnt = (from collitem in this._context.UserCollectionItems
                           where collitem.RefType == reftype && collitem.RefID == refid && collitem.ID == collid
                           select collitem.ID).Count();
            if (itemcnt > 0)
                return NoContent();

            // Check existence of ref. id
            switch (reftype)
            {
                case TagRefType.KnowledgeItem:
                    break;

                case TagRefType.ExerciseItem:
                default:
                    {
                        var refcnt = (from exec in _context.ExerciseItems
                                      where exec.ID == refid
                                      select exec.ID).Count();
                        if (refcnt != 1)
                            return BadRequest("Invalid refence ID");
                    }
                    break;
            }

            var nitem = new UserCollectionItem();
            nitem.ID = collid;
            nitem.RefID = refid;
            nitem.RefType = reftype;
            nitem.CreatedAt = createdAt;
            this._context.UserCollectionItems.Add(nitem);
            await this._context.SaveChangesAsync();

            return Ok(nitem);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToCollectionEx([FromBody] ODataActionParameters parameters)
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

            var items = (HashSet<UserCollectionItem>)parameters["UserCollectionItems"];
            String user = (String)parameters["User"];
            int collid = (int)parameters["ID"];
            int refid = (int)parameters["RefID"];
            TagRefType reftype = (TagRefType)parameters["RefType"];
            var createAtValue = parameters["CreatedAt"];
            DateTime? createdAt = null;
            if (createAtValue != null)
                createdAt = ((DateTimeOffset)createAtValue).DateTime;
            else
                createdAt = DateTime.Today;
            if (String.IsNullOrEmpty(user) || refid <= 0)
                return BadRequest("Invalid input");

            // Check collection header
            var collcnt = (from collheader in this._context.UserCollections
                           where collheader.ID == collid && collheader.User == user
                           select collheader.ID).Count();
            if (collcnt != 1)
                return BadRequest("Invalid collection");

            // Check existence of item
            var itemcnt = (from collitem in this._context.UserCollectionItems
                           where collitem.RefType == reftype && collitem.RefID == refid && collitem.ID == collid
                           select collitem.ID).Count();
            if (itemcnt > 0)
                return NoContent();

            // Check existence of ref. id
            switch (reftype)
            {
                case TagRefType.KnowledgeItem:
                    break;

                case TagRefType.ExerciseItem:
                default:
                    {
                        var refcnt = (from exec in _context.ExerciseItems
                                      where exec.ID == refid
                                      select exec.ID).Count();
                        if (refcnt != 1)
                            return BadRequest("Invalid refence ID");
                    }
                    break;
            }

            var nitem = new UserCollectionItem();
            nitem.ID = collid;
            nitem.RefID = refid;
            nitem.RefType = reftype;
            nitem.CreatedAt = createdAt;
            this._context.UserCollectionItems.Add(nitem);
            await this._context.SaveChangesAsync();

            return Ok(nitem);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItemFromCollection([FromBody] ODataActionParameters parameters)
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

            String user = (String)parameters["User"];
            int collid = (int)parameters["ID"];
            int refid = (int)parameters["RefID"];
            TagRefType reftype = (TagRefType)parameters["RefType"];
            if (String.IsNullOrEmpty(user) || refid <= 0)
                return BadRequest("Invalid input");

            // Check collection header
            var collcnt = (from collheader in this._context.UserCollections
                           where collheader.ID == collid && collheader.User == user select collheader.ID).Count();
            if (collcnt != 1)
                return BadRequest("Invalid collection");

            // Check existence of item
            var nitem = _context.UserCollectionItems.SingleOrDefault(p => p.RefType == reftype && p.RefID == refid && p.ID == collid);
            if (nitem == null)
                return Ok(false);

            _context.UserCollectionItems.Remove(nitem);
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}
