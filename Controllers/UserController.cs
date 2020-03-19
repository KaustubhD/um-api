using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UserManagement.Models;
using UserManagement.Utilities;
using System.Web.Http.Results;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        public UserController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        [Route("{username}")]
        public IActionResult GetOneUser(string username)
        {
            Db.Connection.Open();
            var query = new User(Db);
            var result = query.getUserByUsername(username);
            Db.Connection.Close();
            return Ok(result);
        }

        [HttpGet]
        public JsonResult GetAllUsersInCustomFormat()
        {
            string fieldsToDisplay = HttpContext.Request.Query["fields"];
            string namesToSearch = String.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["search"]))
                namesToSearch = HttpContext.Request.Query["search"];

            Db.Connection.Open();
            var query = new User(Db);
            var result = query.GetAllUsers(namesToSearch);
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

        [HttpPost]
        [Route("add")]
        public IActionResult Post([FromBody]User item)
        {
            Db.Connection.Open();
            Console.WriteLine("------------" + item.addresses[0].AddressLine + "----------------");
            item.Db = Db;
            var result = item.AddOneUser();
            Db.Connection.Close();
            return new OkObjectResult(item);
        }

        /*
        [HttpPost]
        [Route("addAll")]
        public IActionResult Post([FromBody]MultipleUsers body)
        {
            Db.Connection.Open();
            body.Db = Db;
            body.AddAllUsers();
            Db.Connection.Close();
            return Ok();
        }*/

        [HttpPut]
        [Route("{username}/update")]
         public ActionResult Put(String username, [FromBody]User body)
         {
            Db.Connection.Open();
            body.Db = Db;
            body.UserName = username;
            Console.WriteLine(body.FirstName);
            var result = body.UpdateUser();
            Db.Connection.Close();
            return Ok(result);
        
        }


        [HttpGet]
        [Route("{username}/inactive")]
        public IActionResult PutOne(string username)
        {
            Db.Connection.Open();
            var query = new User(Db);
            query.UserName = username;
            query.MarkUserInactive();
            Db.Connection.Close();
            return Ok();
        }
        
        [HttpDelete]
        [Route("{username}/remove")]
        public IActionResult DeleteOne(string username)
        {
            Db.Connection.Open();
            User query = new User(Db);
            query.UserName = username;

            query.Delete();
            Db.Connection.Close();
            return  Ok();
        }
        public AppDb Db { get; }
    }

}
