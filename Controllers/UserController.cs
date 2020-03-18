using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        public UserController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllUsers()
        {
            Db.Connection.Open();
            var query = new User(Db);
            var result = query.GetAllUsers();
            Db.Connection.Close();
            return new OkObjectResult(result);
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

        [HttpPost]
        [Route("addAll")]
        public IActionResult Post([FromBody]MultipleUsers body)
        {
            Db.Connection.Open();
            body.Db = Db;
            body.AddAllUsers();
            Db.Connection.Close();
            return Ok();
        }

        [HttpPut]
        [Route("{username}/update")]
         public ActionResult Put(String username, [FromBody]User body)
         {
            Db.Connection.Open();
            //var query = new User(Db);
            //query.UpdateUser(username, body);
            body.Db = Db;
            body.UserName = username;
            Console.WriteLine(body.FirstName);
            var result = body.UpdateUser();
            Db.Connection.Close();
            return Ok(result);
            //return Ok();


            //body.Db = neDb;
            //body.UserName = username;
            //var result = body.UpdateUser(username,body);
           
            

        }



        [HttpPut]
        [Route("{username}/inactive")]
        public IActionResult PutOne(string username)
        {
            Db.Connection.Open();
            var query = new User(Db);
            query.UserName = username;
            query.Update();
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
            /*
            var result = await query.FindOneAsync(username);
            if (!result)
                return new NotFoundResult();
            */
            query.Delete();
            Db.Connection.Close();
            return new OkResult();
        }
        public AppDb Db { get; }
    }

}
