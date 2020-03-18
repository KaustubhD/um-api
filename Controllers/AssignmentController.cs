using System;
using System.Text.Json;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Utilities;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : Controller
    {
        public AssignmentController(AppDb db)
        {
            Db = db;
        }

        // GET: api/Assignment
        [HttpGet]
        public JsonResult GetAll()
        {
            string fieldsToDisplay = HttpContext.Request.Query["fields"];
            string namesToSearch = String.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["search"]))
                namesToSearch = HttpContext.Request.Query["search"];
   
            //Console.WriteLine("-----------" + (HttpContext.Request.Query["search"] == null) + "------------");
            //Console.WriteLine("-----------" + (HttpContext.Request.Query["search"] == null) + "------------");
            Db.Connection.Open();
            var query = new AssignmentModel(Db);
            var result = query.getAllUsersInCustomFormat(namesToSearch);
            Db.Connection.Close();
            foreach (var m1 in result)
            {
                m1.SetSerializableProperties(fieldsToDisplay);
            }
            return Json(result, new JsonSerializerSettings()
            {
                ContractResolver = new ShouldSerializeContractResolver()
            });
        }
        
        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            String fields = HttpContext.Request.Query["fields"];
            
            Db.Connection.Open();
            var query = new AssignmentModel(Db);
            var result = query.getUserInCustomFormat(id);
            Db.Connection.Close();
            result.SetSerializableProperties(fields);
            return Json(result, new JsonSerializerSettings()
            {
                ContractResolver = new ShouldSerializeContractResolver()
            });
            //return Ok(result);
        }
        
        // POST: api/Assignment
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Assignment/5
        [HttpPut("{username}")]
        public ActionResult Put(String username, [FromBody]AssignmentModel body)
        {
            Db.Connection.Open();
            var query = new AssignmentModel(Db);
            query.Update(username, body);
            Db.Connection.Close();
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]

        public void Delete(int id)
        {
        }
        public AppDb Db { get; }
    }
}
