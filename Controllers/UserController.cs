using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
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
        public async Task<IActionResult> Post([FromBody]User body)
        {
            Db.Connection.Open();
            body.Db = Db;
            var result = body.AddOneUser();
            Db.Connection.Close();
            return new OkObjectResult(result);

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
