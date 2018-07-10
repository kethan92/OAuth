using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
//using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using WebApplication6.Entities;

namespace WebApplication6.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebApplication6.Entities;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Login>("Logins");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class LoginsController : ODataController
    {
        private DB_Oauth_APIEntities db = new DB_Oauth_APIEntities();

        // GET: odata/Logins
        [EnableQuery]
        [HttpGet]
        [Authorize]
        public IQueryable<Login> GetLogins()
        {
            return db.Logins;
        }

        // GET: odata/Logins(5)
        [EnableQuery]
        public SingleResult<Login> GetLogin([FromODataUri] int key)
        {
            return SingleResult.Create(db.Logins.Where(login => login.id == key));
        }

        // PUT: odata/Logins(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Login> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Login login = db.Logins.Find(key);
            if (login == null)
            {
                return NotFound();
            }

            patch.Put(login);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(login);
        }

        // POST: odata/Logins
        public IHttpActionResult Post(Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Logins.Add(login);
            db.SaveChanges();

            return Created(login);
        }

        // PATCH: odata/Logins(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Login> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Login login = db.Logins.Find(key);
            if (login == null)
            {
                return NotFound();
            }

            patch.Patch(login);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(login);
        }

        // DELETE: odata/Logins(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Login login = db.Logins.Find(key);
            if (login == null)
            {
                return NotFound();
            }

            db.Logins.Remove(login);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LoginExists(int key)
        {
            return db.Logins.Count(e => e.id == key) > 0;
        }
    }
}
