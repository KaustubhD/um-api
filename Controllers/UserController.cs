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
        public async Task<IActionResult> GetAllUsers()
        {
            Db.Connection.Open();
            var query = new User(Db);
            var result = query.GetAllUsers();
            Db.Connection.Close();
            return new OkObjectResult(result);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromBody]User item)
        {
            Db.Connection.Open();
            item.Db = Db;
            var result = item.AddOneUser();
            Db.Connection.Close();
            return new OkObjectResult(result);
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
            var result = query.getAllUsersInCustomFormat();
            Db.Connection.Close();
            return Ok(result);
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
