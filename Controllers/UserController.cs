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
        public async Task<IActionResult> GetAllUsers()
        {
            await Db.Connection.OpenAsync();
            var query = new User(Db);
            var result = await query.GetAllUsersAsync();
            return new OkObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            var result = await body.AddOneUser();
            return new OkObjectResult(result);

            // return Ok();
        }
        [HttpPost("/addAll")]
        public async Task<IActionResult> Post([FromBody]MultipleUsers body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.AddAllUsers();
            Db.Connection.Close();
            return Ok();

            // return Ok();
        }
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteOne(string username)
        {
            await Db.Connection.OpenAsync();
            User query = new User(Db);
            query.UserName = username;
            /*
            var result = await query.FindOneAsync(username);
            if (!result)
                return new NotFoundResult();
            */
            await query.DeleteAsync();
            Db.Connection.Close();
            return new OkResult();
        }
        public AppDb Db { get; }
    }

}
