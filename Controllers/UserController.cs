using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    //[AllowCrossSiteJson]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        public UserController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            Db.Connection.Open();
            var query = new User(Db);
            var result = query.GetAllUsers();
            Db.Connection.Close();
            //Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromBody]User item)
        {
            //User item = JsonConvert.DeserializeObject<User>(jres.ToString());
            //JsonModel j = new JsonModel( HttpContext.Request.Body);
            //System.Diagnostics.Debug.Write(item.FirstName);
           // var item = await Request.Content.ReadAsStringAsync();
            Db.Connection.Open();
     
            //string jsonString = JsonConvert.SerializeObject(item);
            //User us = JsonConvert.DeserializeObject<User>(item);
            item.Db = Db;
            //System.Diagnostics.Debug.Write(jsonString);
            var result = item.AddOneUser();
            Db.Connection.Close();
            //item.FirstName = "abc";
            return new OkObjectResult(result);
            //return item;
            // return Ok();
        }
        [HttpPost]
        [Route("addAll")]
        public async Task<IActionResult> Post([FromBody]MultipleUsers body)
        {
            Db.Connection.Open();
            body.Db = Db;
            body.AddAllUsers();
            Db.Connection.Close();
            return Ok();

            // return Ok();
        }


        [HttpGet]
        [Route("assignment")]
        public async Task<IActionResult> Get()
        {
            Db.Connection.Open();
            var query = new  AssignmentModel(Db);
            var result = query.getAllUsers();
            Db.Connection.Close();
            return Ok(result);

            // return Ok();
        }

        // [HttpPut]

        [Route("{username}/inactive")]
        public async  Task<IActionResult> PutOne(string username)
        {
            Db.Connection.Open();
            var query = new User(Db);
            query.UserName = username;
            query.Update();
            return Ok();
        }
        
        //[HttpDelete]
        [Route("{username}/remove")]
        public async Task<IActionResult> DeleteOne(string username)
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
